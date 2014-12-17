using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using NUnit.Framework;

namespace factor10.VisionaryHeads.Tests
{
    [TestFixture]
    public class RegistryTest
    {
        [Test]
        public void X()
        {
            var w = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            var x = w.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio");
            var q = x.GetSubKeyNames();
            x.Close();
        }

    }
}
