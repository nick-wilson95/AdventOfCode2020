using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Template
{
    class Program
    {
        static Dictionary<int, char> letterRules;
        static Dictionary<int, int> singleRules;
        static Dictionary<int, int[]> doubleRules;
        static Dictionary<int, int[]> singleOrRules;
        static Dictionary<int, int[]> doubleOrRules;

        static void Main()
        {
            var text = File.ReadAllText("./input.txt");

            var inputParts = text.Split("\r\n\r\n");

            letterRules = new Dictionary<int, char>();
            singleRules = new Dictionary<int, int>();
            doubleRules = new Dictionary<int, int[]>();
            singleOrRules = new Dictionary<int, int[]>();
            doubleOrRules = new Dictionary<int, int[]>();

            var rules = inputParts[0].Split("\r\n");

            foreach (var rule in rules)
            {
                var parts = rule.Split(": ");
                var index = int.Parse(parts[0]);
                var valueString = parts[1];

                if (valueString.First() == '"')
                {
                    letterRules.Add(index, valueString[1]);
                    continue;
                }

                var values = valueString.Split(' ').ToList();

                if (values.Count() > 3)
                {
                    values.RemoveAt(2);
                    doubleOrRules.Add(index, values.Select(int.Parse).ToArray());
                }
                else if (values.Count() > 2)
                {
                    values.RemoveAt(1);
                    singleOrRules.Add(index, values.Select(int.Parse).ToArray());
                }
                else if (values.Count() > 1)
                {
                    doubleRules.Add(index, values.Select(int.Parse).ToArray());
                }
                else
                {
                    singleRules.Add(index, int.Parse(values[0]));
                }
            }

            var validStrings = new Dictionary<int, string[]>();
            SetValidStrings(0, validStrings);

            var messages = inputParts[1].Split("\r\n");

            var answer1 = messages.Count(m => validStrings[0].Contains(m));
            WriteAnswer(1, answer1.ToString());

            var validStrings1 = validStrings[42];
            var validStrings2 = validStrings[31];

            var answer2 = messages.Count(m => TestValidity(m, validStrings1, validStrings2));
            WriteAnswer(2, answer2.ToString());
        }

        private static bool TestValidity(string message, string[] validStrings1, string[] validStrings2)
        {
            var sectionCount = message.Length / 8;

            for (var i = sectionCount/2 + 1; i < sectionCount; i++)
            {
                var isValid = true;

                for (var j = 0; j < i; j++)
                {
                    if (!validStrings1.Contains(message.Substring(j * 8, 8))) isValid = false;
                }

                for (var j = i; j < sectionCount; j++)
                {
                    if (!validStrings2.Contains(message.Substring(j * 8, 8))) isValid = false;
                }

                if (isValid) return true;
            }

            return false;
        }

        private static string[] SetValidStrings(int ruleNum, Dictionary<int, string[]> validStrings)
        {
            if (validStrings.ContainsKey(ruleNum)) return validStrings[ruleNum];

            if (letterRules.ContainsKey(ruleNum))
            {
                validStrings[ruleNum] = new string[] { letterRules[ruleNum].ToString() };
                return validStrings[ruleNum];
            }

            if (singleRules.ContainsKey(ruleNum))
            {
                validStrings[ruleNum] = SetValidStrings(singleRules[ruleNum], validStrings);
                return validStrings[ruleNum];
            }

            if (doubleRules.ContainsKey(ruleNum))
            {
                var stringParts1 = SetValidStrings(doubleRules[ruleNum][0], validStrings);
                var stringParts2 = SetValidStrings(doubleRules[ruleNum][1], validStrings);
                validStrings[ruleNum] = stringParts1.Select(s1 => stringParts2.Select(s2 => s1+s2)).SelectMany(s => s).Distinct().ToArray();
                return validStrings[ruleNum];
            }

            if (singleOrRules.ContainsKey(ruleNum))
            {
                var validStrings1 = SetValidStrings(singleOrRules[ruleNum][0], validStrings);
                var validStrings2 = SetValidStrings(singleOrRules[ruleNum][1], validStrings);
                validStrings[ruleNum] = validStrings1.Concat(validStrings2).Distinct().ToArray();
                return validStrings[ruleNum];
            }

            if (doubleOrRules.ContainsKey(ruleNum))
            {
                var stringParts1 = SetValidStrings(doubleOrRules[ruleNum][0], validStrings);
                var stringParts2 = SetValidStrings(doubleOrRules[ruleNum][1], validStrings);
                var validStrings1 = stringParts1.Select(s1 => stringParts2.Select(s2 => s1 + s2)).SelectMany(s => s);

                var stringParts3 = SetValidStrings(doubleOrRules[ruleNum][2], validStrings);
                var stringParts4 = SetValidStrings(doubleOrRules[ruleNum][3], validStrings);
                var validStrings2 = stringParts3.Select(s1 => stringParts4.Select(s2 => s1 + s2)).SelectMany(s => s);

                validStrings[ruleNum] = validStrings1.Concat(validStrings2).Distinct().ToArray();
                return validStrings[ruleNum];
            }

            throw new Exception("Invalid rule.");
        }

        private static void WriteAnswer(int part, string answer)
        {
            Console.WriteLine($"Answer for part {part}:");
            Console.WriteLine(answer);
            Console.WriteLine("");
        }
    }
}
