using System;
using System.Drawing;
using System.Windows.Forms;
using factor10.VisionaryHeads;
using factor10.VisionQuest.x;
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

            var wa = SystemInformation.WorkingArea;
            wa.Inflate(-wa.Width / 20, -wa.Height / 20);
            Bounds = wa;

            _data = data;
            _data.Size = new Size(ClientSize.Width - pnRenderControlPanel.Left - 10, ClientSize.Height - pnRenderControlPanel.Top - 10);
            _data.Size = new Size(_data.Size.Width/2, _data.Size.Height/2);
            _data.Storage = Storage.Load();

            // create the RenderControl
            _renderControl = new RenderControl
            {
                Dock = DockStyle.Fill,
                Size = data.Size,
                Location = Point.Empty
            };

            pnRenderControlPanel.Size = data.Size;
            pnRenderControlPanel.Controls.Add(_renderControl);

            optAllLines.CheckedChanged += (sender, args) => uiChanged();
            optNoLines.CheckedChanged += (sender, args) => uiChanged();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            pnRenderControlPanel.Size = new Size(pnRenderControlPanel.Width * 2, pnRenderControlPanel.Height * 2);
        }

        // Expose the render conttrol to the game class
        public RenderControl RenderControl
        {
            get { return _renderControl; }
        }

        public void Render()
        {
            //_renderControl.Render();
        }

        public void uiChanged()
        {
            _data.Storage.DrawLines = optAllLines.Checked;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FManageProjects.showDialog(this, _data.Storage);
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

        private void btnProperties_Click(object sender, EventArgs e)
        {
            _data.LoadProgram = new VProgram(@"C:\proj\photomic.old\src\Plata\bin\Release\Plåta.exe");
        }

    }

}
