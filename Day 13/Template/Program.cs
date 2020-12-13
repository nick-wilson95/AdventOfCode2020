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
                .ToList();

            var time = 0d;
            var increment = 1d;

            while (true)
            {
                for (var i = busList.Count() - 1; i >= 0; i--)
                {
                    var bus = busList[i];
                    if ((time + bus.Position) % bus.Interval == 0)
                    {
                        increment *= bus.Interval;
                        busList.RemoveAt(i);
                    }
                }

                if (!busList.Any()) break;

                time += increment;
            }

            WriteAnswer(2, time.ToString());
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
