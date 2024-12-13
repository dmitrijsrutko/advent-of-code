using System;
using System.Numerics;

namespace AoC_2024
{
	public class Day13
	{
        public class Point
        {
            public long x, y;

            public Point(long x, long y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public class ClawMachine
        {
            public Point a, b, p;

            public ClawMachine(Point a, Point b, Point p)
            {
                this.a = a;
                this.b = b;
                this.p = p;
            }

            public override string ToString()
            {
                return "prize: " + p.x + " " + p.y;
            }
        }

        /*
 * 

Find min (3 * A + B) that

Button A: X+94, Y+34
Button B: X+22, Y+67
Prize: X=8400, Y=5400

94 * A + 22 * B == 8400
34 * A + 67 * B == 5400

A = 80
B = 40

B = (8400 - (80 * 94)) / 22 = 40

iterate from 0 to 100 (including) for a -> calculate B -> calculate min total cost (?)
*/
        private static long Solve(ClawMachine claw, long MaxPlays)
        {
            long minTokens = long.MaxValue;
            for (long A = 0; A <= MaxPlays; A++)
            {
                long B = (claw.p.x - (A * claw.a.x)) / claw.b.x;
                if ((B >= 0) && (B <= MaxPlays))
                {
                    // verify
                    long px = claw.a.x * A + claw.b.x * B;
                    long py = claw.a.y * A + claw.b.y * B;

                    if ((px == claw.p.x) && (py == claw.p.y))
                    {
                        long tokens = A * 3 + B;
                        minTokens = Math.Min(minTokens, tokens);
                    }
                }
            }

            return minTokens == long.MaxValue ? 0 : minTokens;
        }


        public static long GCD(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }

            return Math.Abs(a); // Return positive GCD
        }

        // Method to find gcd and coefficients x and y
        public static (long gcd, long x, long y) ExtendedGCD(long a, long b)
        {
            // Base case: when b is 0, gcd is a, and x = 1, y = 0
            if (b == 0)
            {
                return (a, 1, 0);
            }

            // Recursive call
            var (gcd, x1, y1) = ExtendedGCD(b, a % b);

            // Update x and y using results from recursion
            long x = y1;
            long y = x1 - (a / b) * y1;

            return (gcd, x, y);
        }

        /*
* 

Find min (3 * A + B) that

Button A: X+94, Y+34
Button B: X+22, Y+67
Prize: X=8400, Y=5400

94 * A + 22 * B == 8400
34 * A + 67 * B == 5400

(94 + 34) * A + (22 + 67) * B = (8400 + 5400)
A * x + B * y = C

A = 80
B = 40

*/
        private static long SolveDiophantine(ClawMachine claw, long MaxPlays)
        {

            long a = claw.a.x + claw.a.y;
            long b = claw.b.x + claw.b.y;
            long c = claw.p.x + claw.p.y;

            // Step 1: Check for solvability
            long d = GCD(a, b);
            if (c % d != 0) return 0;   // not solvable

            // Step 2: Reduce the equation
            a /= d;
            b /= d;
            c /= d;

            // Step 3: Find one particular solution
            var (gcd, x0, y0) = ExtendedGCD(a, b);

            x0 *= c;
            y0 *= c;

            // Step 4: Generate General Solutions

            // Step 5: Ensure Non-Negative Solutions
            long minK = -x0 / b;
            long maxK = y0 / a;

            long minTokens = long.MaxValue;
            for (long k = minK; k <= maxK; k++)
            {
                long x = x0 + k * b;
                long y = y0 - k * a;

                if ((x >= 0) && (y >= 0))
                {
                    if ((x <= MaxPlays) && (y <= MaxPlays))
                    {
                        if (claw.p.x == claw.a.x * x + claw.b.x * y)
                        {
                            if (claw.p.y == claw.a.y * x + claw.b.y * y)
                            {
                                long tokens = x * 3 + y;
                                minTokens = Math.Min(minTokens, tokens);
                                //Console.WriteLine(claw.ToString() + " ---   " + tokens + " :: " + x + " " + y);
                            }
                        }
                    }
                }

            }

            return minTokens == long.MaxValue ? 0 : minTokens;
        }

        private static long Solve01(List<ClawMachine> claws)
        {
            long tokens = 0;
            for (int i = 0; i < claws.Count; i++)
            {
                tokens += Solve(claws[i], 100);
            }

            return tokens;
        }

        private static long Solve02(List<ClawMachine> claws)
        {
            long tokens = 0;
            for (int i = 0; i < claws.Count; i++)
            {

                long token1 = Solve(claws[i], 100);
                long token2 = SolveDiophantine(claws[i], 100);

                if (token1 != token2) throw new ApplicationException("i="  + i + " " + token1 + " " + token2);


                tokens += token2;
            }

            return tokens;
        }

        private static long Solve03(List<ClawMachine> claws)
        {
            long addition = 10000000000000;
            long tokens = 0;

            for (int i = 0; i < claws.Count; i++)
            {
                Console.WriteLine(i);

                claws[i].p.x += addition;
                claws[i].p.y += addition;

                long token2 = SolveDiophantine(claws[i], long.MaxValue);
                tokens += token2;
            }

            return tokens;
        }


        public static void Run()
        {

            string day = "13";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            //StreamReader sr = new StreamReader("Day" + day + "/manual_test" + ".txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;

            List<ClawMachine> claws = new List<ClawMachine>();

            while (true)
            {
                string sa = sr.ReadLine();
                string sb = sr.ReadLine();
                string sp = sr.ReadLine();
                s = sr.ReadLine();

                char[] delimeters = new char[] { ' ', ',', ':', '+', '=' };
                string[] splitted_a = sa.Split(delimeters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ;
                string[] splitted_b = sb.Split(delimeters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                string[] splitted_p = sp.Split(delimeters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);


                Point a = new Point(Convert.ToInt32(splitted_a[3]), Convert.ToInt32(splitted_a[5]));
                Point b = new Point(Convert.ToInt32(splitted_b[3]), Convert.ToInt32(splitted_b[5]));
                Point p = new Point(Convert.ToInt32(splitted_p[2]), Convert.ToInt32(splitted_p[4]));

                ClawMachine claw = new ClawMachine(a, b, p);
                claws.Add(claw);

                if (s == null) break;
            }

            sr.Close();

            long started = Environment.TickCount;

            long solve01 = Solve01(claws);
            Console.WriteLine(solve01);

            long solve02 = Solve02(claws);
            Console.WriteLine(solve02);

            long solve03 = Solve03(claws);
            Console.WriteLine(solve03);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*
 * 
 * 
 * Flawed tests !!!!! does not include cases where it fails on manual case
31589

Elapsed: 13 ms
 * */

/*

31589
98080815200063

Elapsed: 2970340 ms
 * */