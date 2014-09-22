using System;
using System.Windows.Forms;
using SharpDX.Windows;

namespace factor10.VisionQuest
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using(var game = new VisionQuestGame())
            {
                var form = new FMain(game.Data);
                form.Show();

                game.IsMouseVisible = true;
                game.Run(form.RenderControl);
            }
        }
    }
}
