using System;
using factor10.VisionThing.Terrain;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionThing
{
    public class GeneratedTerrain : TerrainBase
    {

        public GeneratedTerrain(
            VisionContent vContent,
            Matrix world,
            Texture2D heightsMap)
            : base(vContent)
        {
            World = world;
            var ground = new Ground(heightsMap, h => (255 - h)/15f);
            ground.AlterValues(h => h*3 + 2);
            ground.ApplyNormalBellShape();

            ground.DrawLine(5, 5, 200, 100, 5, (t, o) => 0);
            ground.DrawLine(200, 100, 250, 300, 5, (t, o) => 0);
            ground.DrawLine(250, 300, 500, 350, 5, (t, o) => 0);
            ground.Soften();

            var weights = ground.CreateWeigthsMap();
            weights.DrawLine(15, 500, 500, 15, 5,
                (c, o) =>
                {
                    var factor = (1 + o)/4f;
                    c.A *= factor;
                    c.B *= factor;
                    c.C *= factor;
                    c.D *= factor;
                    c.E = 1 - c.A - c.B - c.C - c.D;
                    return c;
                },
                200,
                new Random());
            //weights.AlterValues(c => new Color(255,0,0,0));

            initialize(ground, weights, ground.CreateNormalsMap());
        }

    }

}

