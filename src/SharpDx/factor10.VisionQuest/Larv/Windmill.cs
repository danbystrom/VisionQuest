using System.Linq;
using factor10.VisionThing;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Larv
{
    class Windmill : ClipDrawable
    {
        private readonly Model _model;
        private readonly Matrix[] _bones;

        public Matrix World;

        private readonly Texture2D _texture;
        private readonly Texture2D _bumpMap;

        private readonly ModelBone _animatedBone;
        private readonly Matrix _originalBoneTransformation;

        private float _angle;

        public Windmill(VisionContent vContent, Vector3 location)
            : base(vContent.LoadPlainEffect("effects/SimpleTextureEffect"))
        {
            World = Matrix.Scaling(0.004f)*Matrix.RotationY(0.4f)*Matrix.Translation(-1, 0, -1);
            _model = vContent.Load<Model>("models/windmillf");
            _texture = vContent.Load<Texture2D>("models/windmill_diffuse");
            //_bumpMap = vContent.Load<Texture2D>("textures/windmill_normal");
            _bones = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(_bones);

            _animatedBone = _model.Meshes.Single(mesh => mesh.Name == "Fan").ParentBone;
            _originalBoneTransformation = Matrix.Translation(0, 850, 0);
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            _angle += (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (_angle > MathUtil.TwoPi)
                _angle -= MathUtil.TwoPi;
            _animatedBone.Transform =
                Matrix.RotationZ(_angle)*
                _originalBoneTransformation;
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);

            if (drawingReason != DrawingReason.ShadowDepthMap)
            {
                Effect.Texture = _texture;
                //Effect.Parameters["BumpMap"].SetResource(_bumpMap);
            }

            _model.Draw(Effect.GraphicsDevice, World, camera.View, camera.Projection, Effect.Effect);

            return true;
        }
        
    }

}
