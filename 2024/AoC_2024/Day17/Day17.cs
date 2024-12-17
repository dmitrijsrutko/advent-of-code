using System;

namespace AoC_2024
{
    public class Day17
    {
        private static long GetComboValue(long operand, long a, long b, long c)
        {
            long combo = -1;
            switch (operand)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    combo = operand;  // literal values
                    break;
                case 4:
                    combo = a;
                    break;
                case 5:
                    combo = b;
                    break;
                case 6:
                    combo = c;
                    break;
                default:
                    throw new ApplicationException();
            }

            return combo;
        }

        private static long GetLiteral(long operand)
        {
            return operand;
        }

        private static List<long> Solve(long a, long b, long c, long[] program)
        {
            List<long> outputs = new List<long>();
            long pointer = 0;

            while (true)
            {
                // read instructions and process
                if (pointer >= program.Length)
                {
                    break;  // halt
                }

                long instruction = program[pointer];
                long operand = program[pointer + 1];
                pointer += 2;

                long combo;
                switch (instruction)
                {
                    case 0:
                        // The adv instruction (opcode 0) performs division.
                        {
                            long numerator = a;
                            combo = GetComboValue(operand, a, b, c);
                            long denominator = (long)Math.Pow(2, combo);
                            a = numerator / denominator;
                        }
                        break;

                    case 1:
                        // The bxl instruction (opcode 1) calculates the bitwise XOR
                        b = b ^ GetLiteral(operand);
                        break;

                    case 2:
                        // The bst instruction (opcode 2) calculates the value of its combo operand modulo 8
                        combo = GetComboValue(operand, a, b, c);
                        b = combo % 8;
                        break;

                    case 3:
                        // The jnz instruction (opcode 3) does nothing if the A register is 0.
                        if (a == 0)
                        {
                            // skip
                        }
                        else
                        {
                            pointer = GetLiteral(operand);
                        }
                        break;

                    case 4:
                        // The bxc instruction (opcode 4) calculates the bitwise XOR of register B and register C
                        b = b ^ c;
                        break;

                    case 5:
                        // The out instruction (opcode 5) calculates the value of its combo operand modulo 8
                        combo = GetComboValue(operand, a, b, c);
                        long output = combo % 8;
                        outputs.Add(output);
                        break;

                    case 6:
                        // The bdv instruction (opcode 6) works exactly like the adv instruction
                        {
                            long numerator = a;
                            combo = GetComboValue(operand, a, b, c);
                            long denominator = (long)Math.Pow(2, combo);
                            b = numerator / denominator;
                        }
                        break;

                    case 7:
                        // The cdv instruction (opcode 7) works exactly like the adv instruction
                        {
                            long numerator = a;
                            combo = GetComboValue(operand, a, b, c);
                            long denominator = (long)Math.Pow(2, combo);
                            c = numerator / denominator;
                        }
                        break;
                }
            }

            return outputs;
        }

        private static long Solve_SelfPoint_Bitwise(long a, long b, long c, long[] program)
        {
            long length = 0;
            long pointer = 0;

            while (true)
            {
                // read instructions and process
                if (pointer >= program.Length)
                {
                    break;  // halt
                }

                long instruction = program[pointer];
                long operand = program[pointer + 1];
                pointer += 2;

                long combo;
                switch (instruction)
                {
                    case 0:
                        // The adv instruction (opcode 0) performs division.
                        {
                            long numerator = a;
                            combo = GetComboValue(operand, a, b, c);
                            a = numerator >> ((byte)combo);
                        }
                        break;

                    case 1:
                        // The bxl instruction (opcode 1) calculates the bitwise XOR
                        b = b ^ GetLiteral(operand);
                        break;

                    case 2:
                        // The bst instruction (opcode 2) calculates the value of its combo operand modulo 8
                        combo = GetComboValue(operand, a, b, c);
                        b = combo & 7;
                        break;

                    case 3:
                        // The jnz instruction (opcode 3) does nothing if the A register is 0.
                        if (a == 0)
                        {
                            // skip
                        }
                        else
                        {
                            pointer = GetLiteral(operand);
                        }
                        break;

                    case 4:
                        // The bxc instruction (opcode 4) calculates the bitwise XOR of register B and register C
                        b = b ^ c;
                        break;

                    case 5:
                        // The out instruction (opcode 5) calculates the value of its combo operand modulo 8
                        combo = GetComboValue(operand, a, b, c);
                        long output = combo & 7;

                        if (length >= program.Length)
                        {
                            return -1;
                        }

                        if (program[length] != output)
                        {
                            return length;
                        }
                        length++;
                        break;

                    case 6:
                        // The bdv instruction (opcode 6) works exactly like the adv instruction
                        {
                            long numerator = a;
                            combo = GetComboValue(operand, a, b, c);
                            b = numerator >> ((byte)combo);
                        }
                        break;

                    case 7:
                        // The cdv instruction (opcode 7) works exactly like the adv instruction
                        {
                            long numerator = a;
                            combo = GetComboValue(operand, a, b, c);
                            c = numerator >> ((byte)combo);
                        }
                        break;
                }
            }

            return length;
        }

