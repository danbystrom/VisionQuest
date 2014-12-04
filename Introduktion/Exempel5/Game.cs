using System;
using System.Collections.Generic;
using SharpDX;

namespace Exempel5
{
    public class Game
    {
        private float _time;
        public Matrix Projection = Matrix.Identity;

        private readonly List<Line> _cube = new List<Line>();

        public bool RotateXy;
        public bool MoveCamera;

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
            Matrix view;
            if (MoveCamera)
            {
                var cameraX = (float) Math.Sin(_time)*400;
                var cameraZ = (float) Math.Cos(_time)*400;
                var target = new Vector3(0, 0, 500);
                view = Matrix.LookAtLH(target + new Vector3(cameraX, 0, cameraZ), target, Vector3.Up);
            }
            else
                view = Matrix.Identity;

            var extraRot = RotateXy
                ? Matrix.RotationX(_time*0.7f)*Matrix.RotationY(_time*0.8f)
                : Matrix.Identity;
            painter.Paint(Matrix.Scaling(20, 20, 80)*extraRot*Matrix.RotationZ(_time)*Matrix.Translation(0, 0, 500), view, Projection, _cube);

            extraRot = RotateXy
                ? Matrix.RotationX(_time * 0.6f) * Matrix.RotationY(_time * 0.7f)
                : Matrix.Identity;
            painter.Paint(Matrix.Scaling(40) * extraRot * Matrix.RotationZ(_time * 1.5f) * Matrix.Translation(200, 0, 500), view, Projection, _cube);

            extraRot = RotateXy
                ? Matrix.RotationX(_time * 0.1f) * Matrix.RotationY(_time * 0.3f)
                : Matrix.Identity;
            painter.Paint(Matrix.Scaling(40) * extraRot * Matrix.RotationZ(_time * 1.5f) * Matrix.Translation(-200, 0, 500), view, Projection, _cube);
        }

    }

}
