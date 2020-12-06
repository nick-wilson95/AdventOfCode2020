using System;
using System.IO;
using System.Linq;

namespace Template
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = File.ReadAllText("./input.txt");

            var groupStrings = text.Split("\r\n\r\n");

            var counts = groupStrings
                .Select(g => g.Split("\r\n")
                    .Select(s => s.Trim())
                    .Aggregate((s1, s2) =>  new string(s1.Intersect(s2).ToArray()))
                    .Distinct()
                    .Count()
                );

            Console.WriteLine(counts.Sum());
        }
    }
}
