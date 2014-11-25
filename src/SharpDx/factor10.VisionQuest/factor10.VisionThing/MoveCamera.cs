using System;
using System.IO;
using System.Linq;
using SharpDX;
using SharpDX.Toolkit;

namespace factor10.VisionThing
{
    public class MoveCamera
    {
        private readonly Camera _camera;

        private readonly Vector3 _toLookAt;

        private readonly Vector3 _fromLookAt;

        private readonly float _totalTime;
        private float _elapsedTime;

        private readonly Vector3[] _path;

        public static MoveCamera TotalTime(Camera camera, float totalTime, Vector3 toLookAt, params Vector3[] followPath)
        {
            return new MoveCamera(camera, totalTime, 0, toLookAt, followPath);
        }

        public static MoveCamera UnitsPerSecond(Camera camera, float unitsPerSecond, Vector3 toLookAt, params Vector3[] followPath)
        {
            return new MoveCamera(camera, 0, unitsPerSecond, toLookAt, followPath);
        }

        private MoveCamera(Camera camera, float totalTime, float unitsPerSecond, Vector3 toLookAt, params Vector3[] followPath)
        {
            _camera = camera;

            var list = followPath.ToList();
            if (Vector3.DistanceSquared(list.First(), _camera.Position) > 0.1f || followPath.Length < 2)
                list.Insert(0, _camera.Position);
            _path = list.ToArray();

            _totalTime = MathUtil.IsZero(unitsPerSecond)
                ? totalTime
                : (0.1f + pathLength())/unitsPerSecond;

            _toLookAt = toLookAt;
            _fromLookAt = camera.Target;
        }

        private float pathLength()
        {
            var length = 0f;
            var v = _path.First();
            for (var i = 1; i < _path.Length; i++)
                length += Vector3.Distance(v, v = _path[i]);
            return length;
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

        public bool Move(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _elapsedTime += dt;
            var timeFactor = Math.Min(1, _elapsedTime/_totalTime);

            var posFactor = MathUtil.SmootherStep(timeFactor);
            var pos = getPointOnPath(posFactor);

            _camera.Update(
                pos,
                Vector3.Lerp(_fromLookAt, _toLookAt, posFactor));

            return _elapsedTime < _totalTime;
        }

    }

}
