using System;
using factor10.VisionThing;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;

namespace Serpent.Serpent
{
    public enum CameraBehavior
    {
        Static,
        FollowTarget,
        Head,
        FreeFlying
    }

    public class SerpentCamera
    {
        public readonly Camera Camera;

        private CameraBehavior _cameraBehavior;
        private float _acc;
        private Vector3 _desiredUpVector = Vector3.Up;

        private const float TotalStaticMovementTime = 10;
        private float _staticTimeMovement;
        private Vector3 _staticDestinationPosition = new Vector3(12, 20, 25);
        private Vector3 _staticDestinationTarget = new Vector3(12, 0, 12);
        private Vector3 _staticFromPosition;
        private Vector3 _staticFromTarget;

        public SerpentCamera(
            MouseManager mouseManager,
            KeyboardManager keyboardManager,
            PointerManager pointerManager,
            Vector2 clientBounds,
            Vector3 position,
            Vector3 target,
            CameraBehavior cameraBehavior)
        {
            _cameraBehavior = cameraBehavior;
            Camera = new Camera(clientBounds, keyboardManager, mouseManager, pointerManager, position, target);
        }

        public CameraBehavior CameraBehavior
        {
            get { return _cameraBehavior; }
            set
            {
                _cameraBehavior = value;
                switch (CameraBehavior)
                {
                    case CameraBehavior.FollowTarget:
                    case CameraBehavior.Head:
                        _desiredUpVector = Vector3.Up;
                        break;
                    case CameraBehavior.Static:
                        _staticTimeMovement = 0;
                        _staticFromPosition = Camera.Position;
                        _staticFromTarget = Camera.Target;
                        break;
                    case CameraBehavior.FreeFlying:
                        Camera.MouseManager.SetPosition(new Vector2(0.5f, 0.5f));
                        break;
                }
            }
        }

        public void Update(
            GameTime gameTime,
            Vector3 target,
            Direction direction)
        {
            Camera.Up = Vector3.Lerp(Camera.Up, _desiredUpVector, 0.03f);

            switch (CameraBehavior)
            {
                case CameraBehavior.FollowTarget:
                    var target2D = new Vector2(target.X, target.Z);
                    var position2D = moveTo(
                        new Vector2(Camera.Position.X, Camera.Position.Z),
                        target2D,
                        target2D - direction.DirectionAsVector2() * 9,
                        gameTime.ElapsedGameTime.TotalMilliseconds);

                    var newPosition = new Vector3(
                        position2D.X,
                        target.Y + 5,
                        position2D.Y);

                    _acc += (float)Math.Sqrt(Vector3.Distance(newPosition, Camera.Position)) *
                            (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;
                    _acc *= 0.4f;
                    var v = MathUtil.Clamp(_acc, 0.1f, 0.3f);
                    Camera.Update(
                        Vector3.Lerp(Camera.Position, newPosition, v),
                        Vector3.Lerp(Camera.Target, target, v));
                    break;

                case CameraBehavior.Static:
                    if (_staticTimeMovement >= TotalStaticMovementTime)
                        return;
                    _staticTimeMovement = Math.Min(_staticTimeMovement + (float) gameTime.ElapsedGameTime.TotalSeconds, TotalStaticMovementTime);
                    var f = MathUtil.SmootherStep(_staticTimeMovement/TotalStaticMovementTime);
                    Camera.Update(
                        Vector3.Lerp(_staticFromPosition, _staticDestinationPosition, f),
                        Vector3.Lerp(_staticFromTarget, _staticDestinationTarget, f));
                    break;

                case CameraBehavior.FreeFlying:
                    Camera.UpdateInputDevices();
                    Camera.UpdateFreeFlyingCamera(gameTime);
                    break;

                case CameraBehavior.Head:
                    var d3 = direction.DirectionAsVector3() * 0.5f;
                    var newPosition2 = target + d3 + Vector3.Up;
                    target = newPosition2 + d3 + Vector3.Down*0.1f;

                    _acc += (float)Math.Sqrt(Vector3.Distance(newPosition2, Camera.Position)) *
                            (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;
                    _acc *= 0.4f;
                    var v2 = MathUtil.Clamp(_acc, 0.1f, 0.3f);
                    Camera.Update(
                        Vector3.Lerp(Camera.Position, newPosition2, v2),
                        Vector3.Lerp(Camera.Target, target, v2));

                    break;
            }

        }

        private static Vector2 moveTo(
            Vector2 camera,
            Vector2 target,
            Vector2 desired,
            double elapsedTime)
        {
            var d2TargetDesired = Vector2.DistanceSquared(target, desired);
            var d2CameraDesired = Vector2.DistanceSquared(camera, desired);
            var d2TargetCamera = Vector2.DistanceSquared(target, camera);

            if (d2CameraDesired < 0.0001f || d2TargetCamera < 0.0001f)
                return desired;

            var d1 = d2TargetDesired + d2TargetCamera - d2CameraDesired;
            var d2 = Math.Sqrt(4 * d2TargetDesired * d2TargetCamera);
            var div = d1 / d2;
            if (div < -1f)
                div += 2;
            else if (div > 1)
                div -= 2;
            var angle = (float)Math.Acos(div);

            var v1 = camera - target;
            var v2 = desired - target;

            if (v1.X * v2.Y - v2.X * v1.Y > 0)
                angle = -angle;

            var angleFraction = angle * elapsedTime / 100;

            var cosA = (float)Math.Cos(angleFraction);
            var sinA = (float)Math.Sin(angleFraction);
            var direction = new Vector2(
                v1.X * cosA + v1.Y * sinA,
                -v1.X * sinA + v1.Y * cosA);
            direction.Normalize();
            return target + direction * v2.Length();
        }

    }

}
