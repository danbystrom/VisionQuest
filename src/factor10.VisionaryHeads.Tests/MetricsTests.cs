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

        private static void saveMetrics(string source, string destination)
        {
            var arguments = string.Format("/f:{0} /d:{1} /o:{2}",
                                          source, Path.GetDirectoryName(Program), destination);
            var p = new Process
            {
                StartInfo = new ProcessStartInfo(GenerateMetrics.MetricsExe)
                {
                    Arguments = arguments
                }
            };
            Assert.IsTrue(p.Start());
            p.WaitForExit();
        }

        [Test, Explicit]
        public void TestMetrics()
        {
            var destination = Path.GetTempFileName();
            saveMetrics(Program, destination);
            var result = File.ReadAllText(destination);
            File.Delete(destination);
        }

        [Test, Explicit]
        public void SaveOne()
        {
            saveMetrics(Assembly3, @"c:\users\dan\desktop\VisionQuest\vdUsr.Metrics.txt");
        }

        [Test, Explicit]
        public void TestGenerateMetrics()
        {
            var vprogram = new VProgram(Program);
            var genMet = GenerateMetrics.FromCode(vprogram.VAssemblies.Select(a => a.Filename).ToArray());
        }

        [Test, Explicit]
        public void TestGenerateMetricsWithProgram()
        {
            var genMet = GenerateMetrics.FromCode(new[] { Program });
            var vp = new VProgram(Program);

            genMet.UpdateProgramWithMetrics(vp);
        }

    }

}
