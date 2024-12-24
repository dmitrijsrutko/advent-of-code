using System;

namespace AoC_2024
{
    public class Day24
    {
        public struct Operation
        {
            public string operation;
            public string operand_2;
            public string dest;

            public Operation(string operation, string operand_2, string dest)
            {
                this.operation = operation;
                this.operand_2 = operand_2;
                this.dest = dest;
            }
        }

        private static long Solve01(Dictionary<string, long> values, Dictionary<string, List<Operation>> operations, Dictionary<string, string> mapping, HashSet<string> visited)
        {
            // nbw AND gwf -> btm
            // nbw XOR gwf -> z03
            // multiple sources possible -> use set instead of list
            HashSet<string> source = new HashSet<string>(values.Keys);

            while (source.Count != 0)
            {
                SortedSet<string> keys = new SortedSet<string>(source);
                string key = string.Join(',', keys);
                //Console.WriteLine(key);

                if (visited.Contains(key))
                {
                    //Console.WriteLine("visited");
                    return -1;
                }

                visited.Add(key);

                HashSet<string> dest = new HashSet<string>();
                foreach (string operand_1 in source)
                {
                    if (operations.ContainsKey(operand_1))
                    {
                        // perform op
                        if (!values.ContainsKey(operand_1)) throw new ApplicationException();

                        // check if value exists
                        foreach (Operation operation in operations[operand_1])
                        {
                            string operand_2 = operation.operand_2;

                            if (values.ContainsKey(operand_1) && values.ContainsKey(operand_2))
                            {
                                // perform op
                                long new_val = -1;
                                if (operation.operation == "AND")
                                {
                                    new_val = values[operand_1] & values[operand_2];
                                }
                                else if (operation.operation == "OR")
                                {
                                    new_val = values[operand_1] | values[operand_2];
                                }
                                else if (operation.operation == "XOR")
                                {
                                    new_val = values[operand_1] ^ values[operand_2];
                                }
                                else
                                {
                                    throw new ApplicationException();
                                }

                                string operation_dest = operation.dest;
                                operation_dest = mapping.GetValueOrDefault(operation_dest, operation_dest);

                                values.Add(operation_dest, new_val);
                                dest.Add(operation_dest);
                            }
                            else
                            {
                                // postpone
                                dest.Add(operand_1);
                            }
                        }
                    }
                    else
                    {
                        // skip
                    }
                }

                source = dest;
            }

            SortedSet<string> z_keys = GetKeys(values, 'z');
            string[] z_keys_reverse = z_keys.ToArray();
            Array.Reverse(z_keys_reverse);

            long output = 0;
            for (int i = 0; i < z_keys_reverse.Length; i++)
            {
                output = (output << 1) + values[z_keys_reverse[i]];
            }

            return output;
        }

        private static SortedSet<string> GetKeys(Dictionary<string, long> values, char prefix)
        {
            SortedSet<string> keys = new SortedSet<string>();
            foreach (string key in values.Keys)
            {
                if (key.StartsWith(prefix))
                {
                    keys.Add(key);
                }
            }

            return keys;
        }

        private static void SetBits(Dictionary<string, long> values, SortedSet<string> keys, long value)
        {
            foreach (string key in keys)
            {
                long bit = value & 1;
                values.Add(key, bit);
                value = value >> 1;
            }
        }

        private static long BitDistance(long a, long b)
        {
            // number of bits different in a and b
            long distance = 0;
            long xor = a ^ b;

            while (xor > 0)
            {
                distance += xor & 1;
                xor >>= 1;
            }

            return distance;
        }

        private static long TotalDistance(Random random, SortedSet<string> keys_x, SortedSet<string> keys_y, Dictionary<string, string> mapping, Dictionary<string, long> orig_values, Dictionary<string, List<Operation>> operations)
        {
            long Max45 = 35184372088832;

            long distance = 0;
            for (int i = 0; i < 1000; i++)
            {
                long x = random.NextInt64(0, Max45);
                long y = random.NextInt64(0, Max45);

                HashSet<string> visited = new HashSet<string>();

                Dictionary<string, long> values = new Dictionary<string, long>();

                SetBits(values, keys_x, x);
                SetBits(values, keys_y, y);

                long z = Solve01(values, operations, mapping, visited);
                long zz = x + y;

                if (z == -1)
                {
                    return -1;  // cycle
                }

                distance += BitDistance(z, zz);
            }

            return distance;
        }

