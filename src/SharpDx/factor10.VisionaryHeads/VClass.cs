using System.Collections.Generic;
using Mono.Cecil;

namespace factor10.VisionaryHeads
{
    public class VClass : IMetrics
    {
        public readonly VAssembly VAssembly;
        public readonly TypeDefinition TypeDefinition;
        public readonly List<VMethod> VMethods = new List<VMethod>();

        public int InstructionCount { get; set; }
        public int MaintainabilityIndex { get; set; }
        public int CyclomaticComplexity { get; set; }
        public int ClassCoupling { get; set; }
        public int LinesOfCode { get; set; }

        public bool IsInterface { get { return TypeDefinition.IsInterface; } }
        public string Name { get { return TypeDefinition.Name; } }

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
