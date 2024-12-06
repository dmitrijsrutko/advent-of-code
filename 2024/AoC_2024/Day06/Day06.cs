using System;
namespace AoC_2024
{
	public class Day06
	{
        private static int[][] directions = new int[][]
        {
            new int[] {-1, 0},
            new int[] {0, 1},
            new int[] {1, 0},
            new int[] {0, -1}
        };

        private static int[][] directions_optimized = new int[][]
        {
            null,               // 0
            new int[] {-1, 0},  // 1
            new int[] {0, 1},   // 2
            null,               // 3
            new int[] {1, 0},   // 4
            null,               // 5
            null,               // 6
            null,               // 7
            new int[] {0, -1}   // 8
        };

        private static int[] next_direction = new int[]
        {
            0,  // 0
            2,  // 1
            4,  // 2
            0,  // 3
            8,  // 4
            0,  // 5
            0,  // 6
            0,  // 7
            1   // 8
        };

        private static long Solve01(List<char[]> map, int row, int col)
        {
            int d = 0;

            long visited = 0;

            map[row][col] = 'X';
            visited++;

            while (true)
            {
                int nrow = row + directions[d][0];
                int ncol = col + directions[d][1];

                if ((nrow < 0) || (nrow >= map.Count)) break;
                if ((ncol < 0) || (ncol >= map[row].Length)) break;

                if (map[nrow][ncol] == '.')
                {
                    map[nrow][ncol] = 'X';
                    visited++;
                    row = nrow;
                    col = ncol;
                }
                else if (map[nrow][ncol] == 'X')
                {
                    row = nrow;
                    col = ncol;
                }
                else if (map[nrow][ncol] == '#')
                {
                    d = (d + 1) % directions.Length;
                }
                else
                {
                    throw new ApplicationException();
                }
            }

            return visited;
        }

        private static bool IsLoop(List<char[]> map, int row, int col)
        {
            int d = 0;

            HashSet<int>[][] visited = new HashSet<int>[map.Count][];
            for (int i = 0; i < map.Count; i++)
            {
                visited[i] = new HashSet<int>[map[i].Length];
                for (int t = 0; t < map[i].Length; t++)
                {
                    visited[i][t] = new HashSet<int>();
                }
            }

            while (true)
            {
                if (visited[row][col].Contains(d))
                {
                    return true;
                }

                visited[row][col].Add(d);

                int nrow = row + directions[d][0];
                int ncol = col + directions[d][1];

                if ((nrow < 0) || (nrow >= map.Count)) break;
                if ((ncol < 0) || (ncol >= map[row].Length)) break;

                if (map[nrow][ncol] == '.')
                {
                    row = nrow;
                    col = ncol;
                }
                else if (map[nrow][ncol] == 'X')
                {
                    row = nrow;
                    col = ncol;
                }
                else if (map[nrow][ncol] == '^')
                {
                    row = nrow;
                    col = ncol;
                }
                else if (map[nrow][ncol] == '#')
                {
                    d = (d + 1) % directions.Length;
                }
                else
                {
                    throw new ApplicationException();
                }
            }

            return false;
        }

        private static bool IsLoopOptimized(List<char[]> map, int row, int col)
        {
            int d = 1;

            map[row][col] = '.';

            int[][] visited = new int[map.Count][];
            for (int i = 0; i < map.Count; i++)
            {
                visited[i] = new int[map[i].Length];
            }

            while (true)
            {
                if ((visited[row][col] & d) != 0)
                {
                    return true;
                }

                visited[row][col] = visited[row][col] | d;

                int nrow = row + directions_optimized[d][0];
                int ncol = col + directions_optimized[d][1];

                if ((nrow < 0) || (nrow >= map.Count)) break;
                if ((ncol < 0) || (ncol >= map[row].Length)) break;

                if (map[nrow][ncol] == '.')
                {
                    row = nrow;
                    col = ncol;
                }
                else if (map[nrow][ncol] == '#')
                {
                    d = next_direction[d];
                }
                else
                {
                    throw new ApplicationException();
                }
            }

            return false;
        }

        private static long Solve02(List<char[]> map, int row, int col)
        {
            long loops = 0;
            for (int r = 0; r < map.Count; r++)
            {
                for (int c = 0; c < map[r].Length; c++)
                {
                    if (map[r][c] == '.')
                    {
                        map[r][c] = '#';

                        if (IsLoop(map, row, col))
                        {
                            loops++;
                        }

                        map[r][c] = '.';
                    }
                }
            }

            return loops;
        }

        private static long Solve03(List<char[]> map, int row, int col)
        {
            long loops = 0;
            for (int r = 0; r < map.Count; r++)
            {
                for (int c = 0; c < map[r].Length; c++)
                {
                    if (map[r][c] == '.')
                    {
                        map[r][c] = '#';

                        if (IsLoopOptimized(map, row, col))
                        {
                            loops++;
                        }

                        map[r][c] = '.';
                    }
                }
            }

            return loops;
        }

        public static void Run()
        {
            string day = "06";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;

            List<char[]> map = new List<char[]>();
            List<char[]> map2 = new List<char[]>(); // copy

            int row = -1, col = -1;

            while ((s = sr.ReadLine()) != null)
            {
                map.Add(s.ToCharArray());
                map2.Add(s.ToCharArray());

                if (s.Contains('^'))
                {
                    row = map.Count - 1;
                    col = s.IndexOf('^');
                }
            }

            sr.Close();

            long started = Environment.TickCount;

            long safe = Solve01(map, row, col);
            Console.WriteLine(safe);

            //long safe2 = Solve02(map2, row, col);
            //Console.WriteLine(safe2);

            long safe3 = Solve03(map2, row, col);
            Console.WriteLine(safe3);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed);
        }
    }
}



/*

(Dictionary)
Elapsed: 6427

(Bitwise)
Elapsed: 368 ms

 * */