        private static string Solve01(long a, long b, long c, long[] program)
        {
            List<long> outputs = Solve(a, b, c, program);
            string result = string.Join(',', outputs);
            return result;
        }

        private static long Solve02(long a, long b, long c, long[] program)
        {
            // manual binary bitmask search

            // test case
            // 0 ..7 % 64
            // 64 % 128
            // 192 % 256
            // 192 % 512
            // 117440 -> found quickly

            // data case
            // 61 or 53 % 64
            // 61 % 64 -> over larger length only
            // 61 % 128
            // 189 % 256
            // 189 or 445 % 512 -> FORK
            // 701 % 1024
            // 1725 % 2048
            // 3773 % 4096
            // 3773 % 8192
            // 11965 % 16384
            // 28349 % 32768
            // 28349 % 65536
            // 93885 % 131072
            // 93885 % 262144
            // 356029 % 524288
            // 880317 % 1048576
            // 880317 % 2097152
            // 2977469 % 4194304

            long to = 1l * 1000 * 1000 * 1000 * 1000 * 1000;
            for (long aa = 2977469; aa <= to; aa += 4194304)
            {
                long result = Solve_SelfPoint_Bitwise(aa, b, c, program);
                if (result == program.Length)
                {
                    Console.WriteLine(aa);
                    //return;
                }

                if (result >= 15)
                {
                    //Console.WriteLine("result: " + result + " aa = " + (aa % 64));
                    //Console.WriteLine("result: " + result + " aa = " + (aa % 128));
                    //Console.WriteLine("result: " + result + " aa = " + (aa % 256));
                    //Console.WriteLine("result: " + result + " aa = " + (aa % 512));
                    //Console.WriteLine("result: " + result + " aa = " + (aa % 1024));
                    //Console.WriteLine("result: " + result + " aa = " + (aa % 2048));
                    //Console.WriteLine("result: " + result + " aa = " + (aa % 4096));
                    //Console.WriteLine("result: " + result + " aa = " + (aa % 8192));
                    //Console.WriteLine("result: " + result + " aa = " + (aa % 16384));
                    //Console.WriteLine("result: " + result + " aa = " + (aa % 32768));
                    //Console.WriteLine("result: " + result + " aa = " + (aa % 65536));
                    //Console.WriteLine("result: " + result + " aa = " + (aa % 131072));
                    //Console.WriteLine("result: " + result + " aa = " + (aa % 262144));
                    //Console.WriteLine("result: " + result + " aa = " + (aa % 524288));
                    //Console.WriteLine("result: " + result + " aa = " + (aa % 1048576));
                    //Console.WriteLine("result: " + result + " aa = " + (aa % 2097152));
                    //Console.WriteLine("result: " + result + " aa = " + (aa % 4194304));
                    Console.WriteLine("result: " + result + " aa = " + (aa % 8388608));
                }
            }

            return -1;
        }

        public static void Run()
        {
            string day = "17";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + "_2.txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;

            string sa = sr.ReadLine();
            string sb = sr.ReadLine();
            string sc = sr.ReadLine();
            s = sr.ReadLine();
            string sp = sr.ReadLine();

            char[] delimeters = new char[] { ' ', ',', ':', '+', '=' };
            string[] splitted_a = sa.Split(delimeters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            string[] splitted_b = sb.Split(delimeters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            string[] splitted_c = sc.Split(delimeters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            string[] splitted_p = sp.Split(delimeters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            long a = Convert.ToInt64(splitted_a[2]);
            long b = Convert.ToInt64(splitted_b[2]);
            long c = Convert.ToInt64(splitted_c[2]);

            long[] program = new long[splitted_p.Length - 1];
            for (int i = 0; i < program.Length; i++)
            {
                program[i] = Convert.ToInt64(splitted_p[i + 1]);
            }

            sr.Close();

            long started = Environment.TickCount;

            //string solve01 = Solve01(a, b, c, program);
            //Console.WriteLine(solve01);

            long solve02 = Solve02(a, b, c, program);
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*
 * 

7,5,4,3,4,5,3,4,6
Elapsed: 15 ms

164278899142333
Elapsed: 38853 ms

 * */