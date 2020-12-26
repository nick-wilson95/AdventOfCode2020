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
            var pc1 = 18499292;
            var pc2 = 8790390;

            long value = 1;
            long loopSize = 0;
            long subjectValue = 7;

            while (true)
            {
                if (value == pc1)
                {
                    Console.WriteLine($"1: {loopSize}");
                    break;
                }
                if (value == pc2)
                {
                    Console.WriteLine($"2: {loopSize}");
                    break;
                }

                loopSize++;
                value = (value * subjectValue) % 20201227;
            }

            value = 1;
            for (var i = 0; i < loopSize; i++)
            {
                value = (value * pc2) % 20201227;
            }

            Console.WriteLine(value);
        }

        private static void WriteAnswer(int part, string answer)
        {
            Console.WriteLine($"Answer for part {part}:");
            Console.WriteLine(answer);
            Console.WriteLine("");
        }
    }
}
