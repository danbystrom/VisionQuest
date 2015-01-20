using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using factor10.VisionaryHeads;
using factor10.VisionQuest.Forms;

namespace factor10.VisionQuest.Unsorted
{
    public class VqProgram : VProgram
    {
        public readonly Project Project;

        private VqProgram(Project project)
            :base(project.Assemblies.First().FullFilename)
        {
            Project = project;
        }

        public static VqProgram Run(Form parent, Project project, string projectsFolder)
        {
            var metricsFolder = Path.Combine(projectsFolder, project.Name);
            if (!Directory.Exists(metricsFolder))
                Directory.CreateDirectory(metricsFolder);

            var metricsNeeded = new List<Tuple<string, string>>();

            var vqProgram = new VqProgram(project);
            foreach (var vassembly in vqProgram.VAssemblies)
            {
                var metricsFile = metricsFilename(metricsFolder, vassembly);
                if (!File.Exists(metricsFile) || new FileInfo(metricsFile).LastWriteTime < new FileInfo(vassembly.Filename).LastWriteTime)
                    metricsNeeded.Add(new Tuple<string, string>(vassembly.Filename, metricsFile));
            }

            if (metricsNeeded.Any())
                FGenerateMetrics.DoDialog(parent, metricsNeeded);

            foreach (var vassembly in vqProgram.VAssemblies)
                GenerateMetrics.FromPregeneratedFile(metricsFilename(metricsFolder, vassembly)).UpdateProgramWithMetrics(vqProgram);

            return vqProgram;
        }

        private static string metricsFilename(string metricsFolder, VAssembly vassembly)
        {
            return Path.ChangeExtension(Path.Combine(metricsFolder, Path.GetFileName(vassembly.Filename)), ".Metrics.txt");
        }

    }

}
