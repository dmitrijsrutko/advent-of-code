using System;
using System.Reflection.Emit;

namespace AoC_2024
{
	public class Day04
	{
        private static int[][] directions = new int[][]
        {
            new int[] {1, -1},
            new int[] {1, 0},
            new int[] {1, 1},

            new int[] {0, 1},

            new int[] {-1, 1},
            new int[] {-1, 0},
            new int[] {-1, -1},

            new int[] {0, -1}
        };

        private static long Search(List<string> map, int row, int col, int level, string word, int[] direction)
        {
            if ((row < 0) || (row >= map.Count)) return 0;
            if ((col < 0) || (col >= map[row].Length)) return 0;
            if (map[row][col] != word[level]) return 0;
            if (level == word.Length - 1) return 1;

            long count = Search(map, row + direction[0], col + direction[1], level + 1, word, direction);

            return count;
        }

        private static long Solve01(List<string> map)
        {
            string word = "XMAS";

            long count = 0;
            for (int row = 0; row < map.Count; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    for (int d = 0; d < directions.Length; d++)
                    {
                        count += Search(map, row, col, 0, word, directions[d]);

                    }

                    //Console.WriteLine(row + " " +  col + " :: " + count);
                }
            }

            return count;
        }

        private static long IsXMAS(List<string> map, int row, int col)
        {
            if ((row < 1) || (row >= map.Count - 1)) return 0;
            if ((col < 1) || (col >= map[row].Length - 1)) return 0;


            string w1 = "" + map[row + 1][col - 1] + map[row][col] + map[row - 1][col + 1];
            string w2 = "" + map[row - 1][col + 1] + map[row][col] + map[row + 1][col - 1];

            string w3 = "" + map[row - 1][col - 1] + map[row][col] + map[row + 1][col + 1];
            string w4 = "" + map[row + 1][col + 1] + map[row][col] + map[row - 1][col - 1];

            string word = "MAS";

            if (((w1 == word) || (w2 == word)) &&
                    ((w3 == word) || (w4 == word)))
            {
                return 1;
            }


            return 0;
        }

        private static long Solve02(List<string> map)
        {
            long count = 0;
            for (int row = 0; row < map.Count; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    count += IsXMAS(map, row, col);

                    //Console.WriteLine(row + " " +  col + " :: " + count);
                }
            }

            return count;
        }

        public static void Run()
        {
            string day = "04";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;

            List<string> map = new List<string>();

            while ((s = sr.ReadLine()) != null)
            {
                map.Add(s);
            }

            sr.Close();


            long safe = Solve01(map);
            Console.WriteLine(safe);


            long safe2 = Solve02(map);
            Console.WriteLine(safe2);
        }
    }
}

