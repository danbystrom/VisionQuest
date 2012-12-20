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
            var signList = new List<Vector3>();
            foreach (var vc in vassembly.VClasses)
            {
                var z = new VisualClass(vc, x, y);
                Classes.Add(vc.TypeDefinition.Name, z);
                ground.AlterValues(x-8, y-8, 16, 16, h => h + 2 + (float) Math.Pow(z.Instructions, 0.4));
                signList.Add(new Vector3(x, ground[x, y], y));
                x += 16;
                if ( x >= side )
                {
                    x = 16;
                    y += 16;
                }
            }
            var rnd = new Random();
            ground.AlterValues(h => h > 1 ? h + (float)rnd.NextDouble() : 0);
            ground.Soften();
            ground.AlterValues(h => h > 1 ? h + (float)rnd.NextDouble() : 0);
            ground.Soften();
            ground.Soften();

            var normals = ground.CreateNormalsMap();

            var reimersBillboards = new ReimersBillboards(
                world * Matrix.CreateTranslation(-64, -0.1f, -64),
                VisionContent.Load<Texture2D>("textures/woodensign"));
            reimersBillboards.CreateBillboardVerticesFromList(signList);
            Children.Add(reimersBillboards);

            initialize(ground, ground.CreateWeigthsMap(), normals);
        }

    }

}
