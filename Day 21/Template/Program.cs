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

            var ingredients = new Dictionary<int, string[]>();
            var allergens = new Dictionary<int, string[]>();

            for (var i = 0; i < input.Length; i++)
            {
                var parts = input[i].Split(" (contains ");
                ingredients[i] = parts[0].Split(' ');
                allergens[i] = parts[1].Substring(0, parts[1].Length - 1)
                    .Split(", ");
            }

            var allIngredients = ingredients.SelectMany(kvp => kvp.Value).Distinct();
            var allAllergens = allergens.SelectMany(kvp => kvp.Value).Distinct();

            var possibleHosts = new Dictionary<string, List<string>>();
            foreach (var allergen in allAllergens)
            {
                possibleHosts[allergen] = allIngredients.ToList();
            }

            for (var i = 0; i < input.Length; i++)
            {
                foreach (var allergen in allergens[i])
                {
                    possibleHosts[allergen] = possibleHosts[allergen]
                        .Where(x => ingredients[i].Contains(x))
                        .ToList();
                }
            }

            var noAllergens = allIngredients.Where(i => !possibleHosts.SelectMany(kvp => kvp.Value).Contains(i));

            var hosts = new Dictionary<string, string>();

            while (possibleHosts.Keys.Any())
            {
                foreach (var kvp in possibleHosts)
                {
                    if (kvp.Value.Count() == 1)
                    {
                        hosts.Add(kvp.Value[0], kvp.Key);
                        possibleHosts.Remove(kvp.Key);
                        foreach (var key in possibleHosts.Keys)
                        {
                            possibleHosts[key].Remove(kvp.Value[0]);
                        }
                        break;
                    }
                }
            }

            Console.WriteLine(ingredients.SelectMany(kvp => kvp.Value).Count(x => noAllergens.Contains(x)));
        }

        private static void WriteAnswer(int part, string answer)
        {
            Console.WriteLine($"Answer for part {part}:");
            Console.WriteLine(answer);
            Console.WriteLine("");
        }
    }
}
