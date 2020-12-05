using System.IO;

namespace Template
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = File.ReadAllText("./input.txt");

            var lines = text.Split("\n");
        }
    }
}
