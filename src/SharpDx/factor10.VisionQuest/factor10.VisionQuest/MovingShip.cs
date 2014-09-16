using factor10.VisionQuest;
using factor10.VisionThing;
using SharpDX;
using SharpDX.Toolkit;

namespace TestBed
{
    class MovingShip : ClipDrawable
    {
        private readonly ShipModel _shipModel;

        private float _radius = 400;
        private double _angle;

        private Matrix _world;

        public MovingShip(ShipModel shipModel)
            : base(shipModel.Effect)
        {
            _shipModel = shipModel;
            Children.Add(_shipModel);
        }

        public override void Update(GameTime gameTime)
        {
            _shipModel.Update(gameTime);
            _angle += gameTime.ElapsedGameTime.TotalSeconds / 100;
            _world = Matrix.Scaling(0.8f) * Matrix.Translation(_radius, 1f, 0) * Matrix.RotationY((float)_angle);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _shipModel.World = _world;
            return true;
        }

    }

}
