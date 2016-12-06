using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyReferenceCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            var allFilesTobeCheck = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll",
                SearchOption.AllDirectories);

            foreach (var s in allFilesTobeCheck)
            {
                ReferenceManager.CheckFile(s);
            }

            var result = ReferenceManager.GetConflicts();
            foreach (var s in result)
            {
                Console.WriteLine(s);
            }

            Console.ReadKey();
        }
    }
}
