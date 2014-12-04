using System;
using System.Windows.Forms;

namespace Exempel4
{
    public partial class Exempel4 : Form
    {
        private DateTime _lastTime;

        private readonly Game _game;

        public Exempel4()
        {
            InitializeComponent();
            _lastTime = DateTime.Now;
            _game = new Game();
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
            _game.Draw(new ObjectPainter(e.Graphics));
        }

        private void chkRotXy_CheckedChanged(object sender, EventArgs e)
        {
            _game.RotateXy = chkRotXy.Checked;
        }

    }

}
