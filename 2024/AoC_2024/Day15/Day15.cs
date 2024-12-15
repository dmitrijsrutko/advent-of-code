using System;
namespace AoC_2024
{
	public class Day15
	{
        private static bool GetNextPosition(List<char[]> map, int row, int col, int[] d, ref int nrow, ref int ncol)
        {
            while (true)
            {
                row += d[0];
                col += d[1];

                if (map[row][col] == '#')
                {
                    return false;
                }
                else if (map[row][col] == '.')
                {
                    nrow = row;
                    ncol = col;
                    return true;
                }
                else if (map[row][col] == 'O')
                {
                    // skip
                }
                else
                {
                    throw new ApplicationException();
                }
            }
        }

        private static bool CanMoveHorizontally(List<char[]> map, int row, int col, int[] d)
        {
            while (true)
            {
                row += d[0];
                col += d[1];

                if (map[row][col] == '#')
                {
                    return false;
                }
                else if (map[row][col] == '.')
                {
                    return true;
                }
                else if ((map[row][col] == '[') || (map[row][col] == ']'))
                {
                    // skip
                }
                else
                {
                    throw new ApplicationException();
                }
            }
        }

        private static void MoveHorizontally(List<char[]> map, int row, int col, int[] d)
        {
            char previous = map[row][col];
            map[row][col] = '.';

            while (true)
            {
                row += d[0];
                col += d[1];

                if (map[row][col] == '#')
                {
                    throw new ApplicationException();
                }
                else if (map[row][col] == '.')
                {
                    map[row][col] = previous;
                    return;
                }
                else if ((map[row][col] == '[') || (map[row][col] == ']'))
                {
                    char tmp = map[row][col];
                    map[row][col] = previous;
                    previous = tmp;
                }
                else
                {
                    throw new ApplicationException();
                }
            }
        }

        private static bool CanMoveVertically(List<char[]> map, int row, int col, int[] d)
        {
            char c = map[row][col];

            if (c == '#')
            {
                return false;
            }
            else if (c == '.')
            {
                return true;
            }
            else if (c == '@')
            {
                return CanMoveVertically(map, row + d[0], col, d);
            }
            else if (c == '[')
            {
                return CanMoveVertically(map, row + d[0], col, d) && CanMoveVertically(map, row + d[0], col + 1, d);
            }
            else if (c == ']')
            {
                return CanMoveVertically(map, row + d[0], col - 1, d) && CanMoveVertically(map, row + d[0], col, d);
            }
            else
            {
                throw new ApplicationException();
            }
        }

        private static void MoveVertically(List<char[]> map, int row, int col, int[] d)
        {
            char c = map[row][col];

            if (c == '#')
            {
                throw new ApplicationException();
            }
            else if (c == '.')
            {
                return;
            }
            else if (c == '@')
            {
                MoveVertically(map, row + d[0], col, d);

                map[row + d[0]][col] = '@';
                map[row][col] = '.';

                return;
            }
            else if (c == '[')
            {
                MoveVertically(map, row + d[0], col, d);
                MoveVertically(map, row + d[0], col + 1, d);

                map[row + d[0]][col] = '[';
                map[row + d[0]][col + 1] = ']';

                map[row][col] = '.';
                map[row][col + 1] = '.';

                return;
            }
            else if (c == ']')
            {
                MoveVertically(map, row + d[0], col - 1, d);
                MoveVertically(map, row + d[0], col, d);

                map[row + d[0]][col - 1] = '[';
                map[row + d[0]][col] = ']';

                map[row][col - 1] = '.';
                map[row][col] = '.';

                return;
            }
            else
            {
                throw new ApplicationException();
            }
        }

        private static void Print(List<char[]> map)
        {
            for (int row = 0; row < map.Count; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    Console.Write(map[row][col]);
                }
                Console.WriteLine();
            }
        }

        private static Dictionary<char, int[]> GetDirections()
        {
            int[] up = new int[] { -1, 0 };
            int[] down = new int[] { 1, 0 };

            int[] left = new int[] { 0, -1 };
            int[] right = new int[] { 0, 1 };

            Dictionary<char, int[]> directions = new Dictionary<char, int[]>();

            // <^^>>>vv<v>>v<<
            directions.Add('^', up);
            directions.Add('v', down);

            directions.Add('<', left);
            directions.Add('>', right);

            return directions;
        }

        private static long GetSum(List<char[]> map, long width, char box)
        {
            long sum = 0;
            for (int i = 0; i < map.Count; i++)
            {
                for (int t = 0; t < map[i].Length; t++)
                {
                    if (map[i][t] == box)
                    {
                        sum += i * width + t;
                    }
                }
            }

            return sum;
        }

        private static long Solve01(List<char[]> map, string moves, int row, int col, long width)
        {
            Dictionary<char, int[]> directions = GetDirections();

            foreach (char move in moves)
            {
                int[] d = directions[move];
                int nrow = -1, ncol = -1;

                bool result = GetNextPosition(map, row, col, d, ref nrow, ref ncol);

                if (result)
                {
                    map[nrow][ncol] = 'O';  // move box if any
                    map[row][col] = '.';

                    row += d[0];
                    col += d[1];
                    map[row][col] = '@';
                }
            }

            return GetSum(map, width, 'O');
        }

        private static long Solve02(List<char[]> map, string moves, int row, int col, long width)
        {
            Dictionary<char, int[]> directions = GetDirections();

            foreach (char move in moves)
            {
                int[] d = directions[move];

                if ((move == '<') || (move == '>'))
                {
                    bool result = CanMoveHorizontally(map, row, col, d);
                    if (result)
                    {
                        MoveHorizontally(map, row, col, d);
                        row += d[0];
                        col += d[1];
                    }
                }
                else if ((move == '^') || (move == 'v'))
                {
                    bool result = CanMoveVertically(map, row, col, d);
                    if (result)
                    {
                        MoveVertically(map, row, col, d);
                        row += d[0];
                        col += d[1];
                    }
                }
                else
                {
                    throw new ApplicationException();
                }
            }

            return GetSum(map, width, '[');
        }


        public static void Run()
        {
            string day = "15";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + "_small.txt");
            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;

            List<char[]> map = new List<char[]>();
            int row = -1, col = -1;

            List<char[]> map2 = new List<char[]>();
            int row2 = -1, col2 = -1;

            while ((s = sr.ReadLine()) != null)
            {
                if (s == "") break;

                if (s.Contains('@'))
                {
                    row = map.Count();
                    col = s.IndexOf('@');
                }

                map.Add(s.ToCharArray());

                // scale up
                string s2 = "";
                foreach (char c in s)
                {
                    if (c == '#')
                    {
                        s2 += "##";
                    }
                    else if (c == 'O')
                    {
                        s2 += "[]";
                    }
                    else if (c == '.')
                    {
                        s2 += "..";
                    }
                    else if (c == '@')
                    {
                        s2 += "@.";
                    }
                    else
                    {
                        throw new ApplicationException();
                    }
                }

                if (s2.Contains('@'))
                {
                    row2 = map2.Count();
                    col2 = s2.IndexOf('@');
                }

                map2.Add(s2.ToCharArray());
            }

            string moves = "";
            while ((s = sr.ReadLine()) != null)
            {
                moves += s;
            }

            sr.Close();

            long started = Environment.TickCount;

            long width = 100;

            long solve01 = Solve01(map, moves, row, col, width);
            Console.WriteLine(solve01);

            long solve02 = Solve02(map2, moves, row2, col2, width);
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*
1505963
1543141

Elapsed: 15 ms
 * */
