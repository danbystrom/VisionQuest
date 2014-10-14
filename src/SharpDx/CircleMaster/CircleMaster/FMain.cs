using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using factor10.VisionQuest.GroundControl;

namespace CircleMasterApp
{
    public partial class FMain : Form
    {
        private readonly CircleMaster<int> _circles;
        private Circle<int> _circle;

        public FMain()
        {
            InitializeComponent();

            _circles = new CircleMaster<int>(_ =>
            {
                _circle = _;
                Refresh();
                _circle = null;
            });
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var wh = ClientSize.Width/2;
            var hh = ClientSize.Height/2;
            e.Graphics.TranslateTransform(wh, hh);
            e.Graphics.DrawLine(Pens.Black, 0, -hh, 0, hh);
            e.Graphics.DrawLine(Pens.Black, -wh, 0, wh, 0);

            var cs = _circles.Circles.ToList();
            if(_circle!=null)
                cs.Add(_circle);
            foreach (var circle in cs)
                e.Graphics.DrawEllipse(Pens.Black, circle.BoundingRectangle);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _circles.Drop(e.X - ClientSize.Width/2, e.Y - ClientSize.Height/2, (int) numRadius.Value);
            Invalidate();
        }

    }

}
