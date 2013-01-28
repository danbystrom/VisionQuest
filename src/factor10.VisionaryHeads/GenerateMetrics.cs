﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace factor10.VisionaryHeads
{
    public class GenerateMetrics
    {
        public const string MetricsExe = @"C:\Program Files (x86)\Microsoft Visual Studio 10.0\Team Tools\Static Analysis Tools\FxCop\metrics.exe";

        private readonly SimpleXmlReader _x = new SimpleXmlReader();

        private readonly static Dictionary<string, string> TypeTranslator =
            new Dictionary<string, string>()
                {
                    {"Boolean", "bool"},
                    {"String", "string"},
                    {"Int32", "int"},
                    {"Int64", "long"},
                    {"Object", "object"}
                };
 
        private GenerateMetrics()
        {
        }

        private GenerateMetrics( string xmlFile)
        {
            _x.LoadFile(xmlFile);
            _x.descend("CodeMetricsReport");
        }

        public static GenerateMetrics FromCode(string[] assemblies)
        {
            var destination = Path.GetTempFileName();
            var arguments = string.Format("/f:{0} /d:{1} /o:{2}",
                                          assemblies[0], Path.GetDirectoryName(assemblies[0]), destination);
            var p = new Process
            {
                StartInfo = new ProcessStartInfo(MetricsExe)
                {
                    Arguments = arguments
                }
            };
            p.Start();
            p.WaitForExit();

            var result = new GenerateMetrics(destination);
            File.Delete(destination);
            return result;
        }

        public static GenerateMetrics FromPregeneratedFile(string xmlFile)
        {
            return new GenerateMetrics(xmlFile);
        }

        public void UpdateProgramWithMetrics(VProgram vprogram)
        {
            var methods = buildMethodIndex(vprogram);
            var classes = buildClassIndex(vprogram);

            _x.descend("Targets");
            _x.descend("Target");
            _x.descend("Modules");
            _x.descend("Module");
            _x.descend("Namespaces");
            for ( _x.descendCollection("Namespace"); _x.nextInCollection() ;)
            {
                var ns = _x["Name"] + ".";
                _x.descend("Types");
                for (_x.descendCollection("Type"); _x.nextInCollection(); )
                {
                    var typeName = ns + _x["Name"];
                    if ( typeName.Contains("Image"))
                    {
                        
                    }
                    if (classes.ContainsKey(typeName))
                        setMetrics(classes[typeName]);
                    updateMethods(typeName + "::", methods);
                }
                _x.ascend();
            }
        }

        private void updateMethods(string typeName, Dictionary<string, VMethod> methods)
        {
            _x.descend("Members");
            for (_x.descendCollection("Member"); _x.nextInCollection(); )
            {
                var name = _x["Name"];
                var i = name.IndexOf(':');
                var memberName = typeName + (i < 0 ? name : name.Substring(0, i - 1));
                if (!methods.ContainsKey(memberName))
                {
                    System.Diagnostics.Debug.Print(memberName);
                    continue;
                }
                setMetrics(methods[memberName]);
            }
            _x.ascend();
        }

        private void setMetrics(IMetrics metrics)
        {
            _x.descend("Metrics");
            for (_x.descendCollection("Metric"); _x.nextInCollection();)
            {
                var value = _x.getValueAsInt("Value");
                switch (_x["Name"])
                {
                    case "MaintainabilityIndex":
                        metrics.MaintainabilityIndex = value;
                        break;
                    case "CyclomaticComplexity":
                        metrics.CyclomaticComplexity = value;
                        break;
                    case "ClassCoupling":
                        metrics.ClassCoupling = value;
                        break;
                    case "LinesOfCode":
                        metrics.LinesOfCode = value;
                        break;
                }
            }
            _x.ascend();
        }

        private static Dictionary<string, VClass> buildClassIndex(VProgram vprogram)
        {
            var dic = new Dictionary<string, VClass>();
            foreach ( var vassembly in vprogram.VAssemblies )
                foreach ( var vclass in vassembly.VClasses )
                    if ( !dic.ContainsKey(vclass.FullName ))
                        dic.Add(vclass.FullName, vclass);
            return dic;
        }

        private static Dictionary<string, VMethod> buildMethodIndex(VProgram vprogram)
        {
            var dic = new Dictionary<string, VMethod>();
            foreach (var vm in vprogram.VMethods.Values)
            {
                var md = vm.MethodDefinition;
                var name = md.Name;
                if (md.IsGetter)
                    name = name.Substring(4) + ".get";
                else if (md.IsSetter)
                    name = name.Substring(4) + ".set";
                else if (md.IsConstructor)
                    name = vm.VClass.TypeDefinition.Name;
                var parameters = "";
                foreach (var param in md.Parameters)
                {
                    var prefix = "";
                    var tn = param.ParameterType.Name.TrimEnd('&');
                    if (param.ParameterType.IsByReference)
                        prefix = "ref ";
                    else if (param.IsOut)
                        prefix = "out ";

                    var i = param.ParameterType.FullName.IndexOf('/');
                    if (i > 0)
                    {
                        var j = param.ParameterType.FullName.LastIndexOf('.', i);
                        tn = param.ParameterType.FullName.Substring(j + 1, i - j - 1) + "." + tn;
                    }

                    string tn2;
                    if (TypeTranslator.TryGetValue(tn, out tn2))
                        tn = tn2;
                    if (parameters.Length != 0)
                        parameters += ", ";
                    parameters += prefix + tn;
                }
                var adaptedName = string.Format("{0}::{1}({2})", md.DeclaringType, name, parameters);
                if (!dic.ContainsKey(adaptedName))
                    dic.Add(adaptedName, vm);
            }
            return dic;
        }

    }

}
