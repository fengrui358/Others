using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyReferenceCheck
{
    /// <summary>
    /// 引用管理器
    /// </summary>
    public class ReferenceManager
    {
        private static readonly IList<string> SourceFilePaths = new List<string>();
        private static readonly Dictionary<string, ReferenceInfo> AllReferenceInfos = new Dictionary<string, ReferenceInfo>();

        public static void CheckFile(string filePath)
        {
            if (File.Exists(filePath) && !SourceFilePaths.Contains(filePath))
            {
                Assembly assembly = null;

                try
                {
                    assembly = Assembly.ReflectionOnlyLoadFrom(filePath);
                }
                catch (Exception)
                {
                        Console.WriteLine("Can't reflection " + filePath);
                }

                if (assembly != null)
                {
                    SourceFilePaths.Add(filePath);

                    var references = assembly.GetReferencedAssemblies();

                    foreach (var assemblyName in references)
                    {
                        if (!AllReferenceInfos.ContainsKey(assemblyName.Name))
                        {
                            var referenceInfo = new ReferenceInfo(assemblyName.Name);
                            referenceInfo.Add(assemblyName.FullName, filePath);

                            AllReferenceInfos.Add(assemblyName.Name, referenceInfo);
                        }
                    }
                }
            }
        }

        public static IEnumerable<string> GetConflicts()
        {
            var result = new List<string>();
            foreach (var referenceInfo in AllReferenceInfos)
            {
                string s;
                if (referenceInfo.Value.TryGetConflicts(out s))
                {
                    result.Add(s);
                }
            }

            return result;
        }
    }
}
