using System.Diagnostics;
using SharpDX;
using System;

namespace factor10.VisionThing
{
    public class MoveCameraArc : MoveCameraBase
    {
        public readonly Vector3 _endPoint;
        public readonly Vector3 _incomingDirection;
        public readonly Vector3 _startPoint;
        public readonly Vector3 _turningPoint;
        public readonly Vector3 _outVector;
        public readonly float _incomingLength;
        public readonly float _arcLength;
        public readonly float _angle;
        public readonly Vector3 _origo;
        public readonly Vector3 _rotationAxis;
        public readonly float _diameter;
        public readonly float _arcFactor;

        private Func<Vector3> _toLookAt;

        public MoveCameraArc(
            Camera camera,
            MovementTime time,
            Vector3 endPoint,
            Vector3 incomingDirection,
            float incomingLength,
            Func<Vector3> toLookAt = null)
            : base(camera)
        {
            _startPoint = camera.Position;
            incomingDirection.Normalize();
            _turningPoint = endPoint - incomingDirection*incomingLength;
            _incomingDirection = incomingDirection;
            _incomingLength = incomingLength;
            _endPoint = endPoint;
            _toLookAt = toLookAt ?? (() => _endPoint + _incomingDirection);

            var v = _startPoint - _turningPoint;
            if (!v.IsZero)
            {
                _outVector = Vector3.Cross(incomingDirection, v);
                var vLength = v.Length();
                var sinAngle = _outVector.Length()/vLength; // incomingDirection has length 1
                _angle = (float) Math.Asin(sinAngle);
                _outVector.Normalize();

                var o = Vector3.Cross(_outVector, v);
                o.Normalize();
                o *= vLength/(float) Math.Tan(Math.PI - _angle)/2;
                _origo = o + (_startPoint + _turningPoint)/2;

                _diameter = Vector3.Distance(_origo, _startPoint)*2;
                _arcLength = _diameter*_angle/MathUtil.Pi;
                _arcFactor = _arcLength/(_arcLength + _incomingLength);
            }

            EndTime = time.GetTotalTime(_arcLength + _incomingLength);
        }

        private Vector3 getPointOnPath(float x)
        {
            if (x >= _arcFactor)
                return Vector3.Lerp(_turningPoint, _endPoint, (x - _arcFactor)/(1 - _arcFactor));

            var m = Matrix.RotationAxis(_outVector, _angle*2*(x/_arcFactor));
            return _origo + Vector3.TransformNormal(_startPoint - _origo, m);
        }

        protected override bool MoveAround()
        {
            var timeFactor = Math.Min(1, ElapsedTime/EndTime);

            var posFactor = MathUtil.SmootherStep(timeFactor);
            var lookFactor = Math.Min(1, timeFactor*2f);  // zoom in on "look at" faster

            Camera.Update(
                getPointOnPath(posFactor),
                Vector3.Lerp(FromLookAt, _toLookAt(), lookFactor));

            Debug.Assert(!float.IsNaN(Camera.Position.X));

            return ElapsedTime < EndTime;
        }

    }

}
