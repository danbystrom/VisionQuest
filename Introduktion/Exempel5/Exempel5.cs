using System;
using System.Windows.Forms;
using SharpDX;

namespace Exempel5
{
    public partial class Exempel5 : Form
    {
        private DateTime _lastTime;

        private readonly Game _game;

        public Exempel5()
        {
            _game = new Game();
            InitializeComponent();
            _lastTime = DateTime.Now;
            Application.Idle += Application_Idle;
        }

        void Application_Idle(object sender, EventArgs e)
        {
            var thisTime = DateTime.Now;
            var elapsedTime = (float)(thisTime - _lastTime).TotalSeconds;
            _lastTime = thisTime;
            if(!chkPause.Checked)
                _game.Update(elapsedTime);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _game.Draw(new ObjectPainter(e.Graphics, ClientSize.Height, ClientSize.Width));
        }

        protected override void OnResize(EventArgs e)
        {
            _game.Projection =  Matrix.PerspectiveFovLH(MathUtil.PiOverFour, ClientSize.Width / (float)ClientSize.Height, 10, 2000);
        }

        private void chkRotXy_CheckedChanged(object sender, EventArgs e)
        {
            _game.RotateXy = chkRotXy.Checked;
        }

        private void chkMoveCamera_CheckedChanged(object sender, EventArgs e)
        {
            _game.MoveCamera = chkMoveCamera.Checked;
        }

    }

}
