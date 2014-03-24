using System;
using System.Collections.Generic;
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

        public VAssembly(VProgram vprogram, string filename)
        {
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

        public override string ToString()
        {
            return AssemblyDefinition.ToString();
        }

    }

}
