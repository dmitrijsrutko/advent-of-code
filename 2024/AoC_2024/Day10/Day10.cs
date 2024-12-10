using System;
namespace AoC_2024
{
	public class Day10
	{
        private static long Traverse(List<int[]> map, int row, int col, int level, HashSet<int> visited)
        {
            if ((row < 0) || (row >= map.Count)) return 0;
            if ((col < 0) || (col >= map[row].Length)) return 0;
            if (map[row][col] != level) return 0;
            if (map[row][col] == 9)
            {
                visited.Add(row * map[row].Length + col);
                return 1;
            }

            long rating = 0;
            rating += Traverse(map, row - 1, col, level + 1, visited);
            rating += Traverse(map, row, col + 1, level + 1, visited);
            rating += Traverse(map, row + 1, col, level + 1, visited);
            rating += Traverse(map, row, col - 1, level + 1, visited);
            return rating;
        }

        private static long TrailheadScore(List<int[]> map, int row, int col)
        {
            HashSet<int> visited = new HashSet<int>();
            Traverse(map, row, col, 0, visited);
            return visited.Count;
        }

        private static long TrailheadRating(List<int[]> map, int row, int col)
        {
            long rating = Traverse(map, row, col, 0, new HashSet<int>());
            return rating;
        }

        private static long Solve01(List<int[]> map)
        {
            long sum = 0;
            for (int row = 0; row < map.Count; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    sum += TrailheadScore(map, row, col);
                }
            }

            return sum;
        }

        private static long Solve02(List<int[]> map)
        {
            long sum = 0;
            for (int row = 0; row < map.Count; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    sum += TrailheadRating(map, row, col);
                }
            }

            return sum;
        }

        public static void Run()
        {
            string day = "10";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;

            List<int[]> map = new List<int[]>();

            while ((s = sr.ReadLine()) != null)
            {
                int[] row = new int[s.Length];
                for (int i = 0; i < s.Length; i++)
                {
                    row[i] = s[i] - '0';
                }
                map.Add(row);
            }

            sr.Close();

            long started = Environment.TickCount;

            long solve01 = Solve01(map);
            Console.WriteLine(solve01);

            long solve02 = Solve02(map);
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*
698
1436

Elapsed: 12 ms
 * */