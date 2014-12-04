using System.Collections.Generic;
using System.Drawing;
using SharpDX;

namespace Exempel3
{
    public class ObjectPainter
    {
        private readonly Graphics _graphics;

        public ObjectPainter(Graphics graphics)
        {
            _graphics = graphics;
        }

        public void Paint(Matrix world, IEnumerable<Line> lines)
        {
            foreach (var line in lines)
            {
                var p1 = Vector3.Transform(line.P1, world);
                var p2 = Vector3.Transform(line.P2, world);
                _graphics.DrawLine(Pens.Black, new PointF(p1.X, p1.Y), new PointF(p2.X, p2.Y));
            }
        }

    }

}
