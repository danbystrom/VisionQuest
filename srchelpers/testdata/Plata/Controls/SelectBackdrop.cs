using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Plata.Controls
{
    public partial class SelectBackdrop : UserControl
    {
        public SelectBackdrop()
        {
            InitializeComponent();
        }

        public string Backdrop
        {
            get { return cbo.SelectedItem as string; }
            set
            {
                if (cbo.Items.Count == 0)
                    try
                    {
                        cbo.Items.Add("");
                        foreach (var fn in Directory.GetFiles(Path.Combine(Global.Preferences.MainPath, "_backdrops"), "*.jpg"))
                            cbo.Items.Add(Path.GetFileNameWithoutExtension(fn));
                    }
                    catch
                    {
                    }
                cbo.SelectedItem = value;
                if (cbo.SelectedIndex < 0)
                    cbo.SelectedIndex = 0;
            }
        }

        private void cbo_Format(object sender, ListControlConvertEventArgs e)
        {
            if (string.IsNullOrEmpty(e.ListItem as string))
                e.Value = "(ingen)";
        }

    }

}
