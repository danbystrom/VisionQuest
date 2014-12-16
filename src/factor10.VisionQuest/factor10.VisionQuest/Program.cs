using System;
using System.Windows.Forms;

namespace factor10.VisionQuest
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var data = new SharedData();
            var form = new FMain(data);

            using (var game = new VisionQuestGame(data))
            {
                form.Show();

                game.IsMouseVisible = true;
                game.Run(form.RenderControl);
            }
        }
    }
}
