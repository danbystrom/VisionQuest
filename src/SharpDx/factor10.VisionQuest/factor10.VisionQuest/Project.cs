using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using factor10.VisionThing;
using SharpDX;

namespace factor10.VisionQuest
{
    public class ProjectAssembly
    {
        public string Name;
        public string FullFilename;

        public override string ToString()
        {
            return Name;
        }
    }

    public class Project
    {
        public string Name;

        public Vector3 CameraPosition = new Vector3(0, 5, 0);
        public Vector3 CameraLookAt = new Vector3(10, 4, 10);

        public DateTime Created;
        public DateTime Accessed;

        public string MainAssemblyName;

        public List<ProjectAssembly> Assemblies;
        public List<ProjectAssembly> ThirdPartyAssemblies;
        public List<ProjectAssembly> IgnoredAssemblies;

        public Project()
        {
        }

        private static string fullName(string folder, string filename)
        {
            return Path.Combine(folder, filename) + ".vqp";
        }

        private Project(string name)
        {
            Name = name;
            Created = DateTime.Now;
            Assemblies = new List<ProjectAssembly>();
            ThirdPartyAssemblies = new List<ProjectAssembly>();
            IgnoredAssemblies = new List<ProjectAssembly>();
        }

        public static Project Load(string filename)
        {
            try
            {
                var project = File.ReadAllText(filename).FromXml<Project>();
                project.Accessed = File.GetLastAccessTime(filename);
                return project;
            }
            catch (Exception ex)
            {
                return new Project(Path.GetFileNameWithoutExtension(filename));
            }
        }

        public void Save(string projectsFolder)
        {
            File.WriteAllText(fullName(projectsFolder, Name), this.ToXml());
        }

        public static IList<Project> LoadAll(string projectsFolder)
        {
            return Directory.GetFiles(projectsFolder, "*.vqp").Select(Load).ToList();
        }

    }

}
