using System.Collections.Generic;
using System.Drawing;

namespace Exempel1
{
    public class ObjectPainter
    {
        private readonly Graphics _graphics;

        public ObjectPainter(Graphics graphics)
        {
            _graphics = graphics;
        }

        public void Paint(Matrix2 world, IEnumerable<Line> lines)
        {
            foreach (var line in lines)
            {
                var p1 = world.TransformPoint(line.P1);
                var p2 = world.TransformPoint(line.P2);
                _graphics.DrawLine(Pens.Black, p1, p2);
            }
        }

    }
}
