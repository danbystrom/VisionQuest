using factor10.VisionThing;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Larv
{
    class CaveModel : ClipDrawable
    {
        private readonly Model _caveModel;
        private readonly Model _gratingModel;

        public Matrix PlayerCaveWorld;

        private readonly Texture2D _texture;
        private readonly Texture2D _bumpMap;

        private float _angle;

        public CaveModel(VisionContent vContent, Vector3 location)
            : base(vContent.LoadEffect("effects/SimpleBumpEffect"))
        {
            PlayerCaveWorld = Matrix.Scaling(0.7f, 0.7f, 0.5f)*Matrix.RotationY(MathUtil.PiOverTwo)*Matrix.Translation(23.3f, 0.3f, 15);
            _caveModel = vContent.Load<Model>("models/cave");
            _gratingModel = vContent.Load<Model>("models/grating");
            _texture = vContent.Load<Texture2D>("textures/cave");
            _bumpMap = vContent.Load<Texture2D>("textures/rocknormal");
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);
            Effect.Texture = _texture;
            Effect.Parameters["BumpMap"].SetResource(_bumpMap);
            _caveModel.Draw(Effect.GraphicsDevice, PlayerCaveWorld, camera.View, camera.Projection, Effect.Effect);
            //_gratingModel.Draw(Effect.GraphicsDevice, Matrix.Scaling(4) * PlayerCaveWorld, camera.View, camera.Projection, Effect.Effect);
            return true;
        }
        
    }

}
