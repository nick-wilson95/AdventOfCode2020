using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Template
{
    class Program
    {
        static void Main()
        {
            var startingNumbers = "5,2,8,16,18,0,1";

            // Part 1:
            var answer1 = GetAnswer1(startingNumbers);
            WriteAnswer(1, answer1.ToString());

            // Part 2:
            var numbers = startingNumbers.Split(',')
                .Select(long.Parse)
                .ToList();

            var lastLocations = new Dictionary<long, int>();
            for (var i = 0; i < numbers.Count() - 1; i++)
            {
                lastLocations.Add(numbers[i], i);
            }

            while (numbers.Count() < 30000000)
            {
                if (!lastLocations.ContainsKey(numbers.Last()))
                {
                    numbers.Add(0);
                }
                else
                {
                    var lastLocation = lastLocations[numbers.Last()];
                    numbers.Add(numbers.Count() - 1 - lastLocation);
                }

                lastLocations[numbers[numbers.Count() - 2]] = numbers.Count() - 2;
            }

            WriteAnswer(2, numbers.Last().ToString());
        }

        private static int GetAnswer1(string startingNumbers)
        {
            var numbers1 = startingNumbers.Split(',')
                   .Select(int.Parse)
                   .ToList();

            while (numbers1.Count() < 2020)
            {
                if (numbers1.Count(n => n == numbers1.Last()) == 1)
                {
                    numbers1.Add(0);
                    continue;
                }

                var previousIndex = numbers1.Select((n, i) => new { Number = n, Index = i })
                    .Where(n => n.Number == numbers1.Last())
                    .Select(x => x.Index)
                    .Reverse()
                    .Skip(1)
                    .First();

                numbers1.Add(numbers1.Count() - 1 - previousIndex);
            }

            return numbers1.Last();
        }

        private static void WriteAnswer(int part, string answer)
        {
            Console.WriteLine($"Answer for part {part}:");
            Console.WriteLine(answer);
        }
    }
}
