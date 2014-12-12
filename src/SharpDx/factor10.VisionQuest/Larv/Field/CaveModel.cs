using factor10.VisionThing;
using Larv.Util;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace Larv.Field
{
    public class CaveModel : ClipDrawable
    {
        private readonly Model _caveModel;
        private readonly Model _gratingModel;

        public Matrix CaveWorld;
        public Matrix GratingWorld;

        private readonly Texture2D _texture;
        private readonly Texture2D _bumpMap;
        private readonly Texture2D _gratingTexture;
        private float _angle;
        private Matrix _translation;

        public CaveModel(LContent lcontent)
            : base(lcontent.LoadEffect("effects/SimpleBumpEffect"))
        {
            _caveModel = lcontent.Load<Model>("models/cave");
            _texture = lcontent.Load<Texture2D>("textures/cave");
            _bumpMap = lcontent.Load<Texture2D>("textures/rocknormal");
            _gratingModel = lcontent.Load<Model>("models/grating");
            _gratingTexture = lcontent.Load<Texture2D>("textures/black");
        }

        public void SetPosition(Whereabouts whereabouts, PlayingField playingField)
        {
            SetPosition(whereabouts.GetPosition(playingField), whereabouts.Direction);
        }

        public void SetPosition(Vector3 position, Direction direction)
        {
            System.Diagnostics.Debug.Print("{0}", position);
            CaveWorld = Matrix.Scaling(0.9f, 0.7f, 0.5f)
                        *Matrix.Translation(5, 0.3f, -0.5f)
                        *Matrix.RotationY(MathUtil.PiOverTwo)
                        *Matrix.Translation(position);
            GratingWorld = Matrix.RotationY(MathUtil.PiOverFour)
                           *Matrix.Scaling(0.5f, 0.7f, 0.5f)
                //*Matrix.Translation(5, 0.3f, -0.5f)
                           *Matrix.Translation(position);
            _translation = 
                           Matrix.Translation(0.5f, 0.3f, -5)
                           *Matrix.Translation(position);
        }

        private float _x = 0.3f;
        private float _z = -0.3f;

        public override void Update(Camera camera, GameTime gameTime)
        {
            var dx = camera.KeyboardState.IsKeyPressed(Keys.X) ? 0.02f : 0;
            var dz = camera.KeyboardState.IsKeyPressed(Keys.Z) ? 0.02f : 0;
            if (camera.KeyboardState.IsKeyDown(Keys.Shift))
            {
                dx = -dx;
                dz = -dz;
            }
            _x += dx;
            _z += dz;
            _angle += (float) gameTime.ElapsedGameTime.TotalSeconds;
            GratingWorld = Matrix.Translation(_x, 0, _z)*Matrix.Scaling(0.5f, 0.7f, 0.4f)*Matrix.RotationY(_angle)*_translation;
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);
            Effect.DiffuseColor = new Vector4(0.6f, 0.6f, 0.6f, 1);
            Effect.Texture = _texture;
            Effect.Parameters["BumpMap"].SetResource(_bumpMap);
            _caveModel.Draw(Effect.GraphicsDevice, CaveWorld, camera.View, camera.Projection, Effect.Effect);
            Effect.Texture = _gratingTexture;
            _gratingModel.Draw(Effect.GraphicsDevice, GratingWorld, camera.View, camera.Projection, Effect.Effect);
            return true;
        }
        
    }

}
