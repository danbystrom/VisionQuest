using System;
using System.Collections.Generic;

namespace DxExempel1
{
    /// <summary>
    /// Simple DxExempel1 application using SharpDX.Toolkit.
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
            using (var program = new DxExempel1())
                program.Run();

        }
    }
}