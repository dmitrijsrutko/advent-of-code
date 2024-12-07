using System;
namespace AoC_2024
{
	public class Day07
	{
        private static long IsSolvable(long result, long[] ops)
        {
            HashSet<long> source = new HashSet<long>();
            source.Add(ops[0]);

            for (int i = 1; i < ops.Length; i++)
            {
                HashSet<long> dest = new HashSet<long>();
                long op = ops[i];

                // apply to all existing -> two operations -> put to dest
                foreach(long s in source)
                {
                    long sum = s + op;
                    long mult = s * op;

                    dest.Add(sum);
                    dest.Add(mult);
                }

                source = dest;
            }

            return source.Contains(result) ? result : 0;
        }

        private static long IsSolvable2(long result, long[] ops)
        {
            HashSet<long> source = new HashSet<long>();
            source.Add(ops[0]);

            for (int i = 1; i < ops.Length; i++)
            {
                HashSet<long> dest = new HashSet<long>();
                long op = ops[i];

                // apply to all existing -> three operations -> put to dest
                foreach (long s in source)
                {
                    long sum = s + op;
                    long mult = s * op;
                    long concat = Convert.ToInt64(Convert.ToString(s) + Convert.ToString(op));

                    dest.Add(sum);
                    dest.Add(mult);
                    dest.Add(concat);
                }

                source = dest;
            }

            return source.Contains(result) ? result : 0;
        }

        private static long IsSolvable3(long result, long[] ops)
        {
            // avoid string conversion

            HashSet<long> source = new HashSet<long>();
            source.Add(ops[0]);

            for (int i = 1; i < ops.Length; i++)
            {
                HashSet<long> dest = new HashSet<long>();
                long op = ops[i];

                // apply to all existing -> three operations -> put to dest
                foreach (long s in source)
                {
                    long sum = s + op;
                    long mult = s * op;
                    long concat;

                    if (op < 10)
                    {
                        concat = s * 10 + op;
                    }
                    else if (op < 100)
                    {
                        concat = s * 100 + op;
                    }
                    else if (op < 1000)
                    {
                        concat = s * 1000 + op;
                    }
                    else
                    {
                        throw new ApplicationException();
                    }

                    dest.Add(sum);
                    dest.Add(mult);
                    dest.Add(concat);
                }

                source = dest;
            }

            return source.Contains(result) ? result : 0;
        }

        private static long Solve01(List<long> results, List<long[]> operators)
        {
            long count = 0;
            for (int i = 0; i < results.Count; i++)
            {
                count += IsSolvable(results[i], operators[i]);
            }

            return count;
        }

        private static long Solve02(List<long> results, List<long[]> operators)
        {
            long count = 0;
            for (int i = 0; i < results.Count; i++)
            {
                count += IsSolvable2(results[i], operators[i]);
            }

            return count;
        }

        private static long Solve03(List<long> results, List<long[]> operators)
        {
            long count = 0;
            for (int i = 0; i < results.Count; i++)
            {
                count += IsSolvable3(results[i], operators[i]);
            }

            return count;
        }

        public static void Run()
        {
            string day = "07";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;

            List<long> results = new List<long>();
            List<long[]> operators = new List<long[]>();


            while ((s = sr.ReadLine()) != null)
            {
                string[] splitted = s.Split(new string[] { ":", " " }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                long result = Convert.ToInt64(splitted[0]);
                long[] ops = new long[splitted.Length - 1];

                for (int i = 0; i < ops.Length; i++)
                {
                    ops[i] = Convert.ToInt64(splitted[i + 1]);
                }

                results.Add(result);
                operators.Add(ops);
            }

            sr.Close();

            long started = Environment.TickCount;

            long safe = Solve01(results, operators);
            Console.WriteLine(safe);

            //long safe2 = Solve02(results, operators);
            //Console.WriteLine(safe2);

            long safe3 = Solve03(results, operators);
            Console.WriteLine(safe3);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed);
        }
    }
}

/*

(No optimizations)
Elapsed: 1002 ms

(Avoid string conversion)
Elapsed: 614 ms

 */