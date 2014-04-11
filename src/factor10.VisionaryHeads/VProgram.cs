using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace factor10.VisionaryHeads
{
    public class VProgram
    {
        public readonly List<VAssembly> VAssemblies = new List<VAssembly>();

        public readonly Dictionary<string, VMethod> VMethods = new Dictionary<string, VMethod>();

        public VProgram(string filename)
        {
            loadAssembly(filename);

            foreach (var vm in from va in VAssemblies from vc in va.VClasses from vm in vc.VMethods select vm)
                if (!VMethods.ContainsKey(vm.FullName))
                    VMethods.Add(vm.FullName, vm);

            foreach (var vm in from va in VAssemblies where !va.IsFortress from vc in va.VClasses from vm in vc.VMethods select vm)
                vm.BuildCallingRelations();

            // check for orphan assemblies - but don't check the first one (the program itself)
            for (var i = VAssemblies.Count - 1; i >= 1; i--)
                switch (VAssemblies[i].CalledBy.Count)
                {
                    case 0:  // no calls to the assembly
                        VAssemblies.RemoveAt(i);
                        break;
                    case 1:  // the assembly only calles itself
                        if (VAssemblies[i].CalledBy.Single() == VAssemblies[i])
                            VAssemblies.RemoveAt(i);
                        break;
                }
        }

        private void loadAssembly(string filename)
        {
            filename = filename.ToLower();

            if (VAssemblies.Any(va => va.Filename == filename))
                return;

            if (!File.Exists(filename))
                return;

            System.Diagnostics.Debug.Print(filename);

            var vassembly = new VAssembly(this, filename);
            VAssemblies.Add(vassembly);

            foreach (var m in vassembly.AssemblyDefinition.Modules)
                foreach (var ar in m.AssemblyReferences)
                {
                    loadAssembly(Path.Combine(Path.GetDirectoryName(filename), ar.Name + ".dll"));
                }

        }

    }

}
