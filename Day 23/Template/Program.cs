using System;
using System.Collections.Generic;
using System.Linq;

namespace Template
{
    class Program
    {
        static void Main()
        {
            // Keys = positions, Values = values
            var cups = new Dictionary<long, Cup>();
            for (var i = 1; i <= 1000000; i++)
            {
                cups.Add(i, new Cup { Value = i });
            }

            for (var i = 1; i <= 1000000; i++)
            {
                cups[i].NextCup = cups[(i % 1000000) + 1];
            }

            var order = new List<int> { 1, 9, 8, 7, 5, 3, 4, 6, 2 };
            for (var i = 0; i < order.Count() - 1; i++)
            {
                cups[order[i]].NextCup = cups[order[i+1]];
            }

            cups[2].NextCup = cups[10];

            var currentCup = cups[1];

            for (var i = 0; i < 10000000; i++)
            {
                var firstCup = currentCup.NextCup;
                var secondCup = firstCup.NextCup;
                var thirdCup = secondCup.NextCup;

                var sectionValues = new long[] { firstCup.Value, secondCup.Value, thirdCup.Value };

                var targetValues = new long[] {
                    currentCup.Value - 1 < 1 ? currentCup.Value + 1000000 - 1 : currentCup.Value - 1,
                    currentCup.Value - 2 < 1 ? currentCup.Value + 1000000 - 2 : currentCup.Value - 2,
                    currentCup.Value - 3 < 1 ? currentCup.Value + 1000000 - 3 : currentCup.Value - 3,
                    currentCup.Value - 4 < 1 ? currentCup.Value + 1000000 - 4 : currentCup.Value - 4
                };

                var targetValue = targetValues.First(v => !sectionValues.Contains(v));

                currentCup.NextCup = thirdCup.NextCup;
                thirdCup.NextCup = cups[targetValue].NextCup;
                cups[targetValue].NextCup = firstCup;

                currentCup = currentCup.NextCup;
            }

            Console.WriteLine(cups[1].NextCup.Value);
            Console.WriteLine(cups[1].NextCup.NextCup.Value);
        }

        public class Cup
        {
            public long Value { get; set; }
            public Cup NextCup { get; set; }
        }

        private static void WriteAnswer(int part, string answer)
        {
            Console.WriteLine($"Answer for part {part}:");
            Console.WriteLine(answer);
            Console.WriteLine("");
        }
    }
}
