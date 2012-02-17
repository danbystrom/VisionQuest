using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace factor10.VisionQuest.Metrics
{
    public class ReadAssembly
    {
        public void Z( string filename )
        {
            var sourceAssembly = AssemblyDefinition.ReadAssembly(filename);
            foreach (var type in sourceAssembly.MainModule.Types)
            {
                System.Diagnostics.Debug.Print("{0}", type);
                foreach (var method in type.Methods)
                {
                    System.Diagnostics.Debug.Print("  {0}", method);
                    PrintMethods(method);
                }
            }
        }

        public static void PrintMethods(MethodDefinition method)
        {
            if (!method.HasBody)
                return;
            System.Diagnostics.Debug.Print("    Calling Methods");
            foreach (var instruction in method.Body.Instructions)
                if (instruction.OpCode == OpCodes.Call || instruction.OpCode == OpCodes.Callvirt )
                {
                    var methodCall = instruction.Operand as MethodReference;
                    if (methodCall != null)
                        System.Diagnostics.Debug.Print("      " + methodCall);
                }
        }


        public static void PrintFields(MethodDefinition method)
        {
            if (!method.HasBody)
                return;
            System.Diagnostics.Debug.Print("    Fields");
            foreach (var instruction in method.Body.Instructions)
            {
                if (instruction.OpCode == OpCodes.Ldfld)
                {
                    var field = instruction.Operand as FieldReference;
                    if (field != null)
                        Console.WriteLine("      " + field.Name);
                }
            }
        } 

    }
}
