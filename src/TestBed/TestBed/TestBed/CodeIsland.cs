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
        public readonly VAssembly VAssembly;
        public readonly Dictionary<string, VisualClass> Classes = new Dictionary<string, VisualClass>();

        public CodeIsland(
            Matrix world,
            VAssembly vassembly)
        {
            World = world;
            VAssembly = vassembly;

            var side = 16 * (int)Math.Ceiling(Math.Sqrt(vassembly.VClasses.Count));
            var surfaceSide = (side + 16 + 15)/32;

            var ground = new Ground(surfaceSide*64, surfaceSide*64);
            var x = 16;
            var y = 16;
            foreach (var vc in vassembly.VClasses)
            {
                var z = new VisualClass(vc, x, y);
                ground.AlterValues(x - 8, y - 8, 16, 17, h => h + 2 + (float) Math.Pow(z.InstructionCount, 0.3));

                var height = ground[x, y];
                z.Height = height;
                Classes.Add(vc.FullName, z);
                x += 16;
                if (x >= side)
                {
                    x = 16;
                    y += 16;
                }
            }

            var rnd = new Random();
            ground.AlterValues(h => h > 1 ? h + 8*(float)rnd.NextDouble() : 0);
            ground.Soften(3);

            foreach (var vc in Classes.Values)
                vc.Height = ground[vc.X, vc.Y];

            var normals = ground.CreateNormalsMap();

            var signs = new Signs( 
                world * Matrix.CreateTranslation(-64, -0.1f, -64),
                VisionContent.Load<Texture2D>("textures/woodensign"),
                Classes.Values.ToList(),
                8,
                2);
            Children.Add(signs);

            initialize(ground, ground.CreateWeigthsMap(new[] { 0, 0.5f, 0.95f, 1 }), normals);
        }

    }

}
