using System;
using System.Collections.Generic;

namespace TestBed
{
    public class Circle
    {
        public float X;
        public float Y;
        public float R;

        public Circle(float x, float y, float r)
        {
            X = x;
            Y = y;
            R = r;
        }

        public bool IntersectsWith(Circle other)
        {
            var dx = X - other.X;
            var dy = Y - other.Y;
            var r = R + other.R;
            return dx * dx + dy * dy < r * r;
        }

    }

    public class CircleContainer
    {
        public readonly float Width;

        public List<Circle> Circles = new List<Circle>();
        public float NextDropX;
        public float MaxUsedY;

        public CircleContainer(float width, float startCornerSize)
        {
            Width = width;
            MaxUsedY = startCornerSize;
            Add(0, 0, startCornerSize);
            Add(width, 0, startCornerSize);
            NextDropX = width - startCornerSize;
        }

        public Circle Drop(float r)
        {
            if (r * 2 > NextDropX)
                NextDropX = Width;
            var circle = createNewCircleAtLowestPossiblePlace(r);
            var rightStop = Width - r;
            while (circle.X < rightStop)
            {
                circle.X++;
                if (!fits(circle))
                    break;
            }
            NextDropX = Math.Min(NextDropX, circle.X - circle.R);
            MaxUsedY = Math.Max(MaxUsedY, circle.Y + circle.R);
            Circles.Add(circle);
            return circle;
        }

        private Circle createNewCircleAtLowestPossiblePlace(float r)
        {
            const float delta = 2;
            var lo = r;
            var hi = MaxUsedY + r;
            var circle = new Circle(NextDropX - r, 0, r);

            while (lo + delta < hi)
            {
                circle.Y = (lo + hi) / 2;
                if (fits(circle))
                    hi = circle.Y - 1;
                else
                    lo = circle.Y + 1;
            }

            return circle;
        }


        private bool fits(Circle circle)
        {
            return Circles.TrueForAll(_ => !_.IntersectsWith(circle));
        }

        public void Add(float x, float y, float r)
        {
            Circles.Add(new Circle(x, y, r));
        }

    }

}
