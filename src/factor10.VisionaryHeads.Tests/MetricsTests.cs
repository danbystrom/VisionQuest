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

        [Test]
        public void TestMetrics()
        {
            var destination = Path.GetTempFileName();
            var arguments = string.Format("/f:{0} /d:{1} /o:{2}",
                                          Program, Path.GetDirectoryName(Program), destination);
            var p = new Process
                        {
                            StartInfo = new ProcessStartInfo(GenerateMetrics.MetricsExe)
                                            {
                                                Arguments = arguments
                                            }
                        };
            Assert.IsTrue(p.Start());
            p.WaitForExit();

            var result = File.ReadAllText(destination);
            File.Delete(destination);
        }

        [Test]
        public void TestGenerateMetrics()
        {
            var genMet = GenerateMetrics.FromCode(new[] {Program});
        }

        [Test]
        public void TestGenerateMetricsWithProgram()
        {
            var genMet = GenerateMetrics.FromCode(new[] { Program });
            var vp = new VProgram(Program);

            genMet.UpdateProgramWithMetrics(vp);
        }

    }

}
