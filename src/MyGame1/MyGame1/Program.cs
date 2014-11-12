using System;
using System.Collections.Generic;

namespace MyGame1
{
    /// <summary>
    /// Simple MyGame1 application using SharpDX.Toolkit.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
#if NETFX_CORE
        [MTAThread]
#else
        [STAThread]
#endif
        static void Main()
        {
            using (var program = new MyGame1())
                program.Run();

        }
    }
}