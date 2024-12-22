using System;

namespace AoC_2024
{
	public class Day22
	{
        private static long Calculate_overall_bananas(long number, int iterations, Dictionary<long, long> overall_bananas)
        {
            long previous_price = number % 10;
            long sequence = 0;  // use bitwise rotation of 5 bits
            long mask = (1 << 15) - 1;  // erase first 5 bits

            HashSet<long> exists = new HashSet<long>(); // exists in this iteration [for a given number]

            for (int i = 0; i < iterations; i++)
            {
                // Calculate the result of multiplying the secret number by 64.
                long n_number = number << 6;    // n_number = number * 64;

                // Then, mix this result into the secret number.
                number = number ^ n_number;

                // Finally, prune the secret number.                
                number = number & 0xFFFFFF; // number = number % 16777216;

                // Calculate the result of dividing the secret number by 32.                
                n_number = number >> 5; // n_number = number / 32;

                // Then, mix this result into the secret number.
                number = number ^ n_number;

                // Finally, prune the secret number.
                number = number & 0xFFFFFF; // number = number % 16777216;

                // Calculate the result of multiplying the secret number by 2048.                
                n_number = number << 11;    // n_number = number * 2048;

                // Then, mix this result into the secret number.
                number = number ^ n_number;

                // Finally, prune the secret number.
                number = number & 0xFFFFFF; // number = number % 16777216;

                if (overall_bananas != null)
                {
                    long price = number % 10;
                    long change = price - previous_price;
                    sequence = ((sequence & mask)  << 5) + (change + 10);

                    if (i >= 3)
                    {
                        if (!exists.Contains(sequence))
                        {
                            exists.Add(sequence);

                            // find bucket -> increse
                            if (overall_bananas.ContainsKey(sequence))
                            {
                                overall_bananas[sequence] += price;
                            }
                            else
                            {
                                overall_bananas.Add(sequence, price);
                            }
                        }
                    }

                    previous_price = price;
                }
            }

            return number;
        }

        private static long Solve01(List<long> numbers, int iterations)
        {
            long sum = 0;
            foreach (long number in numbers)
            {
                sum += Calculate_overall_bananas(number, iterations, null);
            }
            return sum;
        }

        private static long Solve02(List<long> numbers, int iterations)
        {
            Dictionary<long, long> overall_bananas = new Dictionary<long, long>();
            foreach (long number in numbers)
            {
                Calculate_overall_bananas(number, iterations, overall_bananas);
            }

            long max_bananas = long.MinValue;
            foreach (long bananas in overall_bananas.Values)
            {
                max_bananas = Math.Max(max_bananas, bananas);
            }

            return max_bananas;
        }

        public static void Run()
        {
            string day = "22";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + "_2.txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;

            List<long> numbers = new List<long>();

            while ((s = sr.ReadLine()) != null)
            {
                long number = Convert.ToInt64(s);
                numbers.Add(number);
            }

            sr.Close();

            long started = Environment.TickCount;

            int iterations = 2000;

            long solve01 = Solve01(numbers, iterations);
            Console.WriteLine(solve01);

            long solve02 = Solve02(numbers, iterations);
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*
 * 

20332089158
2191

Elapsed: 405 ms

 * */