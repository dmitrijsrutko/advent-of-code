using System;

namespace AoC_2024
{
	public class Day20
	{
        public struct Move
        {
            public int row, col;

            public Move(int row, int col)
            {
                this.row = row;
                this.col = col;
            }
        }

        private static int[,] BFS(List<string> map, Move start)
        {
            int[,] visited = new int[map.Count, map[0].Length];
            for (int row = 0; row < map.Count; row++)
            {
                for (int col = 0; col < map[0].Length; col++)
                {
                    visited[row, col] = -1;
                }
            }

            List<Move> source = new List<Move>();
            source.Add(start);

            int distance = 0;

            while (source.Count != 0)
            {
                List<Move> dest = new List<Move>();
                foreach (Move move in source)
                {
                    if ((move.row < 0) || (move.row >= map.Count)) continue;
                    if ((move.col < 0) || (move.col >= map[0].Length)) continue;
                    if (map[move.row][move.col] == '#') continue;
                    if (visited[move.row, move.col] != -1) continue;

                    visited[move.row, move.col] = distance;

                    dest.Add(new Move(move.row - 1, move.col));
                    dest.Add(new Move(move.row, move.col + 1));
                    dest.Add(new Move(move.row + 1, move.col));
                    dest.Add(new Move(move.row, move.col - 1));
                }

                distance++;
                source = dest;
            }

            return visited;
        }

        private static long GetCheatCount(List<string> map, Move move, int[,] dest_distance, int CHEAT_LENGTH, int source_distance, int min_distance, int cheat_difference)
        {
            long cheats = 0;
            for (int row = move.row - CHEAT_LENGTH; row <= move.row + CHEAT_LENGTH; row++)
            {
                if ((row < 0) || (row >= map.Count)) continue;

                for (int col = move.col - CHEAT_LENGTH; col <= move.col + CHEAT_LENGTH; col++)
                {
                    if ((col < 0) || (col >= map[0].Length)) continue;
                    if (map[row][col] == '#') continue;

                    int manhattan_distance = Math.Abs(row - move.row) + Math.Abs(col - move.col);
                    if (manhattan_distance <= CHEAT_LENGTH)
                    {
                        int total_distance = source_distance + manhattan_distance + dest_distance[row, col];
                        if (total_distance <= min_distance - cheat_difference)
                        {
                            cheats++;
                        }
                    }
                }
            }

            return cheats;
        }

        private static long Solve01(List<string> map, Move start, Move end, int CHEAT_LENGTH)
        {
            // 1) Do BFS from S(start) -> get list of reachable points and their distances -> and min non-wall distance
            // 2) Do BFS from E(end) -> get list of shortest distances from all reachable points to E
            // 3) scan all reachable points from received at step (1)
            //   -- simulate all possible cheats of length CHEAT_LENGTH (manhattance distance) -> grid scan (row, col)
            //   -- calculate cheat distance by adding source distance (1) + dest distance (2) + manhattan distance of cheat jump 

            int[,] source_distance = BFS(map, start);
            int[,] dest_distance = BFS(map, end);
            int min_distance = source_distance[end.row, end.col];

            long cheats = 0;
            for (int row = 0; row < map.Count; row++)
            {
                for (int col = 0; col < map[0].Length; col++)
                {
                    if (source_distance[row, col] != -1)
                    {
                        cheats += GetCheatCount(map, new Move(row, col), dest_distance, CHEAT_LENGTH, source_distance[row, col], min_distance, 100);
                    }
                }
            }

            return cheats;
        }

        public static void Run()
        {
            string day = "20";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;
            List<string> map = new List<string>();
            Move start = new Move(0, 0), end = new Move(0, 0);

            while ((s = sr.ReadLine()) != null)
            {
                if (s.Contains('S'))
                {
                    start = new Move(map.Count, s.IndexOf('S'));
                }

                if (s.Contains('E'))
                {
                    end = new Move(map.Count, s.IndexOf('E'));
                }

                map.Add(s);
            }

            sr.Close();

            long started = Environment.TickCount;

            long solve01 = Solve01(map, start, end, 2);
            Console.WriteLine(solve01);

            long solve02 = Solve01(map, start, end, 20);
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*
 * 

1395
993178

Elapsed: 74 ms

 * */