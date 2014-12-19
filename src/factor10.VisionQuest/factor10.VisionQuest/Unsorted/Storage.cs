using System;
using System.IO;
using System.IO.IsolatedStorage;
using factor10.VisionThing;

namespace factor10.VisionQuest.Unsorted
{
    public class Storage
    {
        private const string Filename = "TestFile.txt";

        public string ProjectFolder;
        public string LastRecentlyUsedProject;
        public bool DrawLines;

        private Storage()
        {
        }

        public string SafeProjectFolder()
        {
            if (ProjectFolder == null || !File.Exists(ProjectFolder))
            {
                ProjectFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "VisionQuest");
                if (!Directory.Exists(ProjectFolder))
                    Directory.CreateDirectory(ProjectFolder);
            }
            return ProjectFolder;
        }

        public void Save()
        {
            withStorage(
                file =>
                {
                    using (var writer = new StreamWriter(file))
                    {
                        writer.WriteLine(this.ToXml());
                        return 0;
                    }
                });
        }

        public static Storage Load()
        {
            return withStorage(
                file =>
                {
                    try
                    {
                        using (var reader = new StreamReader(file))
                            return reader.ReadToEnd().FromXml<Storage>();
                    }
                    catch
                    {
                        return new Storage();
                    }
                });
        }

        private static T withStorage<T>(Func<IsolatedStorageFileStream, T> action)
        {
            using (
                var storage = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null))
            using (var file = storage.OpenFile(Filename, FileMode.OpenOrCreate))
                return action(file);
        }

    }

}
