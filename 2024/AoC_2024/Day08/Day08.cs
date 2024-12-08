using System;
namespace AoC_2024
{
	public class Day08
	{
        struct Point
        {
            public int row;
            public int col;

            public Point(int row, int col)
            {
                this.row = row;
                this.col = col;
            }
        }

        private static void MarkNode(List<string> map, Point p, long[][] visited)
        {
            if ((p.row < 0) || (p.row >= map.Count)) return;
            if ((p.col < 0) || (p.col >= map[p.row].Length)) return;

            visited[p.row][p.col] = 1;
        }

        private static void CheckAntiNode(List<string> map, Point a, Point b, long[][] visited, int from, int to)
        {
            // check two anti-nodes
            Point diff = new Point(a.row - b.row, a.col - b.col);

            for (int i = from; i < to; i++)
            {
                Point a2 = new Point(a.row + diff.row * i, a.col + diff.col * i);
                Point b2 = new Point(b.row - diff.row * i, b.col - diff.col * i);

                MarkNode(map, a2, visited);
                MarkNode(map, b2, visited);
            }
        }

        private static void Solve01(List<string> map, List<Point> antennas, long[][] visited, int from, int to)
        {
            // check all pair-wise antennas
            for (int i = 0; i < antennas.Count; i++)
            {
                for (int t = i + 1; t < antennas.Count; t++)
                {
                    CheckAntiNode(map, antennas[i], antennas[t], visited, from, to);
                }
            }
        }

        private static long Solve01(List<string> map, Dictionary<char, List<Point>> antennas, int from, int to)
        {
            long[][] visited = new long[map.Count][];
            for (int i = 0; i < map.Count; i++)
            {
                visited[i] = new long[map[i].Length];
            }

            foreach (List<Point> aset in antennas.Values)
            {
                Solve01(map, aset, visited, from, to);
            }

            long count = 0;
            for (int i = 0; i < map.Count; i++)
            {
                for (int t = 0; t < map[i].Length; t++)
                {
                    count += visited[i][t];
                }
            }

            return count;
        }

        public static void Run()
        {
            string day = "08";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;

            List<string> map = new List<string>();
            while ((s = sr.ReadLine()) != null)
            {
                map.Add(s);
            }

            sr.Close();

            // Pre-process
            Dictionary<char, List<Point>> antennas = new Dictionary<char, List<Point>>();
            for (int i = 0; i < map.Count; i++)
            {
                for (int t = 0; t < map[i].Length; t++)
                {
                    char c = map[i][t];
                    if (c != '.')
                    {
                        if (!antennas.ContainsKey(c))
                        {
                            antennas.Add(c, new List<Point>());
                        }
                        antennas[c].Add(new Point(i, t));
                    }
                }
            }

            long started = Environment.TickCount;

            long solve01 = Solve01(map, antennas, 1, 2);
            Console.WriteLine(solve01);

            long solve02 = Solve01(map, antennas, 0, Math.Max(map.Count, map[0].Length));
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed);
        }
    }
}
