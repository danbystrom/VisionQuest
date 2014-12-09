using factor10.VisionThing;
using Larv.Util;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Larv
{
    public class CaveModel : ClipDrawable
    {
        private readonly Model _caveModel;
        private readonly Model _gratingModel;

        public Matrix World;

        private readonly Texture2D _texture;
        private readonly Texture2D _bumpMap;

        public CaveModel(VisionContent vContent)
            : base(vContent.LoadEffect("effects/SimpleBumpEffect"))
        {
            _caveModel = vContent.Load<Model>("models/cave");
            _gratingModel = vContent.Load<Model>("models/grating");
            _texture = vContent.Load<Texture2D>("textures/cave");
            _bumpMap = vContent.Load<Texture2D>("textures/rocknormal");
        }

        public void SetPosition(Whereabouts whereabouts, PlayingField playingField)
        {
            SetPosition(whereabouts.GetPosition(playingField), whereabouts.Direction);
        }

        public void SetPosition(Vector3 position, Direction direction)
        {
            System.Diagnostics.Debug.Print("{0}", position);
            World = Matrix.Scaling(0.9f, 0.7f, 0.5f)
                    *Matrix.Translation(5, 0.3f, -0.7f)
                    *Matrix.RotationY(MathUtil.PiOverTwo)
                    *Matrix.Translation(position);
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);
            Effect.DiffuseColor = new Vector4(0.6f, 0.6f, 0.6f, 1);
            Effect.Texture = _texture;
            Effect.Parameters["BumpMap"].SetResource(_bumpMap);
            _caveModel.Draw(Effect.GraphicsDevice, World, camera.View, camera.Projection, Effect.Effect);
            //_gratingModel.Draw(Effect.GraphicsDevice, Matrix.Scaling(4) * World, camera.View, camera.Projection, Effect.Effect);
            return true;
        }
        
    }

}
