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

            var rawDocStrings = text.Split("\n\r\n");

            var docStrings = rawDocStrings
                .Select(d => d.Replace('\n', ' '))
                .Select(d => d.Split(' '));

            var docs = docStrings.Select(ParseDocString);

            var numValid = docs.Count(CheckValid);

            Console.WriteLine(numValid);
        }

        public static Dictionary<string, string> ParseDocString(string[] docString)
        {
            var doc = new Dictionary<string, string>();

            foreach (var line in docString)
            {
                var parts = line.Split(':');
                doc.Add(parts[0], parts[1]);
            }

            return doc;
        }

        private static bool CheckValid(Dictionary<string, string> doc)
        {
            var requiredKeys = new string[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

            doc.Remove("cid");

            var docKeys = doc.Keys.ToList();

            if (docKeys.Intersect(requiredKeys).Count() != requiredKeys.Count()) return false;

            if (!int.TryParse(doc["byr"], out int byr) || byr < 1920 || byr > 2002) return false;

            if (!int.TryParse(doc["iyr"], out int iyr) || iyr < 2010 || iyr > 2020) return false;

            if (!int.TryParse(doc["eyr"], out int eyr) || eyr < 2020 || eyr > 2030) return false;

            if (!ValidHeight(doc["hgt"].Trim())) return false;

            var hclString = doc["hcl"].Trim();
            if (hclString.Length != 7 || hclString[0] != '#' || hclString.Skip(1).Any(c => !"0123456789abcdef".Contains(c))) return false;

            if (!new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(doc["ecl"].Trim())) return false;

            if (doc["pid"].Trim().Count() != 9 || !int.TryParse(doc["pid"], out int pid)) return false;


            return true;
        }

        private static bool ValidHeight(string heightString)
        {
            switch (heightString.Count())
            {
                case 4:
                    if (heightString[2] != 'i') return false;
                    if (heightString[3] != 'n') return false;
                    var inString = heightString.Substring(0, 2);
                    if (!int.TryParse(inString, out int inches) || inches < 59 || inches > 76) return false;
                    return true;
                case 5:
                    if (heightString[3] != 'c') return false;
                    if (heightString[4] != 'm') return false;
                    var cmString = heightString.Substring(0, 3);
                    if (!int.TryParse(cmString, out int cm) || cm < 150 || cm > 193) return false;
                    return true;
                default:
                    return false;
            }
        }
    }
}
