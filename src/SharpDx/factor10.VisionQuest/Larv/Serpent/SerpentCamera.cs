using System;
using factor10.VisionThing;
using SharpDX;

namespace Larv.Serpent
{
    public class SerpentCamera : MoveCameraBase
    {
        public const float CameraDistanceToHeadXz = 9;
        public const float CameraDistanceToHeadY = 5;
        public const float DefaultTension = 10;

        private float _lastTime;

        private readonly BaseSerpent _serpent;

        public float Tension = DefaultTension;

        public SerpentCamera(Camera camera, BaseSerpent serpent)
            : base(camera)
        {
            _serpent = serpent;
        }

        public void IncreaseTensionUntilMax(float delta, float max = DefaultTension)
        {
            Tension = Math.Min(max, Tension + delta);
        }

        protected override bool MoveAround()
        {
            var dt = ElapsedTime - _lastTime;
            _lastTime = ElapsedTime;

            var target = _serpent.LookAtPosition;
            var direction = _serpent.HeadDirection;

            var target2D = new Vector2(target.X, target.Z);
            var position2D = moveTo(
                new Vector2(Camera.Position.X, Camera.Position.Z),
                target2D,
                target2D - direction.DirectionAsVector2()*CameraDistanceToHeadXz,
                dt);

            var newPosition = new Vector3(
                position2D.X,
                target.Y + CameraDistanceToHeadY,
                position2D.Y);

            var v = dt * Tension;

            //System.Diagnostics.Debug.Print("{0:0.000} {1:0.000} / {2:0.000} {3:0.000}",
            //    Vector3.Distance(Camera.Position,Vector3.Lerp(Camera.Position, newPosition, v)),
            //    Vector3.Distance(Camera.Target, Vector3.Lerp(Camera.Target, target, v)),
            //    dt, v);
            Camera.Update(
                Vector3.Lerp(Camera.Position, newPosition, v),
                Vector3.Lerp(Camera.Target, target, v));

            return true;
        }

        private static Vector2 moveTo(
            Vector2 camera,
            Vector2 target,
            Vector2 desired,
            double xelapsedTime)
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

            var angleFraction = angle*xelapsedTime*10;

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
