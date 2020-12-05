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

            var lines = text.Split("\n");

            var trees = 0;

            for (int i = 0; i < lines.Count(); i += 2)
            {
                var line = lines[i];

                if (hitsTree(line, i/2, 1))
                {
                    trees++;
                }
            }

            Console.WriteLine(trees);
        }

        private static bool hitsTree(string line, int lineIndex, int slope)
        {
            var position = lineIndex * slope;

            var characterPosition = position % 31;

            var character = line[characterPosition];

            return character == '#';
        }
    }
}
