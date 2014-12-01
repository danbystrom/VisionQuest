using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using WindowsFormsApplication1;

namespace Exempel2
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

        public bool UseTranslation;

        public Game()
        {
        }

        public void Update(float elapsedTime)
        {
            _time += elapsedTime;
        }

        public void Draw(ObjectPainter painter)
        {
            var angle = _time*90/(float) Math.PI;
            var world = new Matrix();
            if (UseTranslation)
                world.Translate(_time * 25, _time * 30);
            world.Rotate(angle);
            var scale = new Matrix();
            scale.Scale(100, 100);

            world.Multiply(scale);
            painter.Paint(world, _square);

            angle = _time*1.5f*90/(float) Math.PI;
            world = new Matrix();
            if (UseTranslation)
                world.Translate(_time * 20, _time * 25);
            world.Rotate(angle);
            scale = new Matrix();
            scale.Scale(200, 200);

            world.Multiply(scale);
            painter.Paint(world, _square);

        }

    }

}
