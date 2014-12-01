using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exempel1;

namespace WindowsFormsApplication1
{
    public class Game
    {
        private float _time;

        private List<Line> _square = new List<Line>
        {
            new Line(-1, -1, 1, -1),
            new Line(1, -1, 1, 1),
            new Line(1, 1, -1, 1),
            new Line(-1, 1, -1, -1)
        };

        public Game()
        {
            
        }

        public void Update(float elapsedTime)
        {
            _time += elapsedTime;
        }

        public void Draw(ObjectPainter painter)
        {
            var angle = _time;
            var cos = (float) Math.Cos(angle);
            var sin = (float) Math.Sin(angle);
            var w = new Matrix2(cos, -sin, sin, cos);

            painter.Paint(w * new Matrix2(100, 0, 0, 100), _square);

            angle = _time*1.5f;
            cos = (float)Math.Cos(angle);
            sin = (float)Math.Sin(angle);
            w = new Matrix2(cos, -sin, sin, cos);
            painter.Paint(w * new Matrix2(200, 0, 0, 200), _square);

        }

    }
}
