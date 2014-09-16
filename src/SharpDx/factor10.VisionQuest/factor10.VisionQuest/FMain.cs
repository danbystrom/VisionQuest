using System;
using System.Windows.Forms;
using ShaderLinking;
using SharpDX.Windows;

namespace factor10.VisionQuest
{

    internal partial class FMain : Form, IMessageFilter
    {
        private readonly SharedData _data;
        private readonly RenderControl _renderControl;

        public FMain(SharedData data)
        {
            InitializeComponent();
            Application.AddMessageFilter(this);

            _data = data;

            // create the RenderControl
            _renderControl = new MyRenderControl();
            _renderControl.Dock = DockStyle.Fill;
            pnRenderControlPanel.Controls.Add(_renderControl);
        }

        // Expose the render conttrol to the game class
        public RenderControl RenderControl
        {
            get { return _renderControl; }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BtnRebuildClick(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool PreFilterMessage(ref Message m)
        {
            if ((m.Msg == 256 || m.Msg == 257) && m.WParam.ToInt32() == (int) Keys.Tab)
            {
                if (m.Msg == 256 && (ModifierKeys == Keys.None || ModifierKeys == Keys.Shift))
                    SelectNextControl(ActiveControl, ModifierKeys == Keys.None, true, true, true);
                return true;
            }
            if (m.Msg == 256) // && m.WParam.ToInt32() == (int) Keys.F4)
            {
                //Close();
                //return true;
            }
            return false;
        }

    }

}
