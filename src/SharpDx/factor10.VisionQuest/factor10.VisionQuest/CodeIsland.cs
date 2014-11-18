using System;
using System.Collections.Generic;
using System.Linq;
using factor10.VisionaryHeads;
using factor10.VisionQuest.GroundControl;
using factor10.VisionThing;
using factor10.VisionThing.Objects;
using factor10.VisionThing.Terrain;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionQuest
{
    public class CodeIsland : TerrainBase
    {
        public readonly Archipelag Archipelag;

        public const int ClassSide = 32;

        public readonly VAssembly VAssembly;
        public readonly Dictionary<string, VisionClass> Classes = new Dictionary<string, VisionClass>();

        public CodeIsland(
            VisionContent vContent,
            Archipelag archipelag,
            Matrix world,
            VAssembly vassembly)
            : base(vContent)
        {
            Archipelag = archipelag;
            VAssembly = vassembly;
            World = world;

            if (VAssembly.Is3DParty)
            {
                Box = new Box(vContent, World, new Vector3(50, 20, 50), 0.01f);
                foreach (var vclass in vassembly.VClasses)
                {
                    var vc = new VisionClass(this, vclass, 75, 75, 5) {Height = 10};
                    Classes.Add(vclass.FullName, vc);
                }
                return;
            }

            var rnd = new Random();

            var interfaceClasses = new List<VClass>();
            var implementationClasses = new List<VClass>();
            foreach (var vclass in vassembly.VClasses)
                if (vclass.IsInterface)
                    interfaceClasses.Add(vclass);
                else
                    implementationClasses.Add(vclass);

            var circleMaster = new CircleMaster<VClass>();
            var q = 0;
            foreach (var vclass in interfaceClasses)
                circleMaster.Drop(q += 10, 0, 10, vclass);

            foreach (var vclass in implementationClasses)
                circleMaster.Drop(q += 5, 0, 4 + (int) Math.Sqrt(vclass.InstructionCount), vclass);

            int left, top, right, bottom;
            circleMaster.GetBounds(out left, out top, out right, out bottom);
            foreach (var c in circleMaster.Circles)
            {
                var vc = new VisionClass(this, c.Tag, ClassSide/2 + c.X - left, ClassSide/2 + c.Y - top, c.R);
                Classes.Add(c.Tag.FullName, vc);
            }

            var surfaceWidth = 1 << (1 + (int) (Math.Log(right - left)/Math.Log(2)));
            var surfaceHeight = 1 << (1 + (int) (Math.Log(bottom - top)/Math.Log(2)));

            System.Diagnostics.Debug.Print("{0}: {1} {2}", vassembly.Name, surfaceWidth, surfaceHeight);

            //var qq = Math.Max(surfaceWidth, surfaceHeight);
            var ground = new Ground(surfaceWidth, surfaceHeight);

            BoundingSphere = new BoundingSphere(new Vector3(
                world.TranslationVector.X + ground.Width / 2f,
                world.TranslationVector.Y,
                world.TranslationVector.Z + ground.Height / 2f),
                (float)Math.Sqrt(ground.Width * ground.Width + ground.Height * ground.Height) / 2);

            foreach (var vc in Classes.Values)
            {
                var instructHeight = (vc.VClass.IsInterface ? 40 : 10) + (float) Math.Pow(vc.VClass.InstructionCount, 0.3);
                var maintainabilityFactor = 3*(10 - vc.MaintainabilityIndex/10);
                var radius = vc.R;
                var middleX = vc.X - radius;
                var middleY = vc.Y - radius;
                var bellShapeFactor = 2f/(radius*1.7f);
                ground.AlterValues(
                    middleX, middleY,
                    radius*2, radius*2,
                    (px, py,  h) =>
                    {
                        var dx = px - radius;
                        var dy = py - radius;
                        var d = (dx*dx + dy*dy)*bellShapeFactor*bellShapeFactor;
                        var sharpness = (px & 1) != (py & 1) ? maintainabilityFactor : 0;
                        return h + instructHeight*(float) Math.Exp(-d*d) + sharpness*(float) rnd.NextDouble();
                    });

                var height = ground[vc.X, vc.Y];
                vc.Height = height;
            }

            // raise the point where the sign is
            foreach (var vc in Classes.Values)
                ground[vc.X, vc.Y] += 6;

            //...and lower it...
            ground.Soften(2);

            //make ground slices seamless
            for (var x = 64; x < surfaceWidth; x += TerrainPlane.SquareSize)
                for (var y = 0; y < surfaceHeight; y++)
                    ground[x, y] = ground[x - 1, y] = (ground[x, y] + ground[x - 1, y])/2;
            for (var y = 64; y < surfaceHeight; y += TerrainPlane.SquareSize)
                for (var x = 0; x < surfaceWidth; x++)
                    ground[x, y] = ground[x, y - 1] = (ground[x, y] + ground[x, y - 1])/2;

            foreach (var vc in Classes.Values)
                vc.Height = ground[vc.X, vc.Y];

            var normals = ground.CreateNormalsMap();

            var signs = new Signs(
                vContent,
                world*Matrix.Translation(0, -0.1f, 0),
                vContent.Load<Texture2D>("textures/woodensign"),
                Classes.Values.ToList(),
                16,
                4);
            Children.Add(signs);

            var qqq = world; //*Matrix.Translation(0, 0.05f, 0);
            var grass = new List<Tuple<Vector3, Vector3>>();
            foreach (var vc in Classes.Values)
            {
                for (var i = (vc.CyclomaticComplexity - 1)*2; i > 0; i--)
                {
                    var gx = vc.X + ((float) rnd.NextDouble() - 0.5f)*(ClassSide - 3);
                    var gy = vc.Y + ((float) rnd.NextDouble() - 0.5f)*(ClassSide - 3);
                    grass.Add(new Tuple<Vector3, Vector3>(
                        Vector3.TransformCoordinate(new Vector3(gx, ground.GetExactHeight(gx, gy), gy),qqq),
                        normals.AsVector3(vc.X, vc.Y)));
                }
            }
            var ms = new CxBillboard(vContent, Matrix.Identity, vContent.Load<Texture2D>("billboards/grass"), 1, 1);
            ms.CreateBillboardVerticesFromList(grass);
            Children.Add(ms);

            var weights = ground.CreateWeigthsMap(new[] {0, 0.40f, 0.60f, 0.9f});
            //weights.DrawLine(0, 0, 20, 20, 1, (_, mt) => new Mt9Surface.Mt9 { I = 100 });
            //weights.AlterValues(20, 20, 20, 20, (x, y, mt) =>
            //{
            //    mt.B = 1;
            //    return mt;
            //});
            //weights.AlterValues(40, 40, 20, 20, (x, y, mt) => new Mt9Surface.Mt9 { C = 100 });
            //weights.AlterValues(60, 60, 20, 20, (x, y, mt) => new Mt9Surface.Mt9 { D = 10 });
            //weights.AlterValues(80, 80, 20, 20, (x, y, mt) => new Mt9Surface.Mt9 { E = 10 });
            //weights.AlterValues(100, 100, 20, 20, (x, y, mt) => new Mt9Surface.Mt9 { F = 10 });
            //weights.AlterValues(120, 120, 20, 20, (x, y, mt) => new Mt9Surface.Mt9 { G = 10 });
            //weights.AlterValues(140, 140, 20, 20, (x, y, mt) => new Mt9Surface.Mt9 { H = 10 });
            //weights.AlterValues(160, 160, 20, 20, (x, y, mt) => new Mt9Surface.Mt9 { I = 100, A = (float)Math.Sqrt(x * x + y * y) });

            //foreach (var vc in Classes.Values.Where(_ => !_.CalledClasses.Any()))
            //{
            //    weights.AlterValues(vc.X - vc.R, vc.Y - vc.R, vc.R*2, vc.R*2, (x, y, mt) => new Mt9Surface.Mt9 {I = 100});
            //}

            initialize(ground, weights, normals);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            return VAssembly.Is3DParty
                ? Box.Draw(camera, drawingReason, shadowMap)
                : base.draw(camera, drawingReason, shadowMap);
        }

        public static List<CodeIsland> Create(VisionContent vContent, Archipelag archipelag, List<VAssembly> assemblies)
        {
            int x = 0, y = 0;
            return assemblies.OrderBy(_ => _.Is3DParty).Select(assembly => createOne(vContent, archipelag, assembly, ref x, ref y)).ToList();
        }

        private static CodeIsland createOne(VisionContent vContent, Archipelag archipelag, VAssembly assembly, ref int x, ref int y)
        {
            var t = Matrix.Translation(-y*600, -0.5f, -900 - x*600);
            var codeIsland = new CodeIsland(vContent, archipelag, t, assembly);

            x++;
            if (x > 4)
            {
                x = 0;
                y++;
            }

            return codeIsland;
        }

    }

}
