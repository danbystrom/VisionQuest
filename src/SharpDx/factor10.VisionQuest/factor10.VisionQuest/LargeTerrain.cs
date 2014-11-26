using factor10.VisionThing.Terrain;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionThing
{
    public class LargeTerrain : TerrainBase
    {

        public LargeTerrain(
            VisionContent vContent,
            Matrix world,
            Texture2D heightsMap)
            : base(vContent)
        {
            World = world;
            var ground = new GroundMap(heightsMap, h => (255 - h)/15f);
            ground.AlterValues(h => h + 4);
            ground.ApplyNormalBellShape();
            var normals = ground.CreateNormalsMap(ref world);

            var ms = new CxBillboard(vContent, world*Matrix.Translation(-64, 0.05f, -64), vContent.Load<Texture2D>("textures/grass"), 1, 1);
            ms.GenerateTreePositions(ground, normals);
            Children.Add(ms);

            initialize(ground, ground.CreateWeigthsMap(), normals);
        }

    }

}
