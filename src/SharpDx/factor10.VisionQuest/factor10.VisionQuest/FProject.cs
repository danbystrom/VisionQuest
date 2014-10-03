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

        public static void showDialog(Form parent, string filename)
        {
            var vprogram = new VProgram(filename);
            using (var dlg = new FProject())
            {
                foreach (var vass in vprogram.VAssemblies)
                    dlg.lstAssemblies.Items.Add(vass.Name);
                dlg.ShowDialog(parent);
            }
        }

    }

}
