using System;
using System.Collections.Generic;

namespace NextGame
{
    /// <summary>
    /// Simple NextGame application using SharpDX.Toolkit.
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
            using (var program = new NextGame())
                program.Run();

        }
    }
}