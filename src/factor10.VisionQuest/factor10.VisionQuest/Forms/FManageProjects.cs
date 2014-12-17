using System;
using System.Collections.Generic;
using System.Windows.Forms;
using factor10.VisionThing;

namespace factor10.VisionQuest.Forms
{
    public partial class FManageProjects : BaseForm
    {
        private Storage _storage;

        public FManageProjects()
        {
            InitializeComponent();
        }

        public static Project DoDialog(Form parent, Storage storage)
        {
            using (var dlg = new FManageProjects())
            {
                dlg._storage = storage;
                dlg.loadProjects(Project.LoadAll(storage.SafeProjectFolder()));
                if (dlg.ShowDialog(parent) == DialogResult.OK && dlg.lvProjects.SelectedItems.Count==1)
                    return dlg.lvProjects.SelectedItems[0].Tag as Project;
                return null;
            }
        }

        private void loadProjects(IEnumerable<Project> projects)
        {
            lvProjects.BeginUpdate();
            foreach (var proj in projects)
            {
                var lvi = lvProjects.Items.Add(proj.Name);
                lvi.Tag = proj;
                lvi.SubItems.Add(proj.Created.ToIsoString());
                lvi.SubItems.Add(proj.Accessed.ToIsoString());
            }
            lvProjects.EndUpdate();
            if (lvProjects.Items.Count != 0)
                lvProjects.Items[0].Selected = true;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = "Select application";
                dlg.CheckFileExists = true;
                dlg.Filter = @"Assembly|*.exe;*.dll";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    FProject.DoDialog(this, _storage, dlg.FileName);
            }
        }

        private void lvProjects_DoubleClick(object sender, EventArgs e)
        {
            btnOK.PerformClick();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lvProjects.SelectedItems.Count != 1)
                DialogResult = DialogResult.None;
        }

    }

}
