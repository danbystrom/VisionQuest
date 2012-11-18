﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing;
using factor10.VisionThing.Effects;

namespace TestBed
{
    class Ship : ClipDrawable
    {
        private readonly ShipModel _shipModel;

        public Matrix World = Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(-7, 1f, 33);

        private DoubleSin _bob1 = new DoubleSin(0.05f, 0.010f, 0.3f, 0.9f, 0, 1);
        private DoubleSin _bob2 = new DoubleSin(0.04f, 0.008f, 0.5f, 0.8f, 2, 3);

        private readonly Texture2D _texture;

        public Ship(ShipModel shipModel)
            : base(null)
        {
            _shipModel = shipModel;
        }

        public void Update(GameTime gameTime)
        {
            _shipModel.Update(gameTime);
        }
    
        public override void Draw(Camera camera, IEffect effect)
        {
            _shipModel.World = World;
            _shipModel.Draw(camera, effect);
        }

        public override void Draw(Camera camera)
        {
            _shipModel.World = World;
            _shipModel.Draw(camera);
        }

        public override void Draw(Camera camera, Vector4? clipPlane)
        {
            _shipModel.World = World;
            _shipModel.Draw(camera, clipPlane);
        }
    }

}
