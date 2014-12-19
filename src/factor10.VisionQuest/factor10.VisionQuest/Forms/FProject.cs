using System.IO;
using System.Linq;
using System.Windows.Forms;
using factor10.VisionaryHeads;
using factor10.VisionQuest.Unsorted;

namespace factor10.VisionQuest.Forms
{
    public partial class FProject : BaseForm
    {
        public FProject()
        {
            InitializeComponent();
        }

        public static void DoDialog(Form parent, Storage storage, string filename)
        {
            var vprogram = new VProgram(filename);
            using (var dlg = new FProject())
            {
                dlg.Text = vprogram.VAssemblies.First().Name;
                foreach (var vass in vprogram.VAssemblies)
                    dlg.lstAssemblies.Items.Add(new ProjectAssembly {Name = vass.Name, FullFilename = vass.Filename});
                if (dlg.ShowDialog(parent) == DialogResult.OK)
                    dlg.save(storage.ProjectFolder);
            }
        }

        private void save(string folder)
        {
            var project = new Project
            {
                Name = txtProjectName.Text.Trim(),
                Assemblies = lstAssemblies.Items.Cast<ProjectAssembly>().ToList()
            };
            project.Save(folder);
        }

        private void txtProjectName_TextChanged(object sender, System.EventArgs e)
        {
            btnOK.Enabled = !txtProjectName.Text.Any(Path.GetInvalidFileNameChars().Contains);
        }

    }

}