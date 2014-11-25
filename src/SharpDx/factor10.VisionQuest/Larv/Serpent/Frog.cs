using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Model _model;
        private readonly Matrix _modelRotation;
        private readonly Texture2D _texture;

        public readonly Whereabouts Whereabouts;

        private readonly Ground _ground;
        private float _onTheMove;

        private Vector3 _position;
        private Matrix _rotation;
        private float _currentAngle;

        private readonly List<Func<bool>> _actions = new List<Func<bool>>();
 
        public Frog(
            VisionContent vContent,
            IVEffect effect,
            Whereabouts whereabouts,
            Ground ground)
            : base(effect)
        {
            _model = vContent.Load<Model>(@"Models/frog");
            _modelRotation = Matrix.RotationX(MathUtil.Pi) * Matrix.RotationY(MathUtil.Pi) * Matrix.Scaling(0.1f);

            _texture = vContent.Load<Texture2D>(@"terraintextures/sand");
            Whereabouts = whereabouts;
            _ground = ground;
            _position = new Vector3(10, 0, 10);
            _actions.Add(() => _onTheMove > 1);
        }

        public Vector3 Position
        {
            get { return _position; }    
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            base.Update(camera, gameTime);

            if (camera.KeyboardState.IsKeyPressed(Keys.J))
                _onTheMove = 100;

            _onTheMove += (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (_actions[0]())
            {
                _onTheMove = 0;
                _actions.RemoveAt(0);
            }
            if (_actions.Any())
                return;

            // out of actions - create new commands

            var fromPosition = _position;

            var winv = _ground.World;
            winv.Invert();
            Vector3 gspaceCurrent;
            Vector3.TransformCoordinate(ref _position, ref winv, out gspaceCurrent);

            var rnd = new Random();
            for (var i = 0; i < 100; i++)
            {
                var dx = (float) rnd.NextDouble()*3 - 1.5f;
                var dz = (float) rnd.NextDouble()*3 - 1.5f;
                var toPosition = Position + new Vector3(dx, 0, dz);

                Vector3 gspaceTo;
                Vector3.TransformCoordinate(ref toPosition, ref winv, out gspaceTo);
                var normal = _ground.GroundMap.GetNormal((int) gspaceTo.X, (int) gspaceTo.Z);
                if (normal.Y < 0.5f)
                    continue;

                gspaceTo.Y = _ground.GroundMap.GetExactHeight(gspaceTo.X, gspaceTo.Z);
                gspaceTo = getApproxMax(gspaceTo, gspaceCurrent);
                normal = _ground.GroundMap.GetNormal((int)gspaceTo.X, (int)gspaceTo.Z);
                if (normal.Y < 0.5f)
                    continue;

                // now we have a new position for the frog - set up jump commands
                Vector3.TransformCoordinate(ref gspaceTo, ref _ground.World, out toPosition);
                toPosition.Y = Math.Max(0, toPosition.Y);

                var angle = (float)Math.Atan2(dx, dz);
                var currentNormal = _rotation.Up;
                var delay = rnd.Next(3, 6);
                _actions.Add(() =>
                {
                    if (_onTheMove < 1)
                        return false;
                    _rotation = m(MathUtil.Lerp(_currentAngle,angle,0.33f), currentNormal);
                    return true;
                });
                _actions.Add(() =>
                {
                    if (_onTheMove < 1)
                        return false;
                    _rotation = m(MathUtil.Lerp(_currentAngle, angle, 0.66f), currentNormal);
                    return true;
                });
                _actions.Add(() =>
                {
                    if (_onTheMove < 2)
                        return false;
                    _rotation = m(angle, currentNormal);
                    _currentAngle = angle;
                    return true;
                });
                _actions.Add(() =>
                {
                    var factor = Math.Min(_onTheMove / 0.1f, 0.5f);
                    _position = Vector3.Lerp(fromPosition, toPosition, factor);
                    _position.Y += factor;
                    if (factor < 0.5f)
                        return false;
                    _rotation = m(angle, normal);
                    return true;
                });
                _actions.Add(() =>
                {
                    var factor = Math.Min(0.5f + _onTheMove / 0.1f, 1);
                    _position = Vector3.Lerp(fromPosition, toPosition, factor);
                    _position.Y -= (1 - factor);
                    return factor >= 1;
                });
                _actions.Add(() => _onTheMove > delay);

                return;
            }

            //failed to find a new spot
        }

        private static Matrix m(float angle, Vector3 normal)
        {
            var rotation = Matrix.RotationY(angle);
            rotation.Up = normal;
            rotation.Right = Vector3.Normalize(Vector3.Cross(rotation.Forward, rotation.Up));
            rotation.Forward = Vector3.Normalize(Vector3.Cross(rotation.Up, rotation.Right));
            return rotation;
        }

        private Vector3 getApproxMax(Vector3 vNew, Vector3 vOld)
        {
            const int iterations = 10;
            var result = vNew;
            for (var i = 1; i < iterations; i++)
            {
                var vMiddle = Vector3.Lerp(vNew, vOld, (float) i/iterations);
                var height = _ground.GroundMap.GetExactHeight(vMiddle.X, vMiddle.Z);
                if (height > vMiddle.Y - 0.1f)
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
            var t = _modelRotation * _rotation * Matrix.Translation(_position + new Vector3(0, 0, 0));

            Effect.World = t;
            Effect.Texture = _texture;
            Effect.DiffuseColor = Vector4.One;
            _model.Draw(Effect.GraphicsDevice, t, camera.View, camera.Projection, Effect.Effect);

            return true;
        }

    }

}
