using System;
namespace AoC_2024
{
	public class Day12
	{
        private static bool IsOuside(List<string> map, int row, int col, char grid)
        {
            if ((row < 0) || (row >= map.Count)) return true;
            if ((col < 0) || (col >= map[row].Length)) return true;
            if (map[row][col] != grid) return true;

            return false;
        }

        private static long IsOusideCorner(List<string> map, int r1, int c1, int r2, int c2, char grid)
        {
            if (IsOuside(map, r1, c1, grid) && IsOuside(map, r2, c2, grid))
            {
                return 1;
            }

            return 0;
        }

        private static long IsInsideCorner(List<string> map, int r, int c, int r1, int c1, int r2, int c2, char grid)
        {
            if (IsOuside(map, r, c, grid) && !IsOuside(map, r1, c1, grid) && !IsOuside(map, r2, c2, grid))
            {
                return 1;
            }

            return 0;
        }

        private static void Corners(List<string> map, int row, int col, char grid, ref long corners)
        {
            corners += IsOusideCorner(map, row, col - 1, row - 1, col, grid);
            corners += IsOusideCorner(map, row - 1, col, row, col + 1, grid);
            corners += IsOusideCorner(map, row, col + 1, row + 1, col, grid);
            corners += IsOusideCorner(map, row + 1, col, row, col - 1, grid);

            corners += IsInsideCorner(map, row - 1, col + 1, row - 1, col, row, col + 1, grid);
            corners += IsInsideCorner(map, row + 1, col + 1, row, col + 1, row + 1, col, grid);
            corners += IsInsideCorner(map, row + 1, col - 1, row + 1, col, row, col - 1, grid);
            corners += IsInsideCorner(map, row - 1, col - 1, row, col - 1, row - 1, col, grid);
        }

        private static void Scan(List<string> map, int row, int col, bool[][] visited, char grid, ref long area, ref long perimeter, ref long sides)
        {
            if (IsOuside(map, row, col, grid)) return;

            if (visited[row][col]) return;
            visited[row][col] = true;

            area++;
            Scan(map, row - 1, col, visited, grid, ref area, ref perimeter, ref sides);
            Scan(map, row, col + 1, visited, grid, ref area, ref perimeter, ref sides);
            Scan(map, row + 1, col, visited, grid, ref area, ref perimeter, ref sides);
            Scan(map, row, col - 1, visited, grid, ref area, ref perimeter, ref sides);

            perimeter += IsOuside(map, row - 1, col, grid) ? 1 : 0;
            perimeter += IsOuside(map, row, col + 1, grid) ? 1 : 0;
            perimeter += IsOuside(map, row + 1, col, grid) ? 1 : 0;
            perimeter += IsOuside(map, row, col - 1, grid) ? 1 : 0;

            Corners(map, row, col, grid, ref sides);
        }

        private static long Solve01(List<string> map, bool corners)
        {
            // Perform DFS -> calculate both area and perimeter

            bool[][] visited = new bool[map.Count][];
            for (int i = 0; i < map.Count; i++)
            {
                visited[i] = new bool[map[i].Length];
            }

            long price = 0;
            for (int row = 0; row < map.Count; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    if (!visited[row][col])
                    {
                        long area = 0;
                        long perimeter = 0;
                        long sides = 0;

                        Scan(map, row, col, visited, map[row][col], ref area, ref perimeter, ref sides);
                        price += area * (corners ? sides : perimeter);
                    }
                }
            }

            return price;
        }

        public static void Run()
        {
            string day = "12";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;

            List<string> map = new List<string>();

            while ((s = sr.ReadLine()) != null)
            {
                map.Add(s);
            }

            sr.Close();

            long started = Environment.TickCount;

            long solve01 = Solve01(map, false);
            Console.WriteLine(solve01);

            long solve02 = Solve01(map, true);
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*

1456082
872382

Elapsed: 20 ms
 * */
