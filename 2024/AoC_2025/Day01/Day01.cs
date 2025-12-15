using System;

namespace AoC_2025
{
	public class Day01
	{
        private static long Solve01(List<string> rotations)
        {
            int position = 50;
            const int MaxPosition = 100;

            int zeros = 0;

            foreach (string rotation in rotations)
            {
                char dir = rotation[0];
                int steps = int.Parse(rotation.Substring(1));

                if (dir == 'L')
                {
                    position = (position - steps) % MaxPosition;
                }
                else if (dir == 'R')
                {
                    position = (position + steps) % MaxPosition;
                }
                else
                {
                    throw new Exception("Invalid direction: " + dir);
                }

                if (position == 0)
                {
                    zeros++;
                } 
            }

            return zeros;
        }

        private static long Solve02(List<string> rotations)
        {
            int position = 50;
            const int MaxPosition = 100;

            int zeros = 0;

            foreach (string rotation in rotations)
            {
                char dir = rotation[0];
                int steps = int.Parse(rotation.Substring(1));

                zeros += steps / MaxPosition;   // full cycles
                steps = steps % MaxPosition;

                if (dir == 'L')
                {
                    if ((position != 0) && (position - steps <= 0))
                    {
                        zeros++;
                    }
                    position = (position - steps + MaxPosition) % MaxPosition;
                }
                else if (dir == 'R')
                {
                    if ((position != 0) && (position + steps >= MaxPosition))
                    {
                        zeros++;
                    }
                    position = (position + steps) % MaxPosition;
                }
                else
                {
                    throw new Exception("Invalid direction: " + dir);
                }
            }

            return zeros;
        }

        public static void Run()
        {
            // StreamReader sr = new StreamReader("Day01/test01.txt");
            StreamReader sr = new StreamReader("Day01/data01.txt");
            string s = null;

            List<string> rotations = new List<string>();

            while ((s = sr.ReadLine()) != null)
            {
                rotations.Add(s);
            }

            sr.Close();

            long started = Environment.TickCount;

            // long solve01 = Solve01(rotations);
            // Console.WriteLine(solve01);

            long solve02 = Solve02(rotations);
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*
1158

Elapsed: 6 ms

6860

Elapsed: 7 ms
*/