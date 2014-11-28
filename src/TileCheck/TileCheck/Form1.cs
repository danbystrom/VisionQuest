using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TileCheck
{
    public partial class Form1 : Form
    {
        private Bitmap _bmp;

        public Form1()
        {
            InitializeComponent();
            AllowDrop = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_bmp == null)
                return;
            var w = _bmp.Width;
            var h = _bmp.Height;
            var nx = 1 + ClientSize.Width/w;
            var ny = 1 + ClientSize.Height/h;
            for (var y = 0; y < ny; y++)
                for (var x = 0; x < nx; x++)
                    e.Graphics.DrawImage(
                        _bmp,
                        new Rectangle(x*w, y*h, w, h),
                        new Rectangle(0, 0, w, h),
                        GraphicsUnit.Pixel);
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            foreach(var fn in (string[])e.Data.GetData(DataFormats.FileDrop))
                try
                {
                    _bmp = (Bitmap)Image.FromFile(fn);
                    Invalidate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
        }

    }

}
