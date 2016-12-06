using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyReferenceCheck
{
    public class ReferenceInfo
    {
        /// <summary>
        /// 被引用的程序集的名称
        /// </summary>
        public string ReferenceAssemblyName { get; private set; }

        /// <summary>
        /// 映射关系，Item1为被引用程序集全名，Item2为引用者程序集
        /// </summary>
        public List<Tuple<string, string>> Mapping { get; private set; }

        public ReferenceInfo(string assemblyName)
        {
            ReferenceAssemblyName = assemblyName;
            Mapping = new List<Tuple<string, string>>();
        }

        public void Add(string referenceAssemblyFullName, string sourceFilePath)
        {
            Mapping.Add(new Tuple<string, string>(referenceAssemblyFullName, sourceFilePath));
        }

        public bool TryGetConflicts(out string msg)
        {
            msg = string.Empty;

            var groupMapping = from m in Mapping group m by m.Item1 into g select new {g.Key, Num = g.Count(), Mapping = g};
            //当分组数超过1时才判断为冲突
            if (groupMapping.Count() > 1)
            {
                var sb = new StringBuilder();
                foreach (var g in groupMapping)
                {
                    sb.AppendLine($"Reference Name:{ReferenceAssemblyName} Full Name:{g.Key}");
                    sb.AppendLine("Source:");

                    foreach (var tuple in g.Mapping)
                    {
                        sb.AppendLine(tuple.Item2);
                    }
                }

                msg = sb.ToString();
                return true;
            }

            return false;
        }
    }
}
