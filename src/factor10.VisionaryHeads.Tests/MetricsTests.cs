using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace factor10.VisionaryHeads.Tests
{
    [TestFixture]
    public class MetricsTests
    {
        public const string Program = @"C:\proj\photomic.old\src\Plata\bin\Release\Plåta.exe";
        public const string Assembly1 = @"C:\proj\photomic.old\src\Plata\bin\Release\vdXceed.dll";
        public const string Assembly2 = @"C:\proj\photomic.old\src\Plata\bin\Release\Photomic.Common.dll";
        public const string Assembly3 = @"C:\proj\photomic.old\src\Plata\bin\Release\vdUsr.dll";

        //[Test, Explicit]
        //public void TestMetrics()
        //{
        //    var vprogram = new VProgram(Program);
        //    foreach(var va in vprogram.VAssemblies)
        //        saveMetrics(va.Filename, @"c:\temp\" + Path.GetFileNameWithoutExtension(va.Filename) + ".Metrics.txt");
        //}

        //[Test, Explicit]
        //public void SaveOne()
        //{
        //    saveMetrics(@"C:\proj\photomic.old\src\Plata\bin\Release\itextsharp.dll", @"c:\temp\itextsharp.Metrics.txt");
        //}

        //[Test, Explicit]
        //public void TestGenerateMetrics()
        //{
        //    var vprogram = new VProgram(Program);
        //    var genMet = GenerateMetrics.FromCode(vprogram.VAssemblies.Select(a => a.Filename).ToArray());
        //}

        //[Test, Explicit]
        //public void TestGenerateMetricsWithProgram()
        //{
        //    var genMet = GenerateMetrics.FromCode(new[] { Program });
        //    var vp = new VProgram(Program);

        //    genMet.UpdateProgramWithMetrics(vp);
        //}

    }

}
