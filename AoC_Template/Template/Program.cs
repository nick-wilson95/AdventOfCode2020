using System;
using System.IO;

namespace Template
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = File.ReadAllText("./input.txt");

            var lines = text.Split("\r\n");
        }

        private static void WriteAnswer(int part, string answer)
        {
            Console.WriteLine("Answer for part 2:");
            Console.WriteLine(answer);
            Console.WriteLine("");
        }
    }
}
