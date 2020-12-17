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

            var input = text.Split("\r\n")
                .Select(l => l.Replace('#', '1')
                    .Replace('.', '0')
                    .Select(c => int.Parse(c.ToString()))
                    .ToArray())
                .ToArray();

            var inputGrid = new int[8, 8, 1, 1];
            for (var i = 0; i < input.Count(); i++)
            for (var j = 0; j < input.Count(); j++)
            {
                inputGrid[i,j,0,0] = input[i][j];
            }

            // Part 2:
            var grid = inputGrid;
            for (var step = 0; step < 6; step++) grid = Iterate(grid);

            WriteAnswer(1, CountActive(grid).ToString());
        }

        private static int[,,,] Iterate(int[,,,] grid)
        {
            var expandedGrid = Expand(grid);

            var newGrid = new int[
                grid.GetLength(0) + 2,
                grid.GetLength(1) + 2,
                grid.GetLength(2) + 2,
                grid.GetLength(3) + 2
            ];

            ForEach(newGrid, (i, j, k, l, v) =>
            {
                var numActiveNeighbours = CountActiveNeighbours(expandedGrid, i, j, k, l);
                if (expandedGrid[i, j, k, l] == 0 && numActiveNeighbours == 3) newGrid[i, j, k, l] = 1;
                if (expandedGrid[i, j, k, l] == 1 && (numActiveNeighbours == 2 || numActiveNeighbours == 3)) newGrid[i, j, k, l] = 1;
            });

            return newGrid;
        }

        private static int[,,,] Expand(int[,,,] grid)
        {
            var newGrid = new int[
                grid.GetLength(0) + 2,
                grid.GetLength(1) + 2,
                grid.GetLength(2) + 2,
                grid.GetLength(3) + 2
            ];

            ForEach(grid, (i, j, k, l, v) => newGrid[i + 1, j + 1, k + 1, l + 1] = v);
            return newGrid;
        }

        private static int CountActiveNeighbours(int[,,,] grid, int x, int y, int z, int w)
        {
            var numActiveNeighbours = 0;

            for (var i = x-1; i < x+2; i++)
            for (var j = y-1; j < y+2; j++)
            for (var k = z-1; k < z+2; k++)
            for (var l = w-1; l < w+2; l++)
            {
                if (i < 0 || i >= grid.GetLength(0)) continue;
                if (j < 0 || j >= grid.GetLength(1)) continue;
                if (k < 0 || k >= grid.GetLength(2)) continue;
                if (l < 0 || l >= grid.GetLength(3)) continue;
                if (i == x && j == y && k == z && l == w) continue;

                numActiveNeighbours += grid[i,j,k,l];
            }

            return numActiveNeighbours;
        }

        private static int CountActive(int[,,,] grid)
        {
            var count = 0;
            ForEach(grid, (i, j, k, l, v) => count += v);
            return count;
        }

        private static void ForEach(int[,,,] grid, Action<int, int, int, int, int> action)
        {
            for (var i = 0; i < grid.GetLength(0); i++)
            for (var j = 0; j < grid.GetLength(1); j++)
            for (var k = 0; k < grid.GetLength(2); k++)
            for (var l = 0; l < grid.GetLength(2); l++)
            {
                action(i, j, k, l, grid[i,j,k,l]);
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
