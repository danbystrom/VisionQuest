using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing.Terrain;

namespace factor10.VisionThing
{
    public class GeneratedTerrain : TerrainBase
    {

        public GeneratedTerrain(
            Matrix world,
            Texture2D heightsMap)
        {
            World = world;
            var ground = new Ground(heightsMap, h => (255 - h)/15f);
            ground.AlterValues(h => h*3 + 2);
            ground.ApplyNormalBellShape();

            ground.DrawLine(5, 5, 200, 100, 5, t => 0);
            ground.DrawLine(200, 100, 250, 300, 5, t => 0);
            ground.DrawLine(250, 300, 500, 350, 5, t => 0);
            ground.Soften();

            var weights = ground.CreateWeigthsMap();
            weights.DrawLine(15, 500, 500, 15, 5,
                             c =>
                                 {
                                     c.R /= 2;
                                     c.G /= 2;
                                     c.B /= 2;
                                     c.A = (byte)(255 - c.R - c.G - c.B);
                                     return c;
                                 },
                                 200,
                                 new Random());
            //weights.AlterValues(c => new Color(255,0,0,0));

            initialize(ground, weights, ground.CreateNormalsMap());
        }

    }

}

