using System.Windows.Forms;
using factor10.VisionThing;
using factor10.VisionThing.Terrain;
using SharpDX;

namespace NextGame
{
    public class Gq : TerrainBase
    {
        public Gq(VisionContent vContent)
            : base(vContent)
        {
            World = Matrix.Translation(-150, -1.1f, -150);

            var ground = new Ground(256, 256);

            ground.AlterValues(100, 200, 10, 10, (x, y, h) => 20);
            ground.AlterValues(100, 210, 10, 10, (x, y, h) => 30);
            ground.AlterValues(100, 220, 10, 10, (x, y, h) => 40);

            ground.DrawLine(2, 2, 253, 2, 1, (a, b) => 100);
            ground.DrawLine(2, 2, 2, 253, 1, (a, b) => 100);
            ground.DrawLine(253, 253, 253, 1, 1, (a, b) => 100);
            ground.DrawLine(253, 253, 2, 253, 1, (a, b) => 100);

            ground.DrawLine(10, 10, 100, 100, 2, (a, b) => 1);
            ground.Soften();
            ground.Soften();

            var weights = ground.CreateWeigthsMap(new[] {0, 0.40f, 0.60f, 0.9f});

            weights.AlterValues(20, 20, 20, 20, (x, y, mt) => new Mt9Surface.Mt8 {B = 1});
            weights.AlterValues(30, 30, 20, 20, (x, y, mt) => new Mt9Surface.Mt8 {C = 1});
            weights.AlterValues(40, 40, 20, 20, (x, y, mt) => new Mt9Surface.Mt8 {D = 1});
            weights.AlterValues(50, 50, 20, 20, (x, y, mt) => new Mt9Surface.Mt8 {E = 1});
            weights.AlterValues(60, 60, 20, 20, (x, y, mt) => new Mt9Surface.Mt8 {F = 1});
            weights.AlterValues(70, 70, 20, 20, (x, y, mt) => new Mt9Surface.Mt8 {G = 1});

            weights.DrawLine(10, 10, 100, 100, 4, (a, b) => new Mt9Surface.Mt8 {F = 1});

            initialize(ground, weights, ground.CreateNormalsMap());
        }

    }

}
