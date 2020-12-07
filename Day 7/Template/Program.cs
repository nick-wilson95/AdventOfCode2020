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

            var statements = text.Split("\r\n");

            Dictionary<string, Dictionary<string, int>> bags = new Dictionary<string, Dictionary<string, int>>();

            foreach (var statement in statements)
            {
                var parts = statement.Split(" contain ");

                var bag = parts[0]
                    .Replace(" bags", "")
                    .Replace(" bag", "");

                var contentString = parts[1];

                bags.Add(bag, new Dictionary<string, int>());

                if (contentString == "no other bags.") continue;

                var contents = contentString
                    .Replace(" bags", "")
                    .Replace(" bag", "")
                    .TrimEnd('.').Split(", ");

                foreach (var item in contents)
                {
                    var itemParts = item.Split(' ');
                    var itemBag = string.Join(' ', itemParts.Skip(1));
                    var quantity = int.Parse(itemParts[0]);
                    bags[bag].Add(itemBag, quantity);
                }
            }

            Console.WriteLine(bags.Keys.Count(b => ContainsBag(b, "shiny gold", bags)));
            Console.WriteLine(CountBags("shiny gold", bags));
        }

        public static bool ContainsBag(string bag, string bagToContain, Dictionary<string, Dictionary<string, int>> bags)
        {
            var contents = bags[bag];

            if (contents.Count() == 0) return false;

            if (contents.Keys.Contains(bagToContain)) return true;

            foreach (var containedBag in contents.Keys)
            {
                if (ContainsBag(containedBag, bagToContain, bags)) return true;
            }

            return false;
        }

        public static int CountBags(string bag, Dictionary<string, Dictionary<string, int>> bags)
        {
            if (bags[bag].Count() == 0) return 1;

            return bags[bag]
                .Select(b => b.Value * CountBags(b.Key, bags))
                .Sum() + 1;
        }
    }
}
