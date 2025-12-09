using System;
namespace AoC_2025
{
	public class Day08
	{
        public class Point
        {
            public long X, Y, Z;
 
            public Point(long x, long y, long z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }

        public struct Edge
        {
            public int id1, id2;

            public Edge(int id1, int id2)
            {
                this.id1 = id1;
                this.id2 = id2;
            }
        }

        private static long GetDistance(Point p1, Point p2)
        {
            // return euclidean distance w/out sqrt
            return (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y) + (p1.Z - p2.Z) * (p1.Z - p2.Z);
        }

        private static long Solve01(List<Point> points, int iterations)
        {
            List<long> distances = new List<long>();
            List<Edge> edges = new List<Edge>();

            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    long dist = GetDistance(points[i], points[j]);
                    distances.Add(dist);
                    edges.Add(new Edge(i, j));
                }
            }

            // sort distances with edges
            long[] sortedDistances = distances.ToArray();
            Edge[] sortedEdges = edges.ToArray();
            Array.Sort(sortedDistances, sortedEdges);

            // implement disjoint set on top of point array
            int[] refereneces = new int[points.Count];
            for (int i = 0; i < refereneces.Length; i++)
            {
                refereneces[i] = i;
            }

            for (int i = 0; i < iterations; i++)
            {
                Edge e = sortedEdges[i];
                int id1 = e.id1;
                int id2 = e.id2;

                // find sink of id1
                while (id1 != refereneces[id1])
                {
                    int previous = id1;
                    id1 = refereneces[id1];
                    refereneces[previous] = refereneces[id1];   // compaction
                }

                // find sink of id2
                while (id2 != refereneces[id2])
                {
                    int previous = id2;
                    id2 = refereneces[id2];
                    refereneces[previous] = refereneces[id2];   // compaction
                }

                // connect two points in disjoint set
                if (id1 < id2)
                {
                    refereneces[id2] = id1;
                }
                else if (id1 > id2)
                {
                    refereneces[id1] = id2;
                } else
                {
                    // already connected
                }
            }

            Dictionary<int, int> clusters = new Dictionary<int, int>();
            for (int i = 0; i < refereneces.Length; i++)
            {
                int id = i;
                // find sink of id
                while (id != refereneces[id])
                {
                    int previous = id;
                    id = refereneces[id];
                    refereneces[previous] = refereneces[id];   // compaction
                }

                if (!clusters.ContainsKey(id))
                {
                    clusters[id] = 0;
                }
                clusters[id]++;
            }

            // sort clusters by size ascending
            int[] cluster_ids = clusters.Keys.ToArray();
            int[] cluster_sizes = clusters.Values.ToArray();
            Array.Sort(cluster_sizes, cluster_ids);

            Array.Reverse(cluster_sizes);
            Array.Reverse(cluster_ids);

            long mult = 1;
            for (int i = 0; i < 3; i++)
            {
                mult *= cluster_sizes[i];
            }   

            return mult;
        }

        private static long Solve02(List<Point> points, int expectedConnections)
        {
            List<long> distances = new List<long>();
            List<Edge> edges = new List<Edge>();

            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    long dist = GetDistance(points[i], points[j]);
                    distances.Add(dist);
                    edges.Add(new Edge(i, j));
                }
            }

            // sort distances with edges
            long[] sortedDistances = distances.ToArray();
            Edge[] sortedEdges = edges.ToArray();
            Array.Sort(sortedDistances, sortedEdges);

            // implement disjoint set on top of point array
            int[] refereneces = new int[points.Count];
            for (int i = 0; i < refereneces.Length; i++)
            {
                refereneces[i] = i;
            }

            for (int i = 0; i < sortedEdges.Length; i++)
            {
                Edge e = sortedEdges[i];
                int id1 = e.id1;
                int id2 = e.id2;

                // find sink of id1
                while (id1 != refereneces[id1])
                {
                    int previous = id1;
                    id1 = refereneces[id1];
                    refereneces[previous] = refereneces[id1];   // compaction
                }

                // find sink of id2
                while (id2 != refereneces[id2])
                {
                    int previous = id2;
                    id2 = refereneces[id2];
                    refereneces[previous] = refereneces[id2];   // compaction
                }

                // connect two points in disjoint set
                if (id1 < id2)
                {
                    refereneces[id2] = id1;
                    expectedConnections--;
                }
                else if (id1 > id2)
                {
                    refereneces[id1] = id2;
                    expectedConnections--;
                } else
                {
                    // already connected
                }

                if (expectedConnections == 0)
                {
                    return points[e.id1].X * points[e.id2].X;
                }
            }

            return -1;
        }

        public static void Run()
        {
            // StreamReader sr = new StreamReader("Day08/test08.txt");
            StreamReader sr = new StreamReader("Day08/data08.txt");
            string s = null;

            List<Point> points = new List<Point>();

            while ((s = sr.ReadLine()) != null)
            {
                string[] parts = s.Split(',');
                points.Add(new Point(long.Parse(parts[0]), long.Parse(parts[1]), long.Parse(parts[2])));
            }

            sr.Close();

            long started = Environment.TickCount;

            // long solve01 = Solve01(points, points.Count);
            // Console.WriteLine(solve01);

            long solve02 = Solve02(points, points.Count - 1);
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*
29406

Elapsed: 96 ms

7499461416

Elapsed: 92 ms
*/