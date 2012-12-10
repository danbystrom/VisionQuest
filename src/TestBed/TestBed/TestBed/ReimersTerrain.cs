using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestBed;
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
            Matrix world)
        {
            World = world;

            _windmill = new Windmill(world.Translation + new Vector3(-53, 3.5f, 15));
            _bridge = new Box(world*Matrix.CreateTranslation(-70, 2, 15), new Vector3(20, 2, 6), 0.1f);
            _ship1 = new Ship(new ShipModel())
                         {
                             World =
                                 Matrix.CreateRotationY(MathHelper.Pi)*Matrix.CreateScale(0.25f)*world*
                                 Matrix.CreateTranslation(-70, 2, 5)
                         };
            _ship2 = new Ship(new ShipModel())
                         {
                             World =
                                 Matrix.CreateRotationY(MathHelper.Pi)*Matrix.CreateScale(0.25f)*world*
                                 Matrix.CreateTranslation(-79, 2, 5)
                         };
            _ship2.Update(new GameTime(new TimeSpan(0, 0, 0, 5), new TimeSpan(0, 0, 0, 5)));


            Children.Add(_windmill);
            Children.Add(_ship1);
            Children.Add(_ship2);
            Children.Add(_bridge);

            var ground = createGround();
            _reimersBillboards = new ReimersBillboards(
                world*Matrix.CreateTranslation(-64, -0.1f, -64),
                ground,
                ground.CreateNormalsMap());
            Children.Add(_reimersBillboards);

            initialize(ground);
        }

        private static Ground createGround()
        {
            var ground = new Ground(VisionContent.Load<Texture2D>("heightmap"), h => h/10f);
            ground.LowerEdges();
            return ground;
        }

    }

}
