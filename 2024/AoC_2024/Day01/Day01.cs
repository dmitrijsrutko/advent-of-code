using System;
namespace AoC_2024
{
	public class Day01
	{
        private static long Solve(long[] list1, long[] list2)
        {
            Array.Sort(list1);
            Array.Sort(list2);

            long distance = 0;
            for (int i = 0; i < list1.Length; i++)
            {
                distance += Math.Abs(list1[i] - list2[i]);
            }

            return distance;
        }

        private static long Solve2(long[] list1, long[] list2)
        {
            Dictionary<long, long> counts = new Dictionary<long, long>();
            for (int i = 0; i < list2.Length; i++)
            {
                long el = list2[i];
                if (counts.ContainsKey(el))
                {
                    counts[el]++;
                }
                else
                {
                    counts[el] = 1;
                }
            }

            long similarity = 0;
            for (int i = 0; i < list1.Length; i++)
            {
                long el = list1[i];
                long count = counts.GetValueOrDefault(el, 0);

                similarity += el * count;
            }


            return similarity;
        }

        public static void Run()
        {
            StreamReader sr = new StreamReader("data1.txt");
            //StreamReader sr = new StreamReader("test.txt");
            string s = null;


            List<long> list1 = new List<long>();
            List<long> list2 = new List<long>();

            while ((s = sr.ReadLine()) != null)
            {
                string[] splitted = s.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                //Console.WriteLine(splitted[0] + " - " + splitted[1]);

                long l1 = Convert.ToInt64(splitted[0]);
                long l2 = Convert.ToInt64(splitted[1]);

                list1.Add(l1);
                list2.Add(l2);
            }

            sr.Close();


            long distance = Solve(list1.ToArray(), list2.ToArray());
            Console.WriteLine(distance);


            long similarity = Solve2(list1.ToArray(), list2.ToArray());
            Console.WriteLine(similarity);
        }
    }
}

