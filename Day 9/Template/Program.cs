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

            var values = text.Split("\r\n")
                .Select(l => double.Parse(l))
                .ToArray();

            // Part 1:
            double answer1 = 0;
            for (var i = 0; i < values.Length - 26; i++)
            {
                var preamble = values.Skip(i).Take(25);
                var pairwiseSums = preamble.SelectMany(x => preamble, (x, y) => x + y);

                var target = values[i + 25];

                if (!pairwiseSums.Contains(target))
                {
                    answer1 = target;
                    break;
                }
            }

            Console.WriteLine(answer1);

            // Part 2:
            var startIndex = 0;
            var endIndex = 1;
            while(true)
            {
                var sum = values.Skip(startIndex).Take(endIndex + 1 - startIndex).Sum();

                if (sum < answer1) endIndex++;
                else if (sum > answer1) startIndex++;
                else break;
            }

            var range = values.Skip(startIndex).Take(endIndex + 1 - startIndex);
            var answer2 = range.Min() + range.Max();

            Console.WriteLine(answer2);
        }
    }
}
