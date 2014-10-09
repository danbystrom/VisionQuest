using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace CircleMaster
{
    public class CircleMaster
    {
        private readonly List<Circle> _circles = new List<Circle>();
        private readonly Action<Circle> _callback;

        public CircleMaster(Action<Circle> callback = null)
        {
            _callback = callback;
        }

        public IEnumerable<Circle> Circles
        {
            get { return _circles; }
        }

        private void callback(Circle circle)
        {
            if (_callback != null)
                _callback(circle);
        }

        public void GetBounds(out int x1, out int y1, out int x2, out int y2)
        {
            x1 = y1 = int.MaxValue;
            x2 = y2 = int.MinValue;
            foreach (var c in _circles)
            {
                x1 = Math.Min(x1, c.X - c.R);
                y1 = Math.Min(y1, c.Y - c.R);
                x2 = Math.Max(x2, c.X + c.R);
                y2 = Math.Max(y2, c.Y + c.R);
            }
        }

        public Circle Drop(int x, int y, int r)
        {
            var circle = new Circle(x, y, r);
            if(_circles.Any())
                drop(circle);
            _circles.Add(circle);
            return circle;
        }

        private void drop(Circle circle)
        {
            var master = Intersections(new Circle(circle.X, circle.Y, 1)).FirstOrDefault();
            if (master != null)
            {
                //try to squeeze it in somewhere around the circle we hit
                circle.X = master.X;
                circle.Y = master.Y - master.R - circle.R - 1;
                if (_circles.Count == 1)
                    return;
                rotate(circle, master, () => !Intersections(circle, master).Any());
            }
            else
                master = _circles.First();

            var x = circle.X - master.X;
            var y = circle.Y - master.Y;
            var angle = Math.Atan2(y, x);
            var sinAngle = Math.Sin(angle);
            var cosAngle = Math.Cos(angle);
            var distance = Math.Sqrt(x * x + y * y);

            //if it intersects anything - move it out until it doesn't
            while (Intersections(circle).Any())
            {
                distance += 1;
                circle.X = master.X + (int)(distance * cosAngle);
                circle.Y = master.Y + (int)(distance * sinAngle);
                callback(circle);
            }

            //move it closer until it just touches
            List<Circle> touches;
            while (true)
            {
                touches = Intersections(circle);
                if (touches.Any())
                    break;
                distance -= 1;
                circle.X = master.X + (int)(distance * cosAngle);
                circle.Y = master.Y + (int)(distance * sinAngle);
                callback(circle);
            }

            // if there are only one circle or if we touche two or more circles - then we're done
            if (_circles.Count < 2 || touches.Count >= 2)
                return;

            // otherwise, rotate until we touch something other than than the master
            master = touches.Single();
            rotate(circle, master, () => Intersections(circle, master).Any());
        }

        private bool rotate(Circle circle, Circle master, Func<bool> stopWhen)
        {
            var x = circle.X - master.X;
            var y = circle.Y - master.Y;
            var angle = Math.Atan2(y, x);
            var distance = Math.Sqrt(x * x + y * y);
            var angleDelta = 1.5 / distance;
            var a = 0.0;

            var steps = (int) (2*Math.PI/angleDelta);
            for (var i = 0; i < steps; i++)
            {
                a = a > 0 ? -(a + angleDelta) : (-a + angleDelta);
                var a2 = angle + a;
                circle.X = master.X + (int)(distance * Math.Cos(a2));
                circle.Y = master.Y + (int)(distance * Math.Sin(a2));
                callback(circle);
                if (stopWhen())
                    return true;
            }
            return false;
        }

        public List<Circle> Intersections(Circle circle, Circle exclude = null)
        {
            return _circles.Where(c => c.Distance2(circle) < 0 && c != exclude).ToList();
        }

    }
}
