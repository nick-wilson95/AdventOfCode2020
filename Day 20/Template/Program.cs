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
                var index = int.Parse(lines[0].Substring(5, 4));

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
                edges.Add(content.Select(r => r.Last()).ToArray());
                edges.Add(content.Last().Reverse().ToArray());
                edges.Add(content.Select(r => r.First()).Reverse().ToArray());

                tileEdges[tile.Key] = edges;
            }

            var edgeMatches = new Dictionary<int, List<Tuple<int, bool>>>();

            foreach (var tile in tiles)
            {
                edgeMatches[tile.Key] = new List<Tuple<int, bool>>();

                foreach (var edge in tileEdges[tile.Key])
                {
                    foreach (var edges in tileEdges)
                    {
                        if (edges.Key == tile.Key) continue;

                        if (
                            Enumerable.SequenceEqual(edge, edges.Value[0])
                            || Enumerable.SequenceEqual(edge, edges.Value[1])
                            || Enumerable.SequenceEqual(edge, edges.Value[2])
                            || Enumerable.SequenceEqual(edge, edges.Value[3])
                        )
                        {
                            edgeMatches[tile.Key].Add(new Tuple<int, bool>(edges.Key, true));
                        }

                        if (
                            Enumerable.SequenceEqual(edge, edges.Value[0].Reverse())
                            || Enumerable.SequenceEqual(edge, edges.Value[1].Reverse())
                            || Enumerable.SequenceEqual(edge, edges.Value[2].Reverse())
                            || Enumerable.SequenceEqual(edge, edges.Value[3].Reverse())
                        )
                        {
                            edgeMatches[tile.Key].Add(new Tuple<int, bool>(edges.Key, false));
                        }
                    }
                }
            }

            var answer1 = edgeMatches.Where(kvp => kvp.Value.Count() == 2)
                .Select(kvp => (long)kvp.Key)
                .Aggregate((a, b) => a * b);

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
                        && kvp.Value.Select(t => t.Item1).Contains(tileGrid[0][i - 1])
                        && !placedTiles.Contains(kvp.Key)
                    ).Key
                );
                placedTiles.Add(tileGrid[0][i]);
            }

            tileGrid[0].Add(edgeMatches.First(kvp => kvp.Value.Count() == 2 && kvp.Value.Select(t => t.Item1).Contains(tileGrid[0][10])).Key);
            placedTiles.Add(tileGrid[0][11]);


            for (var j = 1; j < 11; j++)
            {
                tileGrid.Add(new List<int>());
                var firstRowTileId = edgeMatches.First(kvp => kvp.Value.Count() == 3 && kvp.Value.Select(t => t.Item1).Contains(tileGrid[j - 1][0]) && !placedTiles.Contains(kvp.Key)).Key;
                tileGrid[j].Add(firstRowTileId);
                placedTiles.Add(firstRowTileId);

                for (var i = 1; i < 11; i++)
                {
                    tileGrid[j].Add(
                        edgeMatches.First(kvp =>
                            kvp.Value.Count() == 4
                            && kvp.Value.Select(t => t.Item1).Contains(tileGrid[j][i - 1])
                            && kvp.Value.Select(t => t.Item1).Contains(tileGrid[j - 1][i])
                            && !placedTiles.Contains(kvp.Key)
                        ).Key
                    );
                    placedTiles.Add(tileGrid[j][i]);
                }

                tileGrid[j].Add(edgeMatches.First(kvp =>
                    kvp.Value.Count() == 3
                    && kvp.Value.Select(t => t.Item1).Contains(tileGrid[j][10])
                    && kvp.Value.Select(t => t.Item1).Contains(tileGrid[j - 1][11])
                    && !placedTiles.Contains(kvp.Key)
                ).Key);
                placedTiles.Add(tileGrid[j][11]);
            }

            tileGrid.Add(new List<int>());
            var firstFinalRowTileId = edgeMatches.First(kvp => kvp.Value.Count() == 2 && kvp.Value.Select(t => t.Item1).Contains(tileGrid[10][0]) && !placedTiles.Contains(kvp.Key)).Key;
            tileGrid[11].Add(firstFinalRowTileId);
            placedTiles.Add(firstFinalRowTileId);

            for (var i = 1; i < 11; i++)
            {
                tileGrid[11].Add(
                    edgeMatches.First(kvp =>
                        kvp.Value.Count() == 3
                        && kvp.Value.Select(t => t.Item1).Contains(tileGrid[11][i - 1])
                        && kvp.Value.Select(t => t.Item1).Contains(tileGrid[10][i])
                        && !placedTiles.Contains(kvp.Key)
                    ).Key
                );
                placedTiles.Add(tileGrid[11][i]);
            }

            tileGrid[11].Add(edgeMatches.First(kvp => kvp.Value.Count() == 2 && kvp.Value.Select(t => t.Item1).Contains(tileGrid[11][10]) && kvp.Value.Select(t => t.Item1).Contains(tileGrid[10][11])).Key);
            placedTiles.Add(tileGrid[11][11]);

            var tilesFlipped = new Dictionary<int, bool>();
            tilesFlipped.Add(tileGrid[0][0], false);

            for (var i = 1; i < 12; i++)
            {
                var id = tileGrid[0][i];
                var leftId = tileGrid[0][i - 1];

                tilesFlipped.Add(
                    id,
                    tilesFlipped[leftId] ^ edgeMatches[id].First(m => m.Item1 == leftId).Item2
                );
            }

            for (var i = 1; i < 12; i++)
            for (var j = 0; j < 12; j++)
            {
                var id = tileGrid[i][j];
                var aboveId = tileGrid[i-1][j];

                tilesFlipped.Add(
                    id,
                    tilesFlipped[aboveId] ^ edgeMatches[id].First(m => m.Item1 == aboveId).Item2
                );
            }

            foreach(var kvp in tilesFlipped)
            {
                if (kvp.Value)
                {
                    tiles[kvp.Key] = Reflect(tiles[kvp.Key]);
                }
            }

            tiles[firstTileId] = RotateRight(tiles[firstTileId]);

            for (var i = 1; i < 12; i++)
            {
                var tileId = tileGrid[0][i];
                var tile = tiles[tileGrid[0][i]];
                var leftTile = tiles[tileGrid[0][i - 1]];

                while (true)
                {
                    var row = tile.Select(row => row.First());
                    var rowtoMach = leftTile.Select(row => row.Last());

                    if (Enumerable.SequenceEqual(row, rowtoMach)) break;
                    tile = RotateRight(tile);
                }

                tiles[tileGrid[0][i]] = tile;
            }

            for (var i = 1; i < 12; i++)
            for (var j = 0; j < 12; j++)
            {
                var tileId = tileGrid[i][j];
                var aboveTile = tiles[tileGrid[i-1][j]];

                while (!Enumerable.SequenceEqual(aboveTile.Last(), tiles[tileId].First()))
                {
                    tiles[tileId] = RotateRight(tiles[tileId]);
                }
            }

            foreach (var id in tiles.Keys.ToList())
            {
                tiles[id] = tiles[id].Skip(1).Take(8).Select(row => row.Skip(1).Take(8).ToArray()).ToList();
            }

            var grid = tileGrid.Select(row => row.Select(id => tiles[id]).ToList()).ToList();
            var image = new List<List<int>>();
            for (var i = 0; i < 96; i++) image.Add(new List<int>());

            for (var i = 0; i < 12; i++)
            for (var j = 0; j < 12; j++)
            for (var s = 0; s < 8; s++)
            for (var t = 0; t < 8; t++)
            {
                image[i * 8 + s].Add(grid[i][j][s][t]);
            }

            Console.WriteLine(string.Join("\r\n", image.Select(row => string.Concat(row.Select(x => x.ToString())))));

            var seaMonsterPositions = new Dictionary<int, List<int>>
            {
                { 0, new List<int> { 18 } },
                { 1, new List<int> { 0, 5, 6, 11, 12, 17, 18, 19 } },
                { 2, new List<int> { 1, 4, 7, 10, 13, 16 } }
            };

            image = RotateRight(RotateRight(RotateRight(Reflect(image))));

            var seaMonsterLocations = new List<Tuple<int, int>>();
            for (var i = 0; i < 94; i++)
            {
                for (var j = 0; j < 77; j++)
                {
                    if (seaMonsterPositions[0].Any(p => image[i][j + p] != 1)) continue;
                    if (seaMonsterPositions[1].Any(p => image[i + 1][j + p] != 1)) continue;
                    if (seaMonsterPositions[2].Any(p => image[i + 2][j + p] != 1)) continue;

                    seaMonsterLocations.Add(new Tuple<int, int>(i, j));
                }
            }

            var answer = image.Sum(row => row.Count(v => v == 1)) - seaMonsterLocations.Count() * 15;
        }

        private static List<int[]> Reflect(List<int[]> list)
        {
            return list[0].Select((_, i) => list.Select(row => row[i]).ToArray()).ToList();
        }

        private static List<List<int>> Reflect(List<List<int>> list)
        {
            return list[0].Select((_, i) => list.Select(row => row[i]).ToList()).ToList();
        }

        private static List<int[]> RotateRight(List<int[]> list)
        {
            return list[0].Select((_, i) => list.AsEnumerable().Reverse().Select(row => row[i]).ToArray()).ToList();
        }

        private static List<List<int>> RotateRight(List<List<int>> list)
        {
            return list[0].Select((_, i) => list.AsEnumerable().Reverse().Select(row => row[i]).ToList()).ToList();
        }

        private static void WriteAnswer(int part, string answer)
        {
            Console.WriteLine($"Answer for part {part}:");
            Console.WriteLine(answer);
            Console.WriteLine("");
        }
    }
}
