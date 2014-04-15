using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using factor10.VisionThing;

namespace Serpent.Serpent
{
    public enum CameraBehavior
    {
        Static,
        FollowTarget,
        FreeFlying
    }

    public class SerpentCamera
    {
        public readonly Camera Camera;

        private CameraBehavior _cameraBehavior;
        private float _acc;
        private Vector3 _desiredUpVector = Vector3.Up;

        public SerpentCamera(
            Rectangle clientBounds,
            Vector3 position,
            Vector3 target,
            CameraBehavior cameraBehavior)
        {
            _cameraBehavior = cameraBehavior;
            Camera = new Camera( clientBounds, position, target );    
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
                        _desiredUpVector = Vector3.Up;
                        break;
                    case CameraBehavior.Static:
                        _desiredUpVector = Vector3.Forward;
                        break;
                    case CameraBehavior.FreeFlying:
                        var t = Camera.Target;
                        var p = Camera.Position;
                        Camera.Yaw = -(float)Math.Atan2(p.X - t.X, p.Z - t.Z);
                        Camera.Pitch = (float)Math.Asin((p.Y - t.Y) / Vector3.Distance(p, t));
                        Mouse.SetPosition((int)Camera.ClientSize.X / 2, (int)Camera.ClientSize.Y / 2);
                        break;
                }
            }
        }

        public void Update(
            GameTime gameTime,
            Vector3 target,
            Direction direction,
            KeyboardState kbd)
        {
            Camera.UpVector = Vector3.Lerp(Camera.UpVector, _desiredUpVector, 0.03f);

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
                    var v = MathHelper.Clamp(_acc, 0.1f, 0.3f);
                    Camera.Update(
                        Vector3.Lerp(Camera.Position, newPosition, v),
                        Vector3.Lerp(Camera.Target, target, v));
                    break;

                case CameraBehavior.Static:
                    Camera.Update(
                        Vector3.Lerp(Camera.Position, new Vector3(10, 30, 10), 0.02f),
                        Vector3.Lerp(Camera.Target, new Vector3(10, 0, 10), 0.02f));
                    break;

                case CameraBehavior.FreeFlying:
                    Camera.UpdateFreeFlyingCamera(gameTime, kbd);
                    return;

                default:
                    return;
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
