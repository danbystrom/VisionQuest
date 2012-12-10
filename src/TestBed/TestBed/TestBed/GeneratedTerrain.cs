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
            var ground = new Ground(heightsMap, h => (255-h)/15f);
            ground.AlterValues(h => h + 1);
            //ground.ApplyNormalBellShape();
            //for (var i = 0; i < 5; i++)
            //{
            //    for (var x = 2; x < 64; x++)
            //    {
            //        ground[x - 1, x] = 0;
            //        ground[x, x - 1] = 0;
            //        ground[x - 2, x] = 0;
            //        ground[x, x - 2] = 0;
            //    }
            //    for (var x = 0; x < 128; x++)
            //    {
            //        ground[x, 0] = 0;
            //        ground[x, 127] = 0;
            //        ground[0, x] = 0;
            //        ground[127, x] = 0;
            //        ground[x, x] = 0;
            //    }
            //    ground.Soften();
            //}
            //ground.FlattenRectangle(10, 10, 20);

            initialize(ground);
        }

    }

}
