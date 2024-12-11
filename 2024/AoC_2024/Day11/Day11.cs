using System;
namespace AoC_2024
{
	public class Day11
	{
        private static long Calculate(long stone, int level, Dictionary<long, long>[] stoneCounts)
        {
            if (level == 0)
            {
                return 1;
            }

            if (stoneCounts[level].ContainsKey(stone)) {
                return stoneCounts[level][stone];
            }

            // Apply rules
            if (stone == 0)
            {
                long newStone = 1;
                long count = Calculate(newStone, level - 1, stoneCounts);

                stoneCounts[level][stone] = count;
                return stoneCounts[level][stone];
            }

            string digits = Convert.ToString(stone);
            if (digits.Length % 2 == 0)
            {
                string left = digits.Substring(0, digits.Length / 2);
                string right = digits.Substring(digits.Length / 2, digits.Length / 2);

                long s1 = Convert.ToInt64(left);
                long s2 = Convert.ToInt64(right);

                long count1 = Calculate(s1, level - 1, stoneCounts);
                long count2 = Calculate(s2, level - 1, stoneCounts);

                stoneCounts[level][stone] = count1 + count2;
                return stoneCounts[level][stone];
            }

            {
                long newStone = stone * 2024;
                long count = Calculate(newStone, level - 1, stoneCounts);

                stoneCounts[level][stone] = count;
                return stoneCounts[level][stone];
            }
        }

        private static long Solve01(List<long> stones, int blinks)
        {
            // Do recursive scan with memory -> technically dynamic programming
            // Stones(level, stone) = Stones(level - 1, stone part #1) + Stones(level - 1, stone part #2)

            Dictionary<long, long>[] stoneCounts = new Dictionary<long, long>[blinks + 1];  // per level
            for (int i = 0; i < stoneCounts.Length; i++)
            {
                stoneCounts[i] = new Dictionary<long, long>();
            }

            long count = 0;
            for (int i = 0; i < stones.Count; i++)
            {
                long stone = stones[i];
                count += Calculate(stone, blinks, stoneCounts);
            }

            return count;
        }

        public static void Run()
        {
            string day = "11";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = sr.ReadLine();
            sr.Close();

            List<long> stones = new List<long>();
            string[] splitted = s.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            for (int i = 0; i < splitted.Length; i++)
            {
                stones.Add(Convert.ToInt64(splitted[i]));
            }

            long started = Environment.TickCount;

            long solve01 = Solve01(stones, 25);
            Console.WriteLine(solve01);

            long solve02 = Solve01(stones, 75);
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*

183620
220377651399268

Elapsed: 51 ms
 */
