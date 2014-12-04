using System.Collections.Generic;
using SharpDX;

namespace Exempel4
{
    public class Game
    {
        private float _time;

        private readonly List<Line> _cube = new List<Line>();

        public bool RotateXy;
        public bool UseProjection;

        public Game()
        {
            var xyz = new Vector3(-1, -1, -1);
            var xYz = new Vector3(-1, 1, -1);
            var XYz = new Vector3(1, 1, -1);
            var Xyz = new Vector3(1, -1, -1);
            var xyZ = new Vector3(-1, -1, 1);
            var xYZ = new Vector3(-1, 1, 1);
            var XYZ = new Vector3(1, 1, 1);
            var XyZ = new Vector3(1, -1, 1);

            _cube.Add(new Line(xyz, Xyz));
            _cube.Add(new Line(xyz, xYz));
            _cube.Add(new Line(xyz, xyZ));

            _cube.Add(new Line(XYz, xYz));
            _cube.Add(new Line(XYz, Xyz));
            _cube.Add(new Line(XYz, XYZ));

            _cube.Add(new Line(xYZ, XYZ));
            _cube.Add(new Line(xYZ, xyZ));
            _cube.Add(new Line(xYZ, xYz));

            _cube.Add(new Line(XyZ, xyZ));
            _cube.Add(new Line(XyZ, XYZ));
            _cube.Add(new Line(XyZ, Xyz));
        }

        public void Update(float elapsedTime)
        {
            _time += elapsedTime;
        }

        public void Draw(ObjectPainter painter)
        {
            var extraRot = RotateXy
                ? Matrix.RotationX(_time*0.9f)*Matrix.RotationY(_time*0.8f)
                : Matrix.Identity;
            painter.Paint(Matrix.Scaling(50, 50, 200)*extraRot*Matrix.RotationZ(_time)*Matrix.Translation(200, 300, 500), _cube);

            extraRot = RotateXy
                ? Matrix.RotationX(_time*1.1f)*Matrix.RotationY(_time*0.7f)
                : Matrix.Identity;
            painter.Paint(Matrix.Scaling(200)*extraRot*Matrix.RotationZ(_time*1.5f)*Matrix.Translation(600, 300, 500), _cube);
        }

    }

}
