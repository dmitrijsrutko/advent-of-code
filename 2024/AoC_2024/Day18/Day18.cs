using System;

namespace AoC_2024
{
	public class Day18
	{
        class Point
        {
            public int x, y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        private static long Solve01(List<Point> corruped, int bytes, int width, int height)
        {
            bool[][] visited = new bool[width][];
            for (int x = 0; x < width; x++)
            {
                visited[x] = new bool[height];
            }

            for (int i = 0; i < bytes; i++)
            {
                Point point = corruped[i];
                visited[point.x][point.y] = true;
            }

            Point start = new Point(0, 0);
            Point end = new Point(width - 1, height - 1);

            List<Point> source = new List<Point>();
            source.Add(start);

            long steps = 0;
            while (source.Count != 0)
            {
                List<Point> dest = new List<Point>();

                foreach (Point point in source)
                {
                    if ((point.x < 0) || (point.x >= width)) continue;
                    if ((point.y < 0) || (point.y >= height)) continue;
                    if (visited[point.x][point.y]) continue;

                    visited[point.x][point.y] = true;
                    if ((point.x == end.x) && (point.y == end.y))
                    {
                        return steps;
                    }

                    dest.Add(new Point(point.x, point.y - 1));
                    dest.Add(new Point(point.x + 1, point.y));
                    dest.Add(new Point(point.x, point.y + 1));
                    dest.Add(new Point(point.x - 1, point.y));
                }

                steps++;
                source = dest;
            }

            return -1;
        }

        private static string Solve02(List<Point> corruped, int bytes, int width, int height)
        {
            for (int i = bytes; i < corruped.Count; i++)
            {
                long steps = Solve01(corruped, i, width, height);
                if (steps == -1)
                {
                    return "" + corruped[i - 1].x + "," + corruped[i - 1].y;
                }
            }

            throw new NotImplementedException();
        }

        public static void Run()
        {
            string day = "18";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;

            List<Point> corruped = new List<Point>();

            while ((s = sr.ReadLine()) != null)
            {
                string[] splitted = s.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                Point point = new Point(Convert.ToInt32(splitted[0]), Convert.ToInt32(splitted[1]));
                corruped.Add(point);
            }

            sr.Close();

            long started = Environment.TickCount;

            /*
            int width = 7;
            int height = 7;
            int bytes = 12;
            long solve01 = Solve01(corruped, bytes, width, height);
            Console.WriteLine(solve01);
            */

            int width = 71;
            int height = 71;
            int bytes = 1024;

            long solve01 = Solve01(corruped, bytes, width, height);
            Console.WriteLine(solve01);

            string solve02 = Solve02(corruped, bytes, width, height);
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*
 * 
 
252
5,60

Elapsed: 277 ms

 * */