using System;
using factor10.VisionQuest;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using TestBed;
using factor10.VisionThing.Objects;
using factor10.VisionThing.Terrain;

namespace factor10.VisionThing
{
    public class ReimersTerrain : TerrainBase
    {
        private readonly Windmill _windmill;
        private readonly Ship _ship1, _ship2;
        private readonly Box _bridge;
        private readonly ReimersBillboards _reimersBillboards;

        public ReimersTerrain(
            VisionContent vContent,
            Matrix world)
            : base(vContent)
        {
            World = world;

            _windmill = new Windmill(vContent, world.TranslationVector + new Vector3(-53, 3.5f, 15));
            _bridge = new Box(vContent, world*Matrix.Translation(-70, 2, 15), new Vector3(20, 2, 6), 0.1f);
            _ship1 = new Ship(new ShipModel(vContent))
            {
                World =
                    Matrix.RotationY(MathUtil.Pi)*Matrix.Scaling(0.25f)*world*
                    Matrix.Translation(-70, 2, 5)
            };
            _ship2 = new Ship(new ShipModel(vContent))
            {
                World =
                    Matrix.RotationY(MathUtil.Pi)*Matrix.Scaling(0.25f)*world*
                    Matrix.Translation(-79, 2, 5)
            };
            _ship2.Update(null, new GameTime(new TimeSpan(0, 0, 0, 5), new TimeSpan(0, 0, 0, 5)));


            Children.Add(_windmill);
            Children.Add(_ship1);
            Children.Add(_ship2);
            Children.Add(_bridge);

            var ground = createGround(vContent);
            _reimersBillboards = new ReimersBillboards(
                vContent,
                world*Matrix.Translation(-64, -0.1f, -64),
                ground,
                ground.CreateNormalsMap(),
                vContent.Load<Texture2D>("tree"));
            Children.Add(_reimersBillboards);

            initialize(ground);
        }

        private Ground createGround(VisionContent vContent)
        {
            var ground = new Ground(vContent.Load<Texture2D>("heightmap"), h => h/10f);
            ground.LowerEdges();
            return ground;
        }

    }

}
