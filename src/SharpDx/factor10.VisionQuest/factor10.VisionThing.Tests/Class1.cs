using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NUnit.Framework;

namespace factor10.VisionThing.Tests
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void Z()
        {
            var files = new DirectoryInfo(@"c:\proj\visionquest\src\sharpdx").GetFiles("*.cs", SearchOption.AllDirectories).ToList();
            files.Sort((x, y) => -x.LastWriteTime.CompareTo(y.LastWriteTime));
            for (var i = 0; i < 99; i++)
                System.Diagnostics.Debug.Print("{0:yyyy MMMM dd HH mm} {1}", files[i].LastWriteTime, files[i].FullName);
        }
    }
}
