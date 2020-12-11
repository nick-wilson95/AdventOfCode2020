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

            var initialSeats = text.Split("\r\n")
                .Select(x => x.ToArray())
                .ToArray();

            // Part 1:
            char[][] iteratedSeats = Copy(initialSeats);
            while (true)
            {
                if (!TryIterate(iteratedSeats, 4, false)) break;
            }

            var asnwer1 = iteratedSeats.Sum(col => col.Count(c => c == '#')).ToString();
            WriteAnswer(1, asnwer1);

            // Part 2:
            iteratedSeats = Copy(initialSeats);
            while (true)
            {
                if (!TryIterate(iteratedSeats, 5, true)) break;
            }

            var asnwer2 = iteratedSeats.Sum(col => col.Count(c => c == '#')).ToString();
            WriteAnswer(2, asnwer2);
        }

        private static bool TryIterate(char[][] seats, int neighbourLimit, bool highVisibility)
        {
            var rowCount = seats.Length;
            var colCount = seats[0].Length;

            var occupiedNeighbours = new int[rowCount, colCount];

            // Loop through seats
            for (var i = 0; i < rowCount; i++)
            for (var j = 0; j < colCount; j++)
            {
                if (seats[i][j] != '#') continue;
                
                if (!highVisibility)
                {
                    // Loop through adjacent seats
                    for (var k=i-1; k<i+2; k++)
                    for (var l=j-1; l<j+2; l++)
                    {
                        if (
                            k >= 0 && k < rowCount
                            && l >= 0 && l < colCount
                            && !(k == i && j == l)
                        ) occupiedNeighbours[k, l]++;
                    }
                }
                else
                {
                    // Loop through adjacent seats
                    for (var k=-1; k<2; k++)
                    for (var l=-1; l<2; l++)
                    {
                        if (k == 0 && l == 0) continue;

                        var counter = 0;
                        while (true)
                        {
                            counter ++;
                            var row = i + counter * k;
                            var col = j + counter * l;

                            if (row < 0 || row >= rowCount || col < 0 || col >= colCount) break;
                            if (seats[row][col] == '.') continue;
                            occupiedNeighbours[row, col]++;
                            break;
                        }
                    }
                }
            }

            var seatsChanged = false;
            for (var i = 0; i < rowCount; i++)
            for (var j = 0; j < colCount; j++)
            {
                if (seats[i][j] == 'L' && occupiedNeighbours[i, j] == 0)
                {
                    seats[i][j] = '#';
                    seatsChanged = true;
                }
                if (seats[i][j] == '#' && occupiedNeighbours[i, j] >= neighbourLimit)
                {
                    seats[i][j] = 'L';
                    seatsChanged = true;
                }
            }

            return seatsChanged;
        }

        private static char[][] Copy(char[][] input)
        {
            return input.Select(x => x.Select(y => y).ToArray()).ToArray();
        }

        private static void WriteAnswer(int part, string answer)
        {
            Console.WriteLine($"Answer for part {part}:");
            Console.WriteLine(answer);
            Console.WriteLine("");
        }
    }
}
