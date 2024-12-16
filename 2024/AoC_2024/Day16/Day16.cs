using System;
namespace AoC_2024
{
    public class Day16
    {
        private static int[][] directions = new int[][]
        {
            new int[] { -1, 0 },
            new int[] { 0, 1 },
            new int[] { 1, 0 },
            new int[] { 0, -1 }
        };

        public class Move
        {
            public int col, row, d;
            public long score;

            public Move(int col, int row, int d, long score)
            {
                this.col = col;
                this.row = row;
                this.d = d;
                this.score = score;
            }

            public override int GetHashCode()
            {
                return d;
            }

            public override bool Equals(object? obj)
            {
                Move other = (Move)obj;
                return this.d == other.d;
            }
        }

        private static long GetTiles(List<string> map, Move start, Move end, HashSet<Move>[][] visited, long score)
        {
            // Reverse path(s) back
            List<Move> source = new List<Move>();
            foreach (Move move in visited[end.row][end.col])
            {
                if (move.score == score)
                {
                    source.Add(move);
                }
            }

            long[][] tilesVisited = new long[map.Count][];
            for (int i = 0; i < map.Count; i++)
            {
                tilesVisited[i] = new long[map[i].Length];
            }

            // Iterate backwards in back direction
            while (source.Count != 0)
            {
                List<Move> dest = new List<Move>();

                foreach (Move move in source)
                {
                    if ((move.row < 0) || (move.row >= map.Count)) continue;
                    if ((move.col < 0) || (move.col >= map[move.row].Length)) continue;

                    if (map[move.row][move.col] == '#')
                    {
                        continue;
                    }

                    Move original = null;
                    bool found = visited[move.row][move.col].TryGetValue(move, out original);
                    if (found)
                    {
                        if (move.score == original.score)
                        {
                            // came from here -> continue iterate backwards
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;   // not possible to come from here
                    }

                    tilesVisited[move.row][move.col] = 1;

                    // Continue same direction -> but backwards
                    int[] d = directions[move.d];
                    dest.Add(new Move(move.col - d[1], move.row - d[0], move.d, move.score - 1));

                    // try 2 rotattions
                    int nd = move.d - 1;
                    if (nd < 0) nd += directions.Length;
                    dest.Add(new Move(move.col, move.row, nd, move.score - 1000));

                    nd = move.d + 1;
                    if (nd >= directions.Length) nd -= directions.Length;
                    dest.Add(new Move(move.col, move.row, nd, move.score - 1000));
                }

                source = dest;
            }

            long tiles = 0;
            for (int row = 0; row < map.Count; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    tiles += tilesVisited[row][col];
                }
            }

            return tiles;
        }

        private static long Solve01(List<string> map, Move start, Move end)
        {
            List<Move> source = new List<Move>();
            source.Add(start);

            HashSet<Move>[][] visited = new HashSet<Move>[map.Count][];
            for (int y = 0; y < map.Count; y++)
            {
                visited[y] = new HashSet<Move>[map[y].Length];
                for (int x = 0; x < map[y].Length; x++)
                {
                    visited[y][x] = new HashSet<Move>();
                }
            }

            while (source.Count != 0)
            {
                List<Move> dest = new List<Move>();

                foreach (Move move in source)
                {
                    if ((move.row < 0) || (move.row >= map.Count)) continue;
                    if ((move.col < 0) || (move.col >= map[move.row].Length)) continue;

                    if (map[move.row][move.col] == '#')
                    {
                        continue;
                    }

                    Move original = null;
                    bool found = visited[move.row][move.col].TryGetValue(move, out original);
                    if (found)
                    {
                        if (move.score < original.score)
                        {
                            original.score = move.score;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    visited[move.row][move.col].Add(move);

                    // Continue same direction
                    int[] d  = directions[move.d];
                    dest.Add(new Move(move.col + d[1], move.row + d[0], move.d, move.score + 1));

                    // try 2 rotattions
                    int nd = move.d + 1;
                    if (nd >= directions.Length) nd -= directions.Length;
                    dest.Add(new Move(move.col, move.row, nd, move.score + 1000));

                    nd = move.d - 1;
                    if (nd < 0) nd += directions.Length;
                    dest.Add(new Move(move.col, move.row, nd, move.score + 1000));
                }

                source = dest;
            }

            long score = long.MaxValue;
            foreach (Move move in visited[end.row][end.col])
            {
                score = Math.Min(score, move.score);
            }

            long tiles = GetTiles(map, start, end, visited, score);
            Console.WriteLine("tiles: " + tiles);

            return score;
        }

        public static void Run()
        {
            string day = "16";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + "_2.txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;
            List<string> map = new List<string>();
            Move start = null, end = null;

            while ((s = sr.ReadLine()) != null)
            {
                if (s.Contains('S'))
                {
                    start = new Move(s.IndexOf('S'), map.Count, 1, 0);
                }

                if (s.Contains('E'))
                {
                    end = new Move(s.IndexOf('E'), map.Count, 0, 0);
                }

                map.Add(s);
            }

            sr.Close();

            long started = Environment.TickCount;

            long solve01 = Solve01(map, start, end);
            Console.WriteLine(solve01);

            //long solve02 = Solve02(points, velocities, width, heigth);
            //Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*

tiles: 541
135512

Elapsed: 34 ms
* */