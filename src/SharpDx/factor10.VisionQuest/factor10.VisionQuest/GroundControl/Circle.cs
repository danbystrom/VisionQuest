using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleMaster
{
    public class Circle
    {
        public int X;
        public int Y;
        public int R;

        public Circle(int x, int y, int r)
        {
            X = x;
            Y = y;
            R = r;
        }

        public int Distance2(Circle other)
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

        public bool Intersects(Circle other)
        {
            return Distance2(other) < 0;
        }

    }

}
