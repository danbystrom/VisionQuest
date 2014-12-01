using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class Line
    {
        public PointF P1;
        public PointF P2;

        public Line(PointF p1, PointF p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public Line(float x1, float y1, float x2, float y2)
            : this(new PointF(x1, y1), new PointF(x2, y2))
        {
        }

    }

}
