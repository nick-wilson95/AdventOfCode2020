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
            var text = File.ReadAllText("./input.txt");

            var input = text.Split("\r\n");

            var tiles = new Dictionary<int, List<int>>();

            foreach (var line in input)
            {
                var x = 0;
                var y = 0;

                var instructionLocation = 0;
                while (instructionLocation < line.Length)
                {
                    switch (line[instructionLocation])
                    {
                        case 'e':
                            instructionLocation++;
                            x++;
                            break;
                        case 'w':
                            instructionLocation++;
                            x--;
                            break;
                        case 'n':
                            y++;
                            if (line[instructionLocation + 1] == 'w') x--;
                            instructionLocation += 2;
                            break;
                        case 's':
                            y--;
                            if (line[instructionLocation + 1] == 'e') x++;
                            instructionLocation += 2;
                            break;
                    }
                }

                if (!tiles.ContainsKey(x)) tiles.Add(x, new List<int>());

                if (tiles[x].Contains(y))
                {
                    tiles[x].Remove(y);
                }
                else
                {
                    tiles[x].Add(y);
                }
            }

            for (var i = 0; i < 100; i++)
            {
                var neighbourCounts = new Dictionary<int, Dictionary<int, int>>();

                foreach (var row in tiles)
                {
                    var x = row.Key;

                    foreach (var y in row.Value)
                    {
                        if (!neighbourCounts.ContainsKey(x)) neighbourCounts.Add(x, new Dictionary<int, int>());
                        if (!neighbourCounts[x].ContainsKey(y)) neighbourCounts[x].Add(y, 0);

                        AddNeighbour(neighbourCounts, x - 1, y);
                        AddNeighbour(neighbourCounts, x + 1, y);
                        AddNeighbour(neighbourCounts, x, y - 1);
                        AddNeighbour(neighbourCounts, x, y + 1);
                        AddNeighbour(neighbourCounts, x - 1, y + 1);
                        AddNeighbour(neighbourCounts, x + 1, y - 1);
                    }
                }

                foreach (var row in neighbourCounts)
                {
                    var x = row.Key;

                    foreach (var y in row.Value)
                    {
                        if (!tiles.ContainsKey(x)) tiles[x] = new List<int>();
                        var isBlack = tiles[x].Contains(y.Key);
                        var neighbourCount = y.Value;

                        if (isBlack && (neighbourCount == 0 || neighbourCount > 2)) tiles[x].Remove(y.Key);
                        if (!isBlack && neighbourCount == 2) tiles[x].Add(y.Key);
                    }
                }
            }

            Console.WriteLine(tiles.Values.Sum(v => v.Count()));
        }

        private static void AddNeighbour(Dictionary<int, Dictionary<int, int>> neighbourCounts, int x, int y)
        {
            if (!neighbourCounts.ContainsKey(x)) neighbourCounts.Add(x, new Dictionary<int, int>());

            if (!neighbourCounts[x].ContainsKey(y)) neighbourCounts[x].Add(y, 0);

            neighbourCounts[x][y]++;
        }

        private static void WriteAnswer(int part, string answer)
        {
            Console.WriteLine($"Answer for part {part}:");
            Console.WriteLine(answer);
            Console.WriteLine("");
        }
    }
}
