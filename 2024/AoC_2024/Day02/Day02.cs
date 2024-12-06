using System;
namespace AoC_2024
{
	public class Day02
	{
        private static bool IsSafe(long[] report)
        {
            bool direction = report[1] > report[0];

            for (int i = 0; i < report.Length - 1; i++)
            {
                if (report[i + 1] == report[i]) return false;

                bool tmp_dir = report[i + 1] > report[i];
                if (tmp_dir != direction) return false;

                long diff = Math.Abs(report[i + 1] - report[i]);
                if (diff > 3) return false;
            }

            return true;
        }

        private static long Solve01(List<long[]> list)
        {
            long safe = 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (IsSafe(list[i]))
                {
                    safe++;
                }
            }

            return safe;
        }

        private static bool IsSafeWithRemoval(long[] report)
        {
            if (IsSafe(report)) return true;

            for (int i = 0; i < report.Length; i++)
            {
                List<long> list = new List<long>(report);
                list.RemoveAt(i);
                long[] tmp_rep = list.ToArray();

                if (IsSafe(tmp_rep)) return true;
            }

            return false;
        }

        private static long Solve02(List<long[]> list)
        {
            long safe = 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (IsSafeWithRemoval(list[i]))
                {
                    safe++;
                }
            }

            return safe;
        }

        public static void Run()
        {
            string day = "02";

            StreamReader sr = new StreamReader("Day02/data" + day + ".txt");
            //StreamReader sr = new StreamReader("Day02/test" + day + ".txt");
            string s = null;

            List<long[]> list = new List<long[]>();

            while ((s = sr.ReadLine()) != null)
            {
                string[] splitted = s.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                long[] report = new long[splitted.Length];
                for (int i = 0; i < splitted.Length; i++)
                {
                    report[i] = Convert.ToInt64(splitted[i]);
                }

                list.Add(report);
            }

            sr.Close();

            long safe = Solve01(list);
            Console.WriteLine(safe);

            long safe2 = Solve02(list);
            Console.WriteLine(safe2);

        }
    }
}

