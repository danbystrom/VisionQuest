using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CircleMasterApp
{
    public partial class FMultitextureTest : Form
    {
        public FMultitextureTest()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            const int H = 170;
            var levels = new[] {(float) numericUpDown1.Value, (float) numericUpDown2.Value, (float) numericUpDown3.Value, (float) numericUpDown4.Value};
            levels = levels.Select(_ => 256*_/100).ToArray();

            var xpos = (float)numericUpDown1.Right + 10;
            var xdiff = (ClientSize.Width - 10 - xpos)/256f;
            if (xdiff < 0.1f)
                return;

            for (var j = 0; j < 4; j++)
            {
                var y = (j+1) * (H+10);
                e.Graphics.DrawRectangle(
                    Pens.Black,
                    xpos,
                    y-H,
                    xdiff*255,
                    H);
                e.Graphics.DrawLine(
                    Pens.Black,
                    xpos+levels[j]*xdiff,
                    y-H,
                    xpos + levels[j] * xdiff,
                    y);
            }

            float[] lastValues = null;

            for (var i = 0; i < 255; i++)
            {
                var t = levels.Select(_ => clamp(1.0f - Math.Abs(i - _)/255f)).ToArray();
                var max = t.Max();

                t = t.Select(_ => (float)Math.Pow(_/max,5)).ToArray();
                var tot = 1 / (t.Sum() + 0.00001f);
                if (tot < 0.5f)
                {

                }

                var values = t.Select(_ => _*tot).ToArray();
                if (lastValues == null)
                {
                    lastValues = values;
                    continue;
                }

                xpos += xdiff;
                for (var j = 0; j < 4; j++)
                {
                    var y = (j + 1) * (H+10);
                    e.Graphics.DrawLine(
                        Pens.Black,
                        xpos - xdiff,
                        y-H*lastValues[j],
                        xpos,
                        y-H*values[j]);
                }

                lastValues = values;
            }

        }

        private float clamp(float x)
        {
            return x < 0 ? 0 : x > 1 ? 1 : x;
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

    }
}
