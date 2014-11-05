using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using factor10.VisionaryHeads;

namespace factor10.VisionQuest
{
    public partial class FProject : Form
    {
        public FProject()
        {
            InitializeComponent();
        }

        public static void showDialog(Form parent, Storage storage, string filename)
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

    }

}