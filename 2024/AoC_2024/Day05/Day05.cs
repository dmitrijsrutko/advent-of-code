using System;
namespace AoC_2024
{
	public class Day05
	{
        private static bool IsValidRule(Dictionary<int, HashSet<int>> dependencies, int[] rule)
        {
            for (int i = 0; i < rule.Length; i++)
            {
                for (int t = i + 1; t < rule.Length; t++)
                {
                    if (dependencies.ContainsKey(rule[t]))
                    {
                        if (dependencies[rule[t]].Contains(rule[i]))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private static bool SwitchOrder(Dictionary<int, HashSet<int>> dependencies, int[] rule)
        {
            for (int i = 0; i < rule.Length; i++)
            {
                for (int t = i + 1; t < rule.Length; t++)
                {
                    if (dependencies.ContainsKey(rule[t]))
                    {
                        if (dependencies[rule[t]].Contains(rule[i]))
                        {
                            // swap
                            int tmp = rule[i];
                            rule[i] = rule[t];
                            rule[t] = tmp;

                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private static long Solve01(Dictionary<int, HashSet<int>> dependencies, List<int[]> rules)
        {
            long sum = 0;
            for (int r = 0; r < rules.Count; r++)
            {
                int[] rule = rules[r];

                if (IsValidRule(dependencies, rule))
                {
                    //Console.WriteLine(r + " - " + true);
                    long mid = rule[rule.Length / 2];
                    sum += mid;
                }

            }

            return sum;
        }

        private static long Solve02(Dictionary<int, HashSet<int>> dependencies, List<int[]> rules)
        {
            long sum = 0;
            for (int r = 0; r < rules.Count; r++)
            {
                int[] rule = rules[r];

                if (!IsValidRule(dependencies, rule))
                {
                    while (true)
                    {
                        // switch order here
                        bool result = SwitchOrder(dependencies, rule);
                        if (result) break;
                    }

                    //Console.WriteLine(r + " - " + true);
                    long mid = rule[rule.Length / 2];
                    sum += mid;
                }

            }

            return sum;
        }

        public static void Run()
        {
            string day = "05";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;

            Dictionary<int, HashSet<int>> dependencies = new Dictionary<int, HashSet<int>>();

            while ((s = sr.ReadLine()) != null)
            {
                if (s.Contains("|"))
                {
                    string[] splitted = s.Split('|');
                    int source = Convert.ToInt32(splitted[0]);
                    int dest = Convert.ToInt32(splitted[1]);

                    if (!dependencies.ContainsKey(source))
                    {
                        dependencies.Add(source, new HashSet<int>());
                    }

                    dependencies[source].Add(dest);
                }
                else
                {
                    break;
                }
            }

            List<int[]> rules = new List<int[]>();

            while ((s = sr.ReadLine()) != null)
            {
                string[] splitted = s.Split(',');
                int[] rule = new int[splitted.Length];

                for (int i = 0; i < rule.Length; i++)
                {
                    rule[i] = Convert.ToInt32(splitted[i]);
                }

                rules.Add(rule);
            }

            sr.Close();

            long safe = Solve01(dependencies, rules);
            Console.WriteLine(safe);

            long safe2 = Solve02(dependencies, rules);
            Console.WriteLine(safe2);
        }
    }
}

