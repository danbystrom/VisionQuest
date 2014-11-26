using System.Linq;
using factor10.VisionThing;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Larv
{
    class CaveModel : ClipDrawable
    {
        private readonly Model _model;

        public Matrix World;

        private readonly Texture2D _texture;
        private readonly Texture2D _bumpMap;

        private float _angle;

        public CaveModel(VisionContent vContent, Vector3 location)
            : base(vContent.LoadPlainEffect("effects/SimpleTextureEffect"))
        {
            World = Matrix.Scaling(0.5f, 0.4f, 0.3f)*Matrix.RotationY(MathUtil.PiOverTwo)*Matrix.RotationX(MathUtil.Pi)*Matrix.Translation(24, 0.3f, 15);
            _model = vContent.Load<Model>("models/mycave");
            _texture = vContent.Load<Texture2D>("textures/bigstone");
            //_bumpMap = vContent.Load<Texture2D>("textures/windmill_normal");
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);
            Effect.Texture = _texture;
            _model.Draw(Effect.GraphicsDevice, World, camera.View, camera.Projection, Effect.Effect);
            return true;
        }
        
    }

}
