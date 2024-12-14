using System;

namespace AoC_2024
{
	public class Day14
	{
        public class Point
        {
            public long x, y;

            public Point(long x, long y)
            {
                this.x = x;
                this.y = y;
            }

            public override int GetHashCode()
            {
                return (int) (this.x * 997 + this.y);
            }

            public override bool Equals(object? obj)
            {
                Point other = (Point)obj;
                return (this.x == other.x) && (this.y == other.y);
            }
        }

        private static bool is_christmas_tree(List<Point> points)
        {
            // calculate quadratic deviation
            long sum_x = 0, sum_y = 0;
            foreach (Point point in points)
            {
                sum_x += point.x;
                sum_y += point.y;
            }

            long avg_x = sum_x / points.Count;
            long avg_y = sum_y / points.Count;

            long quadratic = 0;
            foreach (Point point in points)
            {
                quadratic += (point.x - avg_x) * (point.x - avg_x) + (point.y - avg_y) * (point.y - avg_y);
            }

            return quadratic < 350000;
        }

        private static List<Point> update_positions(List<Point> points, List<Point> velocities, long width, long heigth, long seconds)
        {
            List<Point> newPoints = new List<Point>();

            for (int i = 0; i < points.Count; i++)
            {
                Point p = points[i];
                Point v = velocities[i];

                long nx = (p.x + v.x * seconds) % width;
                if (nx < 0) nx += width;

                long ny = (p.y + v.y * seconds) % heigth;
                if (ny < 0) ny += heigth;

                newPoints.Add(new Point(nx, ny));
            }

            return newPoints;
        }

        private static long Solve01(List<Point> points, List<Point> velocities, long width, long heigth)
        {
            long seconds = 100;

            long[][] map = new long[width][];
            for (int i = 0; i < width; i++)
            {
                map[i] = new long[heigth];
            }

            List<Point> newPoints = update_positions(points, velocities, width, heigth, seconds);
            foreach (Point point in newPoints)
            {
                map[point.x][point.y]++;
            }

            long[][] quadrants = new long[2][];
            quadrants[0] = new long[2];
            quadrants[1] = new long[2];

            for (int i = 0; i < width; i++)
            {
                if (i == width / 2) continue;
                for (int t = 0; t < heigth; t++)
                {
                    if (t == heigth / 2) continue;
                    quadrants[i / (width / 2 + 1)][t / (heigth / 2 + 1)] += map[i][t];
                }
            }

            long safety = quadrants[0][0] * quadrants[0][1] * quadrants[1][0] * quadrants[1][1];
            return safety;
        }

        private static long Solve02(List<Point> points, List<Point> velocities, long width, long heigth)
        {
            long seconds = 0;
            while (true)
            {
                List<Point> newPoints = update_positions(points, velocities, width, heigth, seconds);
                if (is_christmas_tree(newPoints))
                {
                    Print(newPoints, width, heigth);
                    return seconds;
                }

                seconds++;
            }

            return -1;
        }

        private static void Print(List<Point> points, long width, long heigth)
        {
            HashSet<Point> grid = new HashSet<Point>(points);
            for (int r = 0; r < heigth; r++)
            {
                for (int c = 0; c < width; c++)
                {
                    Console.Write(grid.Contains(new Point(c, r)) ? "#" : ".");
                }
                Console.WriteLine();
            }
        }

        public static void Run()
        {
            string day = "14";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;

            List<Point> points = new List<Point>();
            List<Point> velocities = new List<Point>();

            while ((s = sr.ReadLine()) != null)
            {
                char[] delimeters = new char[] { ' ', ',', ':', '+', '=' };
                string[] splitted = s.Split(delimeters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                long px = Convert.ToInt64(splitted[1]);
                long py = Convert.ToInt64(splitted[2]);

                long vx = Convert.ToInt64(splitted[4]);
                long vy = Convert.ToInt64(splitted[5]);

                points.Add(new Point(px, py));
                velocities.Add(new Point(vx, vy));
            }

            sr.Close();

            long started = Environment.TickCount;

            //long width = 11;
            //long heigth = 7;

            long width = 101;
            long heigth = 103;

            long solve01 = Solve01(points, velocities, width, heigth);
            Console.WriteLine(solve01);

            long solve02 = Solve02(points, velocities, width, heigth);
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*
 * 
230686500
7672

Elapsed: 233 ms

 * */
