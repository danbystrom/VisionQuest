using System.Drawing;

namespace factor10.VisionQuest.GroundControl
{
    public class Circle<T>
    {
        public int X;
        public int Y;
        public int R;

        public T Tag;

        public Circle(int x, int y, int r, T tag = default(T))
        {
            X = x;
            Y = y;
            R = r;
            Tag = tag;
        }

        public int Distance2(Circle<T> other)
        {
            var x = X - other.X;
            var y = Y - other.Y;
            var d2 = x*x + y*y;
            var r = R + other.R;
            return d2 - r*r;
        }

        public Rectangle BoundingRectangle
        {
            get { return new Rectangle(X - R, Y - R, R*2, R*2); }
        }

        public bool Intersects(Circle<T> other)
        {
            return Distance2(other) < 0;
        }

    }

}
