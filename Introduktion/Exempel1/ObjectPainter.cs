using System.Collections.Generic;
using System.Drawing;
using WindowsFormsApplication1;

namespace Exempel1
{
    public class ObjectPainter
    {
        private readonly Graphics _graphics;
        private readonly float _height;

        public ObjectPainter(Graphics graphics, float height)
        {
            _graphics = graphics;
            _height = height;
        }

        public void Paint(Matrix2 world, IEnumerable<Line> lines)
        {
            foreach (var line in lines)
            {
                var p1 = world.TransformPoint(line.P1);
                var p2 = world.TransformPoint(line.P2);
                p1.Y = _height - p1.Y;
                p2.Y = _height - p2.Y;
                _graphics.DrawLine(Pens.Black, p1, p2);
            }
        }

    }
}
