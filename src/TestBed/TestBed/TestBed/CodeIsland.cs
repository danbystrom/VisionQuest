using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestBed;
using factor10.VisionThing.Terrain;
using factor10.VisionaryHeads;

namespace factor10.VisionThing
{
    public class CodeIsland : TerrainBase
    {
        public const int ClassSide = 32;

        public readonly VAssembly VAssembly;
        public readonly Dictionary<string, VisualClass> Classes = new Dictionary<string, VisualClass>();

        public CodeIsland(
            Matrix world,
            VAssembly vassembly)
        {
            World = world;
            VAssembly = vassembly;

            var rnd = new Random();

            var totalArea = vassembly.VClasses.Sum(_ => 4 + _.InstructionCount)*1.6;

            var side = (int) Math.Sqrt(totalArea); // (ClassSide/2) * (int)Math.Ceiling(Math.Sqrt(vassembly.VClasses.Count+4));
            var surfaceSide = 2*((side + ClassSide)/ClassSide);
            surfaceSide *= ClassSide;

            var interfaceClasses = new List<VClass>();
            var implementationClasses = new List<VClass>();
            foreach (var vclass in vassembly.VClasses)
                if(vclass.TypeDefinition.IsInterface)
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
                    var vc = new VisualClass(vclass, ClassSide / 2 + (int)circle.X, ClassSide / 2 + (int)circle.Y, r);
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
                var instructHeight = 10 + (float) Math.Pow(vc.VClass.InstructionCount, 0.3);
                var maintainabilityFactor = 3*(10 - vc.MaintainabilityIndex/10);
                var middleX = vc.X - vc.R;
                var middleY = vc.Y - vc.R;
                var bellShapeFactor = 2f / (vc.R * 1.7f);
                ground.AlterValues(
                    middleX, middleY,
                    vc.R*2, vc.R*2,
                    (px, py, h) =>
                    {
                        var dx = (vc.X - px);
                        var dy = (vc.Y - py);
                        var d = (dx * dx + dy * dy) * bellShapeFactor * bellShapeFactor;
                        var sharpness = (px & 1) != (py & 1) ? maintainabilityFactor : 0;
                        return h + instructHeight*(float) Math.Exp(-d*d) + sharpness*(float) rnd.NextDouble();
                    });

                var height = ground[vc.X, vc.Y];
                vc.Height = height;
            }

            // raise the point where the sign is
            foreach (var vc in Classes.Values)
                ground[vc.X, vc.Y] += 6;

            ground.Soften(2);

            for(var x=64;x<surfaceSide ;x+=64)
                for (var y = 0; y < surfaceSide; y++)
                    ground[x, y] = ground[x - 1, y] = (ground[x, y] + ground[x - 1, y])/2;

            for (var y = 64; y < surfaceSide; y += 64)
                for (var x = 0; x < surfaceSide; x++)
                    ground[x, y] = ground[x, y - 1] = (ground[x, y] + ground[x, y - 1])/2;

            foreach (var vc in Classes.Values)
                vc.Height = ground[vc.X, vc.Y];

            var normals = ground.CreateNormalsMap();

            var signs = new Signs( 
                world * Matrix.CreateTranslation(-64, -0.1f, -64),
                VisionContent.Load<Texture2D>("textures/woodensign"),
                Classes.Values.ToList(),
                16,
                4);
            Children.Add(signs);

            var ms = new MicrosoftBillboards(world * Matrix.CreateTranslation(-64, 0.05f, -64));
            var grass = new List<Tuple<Vector3, Vector3>>();
            foreach ( var vc in Classes.Values )
            {
                for ( var i = (vc.CyclomaticComplexity-1)*2; i>0 ; i--)
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

            initialize(ground, ground.CreateWeigthsMap(new[] { 0, 0.5f, 0.95f, 1 }), normals);
        }

        private IEnumerable<Point> positionDispatcher(int surfaceSide)
        {
            var x = ClassSide;
            var y = ClassSide;
            for (var i = 3; i < surfaceSide; i++)
                yield return new Point(x += ClassSide, y);
            for (var j = 3; j < surfaceSide; j++)
            {
                y += ClassSide;
                x = 0;
                for (var i = 1; i < surfaceSide; i++)
                    yield return new Point(x += ClassSide, y);
            }
            y += ClassSide;
            x = ClassSide;
            for (var i = 3; i < surfaceSide*surfaceSide; i++)
                yield return new Point(x += ClassSide, y);
        }
    }

}
