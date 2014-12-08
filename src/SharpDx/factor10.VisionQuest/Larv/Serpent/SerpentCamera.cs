using System;
using factor10.VisionThing;
using Larv.Util;
using Serpent;
using SharpDX;
using SharpDX.Toolkit;

namespace Larv.Serpent
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
        public const float CameraDistanceToHeadXz = 9;
        public const float CameraDistanceToHeadY = 5;

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
            CameraBehavior cameraBehavior)
        {
            _cameraBehavior = cameraBehavior;
        }

        public CameraBehavior CameraBehavior
        {
            get { return _cameraBehavior; }
        }

        public void SetCameraBehavior(Camera camera, CameraBehavior cameraBehavior)
        {
            _cameraBehavior = cameraBehavior;
            switch (CameraBehavior)
            {
                case CameraBehavior.FollowTarget:
                case CameraBehavior.Head:
                    _desiredUpVector = Vector3.Up;
                    break;
                case CameraBehavior.Static:
                    _staticTimeMovement = 0;
                    _staticFromPosition = camera.Position;
                    _staticFromTarget = camera.Target;
                    break;
                case CameraBehavior.FreeFlying:
                    //Camera.MouseManager.SetPosition(new Vector2(0.5f, 0.5f));
                    break;
            }
        }

        public void Update(
            GameTime gameTime,
            Camera camera,
            Vector3 target,
            Direction direction)
        {
            camera.Up = Vector3.Lerp(camera.Up, _desiredUpVector, 0.03f);

            switch (CameraBehavior)
            {
                case CameraBehavior.FollowTarget:
                    var target2D = new Vector2(target.X, target.Z);
                    var position2D = moveTo(
                        new Vector2(camera.Position.X, camera.Position.Z),
                        target2D,
                        target2D - direction.DirectionAsVector2()*CameraDistanceToHeadXz,
                        gameTime.ElapsedGameTime.TotalMilliseconds);

                    var newPosition = new Vector3(
                        position2D.X,
                        target.Y + CameraDistanceToHeadY,
                        position2D.Y);

                    _acc += (float) Math.Sqrt(Vector3.Distance(newPosition, camera.Position))*
                            (float) gameTime.ElapsedGameTime.TotalSeconds;
                    _acc *= 0.4f;
                    var v = MathUtil.Clamp(_acc, 0.1f, 0.3f);
                    camera.Update(
                        Vector3.Lerp(camera.Position, newPosition, v),
                        Vector3.Lerp(camera.Target, target, v));

                    break;

                case CameraBehavior.Static:
                    if (_staticTimeMovement >= TotalStaticMovementTime)
                        return;
                    _staticTimeMovement = Math.Min(_staticTimeMovement + (float) gameTime.ElapsedGameTime.TotalSeconds, TotalStaticMovementTime);
                    var f = MathUtil.SmootherStep(_staticTimeMovement/TotalStaticMovementTime);
                    camera.Update(
                        Vector3.Lerp(_staticFromPosition, _staticDestinationPosition, f),
                        Vector3.Lerp(_staticFromTarget, _staticDestinationTarget, f));
                    break;

                case CameraBehavior.FreeFlying:
                    camera.UpdateInputDevices();
                    camera.UpdateFreeFlyingCamera(gameTime);
                    break;

                case CameraBehavior.Head:
                    var d3 = direction.DirectionAsVector3()*0.5f;
                    var newPosition2 = target + d3 + Vector3.Up;
                    target = newPosition2 + d3 + Vector3.Down*0.1f;

                    _acc += (float) Math.Sqrt(Vector3.Distance(newPosition2, camera.Position))*
                            (float) gameTime.ElapsedGameTime.TotalMilliseconds*0.001f;
                    _acc *= 0.4f;
                    var v2 = MathUtil.Clamp(_acc, 0.1f, 0.3f);
                    camera.Update(
                        Vector3.Lerp(camera.Position, newPosition2, v2),
                        Vector3.Lerp(camera.Target, target, v2));

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

            if (d2TargetDesired<0.0001f || d2CameraDesired < 0.0001f || d2TargetCamera < 0.0001f)
                return desired;

            var d1 = d2TargetDesired + d2TargetCamera - d2CameraDesired;
            var d2 = Math.Sqrt(4*d2TargetDesired*d2TargetCamera);
            var div = d1/d2;
            if (div < -1f)
                div += 2;
            else if (div > 1)
                div -= 2;
            var angle = (float) Math.Acos(div);

            var v1 = camera - target;
            var v2 = desired - target;

            if (v1.X*v2.Y - v2.X*v1.Y > 0)
                angle = -angle;

            var angleFraction = angle*elapsedTime/100;

            var cosA = (float) Math.Cos(angleFraction);
            var sinA = (float) Math.Sin(angleFraction);
            var direction = new Vector2(
                v1.X*cosA + v1.Y*sinA,
                -v1.X*sinA + v1.Y*cosA);
            direction.Normalize();
            return target + direction*v2.Length();
        }

    }

}
