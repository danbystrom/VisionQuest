using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using factor10.VisionaryHeads;
using Microsoft.Win32;

namespace factor10.VisionQuest.Forms
{
    public partial class FGenerateMetrics : BaseForm
    {
        private class Item
        {
            public string Assembly;
            public string MetricsFile;
            public ListViewItem ListViewItem;
        }

        private readonly Queue<Item> _que = new Queue<Item>();
        private string _metricsExe;

        public FGenerateMetrics()
        {
            InitializeComponent();
        }

        public static DialogResult DoDialog(Form parent, List<Tuple<string, string>> list)
        {
            if (!list.Any())
                return DialogResult.OK;
            using (var dlg = new FGenerateMetrics())
            {
                foreach (var itm in list)
                {
                    var item = new Item
                    {
                        Assembly = itm.Item1,
                        MetricsFile = itm.Item2,
                        ListViewItem = dlg.lvProjects.Items.Add("Queued")
                    };
                    item.ListViewItem.SubItems.Add(item.Assembly);
                    dlg._que.Enqueue(item);
                }
                return dlg.ShowDialog(parent);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            _metricsExe = findMetricsExe();
            if (_metricsExe == null)
            {
                MessageBox.Show(this, "You must install \"Visual Studio Code Metrics Powertool for Visual Studio\" in order to calculate code metrics.");
                DialogResult = DialogResult.Cancel;
                return;
            }

            doNext();
        }

        private string findMetricsExe()
        {
            foreach (var vsfolder in vsFolders())
            {
                var metrics = Path.Combine(vsfolder, "Team Tools", "Static Analysis Tools", "FxCop", "metrics.exe");
                if (File.Exists(metrics))
                    return metrics;
            }
            return null;
        }

        private static IEnumerable<string> vsFolders()
        {
            using (var regbase = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            using (var vskey = regbase.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio"))
            {
                if (vskey == null)
                    yield break;
                foreach (var vsversion in vskey.GetSubKeyNames())
                    using (var q = vskey.OpenSubKey(vsversion))
                    {
                        var s = q.GetValue("ShellFolder") as string;
                        if (s != null)
                            yield return s;
                    }
            }
        }

        private void doNext(Item previous=null, string result=null)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => doNext(previous, result)));
                return;
            }

            if (previous != null)
            {
                previous.ListViewItem.Text = result == null ? "OK" : "Error";
                if (result != null)
                    MessageBox.Show(this, result);
            }

            if (!_que.Any())
            {
                DialogResult = DialogResult.OK;
                return;
            }

            var itm = _que.Dequeue();
            itm.ListViewItem.Text = "Working";

            new Thread(obj =>
            {
                try
                {
                    GenerateMetrics.RunFxCopMetrics(_metricsExe, itm.Assembly, itm.MetricsFile);
                    doNext(itm);
                }
                catch (Exception ex)
                {
                    doNext(itm, ex.ToString());
                }
            }).Start();
        }

    }

}