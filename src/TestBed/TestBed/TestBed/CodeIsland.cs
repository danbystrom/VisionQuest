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

            var side = (ClassSide/2) * (int)Math.Ceiling(Math.Sqrt(vassembly.VClasses.Count));
            var surfaceSide = (side + ClassSide - 1) / ClassSide;
            surfaceSide *= ClassSide*2;

            var ground = new Ground(surfaceSide, surfaceSide);
            var x = ClassSide;
            var y = ClassSide;
            foreach (var vc in vassembly.VClasses)
            {
                var z = new VisualClass(vc, x, y);
                var instructHeight = 2 + (float)Math.Pow(z.InstructionCount, 0.3);
                ground.AlterValues(x - ClassSide / 2, y - ClassSide / 2, ClassSide, ClassSide, h => h + instructHeight + (4 - vc.MaintainabilityIndex / 25) * (float)rnd.NextDouble());

                var height = ground[x, y];
                z.Height = height;
                Classes.Add(vc.FullName, z);
                x += ClassSide;
                if (x + ClassSide > surfaceSide)
                {
                    x = ClassSide;
                    y += ClassSide;
                }
            }

            //ground.AlterValues(h => h > 1 ? h + 6*(float)rnd.NextDouble() : 0);
            ground.Soften(1);

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
                    var gx = vc.X + ((float) rnd.NextDouble() - 0.5f)*(ClassSide - 2);
                    var gy = vc.Y + ((float) rnd.NextDouble() - 0.5f)*(ClassSide - 2);
                    grass.Add(new Tuple<Vector3, Vector3>(
                                  new Vector3(gx, ground.GetExactHeight(gx, gy), gy),
                                  normals.AsVector3(vc.X, vc.Y)));
                }
            }
            ms.CreateBillboardVerticesFromList(grass);
            Children.Add(ms);

            initialize(ground, ground.CreateWeigthsMap(new[] { 0, 0.5f, 0.95f, 1 }), normals);
        }

    }

}
