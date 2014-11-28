using System.Linq;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using Larv.Util;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System;

namespace Larv.Serpent
{
    public class Frog : ClipDrawable, IPosition
    {
        private readonly Model _model;
        private readonly Matrix _modelRotation;
        private readonly Texture2D _texture;

        private readonly Serpents _serpents;

        private readonly Ground _ground;

        private Vector3 _position;
        private Matrix _rotation;
        private float _currentAngle;

        private static readonly Random Rnd = new Random();

        private readonly ToDoQue _actions = new ToDoQue();

        public Frog(
            VisionContent vContent,
            IVEffect effect,
            Serpents serpents,
            Ground ground)
            : base(effect)
        {
            _model = vContent.Load<Model>(@"Models/frog");
            _modelRotation = Matrix.RotationX(MathUtil.Pi)*Matrix.RotationY(MathUtil.Pi)*Matrix.Scaling(0.1f);
            
            _texture = vContent.Load<Texture2D>(@"textures/frogskin");
            _serpents = serpents;
            _ground = ground;
            Restart();
        }

        public void Restart()
        {
            _position = new Vector3(_serpents.PlayingField.MiddleX, 0, -2);
            _actions.Add(0);  // start the state machine
        }

        public Vector3 Position
        {
            get { return _position; }
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            base.Update(camera, gameTime);

            if (_actions.Do(gameTime))
                return;

            // out of actions - create new commands

            Vector3 toPosition, normal;
            float angle;
            if (!findNewPosition(out toPosition, out normal, out angle))
                return;

            var fromPosition = _position;

            var currentNormal = _rotation.Up;
            var shortDelay = Rnd.NextFloat(0.3f, 1.1f);
            _actions.Add(shortDelay, () => _rotation = m(MathUtil.Lerp(_currentAngle, angle, 0.33f), currentNormal));
            _actions.Add(shortDelay, () => _rotation = m(MathUtil.Lerp(_currentAngle, angle, 0.66f), currentNormal));
            _actions.Add(shortDelay, () => _rotation = m(_currentAngle = angle, currentNormal));
            _actions.Add(shortDelay, time =>
            {
                var factor = Math.Min(time/0.2f, 0.5f);
                _position = Vector3.Lerp(fromPosition, toPosition, factor);
                _position.Y += factor;
                if (factor < 0.5f)
                    return true;
                _rotation = m(angle, normal);
                return false;
            });
            _actions.Add(time =>
            {
                var factor = Math.Min(0.5f + time/0.2f, 1);
                _position = Vector3.Lerp(fromPosition, toPosition, factor);
                _position.Y -= (1 - factor);
                return factor < 1;
            });
            _actions.Add(Rnd.NextFloat(3, 6));
        }

        private bool findNewPosition(out Vector3 position, out Vector3 normal, out float angle)
        {
            // this is not even called once a second - performance is not a issue here

            try
            {
                var eggs = _serpents.EnemyEggs.Select(_ => _.Position).ToList();
                if (_serpents.PlayerEgg != null)
                    eggs.Add(_serpents.PlayerEgg.Position);
                eggs.Sort((x, y) => Vector3.DistanceSquared(_position, x).CompareTo(Vector3.DistanceSquared(_position, y)));
                var closeToEgg = eggs.Any() && Vector3.DistanceSquared(_position, eggs.First()) < 4;

                eggs.Add(new Vector3(_serpents.PlayingField.MiddleX, 0, _serpents.PlayingField.MiddleY));
                var goodDirection = eggs.FirstOrDefault() - _position;
                goodDirection.Normalize();

                var winv = _ground.World;
                winv.Invert();
                Vector3 gspaceCurrent;
                Vector3.TransformCoordinate(ref _position, ref winv, out gspaceCurrent);

                for (var i = 0; i < 100; i++)
                {
                    var dx = Rnd.NextFloat(1, 2)*(Rnd.NextDouble() < 0.5 ? -1 : 1);
                    var dz = Rnd.NextFloat(1, 2)*(Rnd.NextDouble() < 0.5 ? -1 : 1);

                    var toPosition = closeToEgg
                        ? eggs.First()
                        : Position + new Vector3(dx, 0, dz) + goodDirection*Rnd.NextFloat(0.5f, 1);

                    Vector3 gspaceTo;
                    Vector3.TransformCoordinate(ref toPosition, ref winv, out gspaceTo);
                    normal = _ground.GroundMap.GetNormal((int) gspaceTo.X, (int) gspaceTo.Z, ref _ground.World);
                    if (normal.Y < 0.5f)
                        continue;

                    gspaceTo.Y = _ground.GroundMap.GetExactHeight(gspaceTo.X, gspaceTo.Z);
                    //gspaceTo = getApproxMax(gspaceTo, gspaceCurrent);
                    //normal = _ground.GroundMap.GetNormal((int) gspaceTo.X, (int) gspaceTo.Z, ref _ground.World);
                    //if (normal.Y < 0.5f)
                    //    continue;

                    // now we have a new position for the frog
                    Vector3.TransformCoordinate(ref gspaceTo, ref _ground.World, out position);
                    position.Y = Math.Max(0, position.Y);

                    angle = (float) Math.Atan2(dx, dz);
                    return true;
                }
            }
            catch
            {
            }

            angle = 0;
            position = normal = Vector3.Zero;
            return false;
        }

        private static Matrix m(float angle, Vector3 normal)
        {
            var rotation = Matrix.RotationY(angle);
            rotation.Up = normal;
            rotation.Right = Vector3.Normalize(Vector3.Cross(rotation.Forward, rotation.Up));
            rotation.Forward = Vector3.Normalize(Vector3.Cross(rotation.Up, rotation.Right));
            return rotation;
        }

        private Vector3 getApproxMax(Vector3 gNew, Vector3 gOld)
        {
            const int iterations = 10;
            var result = gNew;
            for (var i = 1; i < iterations; i++)
            {
                var gMiddle = Vector3.Lerp(gNew, gOld, (float) i/iterations);
                var height = _ground.GroundMap.GetExactHeight(gMiddle.X, gMiddle.Z);
                if (height > gMiddle.Y + 5)
                    return new Vector3(gMiddle.X, height, gMiddle.Z);
            }
            return result;
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);
            var t = _modelRotation * _rotation * Matrix.Translation(_position + new Vector3(0, 0, 0));

            Effect.World = t;
            Effect.Texture = _texture;
            Effect.DiffuseColor = Vector4.One;
            _model.Draw(Effect.GraphicsDevice, t, camera.View, camera.Projection, Effect.Effect);

            return true;
        }

    }

}
