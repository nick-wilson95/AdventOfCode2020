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

            var lines = text.Split("\r\n");

            var seats = lines.Select(ParseSeat);

            var indices = Enumerable.Range(0, 1024).ToList();

            foreach (var seat in seats)
            {
                indices.Remove(seat.SeatIndex);
            }

            Console.WriteLine(string.Join('\n', indices));
        }

        private static Seat ParseSeat(string seatString)
        {
            var rowBinary = seatString.Substring(0, 7)
                .Replace('F', '0')
                .Replace('B', '1');

            var colBinary = seatString.Substring(7, 3)
                .Replace('L', '0')
                .Replace('R', '1');


            return new Seat(Convert.ToInt32(rowBinary, 2), Convert.ToInt32(colBinary, 2));
        }

        private class Seat
        {
            public Seat(int row, int column)
            {
                Row = row;
                Column = column;
                SeatIndex = row * 8 + column;
            }

            public int Row { get; }
            public int Column { get; }
            public int SeatIndex { get; }
        }
    }
}