        private static long Solve02(Dictionary<string, long> values, Dictionary<string, List<Operation>> operations)
        {
            string[] theory = new string[] { "tnc", "z39", "fhg", "z17", "vcf", "z10", "dvb", "fsq" };
            SortedSet<string> sorted = new SortedSet<string>(theory);

            string result = string.Join(',', sorted);
            Console.WriteLine(result);
            return -1;

            // simulate addition -> verify and measure an error

            SortedSet<string> keys_x = GetKeys(values, 'x');
            SortedSet<string> keys_y = GetKeys(values, 'y');

            Dictionary<string, string> mapping = new Dictionary<string, string>();

            string[] ss = new string[] { "tnc", "z39" };

            mapping.Add(ss[0], ss[1]);
            mapping.Add(ss[1], ss[0]);

            // fhg <-> z17
            ss = new string[] { "fhg", "z17" };

            mapping.Add(ss[0], ss[1]);
            mapping.Add(ss[1], ss[0]);

            // vcf <-> z10
            ss = new string[] { "vcf", "z10" };

            mapping.Add(ss[0], ss[1]);
            mapping.Add(ss[1], ss[0]);

            // dvb <-> fsq
            ss = new string[] { "dvb", "fsq" };

            mapping.Add(ss[0], ss[1]);
            mapping.Add(ss[1], ss[0]);

            Random random = new Random();

            long distance = TotalDistance(random, keys_x, keys_y, mapping, values, operations);

            // try to simulate all possible swaps -> get the one with minimal distance
            // TODO: should remove from swaps -> those which cycle (?)

            Console.WriteLine("Original distance: " + distance);
            return -1;


            // find pair if gates to switch -> so for this input result is corrected
            SortedSet<string> destinations = new SortedSet<string>();
            foreach (string source in operations.Keys)
            {
                foreach (Operation operation in operations[source])
                {
                    destinations.Add(operation.dest);
                }
            }

            string[] dest_array = destinations.ToArray();
            long count = 0;

            for (int i = 0; i < dest_array.Length; i++)
            {
                for (int t = i + 1; t < dest_array.Length; t++)
                {
                    string source = dest_array[i];
                    string dest = dest_array[t];

                    // probably existing mapping
                    if (mapping.ContainsKey(source)) continue;
                    if (mapping.ContainsKey(dest)) continue;

                    mapping.Add(source, dest);
                    mapping.Add(dest, source);

                    long distance_swap = TotalDistance(random, keys_x, keys_y, mapping, values, operations);
                    if (distance_swap != -1)
                    {
                        if (distance_swap < 150)
                        {
                            Console.WriteLine(source + " <-> " + dest + " :: " + distance_swap + " [" + count + "]");
                        }
                    }
                    count++;

                    // undo
                    mapping.Remove(source);
                    mapping.Remove(dest);
                }
            }

            return -1;
        }

        public static void Run()
        {
            string day = "24";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + "_2.txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;
            Dictionary<string, long> values = new Dictionary<string, long>();
            Dictionary<string, List<Operation>> operations = new Dictionary<string, List<Operation>>();

            while ((s = sr.ReadLine()) != null)
            {
                if ((s == null) || (s == "")) break;

                char[] delimeters = new char[] { ' ', ',', ':', '+', '=' };
                string[] splitted = s.Split(delimeters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                values.Add(splitted[0], long.Parse(splitted[1]));
            }

            while ((s = sr.ReadLine()) != null)
            {
                char[] delimeters = new char[] { ' ', ',', ':', '+', '=', '-', '>' };
                string[] splitted = s.Split(delimeters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                Operation operation = new Operation(splitted[1], splitted[2], splitted[3]);
                string source = splitted[0];
                if (!operations.ContainsKey(source))
                {
                    operations.Add(source, new List<Operation>());
                }
                operations[source].Add(operation);
            }

            sr.Close();

            long started = Environment.TickCount;

            //long solve01 = Solve01(values, operations, new Dictionary<string, string>());
            //Console.WriteLine(solve01);

            long solve02 = Solve02(values, operations);
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*

43942008931358

Elapsed: 15 ms


dvb,fhg,fsq,tnc,vcf,z10,z17,z39
-1

Elapsed: 13 ms

 * */