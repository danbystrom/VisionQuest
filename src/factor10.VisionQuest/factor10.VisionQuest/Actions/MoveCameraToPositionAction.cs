using System;
using System.Diagnostics;
using System.Linq;
using factor10.VisionThing;
using factor10.VisionThing.CameraStuff;
using SharpDX;
using SharpDX.Toolkit;

namespace factor10.VisionQuest.Actions
{
    public class MoveCameraToPositionAction : IAction
    {
        private readonly Vector3 _toPosition;
        private readonly Vector3 _toLookAt;

        private readonly float _toYaw;
        private readonly float _fromYaw;

        private float _time;

        private readonly Vector3[] _path;

        public MoveCameraToPositionAction(Camera camera, Vector3 toPosition, Vector3 toLookAt)
        {
            if (Vector3.DistanceSquared(camera.Position, toPosition) > 400)
            {
                var ag = new ArcGenerator(5);
                ag.CreateArc(camera.Position, toPosition, Vector3.Up, Vector3.Distance(camera.Position, toPosition)/8);
                _path = ag.Points;
            }
            else
                _path = new[] {camera.Position, toPosition};

            _fromYaw = camera.Yaw;
            _toPosition = toPosition;
            _toLookAt = toLookAt;
            _toYaw = (float) Math.Atan2(toPosition.X - toLookAt.X, toPosition.Z - toLookAt.Z);

            Debug.Print("ToPos: {0}  ToLookAt: {1}  ToYaw: {2}", _toPosition, _toLookAt, MathUtil.RadiansToDegrees(_toYaw));

            var angle = _fromYaw - _toYaw;
            if (angle > MathUtil.Pi)
                _fromYaw -= MathUtil.TwoPi;
            else if (angle < -MathUtil.Pi)
                _fromYaw += MathUtil.TwoPi;
        }

        private Vector3 getPointOnPath(float x)
        {
            if (x < 0)
                return _path.First();
            if (x >= 1)
                return _path.Last();
            var step = 1f/(_path.Length - 1);
            var idx = (int) (x/step);
            var frac = (x - idx*step)/step;
            return Vector3.Lerp(_path[idx], _path[idx + 1], frac);
        }

        public bool Do(SharedData data, GameTime gameTime)
        {
            var dt = (float) gameTime.ElapsedGameTime.TotalSeconds;
            _time += dt;
            var posFactor = MathUtil.SmootherStep(_time/5);
            var pos = getPointOnPath(posFactor);

            var newYaw = MathUtil.Lerp(_fromYaw, _toYaw, Math.Min(1, MathUtil.SmootherStep(_time/3)));
            var desiredPitch = -(float)Math.Asin((_toPosition.Y - _toLookAt.Y) / Vector3.Distance(_toPosition, _toLookAt));

            var newPitch = MathUtil.Lerp(data.Camera.Pitch, desiredPitch, dt);
            var rotation = Matrix.RotationYawPitchRoll(newYaw, newPitch, 0);
            data.Camera.Update(
                pos,
                pos + Vector3.TransformCoordinate(Vector3.ForwardRH * 10, rotation));
            return posFactor < 1;
        }

    }

}
