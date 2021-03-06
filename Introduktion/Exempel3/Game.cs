﻿using System.Collections.Generic;
using SharpDX;

namespace Exempel3
{
    public class Game
    {
        private float _time;

        private readonly List<Line> _square = new List<Line>
        {
            new Line(-1, -1, 0, 1, -1, 0),
            new Line(1, -1, 0, 1, 1, 0),
            new Line(1, 1, 0, -1, 1, 0),
            new Line(-1, 1, 0, -1, -1, 0)
        };

        public void Update(float elapsedTime)
        {
            _time += elapsedTime;
        }

        public void Draw(ObjectPainter painter)
        {
            painter.Paint(Matrix.RotationZ(_time) * Matrix.Scaling(100) * Matrix.Translation(_time * 25, _time * 30, 0), _square);
            painter.Paint(Matrix.RotationZ(_time*1.5f) * Matrix.Scaling(200) * Matrix.Translation(_time * 20, _time * 25, 0), _square);
        }

    }

}
