using factor10.VisionThing;
using SharpDX;
using SharpDX.Toolkit;
using TestBed;

namespace factor10.VisionQuest
{
    class Ship : ClipDrawable
    {
        private readonly ShipModel _shipModel;

        public Matrix World = Matrix.Scaling(0.5f) * Matrix.Translation(-7, 1f, 33);

        private DoubleSin _bob1 = new DoubleSin(0.05f, 0.010f, 0.3f, 0.9f, 0, 1);
        private DoubleSin _bob2 = new DoubleSin(0.04f, 0.008f, 0.5f, 0.8f, 2, 3);

        public Ship(ShipModel shipModel)
            : base(shipModel.Effect)
        {
            _shipModel = shipModel;
            Children.Add(_shipModel);
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            _shipModel.Update(camera, gameTime);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
//            _shipModel.World = ;
//            _shipModel.World = World;
            return true;
        }

    }

}
