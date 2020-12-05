using System;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = File.ReadAllText("./input.txt");

            var lines = text.Split("\n");

            var contexts = lines.Select(line => ParseContext(line));

            var validCount = contexts.Count(c => c.IsValid());

            Console.WriteLine(validCount);
        }

        private static PasswordContext ParseContext(string line)
        {
            var context = new PasswordContext();

            var parts = line.Split(' ');

            var range = parts[0];

            var rangeValues = range
                .Split('-')
                .Select(v => int.Parse(v))
                .ToList();

            context.MinPosition = rangeValues[0];
            context.MaxPosition = rangeValues[1];

            context.Character = parts[1][0];

            context.Password = parts[2];

            return context;
        }

        private class PasswordContext
        {
            public int MinPosition { get; set; }
            public int MaxPosition { get; set; }
            public char Character { get; set; }
            public string Password { get; set; }

            public bool IsValid()
            {
                var firstChar = Password[MinPosition - 1];
                var secondChar = Password[MaxPosition - 1];

                return firstChar == Character ^ secondChar == Character;
            }
        }
    }
}
