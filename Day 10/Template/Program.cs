using System;
using System.Collections.Generic;
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
                .Select(x => int.Parse(x))
                .ToList();

            values.Sort();


            // Part 1:
            var jumps = new List<int>();

            var previousValue = 0;
            for (var i = 0; i < values.Count(); i++)
            {
                jumps.Add(values[i] - previousValue);
                previousValue = values[i];
            }

            jumps.Add(3);

            var answer1 = jumps.Count(j => j == 1) * jumps.Count(j => j == 3);
            Console.WriteLine(answer1);


            // Part 2:
            var arrangementsToSocket = new double[values.Last()];

            for (var i = 0; i < values.Count(); i++)
            {
                var adapterValue = values[i];
                var arrangements = 0d;
                if (adapterValue <= 3) arrangements++;

                for (var j = 1; j <= 3; j++)
                {
                    if (adapterValue - 1 - j < 0) break;
                    arrangements += arrangementsToSocket[adapterValue - 1 - j];
                }

                arrangementsToSocket[adapterValue - 1] = arrangements;
            }

            var answer2 = arrangementsToSocket[values.Last() - 1];
            Console.WriteLine(answer2);
        }
    }
}
