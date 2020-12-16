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

            var rules = sections[0]
                .Split("\r\n")
                .Select(l =>
                {
                    var parts = l.Split(": ");

                    var name = parts[0];

                    var values = parts[1]
                        .Split(" or ")
                        .Select(x => x.Split('-'))
                        .SelectMany(x => x.Select(y => int.Parse(y)))
                        .ToArray();

                    return new Rule(name, values);
                });

            var ourValues = sections[1]
                .Split("\r\n")
                .Last()
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            var otherValues = sections[2]
                .Split("\r\n")
                .Skip(1)
                .Select(l => l.Split(',').Select(int.Parse).ToArray());

            // Part 1:
            var errorRate = otherValues.SelectMany(x => x)
                .ToList()
                .Where(v => !IsValid(v, rules))
                .Sum();

            WriteAnswer(1, errorRate.ToString());

            // Part 2:
            var validValues = otherValues.Where(v => !v.Any(x => !IsValid(x, rules)));

            var ruleValidPositions = new Dictionary<string, List<int>>();

            foreach (var rule in rules)
            {
                ruleValidPositions.Add(rule.Name, new List<int>());

                for (var i = 0; i < rules.Count(); i++)
                {
                    var invalidForPosition = validValues.Select(v => v[i])
                        .Any(v => !rule.IsValid(v));

                    if (invalidForPosition) continue;

                    ruleValidPositions[rule.Name].Add(i);
                }
            }

            var rulePositions = new Dictionary<string, int>();

            ruleValidPositions.OrderBy(kvp => kvp.Value.Count())
                .ToList()
                .ForEach(kvp =>
                {
                    rulePositions.Add(kvp.Key, kvp.Value.First(v => !rulePositions.Values.Contains(v)));
                });

            var answer2 = rulePositions.Where(kvp => kvp.Key.Contains("departure"))
                .Select(kvp => (long)ourValues[kvp.Value])
                .Aggregate((a, b) => a * b);

            WriteAnswer(2, answer2.ToString());
        }

        private static bool IsValid(int x, IEnumerable<Rule> rules)
        {
            return rules.Any(r => r.IsValid(x));
        }

        private class Rule
        {
            public Rule(string name, int[] values)
            {
                Name = name;
                Values = values;
            }

            public string Name { get; set; }
            public int[] Values { get; set; }

            public bool IsValid(int x)
            {
                return (x >= Values[0] && x <= Values[1]) || (x >= Values[2] && x <= Values[3]);
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
