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
        private Point _pt1;
        private Point _pt2;

        public FCardinalSplines()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pea)
        {
            var r = Math.Min(ClientSize.Width, ClientSize.Height)/2 - 10;
            if (r < 10)
                return;
            var middle = new Point(r + 10, r + 10);
            pea.Graphics.DrawEllipse(Pens.Black, 10, 10, r*2, r*2);
            pea.Graphics.DrawLine(Pens.Black, middle, _pt1);
            pea.Graphics.DrawLine(Pens.Black, middle, _pt2);

            var a = x(pea.Graphics, middle, _pt1, "A");
            var b = x(pea.Graphics, middle, _pt2, "B");
            var angle = a - b;
            if (angle > 180)
                angle = angle - 360;
            if (angle < -180)
                angle = angle + 360;
            pea.Graphics.DrawString(string.Format("A-B = {0:0}", angle), Font, Brushes.Black, Point.Empty);
        }

        private double x(Graphics g, Point m, Point p, string name)
        {
            var angle = 180 * Math.Atan2(m.Y - p.Y, -m.X + p.X) / Math.PI;
            g.DrawString(string.Format("{0}: {1:0}",name, angle), Font, Brushes.Black, p);
            return angle;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _pt1 = e.Location;
            if (e.Button == MouseButtons.Right)
                _pt2 = e.Location;
            Invalidate();
        }
    }
}
