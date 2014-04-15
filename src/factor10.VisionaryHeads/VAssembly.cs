using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace factor10.VisionaryHeads
{
    public class VAssembly
    {
        public readonly VProgram VProgram;
        public readonly string Filename;
        public readonly AssemblyDefinition AssemblyDefinition;
        public readonly List<VClass> VClasses = new List<VClass>();

        public readonly HashSet<VAssembly> Calling = new HashSet<VAssembly>();
        public readonly HashSet<VAssembly> CalledBy = new HashSet<VAssembly>();

        public readonly bool IsFortress;

        public VAssembly(VProgram vprogram, string filename)
        {
            IsFortress = Path.GetFileName(filename).First() == 'x' || Path.GetFileName(filename).First() == 'i';
            VProgram = vprogram;
            Filename = filename;
            AssemblyDefinition = AssemblyDefinition.ReadAssembly(filename);
            foreach (var type in AssemblyDefinition.MainModule.Types.Where(t => t.Methods.Any(m => !m.IsConstructor)))
            {
                if (type.BaseType!=null && type.BaseType.Name == "MulticastDelegate")
                    continue;
                if (type.Name.Contains("Nytt"))
                {
                    
                }
                VClasses.Add(new VClass(this, type));
            }
        }

        public override int GetHashCode()
        {
            return Filename.GetHashCode();
        }

        public override string ToString()
        {
            return AssemblyDefinition.ToString();
        }

    }

}
