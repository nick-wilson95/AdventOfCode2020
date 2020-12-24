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

            var sections = text.Split("\r\n\r\n");

            var cards = sections.Select(x =>
                x.Split("\r\n")
                    .Skip(1)
                    .Select(int.Parse)
                    .ToList())
                .ToArray();

            var result = PlayRecursively(cards[0], cards[1], new List<string>(), out var deck1Wins);

            Console.WriteLine(result.Select((v, i) => v * (50 - i)).Sum());
        }

        private static List<int> PlayRecursively(List<int> deck1, List<int> deck2, List<string> previousConfigurations, out bool deck1Wins)
        {
            if (!deck2.Any())
            {
                deck1Wins = true;
                return deck1;
            }
            if (!deck1.Any())
            {
                deck1Wins = false;
                return deck2;
            }

            if (deck1.Count() + deck2.Count() == 50)
            {
                Console.WriteLine($"{deck1.Count()}({deck1[0]}) :: {deck2.Count()}({deck2[0]})");
            }

            var configArray = new char[50];
            for (var i = 0; i < configArray.Length; i++) configArray[i] = '0';
            deck1.ForEach(v => configArray[v - 1] = '1');
            deck2.ForEach(v => configArray[v - 1] = '2');
            var configuration = string.Concat(configArray);

            var deck1WinsRound = true;

            if (previousConfigurations.Contains(configuration))
            {
                // Deck 1 wins round
            }
            else if (deck1.Count() - 1 < deck1[0] || deck2.Count - 1 < deck2[0])
            {
                deck1WinsRound = deck1[0] > deck2[0];
            }
            else
            {
                PlayRecursively(
                    deck1.Skip(1).Take(deck1[0]).ToList(),
                    deck2.Skip(1).Take(deck2[0]).ToList(),
                    new List<string>(),
                    out deck1WinsRound
                );
            }

            if (deck1WinsRound)
            {
                HandleWin(deck1, deck2);
            }
            else
            {
                HandleWin(deck2, deck1);
            }

            previousConfigurations.Add(configuration);

            return PlayRecursively(
                deck1.Select(x => x).ToList(),
                deck2.Select(x => x).ToList(),
                previousConfigurations,
                out deck1Wins
            );
        }

        private static void HandleWin(List<int> winningDeck, List<int> losingDeck)
        {
            winningDeck.Add(winningDeck[0]);
            winningDeck.Add(losingDeck[0]);

            winningDeck.RemoveAt(0);
            losingDeck.RemoveAt(0);
        }

        private static void WriteAnswer(int part, string answer)
        {
            Console.WriteLine($"Answer for part {part}:");
            Console.WriteLine(answer);
            Console.WriteLine("");
        }
    }
}
