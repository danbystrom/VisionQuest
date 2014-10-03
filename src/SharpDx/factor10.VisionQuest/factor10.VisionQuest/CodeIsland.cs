using System;
using System.Collections.Generic;
using System.Linq;
using factor10.VisionaryHeads;
using factor10.VisionThing;
using factor10.VisionThing.Objects;
using factor10.VisionThing.Terrain;
using SharpDX;
using SharpDX.Toolkit.Graphics;
using TestBed;

namespace factor10.VisionQuest
{
    public class CodeIsland : TerrainBase
    {
        public const int ClassSide = 32;

        public readonly VAssembly VAssembly;
        public readonly Dictionary<string, VisualClass> Classes = new Dictionary<string, VisualClass>();

        public CodeIsland(
            VisionContent vContent,
            Matrix world,
            VAssembly vassembly)
            : base(vContent)
        {
            World = world;
            VAssembly = vassembly;

            if (VAssembly.Is3dParty)
            {
                Box = new Box(vContent, World, new Vector3(50, 20, 50), 0.01f);
                foreach (var vclass in vassembly.VClasses)
                {
                    var vc = new VisualClass(vclass, 75, 75, 5) {Height = 10};
                    Classes.Add(vclass.FullName, vc);
                }
                return;
            }

            var rnd = new Random();

            var totalArea = vassembly.VClasses.Sum(_ => 4 + _.InstructionCount)*1.6;

            var side = (int) Math.Sqrt(totalArea); // (ClassSide/2) * (int)Math.Ceiling(Math.Sqrt(vassembly.VClasses.Count+4));
            var surfaceSide = 2*((side + ClassSide)/ClassSide);
            surfaceSide *= ClassSide;

            var interfaceClasses = new List<VClass>();
            var implementationClasses = new List<VClass>();
            foreach (var vclass in vassembly.VClasses)
                if (vclass.IsInterface)
                    interfaceClasses.Add(vclass);
                else
                    implementationClasses.Add(vclass);

            while (true)
            {
                var circleContainer = new CircleContainer(surfaceSide - ClassSide, ClassSide/2);

                foreach (var vclass in interfaceClasses)
                {
                    var r = 10;
                    var circle = circleContainer.Drop(r);
                    var vc = new VisualClass(vclass, ClassSide/2 + (int) circle.X, ClassSide/2 + (int) circle.Y, r);
                    Classes.Add(vclass.FullName, vc);
                }

                foreach (var vclass in implementationClasses)
                {
                    var r = 4 + (int) Math.Sqrt(vclass.InstructionCount);
                    var circle = circleContainer.Drop(r);
                    var vc = new VisualClass(vclass, ClassSide/2 + (int) circle.X, ClassSide/2 + (int) circle.Y, r);
                    Classes.Add(vclass.FullName, vc);
                }

                var q = (((int) circleContainer.MaxUsedY + 65)/64)*64;
                if (q < surfaceSide)
                    break;
                Classes.Clear();
                surfaceSide += 64;
            }

            var ground = new Ground(surfaceSide, surfaceSide);

            foreach (var vc in Classes.Values)
            {
                var instructHeight = (vc.VClass.IsInterface ? 40 : 10) + (float) Math.Pow(vc.VClass.InstructionCount, 0.3);
                var maintainabilityFactor = 3*(10 - vc.MaintainabilityIndex/10);
                var middleX = vc.X - vc.R;
                var middleY = vc.Y - vc.R;
                var bellShapeFactor = 2f/(vc.R*1.7f);
                ground.AlterValues(
                    middleX, middleY,
                    vc.R*2, vc.R*2,
                    (px, py, h) =>
                    {
                        var dx = (vc.X - px);
                        var dy = (vc.Y - py);
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
            for (var x = 64; x < surfaceSide; x += 64)
                for (var y = 0; y < surfaceSide; y++)
                    ground[x, y] = ground[x - 1, y] = (ground[x, y] + ground[x - 1, y])/2;

            for (var y = 64; y < surfaceSide; y += 64)
                for (var x = 0; x < surfaceSide; x++)
                    ground[x, y] = ground[x, y - 1] = (ground[x, y] + ground[x, y - 1])/2;


            foreach (var vc in Classes.Values)
                vc.Height = ground[vc.X, vc.Y];

            var normals = ground.CreateNormalsMap();

            var signs = new Signs(
                vContent,
                world*Matrix.Translation(-64, -0.1f, -64),
                vContent.Load<Texture2D>("textures/woodensign"),
                Classes.Values.ToList(),
                16,
                4);
            Children.Add(signs);

            var ms = new MicrosoftBillboards(vContent, world*Matrix.Translation(-64, 0.05f, -64));
            var grass = new List<Tuple<Vector3, Vector3>>();
            foreach (var vc in Classes.Values)
            {
                for (var i = (vc.CyclomaticComplexity - 1)*2; i > 0; i--)
                {
                    var gx = vc.X + ((float) rnd.NextDouble() - 0.5f)*(ClassSide - 3);
                    var gy = vc.Y + ((float) rnd.NextDouble() - 0.5f)*(ClassSide - 3);
                    grass.Add(new Tuple<Vector3, Vector3>(
                        new Vector3(gx, ground.GetExactHeight(gx, gy), gy),
                        normals.AsVector3(vc.X, vc.Y)));
                }
            }
            ms.CreateBillboardVerticesFromList(grass);
            Children.Add(ms);

            var weights = ground.CreateWeigthsMap(new[] {0, 0.5f, 0.95f, 1});
            weights.DrawLine(15, 15, surfaceSide-16, surfaceSide-16, 5,
                (c, o) =>
                {
                    var factor = (1 + o)/4f;
                    c.A *= factor;
                    c.B *= factor;
                    c.C *= factor;
                    c.D *= factor;
                    c.H = 1 - c.A - c.B - c.C - c.D;
                    return c;
                },
                0,
                new Random());

            initialize(ground, weights, normals);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            return VAssembly.Is3dParty
                ? Box.Draw(camera, drawingReason, shadowMap)
                : base.draw(camera, drawingReason, shadowMap);
        }

        public static List<CodeIsland> Create(VisionContent vContent, List<VAssembly> assemblies)
        {
            int x = 0, y = 0;
            return assemblies.OrderBy(_ => _.Is3dParty).Select(assembly => createOne(vContent, assembly, ref x, ref y)).ToList();
        }

        private static CodeIsland createOne(VisionContent vContent, VAssembly assembly, ref int x, ref int y)
        {
            var t = Matrix.Translation(-y*300, -0.5f, -900 - x*300);
            var codeIsland = new CodeIsland(vContent, t, assembly);
            //                codeIsland.World = t;

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
