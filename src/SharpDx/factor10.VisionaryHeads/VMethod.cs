using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace factor10.VisionaryHeads
{
    public class VMethod : IMetrics
    {
        public readonly VClass VClass;
        public readonly MethodDefinition MethodDefinition;
        public readonly string FullName;

        public readonly List<string> Calling = new List<string>();
        public readonly List<string> CalledBy = new List<string>();

        public int InstructionCount { get; set; }
        public int MaintainabilityIndex { get; set; }
        public int CyclomaticComplexity { get; set; }
        public int ClassCoupling { get; set; }
        public int LinesOfCode { get; set; }

        public string AssemblyName { get { return MethodDefinition.Module.Assembly.Name.Name; } }

        public VMethod(VClass vclass, MethodDefinition methodDefinition)
        {
            VClass = vclass;
            MethodDefinition = methodDefinition;
            var fn = MethodDefinition.ToString();
            FullName = fn.Substring(fn.IndexOf(' ') + 1);
            if (!MethodDefinition.HasBody)
                return;

            InstructionCount = MethodDefinition.Body.Instructions.Count;
            VClass.InstructionCount += InstructionCount;
            foreach (var instruction in MethodDefinition.Body.Instructions)
                if (instruction.OpCode == OpCodes.Call || instruction.OpCode == OpCodes.Callvirt)
                {
                    var methodCall = instruction.Operand as MethodReference;
                    if (methodCall == null )
                        continue;
                    var name = methodCall.FullName;
                    name = name.Substring(name.IndexOf(' ') + 1);
                    if (!Calling.Contains(name))
                        Calling.Add(name);
                }
        }

        public void BuildCallingRelations()
        {
            VMethod vmcalled;
            for (var i = Calling.Count - 1; i >= 0; i--)
                if (VClass.VAssembly.VProgram.VMethods.TryGetValue(Calling[i], out vmcalled))
                {
                    vmcalled.CalledBy.Add(FullName);
                    VClass.VAssembly.Calling.Add(vmcalled.VClass.VAssembly);
                    vmcalled.VClass.VAssembly.CalledBy.Add(VClass.VAssembly);
                }
                else
                    Calling.RemoveAt(i); // this call is to a method not loaded, typically in System.xxx
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

        public override string ToString()
        {
            return FullName;
        }

    }

}
