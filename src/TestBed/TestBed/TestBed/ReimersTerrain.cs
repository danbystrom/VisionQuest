using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestBed;

namespace factor10.VisionThing
{
    public class ReimersTerrain : TerrainBase
    {
        private readonly Windmill _windmill;
        private readonly Ship _ship1, _ship2;
        private readonly Box _bridge;

        public ReimersTerrain(
            Matrix world)
            : base(
            createGround(),
            world)
        {
            _windmill = new Windmill(world.Translation + new Vector3(-53,3.5f,15));
            _bridge = new Box(world * Matrix.CreateTranslation(-65, 2, 15), new Vector3(40, 2, 6),0.1f);
            _ship1 = new Ship(new ShipModel());
            _ship1.World = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateScale(0.25f) * world * Matrix.CreateTranslation(-70, 2, 5);
            _ship2 = new Ship(new ShipModel());
            _ship2.World = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateScale(0.25f) * world * Matrix.CreateTranslation(-79, 2, 5);
            _ship2.Update(new GameTime(new TimeSpan(0, 0, 0, 5), new TimeSpan(0, 0, 0, 5)));

            Children.Add(_windmill);
            Children.Add(_ship1);
            Children.Add(_ship2);
        }

        private static Ground createGround()
        {
            var ground = new Ground(VisionContent.Load<Texture2D>("heightmap"));
            ground.LowerEdges();
            return ground;
        }

        protected override void draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _windmill.Draw(camera, drawingReason, shadowMap);
            _ship1.Draw(camera, drawingReason, shadowMap);
            _ship2.Draw(camera, drawingReason, shadowMap);
            _bridge.Draw(camera, drawingReason, shadowMap);
            base.draw(camera, drawingReason, shadowMap);
        }

    }

}
