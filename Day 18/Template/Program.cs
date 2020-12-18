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

            var answer1 = input.Select(i => Equation.Parse(i).GetValue1())
                .Sum();

            WriteAnswer(1, answer1.ToString());

            var answer2 = input.Select(i => Equation.Parse(i).GetValue2())
                .Sum();

            WriteAnswer(2, answer2.ToString());
        }

        private class Equation
        {
            public Equation(List<Equation> equations, List<char> operators)
            {
                Equations = equations;
                Operators = operators;
            }
            public Equation(long numericValue)
            {
                NumericValue = numericValue;
            }

            List<Equation> Equations { get; } = new List<Equation>();
            List<char> Operators { get; } = new List<char>();
            long? NumericValue { get; } = null;

            public long GetValue1()
            {
                if (NumericValue.HasValue) return NumericValue.Value;

                var value = Equations[0].GetValue1();
                for (var i = 0; i < Equations.Count - 1; i++)
                {
                    var nextValue = Equations[i + 1].GetValue1();
                    if (Operators[i] == '*') value *= nextValue;
                    if (Operators[i] == '+') value += nextValue;
                }

                return value;
            }

            public long GetValue2()
            {
                if (NumericValue.HasValue) return NumericValue.Value;

                var values = Equations.Select(e => e.GetValue2()).ToList();

                while (Operators.Any(o => o == '+'))
                {
                    var index = Operators.FindLastIndex(o => o == '+');
                    Operators.RemoveAt(index);

                    values[index] += values[index + 1];
                    values.RemoveAt(index + 1);
                }

                return values.Aggregate((a, b) => a*b);
            }

            public static Equation Parse(string input)
            {
                var equations = new List<Equation>();
                var operators = new List<char>();

                var nestDepth = 0;
                int subEquationStart = -1;

                for (var i = 0; i < input.Length; i++)
                {
                    var character = input[i];

                    if (nestDepth > 0)
                    {
                        if (character == '(') nestDepth++;
                        if (character == ')') nestDepth--;
                        if (nestDepth == 0) equations.Add(Parse(input.Substring(subEquationStart, i - subEquationStart)));
                    }
                    else
                    {
                        if (character == '(')
                        {
                            subEquationStart = i + 1;
                            nestDepth = 1;
                        }
                        else if (int.TryParse(character.ToString(), out var number)) equations.Add(new Equation(number));
                        else if (!char.IsWhiteSpace(character)) operators.Add(character);
                    }
                }

                return new Equation(equations, operators);
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
