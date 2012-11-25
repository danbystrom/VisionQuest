using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing;
using factor10.VisionThing.Effects;

namespace TestBed
{
    class MovingShip : ClipDrawable
    {
        private readonly ShipModel _shipModel;

        private float _radius = 400;
        private double _angle;

        private Matrix _world;

        private DoubleSin _bob1 = new DoubleSin(0.05f, 0.010f, 0.3f, 0.9f, 0, 1);
        private DoubleSin _bob2 = new DoubleSin(0.04f, 0.008f, 0.5f, 0.8f, 2, 3);

        private readonly Texture2D _texture;

        public MovingShip(ShipModel shipModel)
            : base(null)
        {
            _shipModel = shipModel;
        }

        public void Update(GameTime gameTime)
        {
            _shipModel.Update(gameTime);
            _angle += gameTime.ElapsedGameTime.TotalSeconds / 100;
            var sin = (float) Math.Sin(_angle);
            var cos = (float) Math.Cos(_angle);
            _world = Matrix.CreateRotationY(-cos)*Matrix.CreateScale(0.8f)*Matrix.CreateTranslation(_radius*sin, 1f, _radius*cos);
        }

        protected override void draw(Camera camera, DrawingReason drawingReason, IEffect effect, ShadowMap shadowMap)
        {
            _shipModel.World = _world;
            _shipModel.Draw(camera, drawingReason, effect, shadowMap);
        }

        public override void Draw(
            Vector4? clipPlane,
            Camera camera,
            DrawingReason drawingReason = DrawingReason.Normal,
            IEffect effect = null,
            ShadowMap shadowMap = null)
        {
            _shipModel.World = _world;
            _shipModel.Draw(clipPlane, camera, drawingReason, effect, shadowMap);
        }

    }

}
