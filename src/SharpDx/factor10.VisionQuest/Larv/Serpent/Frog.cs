using System.CodeDom;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using Serpent;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System;
using SharpDX.Toolkit.Input;

namespace Larv.Serpent
{
    public class Frog : ClipDrawable
    {
        private Model _model;
        private readonly IVDrawable _sphere;
        private readonly Texture2D _texture;

        public readonly Whereabouts Whereabouts;

        private readonly Ground _ground;
        private float _onTheMove;

        private Vector3 _fromPosition;
        private Vector3 _toPosition;

        private Vector3 _position;

        public Frog(
            VisionContent vContent,
            IVEffect effect,
            IVDrawable sphere,
            Texture2D texture,
            Whereabouts whereabouts,
            Ground ground)
            : base(effect)
        {
            _model = vContent.Load<Model>(@"Models/frog");

            _sphere = sphere;
            _texture = vContent.Load<Texture2D>(@"terraintextures/sand");
            Whereabouts = whereabouts;
            _ground = ground;
            _fromPosition = _toPosition = _position = Vector3.One;
            _onTheMove = -5;
        }

        public Vector3 Position
        {
            get { return _position; }    
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            base.Update(camera, gameTime);

            _onTheMove += (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (_onTheMove < 0 && !camera.KeyboardState.IsKeyPressed(Keys.J))
                return;

            if (_onTheMove > 0)
            {
                var factor = _onTheMove/0.1f;
                _position = Vector3.Lerp(_fromPosition, _toPosition, factor);
                if (factor < 1)
                    return;
            }

            _fromPosition = _position;

            var winv = _ground.World;
            winv.Invert();
            Vector3 gspaceCurrent;
            Vector3.TransformCoordinate(ref _position, ref winv, out gspaceCurrent);

            var rnd = new Random();
            for (var i = 0; i < 100; i++)
            {
                var dx = (float) rnd.NextDouble()*3 - 1.5f;
                var dz = (float) rnd.NextDouble()*3 - 1.5f;
                dx += Math.Sign(dx)*0.5f;
                dz += Math.Sign(dz)*0.5f;
                _toPosition.X = _position.X + dx;
                _toPosition.Z = _position.Z + dz;
                if (_toPosition.X < -5 || _toPosition.Y < -5)
                    continue;

                Vector3 gspaceTo;
                Vector3.TransformCoordinate(ref _toPosition, ref winv, out gspaceTo);
                var normal = _ground.GroundMap.GetNormal((int) gspaceTo.X, (int) gspaceTo.Z);
                if (normal.Y < 0.5f)
                    continue;

                gspaceTo.Y = _ground.GroundMap.GetExactHeight(gspaceTo.X, gspaceTo.Z);
                gspaceTo = getApproxMax(gspaceTo, gspaceCurrent);
                normal = _ground.GroundMap.GetNormal((int)gspaceTo.X, (int)gspaceTo.Z);
                if (normal.Y < 0.5f)
                    continue;

                Vector3.TransformCoordinate(ref gspaceTo, ref _ground.World, out _toPosition);

                _toPosition.Y = Math.Max(0.5f, _toPosition.Y);

                _onTheMove = camera.KeyboardState.IsKeyPressed(Keys.J) ? 0 : - 1000; //rnd.NextFloat(-8, -2);
                return;
            }

            //failed to find a new spot
            _toPosition = _position;
        }

        private Vector3 getApproxMax(Vector3 vNew, Vector3 vOld)
        {
            const int iterations = 10;
            var result = vNew;
            for (var i = 1; i < iterations; i++)
            {
                var vMiddle = Vector3.Lerp(vNew, vOld, (float) i/iterations);
                var height = _ground.GroundMap.GetExactHeight(vMiddle.X, vMiddle.Z);
                if (height > vMiddle.Y)
                    return new Vector3(vMiddle.X, height, vMiddle.Z);
            }
            return result;
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);
            //var t = direction.IsNorthSouth
            //    ? Matrix.Scaling(0.6f, 0.6f, 0.8f)
            //    : Matrix.Scaling(0.8f, 0.6f, 0.6f);
            //var off = direction.DirectionAsVector3()*-0.3f;
            //t.TranslationVector = _position;
            var t = Matrix.RotationX(MathUtil.Pi) * Matrix.Scaling(0.08f) * Matrix.Translation(_position + new Vector3(0, 0.08f, 0));

            Effect.World = t;
            Effect.Texture = _texture;
            Effect.DiffuseColor = Vector4.One;
            _model.Draw(Effect.GraphicsDevice, t, camera.View, camera.Projection, Effect.Effect);

            Effect.World = Matrix.Scaling(0.2f) * Matrix.Translation(_position + new Vector3(0, 0.2f, 0));
            _sphere.Draw(Effect);

            return true;
        }

    }

}
