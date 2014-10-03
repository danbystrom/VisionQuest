using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using factor10.VisionThing;

namespace factor10.VisionQuest.x
{
    public partial class FManageProjects : Form
    {
        public FManageProjects()
        {
            InitializeComponent();
        }

        public static void showDialog(Form parent, Storage storage)
        {
            using (var dlg = new FManageProjects())
            {
                dlg.loadProjects( Project.LoadAll(storage.SafeProjectFolder()));
                dlg.Show(parent);
            }
        }

        private void loadProjects(IEnumerable<Project> projects)
        {
            lvProjects.BeginUpdate();
            foreach (var proj in projects)
            {
                var lvi = lvProjects.Items.Add(proj.Name);
                lvi.SubItems[1].Text = proj.Created.ToIsoString();
            }
            lvProjects.EndUpdate();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = "Select application";
                dlg.CheckFileExists = true;
                dlg.Filter = "Assembly|*.exe;*.dll";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    FProject.showDialog(this, dlg.FileName);
            }
        }

    }

}
