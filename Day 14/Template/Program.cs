using Combinatorics.Collections;
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

            var currentMask = "";
            Dictionary<int, long> memory1 = new Dictionary<int, long>();
            Dictionary<long, long> memory2 = new Dictionary<long, long>();

            for (var i = 0; i < input.Length; i++)
            {
                var parts = input[i].Split(" = ");
                if (parts[0] == "mask")
                {
                    currentMask = parts[1];
                    continue;
                }

                var addressString = parts[0].Skip(4)
                    .TakeWhile(c => c != ']')
                    .ToArray();

                var address = int.Parse(addressString);
                var value = int.Parse(parts[1]);

                memory1[address] = ApplyMaskToValue(currentMask, value);

                ApplyMaskToAddress(currentMask, address)
                    .ForEach(a => memory2[a] = value);
            }

            var answer1 = memory1.Sum(kvp => kvp.Value);
            WriteAnswer(1, answer1.ToString());

            var answer2 = memory2.Sum(kvp => kvp.Value);
            WriteAnswer(2, answer2.ToString());
        }

        private static long ApplyMaskToValue(string mask, int value)
        {
            var binaryValue = Convert.ToString(value, 2);
            var fullBinaryValue = Enumerable.Repeat('0', mask.Length - binaryValue.Length)
                .Concat(binaryValue);

            var maskedValue = mask.Zip(fullBinaryValue, (c1, c2) => c1 == 'X' ? c2 : c1)
                .ToArray();

            return Convert.ToInt64(new string(maskedValue), 2);
        }

        private static List<long> ApplyMaskToAddress(string mask, int address)
        {
            var binaryValue = Convert.ToString(address, 2);
            var fullBinaryValue = Enumerable.Repeat('0', mask.Length - binaryValue.Length)
                .Concat(binaryValue);

            var maskedValue = mask.Zip(fullBinaryValue, (c1, c2) => c1 == '0' ? c2 : c1)
                .ToArray();

            var maskedValueString = new string(maskedValue);

            var variations = new Variations<char>(
                new char[] { '0', '1' },
                maskedValue.Count(c => c == 'X'),
                GenerateOption.WithRepetition
            );

            return variations.Select(variation =>
            {
                var copy = string.Copy(maskedValueString).Split('X');

                int length = Math.Min(copy.Count(), variation.Count());

                var parts = copy.Take(length)
                    .Zip(variation.Take(length), (a, b) => new string[] { a, b.ToString() })
                    .SelectMany(x => x)
                    .Concat(copy.Skip(length))
                    .ToArray();

                var binaryString = string.Concat(parts);
                return Convert.ToInt64(binaryString, 2);
            })
            .ToList();
        }

        private static void WriteAnswer(int part, string answer)
        {
            Console.WriteLine($"Answer for part {part}:");
            Console.WriteLine(answer);
            Console.WriteLine("");
        }
    }
}
