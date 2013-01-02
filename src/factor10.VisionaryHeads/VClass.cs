using System.Collections.Generic;
using Mono.Cecil;

namespace factor10.VisionaryHeads
{
    public class VClass
    {
        public readonly VAssembly VAssembly;
        public readonly TypeDefinition TypeDefinition;
        public readonly List<VMethod> VMethods = new List<VMethod>();

        public VClass(VAssembly assembly, TypeDefinition typeDefinition)
        {
            VAssembly = assembly;
            TypeDefinition = typeDefinition;
            foreach (var method in typeDefinition.Methods)
                VMethods.Add(new VMethod(this, method));
        }

        public string FullName
        {
           get { return TypeDefinition.ToString();}
        }

        public override string ToString()
        {
            return FullName;
        }

    }

}
