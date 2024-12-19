using System;
namespace AoC_2024
{
	public class Day19
	{
        private static long GetPatternsPossible(List<string> towels, string pattern, bool earlyExit)
        {
            // Use dynamic programming
            long[] isPossible = new long[pattern.Length + 1];
            isPossible[pattern.Length] = 1;

            // check if it is possible to find a towel such that
            //   -> there is a full towel match starting at index and
            //     -> sum up # of possible matches after towel ends
            for (int index = pattern.Length - 1; index >= 0; index--)
            {
                foreach (string towel in towels)
                {
                    if (towel.Length > pattern.Length - index) continue;    // too long -> no match possible

                    bool match = true;
                    for (int t = 0; t < towel.Length; t++)
                    {
                        if (pattern[index + t] != towel[t])
                        {
                            match = false;
                            break;
                        }
                    }

                    if (!match) continue;   // no character match at towel level

                    isPossible[index] += isPossible[index + towel.Length];
                    if ((earlyExit) && (isPossible[index] > 0)) break;
                }
            }

            return isPossible[0];
        }

        private static long Solve01(List<string> towels, List<string> patterns)
        {
            long count = 0;
            for (int i = 0; i < patterns.Count; i++)
            {
                string pattern = patterns[i];
                count += GetPatternsPossible(towels, pattern, true) > 0 ? 1 : 0;
            }

            return count;
        }

        private static long Solve02(List<string> towels, List<string> patterns)
        {
            long count = 0;
            for (int i = 0; i < patterns.Count; i++)
            {
                string pattern = patterns[i];
                count += GetPatternsPossible(towels, pattern, false);
            }

            return count;
        }

        public static void Run()
        {
            string day = "19";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            List<string> towels = new List<string>();

            string s = sr.ReadLine();

            char[] delimeters = new char[] { ' ', ',', ':', '+', '=' };
            string[] splitted = s.Split(delimeters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            for (int i = 0; i < splitted.Length; i++)
            {
                towels.Add(splitted[i]);
            }

            sr.ReadLine();

            List<string> patterns = new List<string>();
            while ((s = sr.ReadLine()) != null)
            {
                patterns.Add(s);
            }

            sr.Close();

            long started = Environment.TickCount;

            long solve01 = Solve01(towels, patterns);
            Console.WriteLine(solve01);

            long solve02 = Solve02(towels, patterns);
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*
 * 

304
705756472327497

Elapsed: 53 ms

 */