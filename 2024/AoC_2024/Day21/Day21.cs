using System;
namespace AoC_2024
{
	public class Day21
	{
        private struct Point
        {
            public int row, col;

            public Point(int row, int col)
            {
                this.row = row;
                this.col = col;
            }
        }

        private static string[] keypad_numeric = new string[]
        {
            "789",
            "456",
            "123",
            "#0A"
        };

        private static string[] keypad_directional = new string[]
        {
            "#^A",
            "<v>"
        };

        private static Dictionary<char, Point> GetCoordinates(string[] keypad)
        {
            Dictionary<char, Point> coordinates = new Dictionary<char, Point>();

            for (int row = 0; row < keypad.Length; row++)
            {
                for (int col = 0; col < keypad[row].Length; col++)
                {
                    char c = keypad[row][col];
                    coordinates.Add(c, new Point(row, col));
                }
            }

            return coordinates;
        }

        private static void DFS(string[] keypad, Point source, Point dest, List<string> paths, string path, bool[,] visited, int distance)
        {
            if ((source.row < 0) || (source.row >= keypad.Length)) return;
            if ((source.col < 0) || (source.col >= keypad[0].Length)) return;
            if (keypad[source.row][source.col] == '#') return;
            if (visited[source.row, source.col]) return;
            if (distance < 0) return;

            if ((source.row == dest.row) && (source.col == dest.col))
            {
                if (distance != 0) throw new ApplicationException();
                paths.Add(path);
                return;
            }

            visited[source.row, source.col] = true;

            DFS(keypad, new Point(source.row - 1, source.col), dest, paths, path + "^", visited, distance - 1);
            DFS(keypad, new Point(source.row, source.col + 1), dest, paths, path + ">", visited, distance - 1);
            DFS(keypad, new Point(source.row + 1, source.col), dest, paths, path + "v", visited, distance - 1);
            DFS(keypad, new Point(source.row, source.col - 1), dest, paths, path + "<", visited, distance - 1);

            visited[source.row, source.col] = false;
        }

        private static Dictionary<string, List<string>> GetShortestPaths(string[] keypad)
        {
            Dictionary<char, Point> coordinates = GetCoordinates(keypad);

            // calculate shortest paths between all points -> including itself and wall '#'
            Dictionary<string, List<string>> shortest_paths = new Dictionary<string, List<string>>();

            foreach (KeyValuePair<char, Point> source in coordinates)
            {
                foreach (KeyValuePair<char, Point> dest in coordinates)
                {
                    string move = "" + source.Key + dest.Key;   // e.g. A7 :: from -> to [coordinates]
                    int manhattan_distance = Math.Abs(source.Value.row - dest.Value.row) + Math.Abs(source.Value.col - dest.Value.col);
                    List<string> paths = new List<string>();
                    bool[,] visited = new bool[keypad.Length, keypad[0].Length];

                    DFS(keypad, source.Value, dest.Value, paths, "", visited, manhattan_distance);

                    shortest_paths.Add(move, paths);
                }
            }

            return shortest_paths;
        }

        private static long GetLength_Directional(List<string> sequences, Dictionary<string, List<string>> shortest_paths_directional, int directional_keypads, Dictionary<string, long>[] distances)
        {
            string key = string.Join("", sequences);
            if (distances[directional_keypads].ContainsKey(key))
            {
                return distances[directional_keypads][key];
            }

            long min_length = long.MaxValue;
            if (directional_keypads == 0)
            {
                foreach (string sequence in sequences)
                {
                    min_length = Math.Min(min_length, sequence.Length);
                }

                distances[directional_keypads].Add(key, min_length);
                return min_length;
            }

            foreach (string sequence in sequences)
            {
                char sourceButton = 'A';

                long total_length = 0;
                for (int i = 0; i < sequence.Length; i++)
                {
                    char destButton = sequence[i];
                    string pathButton = "" + sourceButton + destButton;

                    List<string> n_sequences = new List<string>();
                    foreach (string destPath in shortest_paths_directional[pathButton])
                    {
                        string total_path = destPath + 'A';
                        n_sequences.Add(total_path);
                    }

                    long length = GetLength_Directional(n_sequences, shortest_paths_directional, directional_keypads - 1, distances);
                    total_length += length;

                    sourceButton = destButton;
                }

                min_length = Math.Min(min_length, total_length);
            }

            distances[directional_keypads].Add(key, min_length);
            return min_length;
        }

        private static long GetLength_Numeric(string code, Dictionary<string, List<string>> shortest_paths_numeric, Dictionary<string, List<string>> shortest_paths_directional, int directional_keypads)
        {
            Dictionary<string, long>[] distances = new Dictionary<string, long>[directional_keypads + 1];
            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = new Dictionary<string, long>();
            }

            char sourceButton = 'A';

            long total_length = 0;
            for (int i = 0; i < code.Length; i++)
            {
                char destButton = code[i];
                string pathButton = "" + sourceButton + destButton;

                List<string> sequences = new List<string>();

                foreach (string destPath in shortest_paths_numeric[pathButton])
                {
                    string total_path = destPath + 'A';
                    sequences.Add(total_path);
                }

                long length = GetLength_Directional(sequences, shortest_paths_directional, directional_keypads, distances);
                total_length += length;

                sourceButton = destButton;
            }

            long digital_code = Convert.ToInt64(code.Substring(0, code.Length - 1));
            long complexity = digital_code * total_length;

            //Console.WriteLine("code: " + code);
            //Console.WriteLine("complexity: " + complexity);
            //Console.WriteLine();

            return complexity;
        }

        private static long Solve(string code, int directional_keypads)
        {
            // run recursion + memoization

            Dictionary<string, List<string>> shortest_paths_numeric = GetShortestPaths(keypad_numeric);
            Dictionary<string, List<string>> shortest_paths_directional = GetShortestPaths(keypad_directional);

            long result = GetLength_Numeric(code, shortest_paths_numeric, shortest_paths_directional, directional_keypads);
            return result;
        }

        private static long Solve01(List<string> codes, int directional_keypads)
        {
            long sum = 0;
            for (int i = 0; i < codes.Count; i++)
            {
                string code = codes[i];
                sum += Solve(code, directional_keypads);
            }

            return sum;
        }

        public static void Run()
        {
            string day = "21";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;

            List<string> codes = new List<string>();
            while ((s = sr.ReadLine()) != null)
            {
                codes.Add(s);
            }

            sr.Close();

            long started = Environment.TickCount;

            long solve01_rm = Solve01(codes, 2);
            Console.WriteLine(solve01_rm);

            long solve02_rm = Solve01(codes, 25);
            Console.WriteLine(solve02_rm);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*
 * 

246990
306335137543664

Elapsed: 18 ms

 * */
