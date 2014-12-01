using System.Collections.Generic;
using System.Drawing;
using SharpDX;

namespace Exempel5
{
    public class ObjectPainter
    {
        private readonly Graphics _graphics;
        private readonly float _height;
        private readonly float _width;

        public ObjectPainter(Graphics graphics, float height, float width)
        {
            _graphics = graphics;
            _height = height;
            _width = width;
        }

        public void Paint(Matrix world, Matrix view, Matrix projection, IEnumerable<Line> lines)
        {
            var wp = world*view*projection;
            foreach (var line in lines)
            {
                var p1 = Vector3.TransformCoordinate(line.P1, wp);
                var p2 = Vector3.TransformCoordinate(line.P2, wp);
                p1.X = (0.5f + p1.X)*_width;
                p1.Y = (0.5f + p1.Y)*_height;
                p2.X = (0.5f + p2.X)*_width;
                p2.Y = (0.5f + p2.Y)*_height;
                _graphics.DrawLine(Pens.Black, new PointF(p1.X, p1.Y), new PointF(p2.X, p2.Y));
            }
        }

    }

}
