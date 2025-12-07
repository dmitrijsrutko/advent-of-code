using System;
namespace AoC_2025
{
	public class Day07
	{
        private static long Solve01(List<char[]> lines, int row, int col)
        {
            // Do recursive DFS scan
            if (row < 0 || row >= lines.Count || col < 0 || col >= lines[0].Length)
            {
                return 0;
            }

            if (lines[row][col] == '.')
            {
                lines[row][col] = '#'; // mark as visited
                return Solve01(lines, row + 1, col);
            } else if (lines[row][col] == '^')
            {
                // check on left and right
                long splits = 1;

                if (col - 1 >= 0 && lines[row][col - 1] == '.')
                {
                    splits += Solve01(lines, row, col - 1);
                }

                if (col + 1 < lines[0].Length && lines[row][col + 1] == '.')
                {
                    splits += Solve01(lines, row, col + 1);
                }

                return splits;
            } else if (lines[row][col] == '#')
            {
                return 0;
            } else
            {
                throw new Exception("Unknown char: " + lines[row][col]);
            }

            return 0;
        }

       private static long Solve02(List<char[]> lines, int row, int col, List<long[]> numbers)
        {
            // Do recursive DFS scan
            if (row < 0 || row >= lines.Count || col < 0 || col >= lines[0].Length)
            {
                return 1;
            }

            if (numbers[row][col] != 0)
            {
                return numbers[row][col];
            }

            if (lines[row][col] == '.')
            {
                long splits = Solve02(lines, row + 1, col, numbers);
                numbers[row][col] = splits;
                return splits;
            } else if (lines[row][col] == '^')
            {
                // check on left and right
                long splits = 0;

                if (col - 1 >= 0 && lines[row][col - 1] == '.')
                {
                    splits += Solve02(lines, row, col - 1, numbers);
                }

                if (col + 1 < lines[0].Length && lines[row][col + 1] == '.')
                {
                    splits += Solve02(lines, row, col + 1, numbers);
                }

                numbers[row][col] = splits;
                return splits;
            } else if (lines[row][col] == '#')
            {
                return 0;
            } else
            {
                throw new Exception("Unknown char: " + lines[row][col]);
            }

            return 0;
        }

        public static void Run()
        {
            // StreamReader sr = new StreamReader("Day07/test07.txt");
            StreamReader sr = new StreamReader("Day07/data07.txt");
            string s = null;

            List<char[]> lines = new List<char[]>();
            List<long[]> numbers = new List<long[]>();
            int row = -1;
            int col = -1;

            while ((s = sr.ReadLine()) != null)
            {
                lines.Add(s.ToCharArray());
                numbers.Add(new long[s.Length]);

                int indexOf = s.IndexOf('S');
                if (indexOf != -1)
                {
                    row = lines.Count - 1;
                    col = indexOf;
                }
            }

            sr.Close();


            long started = Environment.TickCount;

            // long solve01 = Solve01(lines, row + 1, col);
            // Console.WriteLine(solve01);

            long solve02 = Solve02(lines, row + 1, col, numbers);
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*

1660 - Elapsed: 1 ms

305999729392659 - Elapsed: 0 ms

*/