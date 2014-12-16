using factor10.VisionQuest.Decorations;
using factor10.VisionThing;
using factor10.VisionThing.CameraStuff;
using SharpDX;
using SharpDX.Toolkit;

namespace factor10.VisionQuest
{
    class MovingShip : ClipDrawable
    {
        public readonly ShipModel _shipModel;

        private const float _radius = 400;
        private double _angle;

        private Matrix _world;

        public MovingShip(ShipModel shipModel)
            : base(shipModel.Effect)
        {
            _shipModel = shipModel;
            Children.Add(_shipModel);
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            _shipModel.Update(camera, gameTime);
            _angle += gameTime.ElapsedGameTime.TotalSeconds / 100;
            _world = Matrix.Scaling(0.8f) * Matrix.RotationY(MathUtil.Pi) * Matrix.Translation(_radius, 1f, 0) * Matrix.RotationY((float)_angle);
            _shipModel.World = _world;
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            return true;
        }

    }

}
