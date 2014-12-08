using System;
using System.Drawing;
using System.Windows.Forms;
using SharpDX;

namespace CircleMasterApp
{
    public partial class FCardinalSplines : Form
    {
        private PointF[] _pts = new PointF[4];

        private float _factor;
        private float _angle;
        private Vector3 _out = Vector3.Up;

        public FCardinalSplines()
        {
            InitializeComponent();
        }

        private void setPoint(Graphics g, int idx)
        {
            var p = _pts[idx];
            g.FillEllipse(Brushes.Black, p.X - 2, p.Y - 2, 5, 5);
            g.DrawString(idx.ToString(), Font, Brushes.Black, p.X + 5, p.Y);
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

            var v1 = new Vector3(-_pts[3].X + _pts[2].X, 0, -_pts[3].Y + _pts[2].Y);
            var m = Matrix.RotationAxis(Vector3.Up, (-_angle*2)*_factor);
            var v = Vector3.TransformNormal(v1, m);

            e.Graphics.DrawLine(Pens.Black, _pts[3], new PointF(_pts[3].X + v.X, _pts[3].Y + v.Z));

            //try
            //{
            //    var v0 = new Vector3(_pts[3].X - _pts[1].X, 0, _pts[3].Y - _pts[1].Y);
            //    var q0 = new Quaternion(v0, 0);
            //    var q1 = new Quaternion(v1, 0);
            //    var q = Quaternion.Slerp(q0, q1, _factor);
            //    e.Graphics.DrawLine(Pens.Black, _pts[3], new PointF(_pts[3].X + q.X, _pts[3].Y + q.Z));
            //}
            //catch (Exception)
            //{
            //}
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
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
            v2 *= v1.Length()/(float) Math.Tan(Math.PI - _angle)/2;

            _pts[3] = new PointF(v2.X + (_pts[2].X + _pts[1].X)/2, v2.Z + (_pts[2].Y + _pts[1].Y)/2);

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
