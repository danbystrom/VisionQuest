using System;
using System.IO;
using Microsoft.Win32;
using NUnit.Framework;

namespace factor10.VisionaryHeads.Tests
{
    [TestFixture]
    public class RegistryTest
    {
        [Test, Explicit]
        public void X()
        {
            const string path = @"c:\proj\larv\src\larv\bin\release";
            string lastdir = null;
            foreach (var f in new DirectoryInfo(path).GetFiles("*.*", SearchOption.AllDirectories))
            {
                var dir = Path.GetFileName(f.Directory.Name);
                if (dir != lastdir)
                {
                    if (lastdir != null)
                        System.Diagnostics.Debug.Print("</ComponentGroup>");
                    System.Diagnostics.Debug.Print("<ComponentGroup Id=\"{0}\" Directory=\"{0}\">", dir);
                    lastdir = dir;
                }
                System.Diagnostics.Debug.Print("\t<Component Guid=\"{0}\">", Guid.NewGuid());
                System.Diagnostics.Debug.Print("\t\t<File Source=\"$(var.Larv.ProjectDir){0}\" />", f.FullName.Substring(path.Length));
                System.Diagnostics.Debug.Print("\t</Component>");
            }
            System.Diagnostics.Debug.Print("</ComponentGroup>");
        }

    }

}
