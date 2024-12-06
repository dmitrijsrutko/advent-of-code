using System;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

namespace AoC_2024
{
	public class Day03
	{
        private static string[] matchCollection;

        private static long GetSum(string pattern)
        {
            Regex regex = new Regex(@"[\d]+");
            var matchCollection = regex.Matches(pattern);

            long firstNumber = long.Parse(matchCollection[0].Value);
            long secondNumber = long.Parse(matchCollection[1].Value);

            return firstNumber * secondNumber;
        }

        private static long Solve01(string pattern)
        {
            string expr = @"mul\(\d{1,3},\d{1,3}\)";

            Regex regex = new Regex(expr);
            var matchCollection = regex.Matches(pattern);

            long sum = 0;
            for (int i = 0; i < matchCollection.Count; i++)
            {
                //Console.WriteLine(matchCollection[i].Value);
                sum += GetSum(matchCollection[i].Value);
            }

            return sum;
        }

        private static long SolveEnabled(string pattern, bool enabled)
        {
            string expr = @"do\(\)";
            string[] matchCollection = Regex.Split(pattern, expr);

            long sum = 0;
            for (int i = 0; i < matchCollection.Length; i++)
            {
                if (enabled)
                {
                    sum += Solve01(matchCollection[i]);
                }

                enabled = true; // all further are enabled
            }

            return sum;
        }

        private static long Solve02(string pattern)
        {
            string expr = @"don't\(\)";
            string[] matchCollection = Regex.Split(pattern, expr);

            bool enabled = true;    // default

            long sum = 0;
            for (int i = 0; i < matchCollection.Length; i++)
            {
                sum += SolveEnabled(matchCollection[i], enabled);

                enabled = false;    // all further tockens are disabled
                //sum += GetSum(matchCollection[i].Value);
            }

            return sum;
        }


        public static void Run()
        {
            string day = "03";

            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");
            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + "_02.txt");
            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            string s = null;



            string pattern = sr.ReadLine();
            Console.WriteLine(pattern);


            sr.Close();

            long safe = Solve01(pattern);
            Console.WriteLine(safe);

            long safe2 = Solve02(pattern);
            Console.WriteLine(safe2);

        }
    }
}

