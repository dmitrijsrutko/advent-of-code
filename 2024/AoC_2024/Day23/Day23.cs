using System;
namespace AoC_2024
{
	public class Day23
	{
        private static long Solve01(Dictionary<string, HashSet<string>> connections)
        {
            HashSet<string> sets = new HashSet<string>();
            foreach (string a in connections.Keys)
            {
                if (a.StartsWith('t'))
                {
                    foreach (string b in connections[a])
                    {
                        // a -> b ==> are connected
                        foreach (string c in connections[b])
                        {
                            // b -> c ==> are connected
                            if (connections[a].Contains(c))
                            {
                                SortedSet<string> set = new SortedSet<string>() { a, b, c };
                                string key = string.Join(null, set);
                                if (!sets.Contains(key))
                                {
                                    sets.Add(key);
                                }
                            }
                        }
                    }
                }
            }

            return sets.Count;
        }

        private static int MaxSubGraph = 0;
        private static string pwd = null;

        private static void Scan(Dictionary<string, HashSet<string>> connections, string a, SortedSet<string> set, HashSet<string> visited)
        {
            // verify connectivity remains -> not broken by adding new node
            foreach (string node in set)
            {
                if (!connections[a].Contains(node))
                {
                    return; // no connection possible
                }
            }

            set.Add(a);

            string key = string.Join(null, set);
            if (visited.Contains(key))
            {
                set.Remove(a);
                return;  // already visited
            }
            visited.Add(key);

            if (set.Count > MaxSubGraph)
            {
                MaxSubGraph = set.Count;
                pwd = string.Join(',', set);
            }

            foreach (string b in connections[a])
            {
                Scan(connections, b, set, visited);
            }

            set.Remove(a);
        }

        private static string Solve02(Dictionary<string, HashSet<string>> connections)
        {
            SortedSet<string> set = new SortedSet<string>();
            HashSet<string> visited = new HashSet<string>();

            foreach (string a in connections.Keys)
            {
                Scan(connections, a, set, visited);
            }

            return MaxSubGraph + " :: " + pwd;
        }

        private static long MaxClique = 0;

        /*
         * 
            algorithm BronKerbosch1(R, P, X) is
                if P and X are both empty then
                    report R as a maximal clique
                for each vertex v in P do
                    BronKerbosch1(R ⋃ {v}, P ⋂ N(v), X ⋂ N(v))
                    P := P \ {v}
                    X := X ⋃ {v}
        * */
        private static void BronKerbosch1(HashSet<string> R, HashSet<string> P, HashSet<string> X, Dictionary<string, HashSet<string>> N)
        {
            if ((P.Count == 0) && (X.Count == 0))
            {
                if (R.Count > MaxClique)
                {
                    MaxClique = R.Count;

                    SortedSet<string> sorted = new SortedSet<string>(R);
                    string key = string.Join(',', sorted);
                    Console.WriteLine(MaxClique + " :: " + key);
                }

                return;
            }

            foreach (string v in P)
            {
                HashSet<string> RX = new HashSet<string>(R.Union(new string[] { v }));
                HashSet<string> PX = new HashSet<string>(P.Intersect(N[v]));
                HashSet<string> XX = new HashSet<string>(X.Intersect(N[v]));

                BronKerbosch1(RX, PX, XX, N);

                P.Remove(v);
                X.Add(v);
            }
        }

        /*
         * 
            algorithm BronKerbosch2(R, P, X) is
                if P and X are both empty then
                    report R as a maximal clique
                choose a pivot vertex u in P ⋃ X
                for each vertex v in P \ N(u) do
                    BronKerbosch2(R ⋃ {v}, P ⋂ N(v), X ⋂ N(v))
                    P := P \ {v}
                    X := X ⋃ {v}         
         * */
        private static void BronKerbosch2(HashSet<string> R, HashSet<string> P, HashSet<string> X, Dictionary<string, HashSet<string>> N)
        {
            if ((P.Count == 0) && (X.Count == 0))
            {
                if (R.Count > MaxClique)
                {
                    MaxClique = R.Count;

                    SortedSet<string> sorted = new SortedSet<string>(R);
                    string key = string.Join(',', sorted);
                    Console.WriteLine(MaxClique + " :: " + key);
                }

                return;
            }
                        
            string u = P.Union(X).First();
            foreach (string v in P.Except(N[u]))
            {
                HashSet<string> RX = new HashSet<string>(R.Union(new string[] { v }));
                HashSet<string> PX = new HashSet<string>(P.Intersect(N[v]));
                HashSet<string> XX = new HashSet<string>(X.Intersect(N[v]));

                BronKerbosch2(RX, PX, XX, N);

                P.Remove(v);
                X.Add(v);
            }
        }

        private static void Solve03(Dictionary<string, HashSet<string>> connections)
        {
            // The recursion is initiated by setting R and X to be the empty set and P to be the vertex set of the graph.
            HashSet<string> R = new HashSet<string>();
            HashSet<string> P = new HashSet<string>(connections.Keys);
            HashSet<string> X = new HashSet<string>();

            BronKerbosch1(R, P, X, connections);
        }

        private static void Solve04(Dictionary<string, HashSet<string>> connections)
        {
            // The recursion is initiated by setting R and X to be the empty set and P to be the vertex set of the graph.
            HashSet<string> R = new HashSet<string>();
            HashSet<string> P = new HashSet<string>(connections.Keys);
            HashSet<string> X = new HashSet<string>();

            BronKerbosch2(R, P, X, connections);
        }

        public static void Run()
        {
            string day = "23";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;

            Dictionary<string, HashSet<string>> connections = new Dictionary<string, HashSet<string>>();

            while ((s = sr.ReadLine()) != null)
            {
                string[] splitted = s.Split('-');

                string a = splitted[0];
                string b = splitted[1];

                if (!connections.ContainsKey(a))
                {
                    connections.Add(a, new HashSet<string>());
                }

                if (!connections.ContainsKey(b))
                {
                    connections.Add(b, new HashSet<string>());
                }

                connections[a].Add(b);
                connections[b].Add(a);
            }

            sr.Close();

            long started = Environment.TickCount;

            //long solve01 = Solve01(connections);
            //Console.WriteLine(solve01);

            //string solve02 = Solve02(connections);
            //Console.WriteLine(solve02);

            //Solve03(connections);
            Solve04(connections);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*
 * 

1175
13 :: bw,dr,du,ha,mm,ov,pj,qh,tz,uv,vq,wq,xw

Elapsed: 1581 ms


Max clique by BronKerbosch1

12 :: cn,hx,jq,jr,mg,rk,sk,sl,sv,tg,vn,xv
13 :: bw,dr,du,ha,mm,ov,pj,qh,tz,uv,vq,wq,xw

Elapsed: 466 ms

Max clique by BronKerbosch2 (with pivoting)

12 :: cn,hx,jq,jr,mg,rk,sk,sl,sv,tg,vn,xv
13 :: bw,dr,du,ha,mm,ov,pj,qh,tz,uv,vq,wq,xw

Elapsed: 41 ms

 * */
