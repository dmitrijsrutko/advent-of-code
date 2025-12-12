using System;
namespace AoC_2025
{
	public class Day09
	{
        public struct Point
        {
            public long X, Y;

            public Point(long x, long y)
            {
                X = x;
                Y = y;
            }
        }

        private static long Solve01(List<Point> points)
        {
            // do naive scan
            long maxSquare = -1;
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    long dx = Math.Abs(points[i].X - points[j].X) + 1;
                    long dy = Math.Abs(points[i].Y - points[j].Y) + 1;
                    long square = dx * dy;
                    if (square > maxSquare)
                    {
                        maxSquare = square;
                    }
                }
            }   

            return maxSquare;
        }

        private static long Solve02(List<Point> points)
        {
            Point upperRight = new Point(94581, 50187);
            Point upperRightLimit = new Point(94132, 69159);
            
            long maxSquare = -1;

            // find all points higher or equal to upperRight
            for (int i = 0; i < points.Count; i++)
            {
                if ((points[i].Y >= upperRight.Y) && (points[i].Y <= upperRightLimit.Y))
                {
                    long dx = Math.Abs(points[i].X - upperRight.X) + 1;
                    long dy = Math.Abs(points[i].Y - upperRight.Y) + 1;
                    long square = dx * dy;
                    if (square > maxSquare)
                    {
                        maxSquare = square;
                    }
                }
            }

            Point lowerRight = new Point(94581, 48595);
            Point lowerRightLiit = new Point(94394, 32316);

            // find all points lower or equal to lowerRight
            for (int i = 0; i < points.Count; i++)
            {
                if ((points[i].Y <= lowerRight.Y) && (points[i].Y >= lowerRightLiit.Y))
                {
                    long dx = Math.Abs(points[i].X - lowerRight.X) + 1;
                    long dy = Math.Abs(points[i].Y - lowerRight.Y) + 1;
                    long square = dx * dy;
                    if (square > maxSquare)
                    {
                        maxSquare = square;
                    }                
                }
            }
            
            return maxSquare;
        }

        public static void Run()
        {
            // StreamReader sr = new StreamReader("Day09/test09.txt");
            StreamReader sr = new StreamReader("Day09/data09.txt");
            string s = null;

            List<Point> points = new List<Point>();

            while ((s = sr.ReadLine()) != null)
            {
                string[] parts = s.Split(',');
                points.Add(new Point(long.Parse(parts[0]), long.Parse(parts[1])));
            }

            sr.Close();

            long started = Environment.TickCount;

            // long solve01 = Solve01(points);
            // Console.WriteLine(solve01);

            long solve02 = Solve02(points);
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*
4725826296

Elapsed: 5 ms

1637556834

Elapsed: 5 ms
*/