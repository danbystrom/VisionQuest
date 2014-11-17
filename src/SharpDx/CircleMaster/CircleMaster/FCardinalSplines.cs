using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CircleMasterApp
{
    public partial class FCardinalSplines : Form
    {
        private PointF _c0 = new PointF(100, 100);
        private PointF _c1 = new PointF(300, 100);
        private PointF _c2 = new PointF(100, 200);
        private PointF _c3 = new PointF(300, 200);

        private PointF _ptL;
        private PointF _ptR;

        private List<PointF> _list = new List<PointF>();
 
        public FCardinalSplines()
        {
            InitializeComponent();
        }

        private void setPoint(Graphics g, PointF p)
        {
            g.FillEllipse(Brushes.Black, p.X - 2, p.Y - 2, 5, 5);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.Black, _c0.X, _c0.Y, _c3.X - _c0.X, _c3.Y - _c0.Y);

            setPoint(e.Graphics, _ptL);
            setPoint(e.Graphics, _ptR);

            foreach (var p in _list)
                setPoint(e.Graphics, p);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _ptL = e.Location;
            if (e.Button == MouseButtons.Right)
                _ptR = e.Location;

            _list = new List<PointF?>
            {
                LineIntersect(_ptL, _ptR, _c0, _c1),
                LineIntersect(_ptL, _ptR, _c0, _c2),
                LineIntersect(_ptL, _ptR, _c3, _c1),
                LineIntersect(_ptL, _ptR, _c3, _c2)
            }.Where(_ => _ != null).Select(_ => _.Value).ToList();

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

    }

}
