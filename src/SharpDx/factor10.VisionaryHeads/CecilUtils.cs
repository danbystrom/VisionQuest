using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace factor10.VisionQuest.Metrics
{
    public class CecilUtils
    {
        // see mono Reflector Add-on since it contains this feature (so figure out how they do it)
        public void tryToFixPublicKey(String sAssemblyToFix)
        {
            var adAssembly = AssemblyDefinition.ReadAssembly(sAssemblyToFix);
            adAssembly.Name.PublicKey = null;
            //dan AssemblyDefinition.CreateAssembly(adAssembly, sAssemblyToFix + ".fix.dll");
        }



        public static AssemblyDefinition getAssembly(String assemblyToLoad)
        {
            try
            {
                //     return AssemblyDefinition.ReadAssembly(Files.getFileContentsAsByteArray(assemblyToLoad));

                return AssemblyDefinition.ReadAssembly(assemblyToLoad);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static MethodDefinition getAssemblyEntryPoint(string assemblyToLoad)
        {
            try
            {
                return getAssembly(assemblyToLoad).EntryPoint;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<ModuleDefinition> getModules(String assemblyToLoad)
        {
            try
            {
                if (File.Exists(assemblyToLoad))
                {
                    AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyToLoad);
                    return getModules(assemblyDefinition);
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public static List<ModuleDefinition> getModules(AssemblyDefinition assemblyDefinition)
        {
            try
            {
                var modules = new List<ModuleDefinition>();
                foreach (ModuleDefinition moduleDefinition in assemblyDefinition.Modules)
                    modules.Add(moduleDefinition);
                return modules;
            }
            catch (Exception ex)
            {
                return null;
            }


        }

        public static List<TypeDefinition> getTypes(String assemblyToLoad)
        {
            try
            {
                var types = new List<TypeDefinition>();
                foreach (ModuleDefinition moduleDefinition in getModules(assemblyToLoad))
                    types.AddRange(getTypes(moduleDefinition));
                return types;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<TypeDefinition> getTypes(AssemblyDefinition assemblyDefinition)
        {
            try
            {
                var types = new List<TypeDefinition>();
                foreach (ModuleDefinition module in assemblyDefinition.Modules)
                    foreach (TypeDefinition type in getTypes(module))
                        types.Add(type);
                return types;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<TypeDefinition> getTypes(ModuleDefinition moduleDefinition)
        {
            try
            {
                var types = new List<TypeDefinition>();
                foreach (TypeDefinition typeDefinition in moduleDefinition.Types)
                    types.Add(typeDefinition);
                return types;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public static List<TypeDefinition> getTypes(TypeDefinition typeDefinition)
        {
            try
            {
                var types = new List<TypeDefinition>();
                foreach (TypeDefinition type in typeDefinition.NestedTypes)
                    types.Add(type);
                return types;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /*     public static List<MethodReference> getConstructors(TypeDefinitionDefinition tType)
             {
                 try
                 {
                     var constructors = new List<MethodReference>();
                     foreach(var constructor in tType.Constructors)
                     {
                         var type = constructor.GetType();
                         var typename = constructor.GetType().Name;
                     }
                     return constructors;
                 }
                 catch (Exception ex)
                 {
                     DI.log.ex(ex, "in CecilUtils.getConstructors");
                     return null;
                 }
             }*/

        public static List<MethodDefinition> getMethods(AssemblyDefinition assemblyToLoad)
        {
            try
            {
                var methods = new List<MethodDefinition>();
                foreach (TypeDefinition typeDefinition in getTypes(assemblyToLoad))
                    foreach (MethodDefinition methodDefinition in typeDefinition.Methods)
                        methods.Add(methodDefinition);
                return methods;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static List<MethodDefinition> getMethods(String assemblyToLoad)
        {
            try
            {
                return getMethods(getAssembly(assemblyToLoad));
                /*      var methods = new List<MethodDefinition>();
                      foreach (TypeDefinitionDefinition typeDefinition in getTypes(assemblyToLoad))
                          foreach (MethodDefinition methodDefinition in typeDefinition.Methods)
                              methods.Add(methodDefinition);
                      return methods;*/
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<MethodDefinition> getMethods(TypeDefinition typeDefinition)
        {
            try
            {
                var methods = new List<MethodDefinition>();
                foreach (MethodDefinition methodDefinition in typeDefinition.Methods)
                    methods.Add(methodDefinition);
                return methods;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<MethodDefinition> getMethodsStatic(TypeDefinition tType)
        {
            try
            {
                var methods = new List<MethodDefinition>();
                foreach (MethodDefinition methodDefinition in getMethods(tType))
                    if (methodDefinition.IsStatic)
                        methods.Add(methodDefinition);
                return methods;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static MethodDefinition getMethod(TypeDefinition type, string methodToFind, object[] methodParameters)
        {
            var methodParameterTypes = new List<Type>();
            if (methodParameters != null && methodParameters.Length > 0)
                foreach (var methodParameter in methodParameters)
                    methodParameterTypes.Add(methodParameter.GetType());
            return (getMethod(type, methodToFind, methodParameterTypes.ToArray()));
        }

        public static MethodDefinition getMethod(TypeDefinition type, string methodToFind, Type[] methodParameters)
        {
            if (methodParameters == null)
                methodParameters = new Type[0];
            try
            {
                foreach (MethodDefinition method in getMethods(type))
                {
                    if (method.Name == methodToFind)
                    {
                        var parameters = method.Parameters;
                        if (parameters.Count == 0 && (methodParameters.Length == 0))
                            return method;
                        if (parameters.Count == methodParameters.Length)
                        {
                            bool allParamsMatched = false;
                            for (int i = 0; i < parameters.Count; i++)
                            {
                                if (parameters[i].ParameterType.FullName == methodParameters[i].FullName)
                                    allParamsMatched = true;
                                else
                                {
                                    allParamsMatched = false;
                                    break;
                                }
                            }
                            if (allParamsMatched)
                                return method;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public static List<MethodCalled> getMethodsCalledInsideAssembly(String assemblyToLoad)
        {
            try
            {
                var methodsCalled = new List<MethodCalled>();
                foreach (MethodDefinition methodDefinition in getMethods(assemblyToLoad))
                    methodsCalled.AddRange(getMethodsCalledInsideMethod(methodDefinition));
                return methodsCalled;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<MethodCalled> getMethodsCalledInsideMethod(MethodDefinition methodDefinition)
        {
            try
            {
                var methodsCalled = new List<MethodCalled>();
                if (methodDefinition.Body != null)
                {
                    SequencePoint currentSequencePoint = null;
                    foreach (Instruction instruction in methodDefinition.Body.Instructions)
                    {
                        currentSequencePoint = instruction.SequencePoint ?? currentSequencePoint;
                        if (instruction.Operand != null)
                        {
                            switch (instruction.Operand.GetType().Name)
                            {
                                case "MethodReference":
                                case "MethodDefinition":
                                    methodsCalled.Add(new MethodCalled((IMemberReference)instruction.Operand, currentSequencePoint));
                                    break;
                                default:
                                    break;
                            }
                            //DI.log.info(instruction.Operand.GetType().Name);
                            //if (instruction.Operand.GetType().Name == "MethodReference")
                            // need to check if I also need to hook into MethodDefinition                         
                        }
                    }
                }
                if (methodsCalled.Count == 0)
                {
                    var tokenMethod = methodDefinition.MetadataToken;
                    var tokenType = methodDefinition.DeclaringType.MetadataToken;
                    if (methodDefinition.HasBody)
                    {
                    }
                }
                return methodsCalled;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Object getAttributeValueFromAssembly(String assemblyToLoad, String sAttributeToFetch,
                                                           int iParameterValueIndex)
        {
            try
            {
                AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyToLoad);
                foreach (ModuleDefinition moduleDefinition in assemblyDefinition.Modules)
                    foreach (TypeDefinition typeDefinition in moduleDefinition.Types)
                        foreach (MethodDefinition methodDefinition in typeDefinition.Methods)
                            foreach (CustomAttribute customAttribute in methodDefinition.CustomAttributes)
                                if (customAttribute.Constructor.DeclaringType.Name == sAttributeToFetch)
                                    return customAttribute.ConstructorArguments[iParameterValueIndex];
                return "";
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Dictionary<string, List<TypeDefinition>> getDictionaryWithTypesMappedToNamespaces(
            ModuleDefinition moduleDefinition)
        {
            try
            {
                var typesMappedToNamespaces = new Dictionary<string, List<TypeDefinition>>();
                foreach (TypeDefinition type in getTypes(moduleDefinition))
                {
                    string typeNamespace = type.Namespace;
                    if (typeNamespace == null)
                    {
                        if (!typesMappedToNamespaces.ContainsKey(""))
                            typesMappedToNamespaces.Add("", new List<TypeDefinition>());
                        typesMappedToNamespaces[""].Add(type);
                    }
                    else
                    {
                        if (!typesMappedToNamespaces.ContainsKey(typeNamespace))
                            typesMappedToNamespaces.Add(typeNamespace, new List<TypeDefinition>());
                        typesMappedToNamespaces[typeNamespace].Add(type);
                    }
                }
                return typesMappedToNamespaces;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static TypeDefinition getType(AssemblyDefinition assemblyDefinition, string nameOfTypeToExtract)
        {
            try
            {
                List<TypeDefinition> types = getTypes(assemblyDefinition);
                foreach (TypeDefinition type in types)
                    if (type.Name == nameOfTypeToExtract)
                        return type;
                return null;
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool isDotNetAssembly(string assemblyToCheck)
        {
            return isDotNetAssembly(assemblyToCheck, true);
        }

        public static bool isDotNetAssembly(string assemblyToCheck, bool verbose)
        {
            try
            {
                var assemblyManifest = AssemblyDefinition.ReadAssembly(assemblyToCheck);
                if (assemblyManifest == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static string getMethodTypeNameAndParameters(MethodReference methodReference, string splitStringBetweenTypeAndMethod)
        {
            return string.Format("{0}{1}{2}", methodReference.ReturnType.FullName, splitStringBetweenTypeAndMethod, getMethodParametersSiganature(methodReference));
        }

        public static string getMethodNameAndParameters(MethodReference methodReference)
        {
            return string.Format("{0}{1}", methodReference.Name, getMethodParametersSiganature(methodReference));
        }


        //based on the code from Mono.Cecil.MethodReference.ToString()
        public static string getMethodParametersSiganature(MethodReference methodReference)
        {
            int sentinel = 0; //dan methodReference.GetSentinel();
            var builder = new StringBuilder();
            builder.Append("(");
            for (int i = 0; i < methodReference.Parameters.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append(",");
                }
                if (i == sentinel)
                {
                    builder.Append("...,");
                }
                builder.Append(methodReference.Parameters[i].ParameterType.FullName);
            }
            builder.Append(")");
            return builder.ToString();
        }

    }
}
