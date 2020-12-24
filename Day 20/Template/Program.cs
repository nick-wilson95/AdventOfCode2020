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

            var tileData = text.Split("\r\n\r\n");

            var tiles = new Dictionary<int, List<int[]>>();

            foreach (var data in tileData)
            {
                var lines = data.Split("\r\n");
                var index = int.Parse(lines[0].Substring(5,4));

                var tileContents = new List<int[]>();
                for (var i = 1; i < lines.Length; i++)
                {
                    var row = new int[lines[i].Length];

                    for (var j = 0; j < lines[i].Length; j++)
                    {
                        row[j] = lines[i][j] == '#' ? 1 : 0;
                    }
                    tileContents.Add(row);
                }

                tiles.Add(index, tileContents);
            }

            var tileEdges = new Dictionary<int, List<int[]>>();
            foreach (var tile in tiles)
            {
                var content = tile.Value;

                var edges = new List<int[]>();

                edges.Add(content.First());
                edges.Add(content.Last());
                edges.Add(content.Select(r => r.First()).ToArray());
                edges.Add(content.Select(r => r.Last()).ToArray());

                tileEdges[tile.Key] = edges;
            }

            var edgeMatches = new Dictionary<int, List<int>>();

            foreach (var tile in tiles)
            {
                edgeMatches[tile.Key] = new List<int>();

                foreach (var edge in tileEdges[tile.Key])
                {
                    foreach (var edges in tileEdges)
                    {
                        if (edges.Key == tile.Key) continue;

                        if (
                            Enumerable.SequenceEqual(edge, edges.Value[0])
                            || Enumerable.SequenceEqual(edge, edges.Value[0].Reverse())
                            || Enumerable.SequenceEqual(edge, edges.Value[1])
                            || Enumerable.SequenceEqual(edge, edges.Value[1].Reverse())
                            || Enumerable.SequenceEqual(edge, edges.Value[2])
                            || Enumerable.SequenceEqual(edge, edges.Value[2].Reverse())
                            || Enumerable.SequenceEqual(edge, edges.Value[3])
                            || Enumerable.SequenceEqual(edge, edges.Value[3].Reverse())
                        )
                        {
                            edgeMatches[tile.Key].Add(edges.Key);
                        }
                    }
                }
            }

            var answer1 = edgeMatches.Where(kvp => kvp.Value.Count() == 2)
                .Select(kvp => (long)kvp.Key)
                .Aggregate((a,b) => a*b);

            WriteAnswer(1, answer1.ToString());

            var tileGrid = new List<List<int>>();
            var placedTiles = new List<int>();
            tileGrid.Add(new List<int>());

            var firstTileId = edgeMatches.First(kvp => kvp.Value.Count() == 2).Key;
            tileGrid[0].Add(firstTileId);
            placedTiles.Add(firstTileId);

            for (var i = 1; i < 11; i++)
            {
                tileGrid[0].Add(
                    edgeMatches.First(kvp =>
                        kvp.Value.Count() == 3
                        && kvp.Value.Contains(tileGrid[0][i - 1])
                        && !placedTiles.Contains(kvp.Key)
                    ).Key
                );
                placedTiles.Add(tileGrid[0][i]);
            }

            tileGrid[0].Add(edgeMatches.First(kvp => kvp.Value.Count() == 2 && kvp.Value.Contains(tileGrid[0][10])).Key);
            placedTiles.Add(tileGrid[0][11]);


            for (var j = 1; j < 11; j++)
            {
                tileGrid.Add(new List<int>());
                var firstRowTileId = edgeMatches.First(kvp => kvp.Value.Count() == 3 && kvp.Value.Contains(tileGrid[j - 1][0]) && !placedTiles.Contains(kvp.Key)).Key;
                tileGrid[j].Add(firstRowTileId);
                placedTiles.Add(firstRowTileId);

                for (var i = 1; i < 11; i++)
                {
                    tileGrid[j].Add(
                        edgeMatches.First(kvp =>
                            kvp.Value.Count() == 4
                            && kvp.Value.Contains(tileGrid[j][i - 1])
                            && kvp.Value.Contains(tileGrid[j - 1][i])
                            && !placedTiles.Contains(kvp.Key)
                        ).Key
                    );
                    placedTiles.Add(tileGrid[j][i]);
                }

                tileGrid[j].Add(edgeMatches.First(kvp =>
                    kvp.Value.Count() == 3
                    && kvp.Value.Contains(tileGrid[j][10])
                    && kvp.Value.Contains(tileGrid[j - 1][11])
                    && !placedTiles.Contains(kvp.Key)
                ).Key);
                placedTiles.Add(tileGrid[j][11]);
            }

            tileGrid.Add(new List<int>());
            var firstFinalRowTileId = edgeMatches.First(kvp => kvp.Value.Count() == 2 && kvp.Value.Contains(tileGrid[10][0]) && !placedTiles.Contains(kvp.Key)).Key;
            tileGrid[11].Add(firstFinalRowTileId);
            placedTiles.Add(firstFinalRowTileId);

            for (var i = 1; i < 11; i++)
            {
                tileGrid[11].Add(
                    edgeMatches.First(kvp =>
                        kvp.Value.Count() == 3
                        && kvp.Value.Contains(tileGrid[11][i - 1])
                        && kvp.Value.Contains(tileGrid[10][i])
                        && !placedTiles.Contains(kvp.Key)
                    ).Key
                );
                placedTiles.Add(tileGrid[11][i]);
            }

            tileGrid[11].Add(edgeMatches.First(kvp => kvp.Value.Count() == 2 && kvp.Value.Contains(tileGrid[11][10]) && kvp.Value.Contains(tileGrid[10][11])).Key);
            placedTiles.Add(tileGrid[11][11]);
        }

        private static void WriteAnswer(int part, string answer)
        {
            Console.WriteLine($"Answer for part {part}:");
            Console.WriteLine(answer);
            Console.WriteLine("");
        }
    }
}
