using System;
using System.Windows.Forms;
using CircleMasterApp;

namespace factor10.VisionThing.Tests
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FCameraMovement());
        }
    }
}
