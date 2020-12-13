using System;
using System.IO;
using System.Linq;

namespace Template
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = File.ReadAllText("./input.txt");

            var commands = text.Split("\r\n")
                .Select(x => new Command(x))
                .ToArray();

            var answer1 = CalculateAnswer1(commands);
            WriteAnswer(1, answer1.ToString());

            var answer2 = CalculateAnswer2(commands);
            WriteAnswer(2, answer2.ToString());
        }

        private static int CalculateAnswer1(Command[] commands)
        {
            var north = 0;
            var east = 0;
            var angle = 0;

            foreach (var command in commands)
            {
                switch (command.Action)
                {
                    case 'N':
                        north += command.Value; break;
                    case 'S':
                        north -= command.Value; break;
                    case 'E':
                        east += command.Value; break;
                    case 'W':
                        east -= command.Value; break;
                    case 'L':
                        angle += command.Value; break;
                    case 'R':
                        angle -= command.Value; break;
                    case 'F':
                        switch (Mod(angle, 360))
                        {
                            case 0:
                                east += command.Value; break;
                            case 90:
                                north += command.Value; break;
                            case 180:
                                east -= command.Value; break;
                            case 270:
                                north -= command.Value; break;
                        }
                        break;
                }
            }

            return Math.Abs(east) + Math.Abs(north);
        }

        private static int CalculateAnswer2(Command[] commands)
        {
            var wpOffsetEast = 10;
            var wpOffsetNorth = 1;

            var north = 0;
            var east = 0;

            foreach (var command in commands)
            {
                switch (command.Action)
                {
                    case 'N':
                        wpOffsetNorth += command.Value; break;
                    case 'S':
                        wpOffsetNorth -= command.Value; break;
                    case 'E':
                        wpOffsetEast += command.Value; break;
                    case 'W':
                        wpOffsetEast -= command.Value; break;
                    case 'F':                        
                        north += wpOffsetNorth * command.Value;
                        east += wpOffsetEast * command.Value;
                        break;
                    default:
                        var angle = command.Value * (command.Action == 'L' ? 1 : -1);
                        switch (Mod(angle, 360))
                        {
                            case 90:
                                Switch(ref wpOffsetEast, ref wpOffsetNorth);
                                wpOffsetEast = -wpOffsetEast;
                                break;
                            case 180:
                                wpOffsetEast = -wpOffsetEast;
                                wpOffsetNorth = -wpOffsetNorth;
                                break;
                            case 270:
                                Switch(ref wpOffsetEast, ref wpOffsetNorth);
                                wpOffsetNorth = -wpOffsetNorth;
                                break;
                        }
                        break;
                }
            }

            return Math.Abs(east) + Math.Abs(north);
        }

        private class Command
        {
            public char Action { get; }
            public int Value { get; }

            public Command(string commandString)
            {
                Action = commandString[0];
                Value = int.Parse(commandString.Skip(1).ToArray());
            }
        }
        private static int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }

        private static void Switch(ref int a, ref int b)
        {
            var x = a;
            a = b;
            b = x;
        }

        private static void WriteAnswer(int part, string answer)
        {
            Console.WriteLine($"Answer for part {part}:");
            Console.WriteLine(answer);
            Console.WriteLine("");
        }
    }
}
