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

            var commands = text.Split("\r\n")
                .Select(str => new Command(str))
                .ToArray();

            var result = 0;

            for (var i = 0; i < commands.Length; i++)
            {
                var command = commands[i];

                if (command.Operation == "acc") continue;

                command.Operation = command.Operation == "nop" ? "jmp" : "nop";

                if (TryTerminate(commands, out result)) break;

                command.Operation = command.Operation == "nop" ? "jmp" : "nop";
            }

            Console.WriteLine(result);
        }

        private static bool TryTerminate(Command[] commands, out int result)
        {
            var commandsRun = new List<int>();
            var operationIndex = 0;
            result = 0;

            while (true)
            {
                if (operationIndex >= commands.Length) return true;

                var command = commands[operationIndex];

                if (commandsRun.Contains(operationIndex)) return false;

                commandsRun.Add(operationIndex);

                switch (command.Operation)
                {
                    case "nop":
                        operationIndex++;
                        break;
                    case "acc":
                        result += command.Value;
                        operationIndex++;
                        break;
                    case "jmp":
                        operationIndex += command.Value;
                        break;
                }
            }
        }

        private class Command
        {
            public Command(string commandString)
            {
                var parts = commandString.Split(' ');
                Operation = parts[0];
                Value = int.Parse(parts[1]);
            }

            public string Operation { get; set; }
            public int Value { get; }
        }
    }
}
