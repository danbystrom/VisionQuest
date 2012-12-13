using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing.Terrain;

namespace factor10.VisionThing
{
    public class LargeTerrain : TerrainBase
    {

        public LargeTerrain(
            Matrix world,
            Texture2D heightsMap)
        {
            World = world;
            var ground = new Ground(heightsMap, h => (255 - h) / 15f);
            ground.AlterValues(h => h + 4);
            ground.ApplyNormalBellShape();
            var normals = ground.CreateNormalsMap();

            var ms = new MicrosoftBillboards(world * Matrix.CreateTranslation(-64, 0.05f, -64), ground, normals);
            Children.Add(ms);

            initialize(ground, ground.CreateWeigthsMap(), normals);
        }

    }

}
