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

            var lines = text.Split("\r\n");

            // Part 1:
            var startTime = int.Parse(lines[0]);

            var busIds = lines[1].Split(',')
                .Where(x => x != "x")
                .Select(int.Parse);

            var delays = busIds.Select(x => x - startTime % x);
            var answer1 = busIds.Zip(delays)
                .Where(x => x.Second == delays.Min())
                .Select(x => x.First * x.Second)
                .First();

            WriteAnswer(1, answer1.ToString());

            // Part 2:
            var busList = lines[1].Split(',')
                .Select(Bus.Create)
                .Where(x => x.Active)
                .OrderBy(x => x.Interval)
                .Reverse()
                .ToList();

            var pivot = busList[0].Position;
            busList.ForEach(x => x.Position -= pivot);

            var baseInterval = busList[0].Interval;
            var bussesToCheck = busList.Skip(1)
                .ToArray();

            var counter = 0d;
            var departure = 0d;

            while (true)
            {
                counter++;

                departure = counter * baseInterval;

                var validTime = true;
                for (var i = 0; i < bussesToCheck.Length; i++)
                {
                    var bus = bussesToCheck[i];
                    if ((departure + bus.Position) % bus.Interval != 0)
                    {
                        validTime = false;
                        break;
                    };
                }

                if (validTime) break;
            }

            WriteAnswer(2, departure.ToString());
        }

        private class Bus
        {
            public double Interval { get; private set; }
            public double Position { get; set; }
            public bool Active { get; private set; }

            public static Bus Create(string id, int position)
            {
                var isActive = int.TryParse(id, out var interval);

                return new Bus { Interval = interval, Position = position, Active = isActive };
            }
        }

        private static void WriteAnswer(int part, string answer)
        {
            Console.WriteLine($"Answer for part {part}:");
            Console.WriteLine(answer);
            Console.WriteLine("");
        }
    }
}
