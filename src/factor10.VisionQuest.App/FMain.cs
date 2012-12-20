using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using factor10.VisionQuest.Metrics;
using factor10.VisionaryHeads;

namespace factor10.VisionQuest.App
{
    public partial class FMain : Form
    {
        public FMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var vprogram = new VProgram(textBox1.Text);
        }
    }
}
