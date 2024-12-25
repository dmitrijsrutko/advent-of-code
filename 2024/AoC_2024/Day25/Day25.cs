using System;

namespace AoC_2024
{
	public class Day25
	{
        private static int[] GetLengths(List<string> pattern)
        {
            int[] lengths = new int[pattern[0].Length];
            for (int col = 0; col < pattern[0].Length; col++)
            {
                for (int row = pattern.Count - 1; row >= 0; row--)
                {
                    if (pattern[row][col] == '#')
                    {
                        lengths[col] = row;
                        break;
                    }
                }
            }

            return lengths;
        }

        private static long IsFit(int[] a, int[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] + b[i] > 5)
                {
                    return 0;
                }
            }

            return 1;
        }

        private static long Solve01(List<List<string>> locks, List<List<string>> keys)
        {
            // convert to lengths
            List<int[]> door_lock_lengths = new List<int[]>();
            List<int[]> key_lengths = new List<int[]>();

            for (int i = 0; i < locks.Count; i++)
            {
                door_lock_lengths.Add(GetLengths(locks[i]));
            }

            for (int i = 0; i < keys.Count; i++)
            {
                keys[i].Reverse();
                key_lengths.Add(GetLengths(keys[i]));
            }

            long fits = 0;
            foreach (int[] door_lock_length in door_lock_lengths)
            {
                foreach (int[] key_length in key_lengths)
                {
                    fits += IsFit(door_lock_length, key_length);
                }
            }

            return fits;
        }

        public static void Run()
        {
            string day = "25";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;

            List<List<string>> locks = new List<List<string>>();
            List<List<string>> keys = new List<List<string>>();

            int size = 7;
            while (true)
            {
                List<string> pattern = new List<string>();
                for (int i = 0; i < size; i++)
                {
                    pattern.Add(sr.ReadLine());
                }

                if (pattern[0][0] == '#')
                {

                    locks.Add(pattern);
                }
                else
                {
                    keys.Add(pattern);
                }

                if (sr.ReadLine() == null) break;
            }

            sr.Close();

            long started = Environment.TickCount;

            long solve01 = Solve01(locks, keys);
            Console.WriteLine(solve01);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*
 *

2586

Elapsed: 14 ms 

 * 
 * */
