﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using factor10.VisionaryHeads;
using factor10.VisionQuest.Commands;
using factor10.VisionThing;
using SharpDX.Windows;

namespace factor10.VisionQuest
{

    internal partial class FMain : Form, IMessageFilter
    {
        private readonly SharedData _data;
        private readonly RenderControl _renderControl;

        private VProgram _vprogram;

        public FMain(SharedData data)
        {
            InitializeComponent();
            Application.AddMessageFilter(this);

            var wa = SystemInformation.WorkingArea;
            wa.Inflate(-wa.Width / 20, -wa.Height / 20);
            Bounds = wa;

            _data = data;
            _data.Size = new Size(ClientSize.Width - pnRenderControlPanel.Left, ClientSize.Height - pnRenderControlPanel.Top);
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

            numSurfaceSize.Value = _data.WaterSurfaceSize;
            numSurfaceScale.Value = _data.WaterSurfaceScale;
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
            var project = FManageProjects.showDialog(this, _data.Storage);
            if (project != null)
            {
                _vprogram = new VProgram(project.Assemblies.First().FullFilename);
                _data.Commands.Enqueue(new LoadProgramCommand(_vprogram));                
            }
        }

        public bool PreFilterMessage(ref Message m)
        {
            if ((m.Msg == 256 || m.Msg == 257) && m.WParam.ToInt32() == (int) Keys.Tab)
            {
                if (m.Msg == 256 && (ModifierKeys == Keys.None || ModifierKeys == Keys.Shift))
                    SelectNextControl(ActiveControl, ModifierKeys == Keys.None, true, true, true);
                return true;
            }

            if (m.Msg == 256 && m.WParam.ToInt32() == (int) Keys.Enter)
            {
                if (txtSearchMethod.Focused || lstMethods.Focused)
                    lstMethods_DoubleClick(this, EventArgs.Empty);
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
            _vprogram = new VProgram(@"C:\proj\photomic.old\src\Plata\bin\Release\Plåta.exe");
            _data.Commands.Enqueue(new LoadProgramCommand(_vprogram));
        }

        private void chkHiddenVater_CheckedChanged(object sender, EventArgs e)
        {
            _data.HiddenWater = chkHiddenWater.Checked;
        }

        private void numSurfaceSize_ValueChanged(object sender, EventArgs e)
        {
            _data.WaterSurfaceSize = (int)numSurfaceSize.Value;
        }

        private void numSurcafeScale_ValueChanged(object sender, EventArgs e)
        {
            _data.WaterSurfaceScale = (int) numSurfaceScale.Value;
        }

        private void txtSearchMethod_TextChanged(object sender, EventArgs e)
        {
            lstMethods.Items.Clear();
            var s = txtSearchMethod.Text.Trim();
            if (s.Length<2 || _vprogram==null)
                return;
            var list1 = new List<VClass>();
            var list2 = new List<VClass>();
            foreach (var vc in _vprogram.VAssemblies.Where(_ => !_.Is3DParty).SelectMany(_ => _.VClasses))
                switch (vc.Name.IndexOf(s, StringComparison.OrdinalIgnoreCase))
                {
                    case 0:
                        list1.Add(vc);
                        break;
                    case -1:
                        break;
                    default:
                        list2.Add(vc);
                        break;
                }
            addToListBox(list1);
            addToListBox(list2);
            if (lstMethods.Items.Count != 0)
                lstMethods.SelectedIndex = 0;
        }

        private void addToListBox(List<VClass> list)
        {
            list.Sort((x, y) => x.Name.CompareTo(y.Name));
            foreach (var m in list)
                lstMethods.Items.Add(m);
        }

        private void lstMethods_Format(object sender, ListControlConvertEventArgs e)
        {
            var vc = (VClass) e.ListItem;
            e.Value = "{0} in {1}".Fmt(vc.Name, vc.VAssembly.Name);
        }

        private void txtSearchMethod_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Down:
                        lstMethods.SelectedIndex++;
                        e.Handled = true;
                        break;
                    case Keys.Up:
                        lstMethods.SelectedIndex--;
                        e.Handled = true;
                        break;
                }
            }
            catch
            {
            }
        }

        private void lstMethods_DoubleClick(object sender, EventArgs e)
        {
            var vc = (VClass) lstMethods.SelectedItem;
            if (vc != null)
                _data.Commands.Enqueue(new GotoClassCommand(vc));
        }

    }

}
