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
        private readonly float _height;

        public ObjectPainter(Graphics graphics, float height)
        {
            _graphics = graphics;
            _height = height;
        }

        public void Paint(Matrix world, IEnumerable<Line> lines)
        {
            var pts = lines.SelectMany(_ => new[] {_.P1, _.P2}).ToArray();
            world.TransformPoints(pts);
            for (var i = 0; i < pts.Length; i++)
                pts[i].Y = _height - pts[i].Y;
            _graphics.DrawLines(Pens.Black, pts);
        }

    }

}
