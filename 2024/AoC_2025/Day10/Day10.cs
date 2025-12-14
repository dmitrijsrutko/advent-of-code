using System;
using Google.OrTools.LinearSolver;

namespace AoC_2025
{
	public class Day10
	{
        public class State
        {
            public string lights;
            public List<int[]> toggles;
            public int[] joltages;

            public State(string lights, List<int[]> toggles, int[] joltages)
            {
                this.lights = lights;
                this.toggles = toggles;
                this.joltages = joltages;
            }
        }

        private static long Solve(int lightMask, int[] toggleMasks)
        {
            // Solve with BFS

            List<int> source = new List<int>();
            int sourceMask = 0;
            source.Add(sourceMask);
            HashSet<int> visited = new HashSet<int>();
            visited.Add(sourceMask);

            long presses = 0;
            while (source.Count > 0)
            {
                List<int> dest = new List<int>();

                foreach (int currentMask in source)
                {
                    if (currentMask == lightMask)
                    {
                        return presses;
                    }

                    // try all toggles
                    for (int i = 0; i < toggleMasks.Length; i++)
                    {
                        int nextMask = currentMask ^ toggleMasks[i];
                        if (!visited.Contains(nextMask))
                        {
                            visited.Add(nextMask);
                            dest.Add(nextMask);
                        }
                    }
                }

                source = dest;
                presses++;
            }

            throw new Exception("No solution found");
        }

        private static long Solve01(List<State> states)
        {
            // convert lights to bitmask
            // convert toggles to bitmask

            long presses = 0;
            foreach (State state in states)
            {
                int lightMask = 0;
                for (int i = 0; i < state.lights.Length; i++)
                {
                    if (state.lights[i] == '#')
                    {
                        lightMask |= (1 << i);
                    }
                }

                int[] toggleMasks = new int[state.toggles.Count];
                for (int i = 0; i < state.toggles.Count; i++)
                {
                    int toggleMask = 0;
                    for (int j = 0; j < state.toggles[i].Length; j++)
                    {
                        int pos = state.toggles[i][j];
                        toggleMask |= (1 << pos);
                    }
                    toggleMasks[i] = toggleMask;
                }

                presses += Solve(lightMask, toggleMasks);
            }

            return presses;
        }

        private static long minPresses = long.MaxValue;

        private static void Solve_Recursive(int[] originalJoltages, List<int[]> toggles, int index, int presses)
        {
            bool solved = true;
            for (int i = 0; i < originalJoltages.Length; i++)
            {
                if (originalJoltages[i] != 0)
                {
                    solved = false;
                    break;
                }
            }

            if (solved)
            {
                if (presses < minPresses)
                {
                    minPresses = presses;
                }
                return;
            }

            // recursive
            if (index >= toggles.Count)
            {
                return;
            }

            // try current index intil goes negative
            int[] joltages = new int[originalJoltages.Length];
            Array.Copy(originalJoltages, joltages, originalJoltages.Length);

            while (true)
            {
                Solve_Recursive(joltages, toggles, index + 1, presses);
                
                // press current index
                for (int j = 0; j < toggles[index].Length; j++)
                {
                    int toggleIndex = toggles[index][j];
                    joltages[toggleIndex] -= 1;
                    if (joltages[toggleIndex] < 0)
                    {
                        return;
                    }
                }

                presses++;
            }
        }

        private static long Solve_Recursive(int[] joltages, List<int[]> toggles)
        {
            minPresses = long.MaxValue;
            Solve_Recursive(joltages, toggles, 0, 0);
            return minPresses;
        }

        private static long Solve(int[] joltages, List<int[]> toggles)
        {
            // Create solver (LP)
            Solver solver = Solver.CreateSolver("SCIP");    // CBC or "SCIP" for MIP. GLOP does not support integer variables.

            if (solver == null)
            {
                throw new Exception("Could not create solver.");
            }

            // Variables: ai >= 0
            Variable[] variables = new Variable[toggles.Count];
            for (int i = 0; i < toggles.Count; i++)
            {
                variables[i] = solver.MakeIntVar(0.0, double.PositiveInfinity, "a" + (i + 1));
            }

            // convert toggles to coefficient matrix
            int[,] coefficients = new int[joltages.Length, toggles.Count];
            for (int i = 0; i < toggles.Count; i++)
            {
                for (int j = 0; j < toggles[i].Length; j++)
                {
                    int index = toggles[i][j];
                    coefficients[index, i] += 1;
                }
            }

            // Constraints
            for (int i = 0; i < joltages.Length; i++)
            {
                // Each toggle gives a constraint
                LinearExpr toggleExpr = new LinearExpr();
                for (int j = 0; j < variables.Length; j++)
                {
                    toggleExpr += coefficients[i, j] * variables[j];
                }

                solver.Add(toggleExpr == joltages[i]);
            }

            // Objective: minimize sum
            Objective objective = solver.Objective();
            for (int i = 0; i < variables.Length; i++)
            {
                objective.SetCoefficient(variables[i], 1);
            }

            objective.SetMinimization();

            // Solve
            Solver.ResultStatus resultStatus = solver.Solve();

            // Output
            if (resultStatus == Solver.ResultStatus.OPTIMAL)
            {
                return (long)objective.Value();
            }
            else
            {
                throw new Exception("No optimal solution found.");
            }

            throw new Exception("Not implemented");
        }

        private static long Solve02(List<State> states)
        {
            long presses = 0;
            for (int i = 0; i < states.Count; i++)
            {
                State state = states[i];
                presses += Solve(state.joltages, state.toggles);
            }

            return presses;
        }

        public static void Run()
        {
            // StreamReader sr = new StreamReader("Day10/test10.txt");
            StreamReader sr = new StreamReader("Day10/data10.txt");
            string s = null;

            List<State> states = new List<State>();

            while ((s = sr.ReadLine()) != null)
            {
                string[] parts = s.Split(' ');

                string lights = parts[0].Trim('[').Trim(']');
                List<int[]> toggles = new List<int[]>();

                for (int i = 1; i < parts.Length - 1; i++)
                {
                    string toggleStr = parts[i].Trim('(').Trim(')');
                    string[] toggleParts = toggleStr.Split(',');
                    int[] toggleArray = new int[toggleParts.Length];
                    for (int j = 0; j < toggleParts.Length; j++)
                    {
                        toggleArray[j] = int.Parse(toggleParts[j]);
                    }
                    toggles.Add(toggleArray);
                }

                string joltagesStr = parts[parts.Length - 1].Trim('{').Trim('}');
                string[] joltagesParts = joltagesStr.Split(',');
                int[] joltages = new int[joltagesParts.Length];
                for (int j = 0; j < joltagesParts.Length; j++)
                {
                    joltages[j] = int.Parse(joltagesParts[j]);
                }

                State state = new State(lights, toggles, joltages);
                states.Add(state);
            }

            sr.Close();

            long started = Environment.TickCount;

            // long solve01 = Solve01(states);
            // Console.WriteLine(solve01);

            long solve02 = Solve02(states);
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*
375

Elapsed: 11 ms

15331

Elapsed: 74 ms (GLOP solver)    -> WRONG


15377

Elapsed: 257 ms (CBC solver)

15377

Elapsed: 305 ms (SCIP solver)
*/