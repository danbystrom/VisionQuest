using System;
using factor10.VisionThing;
using Larv.Util;
using SharpDX;
using SharpDX.Toolkit;

namespace Larv.Serpent
{
    public class SerpentCamera
    {
        public const float CameraDistanceToHeadXz = 9;
        public const float CameraDistanceToHeadY = 5;

        private float _acc;

        private float _staticTimeMovement;
        private readonly Vector3 _staticDestinationPosition = new Vector3(12, 20, 25);
        private readonly Vector3 _staticDestinationTarget = new Vector3(12, 0, 12);
        private Vector3 _staticFromPosition;
        private Vector3 _staticFromTarget;

        public SerpentCamera()
        {
        }

        public void Update(
            GameTime gameTime,
            Camera camera,
            Vector3 target,
            Direction direction)
        {

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

            if (d2TargetDesired < 0.0001f || d2CameraDesired < 0.0001f || d2TargetCamera < 0.0001f)
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
