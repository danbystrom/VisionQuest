using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpDX;
using SharpDX.Toolkit;

namespace factor10.VisionThing
{
    public class MoveCamera
    {
        private readonly Camera _camera;

        private readonly Func<Vector3> _toLookAt;

        private readonly Vector3 _fromLookAt;

        private readonly float _endTime;
        private float _totalTime;

        private readonly Vector3[] _path;

        public bool NeverComplete;

        public static MoveCamera TotalTime(Camera camera, float endTime, Func<Vector3> toLookAt, params Vector3[] followPath)
        {
            return new MoveCamera(camera, endTime, 0, toLookAt, followPath);
        }

        public static MoveCamera UnitsPerSecond(Camera camera, float unitsPerSecond, Func<Vector3> toLookAt, params Vector3[] followPath)
        {
            return new MoveCamera(camera, 0, unitsPerSecond, toLookAt, followPath);
        }

        public static MoveCamera TotalTime(Camera camera, float endTime, Vector3 toLookAt, params Vector3[] followPath)
        {
            return new MoveCamera(camera, endTime, 0, () => toLookAt, followPath);
        }

        public static MoveCamera UnitsPerSecond(Camera camera, float unitsPerSecond, Vector3 toLookAt, params Vector3[] followPath)
        {
            return new MoveCamera(camera, 0, unitsPerSecond, () => toLookAt, followPath);
        }

        private MoveCamera(Camera camera, float endTime, float unitsPerSecond, Func<Vector3> toLookAt, params Vector3[] followPath)
        {
            _camera = camera;

            var list = followPath.ToList();
            if (Vector3.DistanceSquared(list.First(), _camera.Position) > 0.1f || followPath.Length < 2)
                list.Insert(0, _camera.Position);
            qwerty(list, toLookAt());
            _path = list.ToArray();

            _endTime = MathUtil.IsZero(unitsPerSecond)
                ? endTime
                : (0.1f + pathLength())/unitsPerSecond;

            _toLookAt = toLookAt;
            _fromLookAt = camera.Target;
        }

        private static void qwerty(List<Vector3> list, Vector3 toLookAt)
        {
            if (list.Count != 2)
                return;
            var v1 = list.Last() - list.First();
            var v2 = toLookAt - list.First();
            var v3 = toLookAt - list.Last();
            var distanceFromLookAtToLine = Vector3.Cross(v1, v2).Length();
            var distanceFromLookAtToEndPoint = v2.Length();
            if (distanceFromLookAtToLine < distanceFromLookAtToEndPoint)
                return;
            // ta fram en punkt som vi måste passera genom och bryt sedan polylinjen tills bitarna blir lagom långa
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
            _totalTime += dt;
            return move();
        }

        public bool Move(float totalTime)
        {
            _totalTime = totalTime;
            return move();
        }

        private bool move()
        {
            var timeFactor = Math.Min(1, _totalTime/_endTime);

            var posFactor = MathUtil.SmootherStep(timeFactor);
            var pos = getPointOnPath(posFactor);

            _camera.Update(
                pos,
                Vector3.Lerp(_fromLookAt, _toLookAt(), posFactor));

            return _totalTime < _endTime || NeverComplete;
        }

    }

}
