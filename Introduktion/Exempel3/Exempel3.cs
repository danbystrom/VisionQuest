using System;
using System.Windows.Forms;
using Exempel2;

namespace Exempel3
{
    public partial class Exempel3 : Form
    {
        private DateTime _lastTime;

        private readonly Game _game;

        public Exempel3()
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
            _game.Update(elapsedTime);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _game.Draw(new ObjectPainter(e.Graphics, ClientSize.Height));
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == 't')
                _game.UseTranslation = !_game.UseTranslation;
        }

    }

}
