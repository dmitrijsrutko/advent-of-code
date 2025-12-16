using System;

namespace AoC_2025
{
	public class Day02
	{
        private static bool IsValid(long value)
        {
            string s = value.ToString();

            for (int i = 1; i <= s.Length / 2; i++)
            {
                string prefix = s.Substring(0, i);

                if (s == (prefix + prefix))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsValid02(long value)
        {
            string s = value.ToString();

            for (int i = 1; i <= s.Length / 2; i++)
            {
                string prefix = s.Substring(0, i);

                string beginning = prefix;
                while (beginning.Length < s.Length)
                {
                    beginning += prefix;
                    if (beginning == s)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static long Solve01(List<long> range_from, List<long> range_to)
        {
            long invalid = 0;
            for (int i = 0; i < range_from.Count; i++)
            {
                for (long v = range_from[i]; v <= range_to[i]; v++)
                {
                    // check if valid
                    if (!IsValid(v))
                    {
                        invalid += v;
                    }
                }
            }

            return invalid;
        }

        private static long Solve02(List<long> range_from, List<long> range_to)
        {
            long invalid = 0;
            for (int i = 0; i < range_from.Count; i++)
            {
                for (long v = range_from[i]; v <= range_to[i]; v++)
                {
                    // check if valid
                    if (!IsValid02(v))
                    {
                        invalid += v;
                    }
                }
            }

            return invalid;
        }

        public static void Run()
        {
            // StreamReader sr = new StreamReader("Day02/test02.txt");
            StreamReader sr = new StreamReader("Day02/data02.txt");
            string s = null;

            List<long> range_from = new List<long>();
            List<long> range_to = new List<long>();

            while ((s = sr.ReadLine()) != null)
            {
                string[] parts = s.Split(',');
                foreach (string part in parts)
                {
                    string[] rangeParts = part.Split('-');
                    range_from.Add(long.Parse(rangeParts[0]));
                    range_to.Add(long.Parse(rangeParts[1]));
                }
            }

            sr.Close();

            long started = Environment.TickCount;

            // long solve01 = Solve01(range_from, range_to);
            // Console.WriteLine(solve01);

            long solve02 = Solve02(range_from, range_to);
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*
20223751480

Elapsed: 127 ms


30260171216

Elapsed: 284 ms
*/