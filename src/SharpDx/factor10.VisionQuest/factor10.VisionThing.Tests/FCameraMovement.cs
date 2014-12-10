using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using factor10.VisionThing;
using factor10.VisionThing.Tests;
using SharpDX;

namespace CircleMasterApp
{
    public partial class FCameraMovement : Form
    {
        private PointF[] _pts = new PointF[4];

        private float _factor;
        private float _angle;
        private Vector3 _out = Vector3.Up;

        private Camera _camera;
        private MoveCameraArc _moveCameraArc;
        private DateTime _time;

        public FCameraMovement()
        {
            new Class1().Z();

            InitializeComponent();

            _pts[1] = new PointF(300, 200);
            copyToCamera();
        }

        private void setPoint(Graphics g, int idx)
        {
            var p = _pts[idx];
            g.FillEllipse(Brushes.Black, p.X - 2, p.Y - 2, 5, 5);
            g.DrawString(idx.ToString(), Font, Brushes.Black, p.X + 5, p.Y);
        }

        private void copyToCamera()
        {
            var vs = _pts.Select(_ => new Vector3(_.X, 0, _.Y)).ToArray();
            _camera = new Camera(Vector2.One, vs[2], Vector3.ForwardRH);
            var d = vs[0] - vs[1];
            _moveCameraArc = new MoveCameraArc(_camera, 5f.Time(), vs[0], d, d.Length());
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            for (var i = 0; i < 4; i++)
                setPoint(e.Graphics, i);

            e.Graphics.DrawLine(Pens.Black, _pts[1], _pts[0]);
            e.Graphics.DrawLine(Pens.Black, _pts[1], _pts[2]);

            var dx = _pts[3].X - _pts[1].X;
            var dy = _pts[3].Y - _pts[1].Y;
            var r = (float) Math.Sqrt(dx*dx + dy*dy);
            e.Graphics.DrawEllipse(Pens.Black, _pts[3].X - r, _pts[3].Y - r, 2*r, 2*r);

            var v1 = new Vector3(-_pts[3].X + _pts[1].X, 0, -_pts[3].Y + _pts[1].Y);
            var mx = Matrix.RotationAxis(Vector3.Up, (_angle*2)*(1 - _factor));
            var v = Vector3.TransformNormal(v1, mx);

            e.Graphics.DrawLine(Pens.Black, _pts[3], new PointF(_pts[3].X + v.X, _pts[3].Y + v.Z));

            try
            {
                var dt = (float)(DateTime.Now - _time).TotalSeconds;

                var m = _moveCameraArc;
                e.Graphics.DrawEllipse(Pens.Blue, m._origo.X - m._diameter/2, m._origo.Z - m._diameter/2,
                    m._diameter, m._diameter);
                var t = m._endPoint - m._incomingDirection*m._incomingLength;
                e.Graphics.DrawLine(Pens.Blue, m._startPoint.X, m._startPoint.Z, t.X, t.Z);
                e.Graphics.DrawLine(Pens.Blue, m._endPoint.X, m._endPoint.Z, t.X, t.Z);

                m.Move(dt);
                e.Graphics.FillRectangle(Brushes.Blue, _camera.Position.X, _camera.Position.Z, 3, 3);

                var mx2 = Matrix.RotationAxis(m._outVector, 0);
                var q2 = m._origo + Vector3.TransformNormal(m._startPoint - m._origo, mx2);
                var mx3 = Matrix.RotationAxis(m._outVector, m._angle*2);
                var q3 = m._origo + Vector3.TransformNormal(m._startPoint - m._origo, mx3);

                e.Graphics.FillRectangle(Brushes.Red, q2.X, q2.Z, 5, 5);
                e.Graphics.FillRectangle(Brushes.Red, q3.X, q3.Z, 5, 5);

                //var v0 = new Vector3(_pts[3].X - _pts[1].X, 0, _pts[3].Y - _pts[1].Y);
                //var q0 = new Quaternion(v0, 0);
                //var q1 = new Quaternion(v1, 0);
                //var q = Quaternion.Slerp(q0, q1, _factor);
                //e.Graphics.DrawLine(Pens.Black, _pts[3], new PointF(_pts[3].X + q.X, _pts[3].Y + q.Z));
            }
            catch (Exception)
            {
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _time = DateTime.Now;

            if (e.Button == MouseButtons.Left)
                _pts[0] = e.Location;
            if (e.Button == MouseButtons.Middle)
                _pts[1] = e.Location;
            if (e.Button == MouseButtons.Right)
                _pts[2] = e.Location;

            var v0 = new Vector3(_pts[1].X - _pts[0].X, 0, _pts[1].Y - _pts[0].Y);
            var v1 = new Vector3(_pts[1].X - _pts[2].X, 0, _pts[1].Y - _pts[2].Y);
            _out = Vector3.Cross(v0, v1);
            var sinAngle = _out.Length()/(v0.Length()*v1.Length());
            _angle = (float)Math.Asin(sinAngle);
            System.Diagnostics.Debug.Print("{0:0.0}", MathUtil.RadiansToDegrees(_angle));

            var v2 = Vector3.Cross(v1, _out);
            v2.Normalize();
            System.Diagnostics.Debug.Print("v2:{0}  d:{1}", v2, v1.Length());
            v2 *= v1.Length() / (float)Math.Tan(Math.PI - _angle) / 2;

            _pts[3] = new PointF(v2.X + (_pts[2].X + _pts[1].X)/2, v2.Z + (_pts[2].Y + _pts[1].Y)/2);

            copyToCamera();
            System.Diagnostics.Debug.Print("MC {0:0.0}", MathUtil.RadiansToDegrees(_moveCameraArc._angle));

            Invalidate();
        }

        public static PointF? LineIntersect(PointF x1, PointF x2, PointF y1, PointF y2)
        {
            var dx = x2.X - x1.X;
            var dy = x2.Y - x1.Y;
            var da = y2.X - y1.X;
            var db = y2.Y - y1.Y;

            if (Math.Abs(da * dy - db * dx)<0.0001f)
                return null; // The segments are parallel.

            var s = (dx * (y1.Y - x1.Y) + dy * (x1.X - y1.X)) / (da * dy - db * dx);
            var t = (da * (x1.Y - y1.Y) + db * (y1.X - x1.X)) / (db * dx - da * dy);

            if (s < 0 || s > 1 || t < 0 || t > 1)
                return null;

            return new PointF(x1.X + t * dx, x1.Y + t * dy);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _factor += 0.01f;
            if (_factor > 1)
                _factor -= 1;
            Invalidate();
        }

    }

}
