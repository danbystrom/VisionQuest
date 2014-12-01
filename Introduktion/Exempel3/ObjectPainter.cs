using System.Collections.Generic;
using System.Drawing;
using WindowsFormsApplication1;
using SharpDX;

namespace Exempel2
{
    public class ObjectPainter
    {
        private readonly 
            Graphics _graphics;
        private readonly float _height;

        public ObjectPainter(Graphics graphics, float height)
        {
            _graphics = graphics;
            _height = height;
        }

        public void Paint(Matrix world, IEnumerable<Line> lines)
        {
            foreach (var line in lines)
            {
                var p1 = Vector3.Transform(line.P1, world);
                var p2 = Vector3.Transform(line.P2, world);
                p1.Y = _height - p1.Y;
                p2.Y = _height - p2.Y;
                _graphics.DrawLine(Pens.Black, new PointF(p1.X, p1.Y), new PointF(p2.X, p2.Y));
            }
        }

    }

}
