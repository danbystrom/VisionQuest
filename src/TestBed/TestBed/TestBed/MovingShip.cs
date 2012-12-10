﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing;
using factor10.VisionThing.Effects;

namespace TestBed
{
    class MovingShip : ClipDrawable
    {
        private readonly ShipModel _shipModel;

        private float _radius = 500;
        private double _angle;

        private Matrix _world;

        public MovingShip(ShipModel shipModel)
            : base(shipModel.Effect)
        {
            _shipModel = shipModel;
        }

        public override void Update(GameTime gameTime)
        {
            _shipModel.Update(gameTime);
            _angle += gameTime.ElapsedGameTime.TotalSeconds / 100;
            _world = Matrix.CreateScale(0.8f) * Matrix.CreateTranslation(_radius, 1f, 0) * Matrix.CreateRotationY((float)_angle);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _shipModel.World = _world;
            return _shipModel.Draw(camera, drawingReason, shadowMap);
        }

        public override void Draw(
            Vector4? clipPlane,
            Camera camera,
            ShadowMap shadowMap = null)
        {
            _shipModel.World = _world;
            _shipModel.Draw(clipPlane, camera, shadowMap);
        }

    }

}
