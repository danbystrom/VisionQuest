using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing
{
    public class GeneratedTerrain : TerrainBase
    {

        public GeneratedTerrain(
            Matrix world,
            Texture2D heightsMap)
            : base(
                createGround(heightsMap),
                world,
                VisionContent.Load<Texture2D>("TerrainTextures/texBase"),
                VisionContent.Load<Texture2D>("TerrainTextures/texR"),
                VisionContent.Load<Texture2D>("TerrainTextures/texG"),
                VisionContent.Load<Texture2D>("TerrainTextures/texB"))
        {
        }

        private static Ground createGround(Texture2D heightsMap)
        {
            var ground = new Ground(heightsMap); // Ground.CreateDoubleSizeMirrored(heightsMap));

                //var rnd = new Random();
                //ground.AlterValues(() => 5+5*(float)rnd.NextDouble());
                //ground.FlattenRectangle(42, 42, 32);

            ground.ApplyNormalBellShape();
            for (var i = 0; i < 5; i++)
            {
                for (var x = 2; x < 64; x++)
                {
                    ground[x - 1, x] = 0;
                    ground[x, x - 1] = 0;
                    ground[x - 2, x] = 0;
                    ground[x, x - 2] = 0;
                }
                for (var x = 0; x < 128; x++)
                {
                    ground[x, 0] = 0;
                    ground[x, 127] = 0;
                    ground[0, x] = 0;
                    ground[127, x] = 0;
                    ground[x, x] = 0;
                }
                ground.Soften();
            }
            ground.FlattenRectangle(10, 10, 20);

            return ground;
        }

    }

}
