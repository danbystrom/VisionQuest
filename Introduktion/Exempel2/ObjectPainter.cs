using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using WindowsFormsApplication1;

namespace Exempel2
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
            var pts = lines.SelectMany(_ => new[] {_.P1, _.P2}).ToArray();
            world.TransformPoints(pts);
            _graphics.DrawLines(Pens.Black, pts);
        }

    }

}
