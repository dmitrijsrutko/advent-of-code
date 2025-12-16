using System;

namespace AoC_2025
{
	public class Day03
	{
        private static long GetMax(string bank)
        {
            int lastMax = bank[bank.Length - 1] - '0';
            long max = -1;

            for (int i = bank.Length - 2; i >= 0; i--)
            {
                int current = bank[i] - '0';
                int twoDigit = current * 10 + lastMax;

                max = Math.Max(max, twoDigit);
                lastMax = Math.Max(lastMax, current);
            }

            return max;
        }

        private static long GetMax02(string bank)
        {
            // use exactly 12 digits
            const int digits = 12;

            long max = 0;
            for (int d = digits - 1; d >= 0; d--)
            {
                int current = bank[bank.Length - 1 - d] - '0';
                max = max * 10 + current;
            }

            for (int i = bank.Length - digits - 1; i >= 0; i--)
            {
                // remove any character
                string max_s = max.ToString();
                for (int d = 0; d < digits; d++)
                {
                    string removed_s = max_s.Substring(0, d) + max_s.Substring(d + 1);
                    string current_s = bank[i] + removed_s;
                    long current = long.Parse(current_s);
                    max = Math.Max(max, current);
                }
            }

            return max;
        }

        private static long Solve01(List<string> banks)
        {
            long sum = 0;
            foreach (string bank in banks)
            {
                sum += GetMax(bank);
            }

            return sum;
        }

        private static long Solve02(List<string> banks)
        {
            long sum = 0;
            foreach (string bank in banks)
            {
                sum += GetMax02(bank);
            }

            return sum;
        }

        public static void Run()
        {
            // StreamReader sr = new StreamReader("Day03/test03.txt");
            StreamReader sr = new StreamReader("Day03/data03.txt");
            string s = null;

            List<string> banks = new List<string>();

            while ((s = sr.ReadLine()) != null)
            {
                banks.Add(s);
            }

            sr.Close();

            long started = Environment.TickCount;

            // long solve01 = Solve01(banks);
            // Console.WriteLine(solve01);

            long solve02 = Solve02(banks);
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*
16812

Elapsed: 5 ms

166345822896410

Elapsed: 29 ms
*/