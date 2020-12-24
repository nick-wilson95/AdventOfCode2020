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

            var input = text.Split("\r\n\r\n");

            var tiles = new Dictionary<int, List<List<int>>>();

            foreach (var tileString in input)
            {
                var lines = tileString.Split("\r\n");

                var id = int.Parse(lines[0].Substring(5, 4));

                tiles[id] = new List<List<int>>();

                for (var i = 1; i < lines.Length; i++)
                {
                    tiles[id].Add(new List<int>());
                    for (var j = 0; j < lines[i].Length; j++)
                    {
                        tiles[id].Last().Add(lines[i][j] == '#' ? 1 : 0);
                    }
                }
            }

            var tilePositions = new List<List<List<List<int>>>>();

            var edgesToMatch = new List<List<int>>
            {
                tiles[3643].First(),
                tiles[3643].Select(row => row.Last()).ToList(),
                tiles[3643].Last(),
                tiles[3643].Select(row => row.First()).Reverse().ToList(),
                tiles[3643].First().AsEnumerable().Reverse().ToList(),
                tiles[3643].Select(row => row.Last()).Reverse().ToList(),
                tiles[3643].Last().AsEnumerable().Reverse().ToList(),
                tiles[3643].Select(row => row.First()).ToList()
            };

            foreach (var id in tiles.Keys)
            {
                if (id == 3643) continue;

                var edges = new List<List<int>>
                {
                    tiles[id].First(),
                    tiles[id].Select(row => row.Last()).ToList(),
                    tiles[id].Last(),
                    tiles[id].Select(row => row.First()).Reverse().ToList()
                };

                //foreach()
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
