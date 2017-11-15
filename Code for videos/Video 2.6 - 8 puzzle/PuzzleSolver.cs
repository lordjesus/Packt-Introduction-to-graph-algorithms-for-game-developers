using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_2._6___8_puzzle
{
    class PuzzleSolver
    {
        #region Utility_Functions
        public static bool IsSolvable(string state)
        {
            int inversions = 0;
            for (int i = 0; i < 9; i++)
            {
                if (state[i] == '*')
                {
                    continue;
                }
                for (int j = i + 1; j < 9; j++)
                {
                    if (state[j] == '*')
                    {
                        continue;
                    }
                    if (state[i] > state[j])
                    {
                        inversions++;
                    }
                }
            }

            return inversions % 2 == 0;
        }

        public static string GenerateRandomSolvableState()
        {
            Random rand = new Random();
            string state = new string("12345678*".ToCharArray().OrderBy(a => rand.Next()).ToArray());
            while (!IsSolvable(state))
            {
                state = new string("12345678*".ToCharArray().OrderBy(a => rand.Next()).ToArray());
            }
            return state;
        }        

        public static bool IsWinCondition(string state)
        {
            return state.Equals("12345678*");
        }

        public static List<string> GetNeighbourStates(string state)
        {
            // string state: 1234*5678
            // represents: 1 2 3
            //             4 * 5
            //             6 7 8

            int emptyIndex = state.IndexOf("*");

            bool canMoveLeft = emptyIndex % 3 > 0;
            bool canMoveRight = emptyIndex % 3 < 2;
            bool canMoveUp = emptyIndex / 3 > 0;
            bool canMoveDown = emptyIndex / 3 < 2;

            List<string> neighbours = new List<string>();

            if (canMoveLeft)
            {
                StringBuilder sb = new StringBuilder(state);
                char newChar = sb[emptyIndex - 1];
                sb[emptyIndex] = newChar;
                sb[emptyIndex - 1] = '*';
                neighbours.Add(sb.ToString());
            }
            if (canMoveRight)
            {
                StringBuilder sb = new StringBuilder(state);
                char newChar = sb[emptyIndex + 1];
                sb[emptyIndex] = newChar;
                sb[emptyIndex + 1] = '*';
                neighbours.Add(sb.ToString());
            }
            if (canMoveUp)
            {
                StringBuilder sb = new StringBuilder(state);
                char newChar = sb[emptyIndex - 3];
                sb[emptyIndex] = newChar;
                sb[emptyIndex - 3] = '*';
                neighbours.Add(sb.ToString());
            }
            if (canMoveDown)
            {
                StringBuilder sb = new StringBuilder(state);
                char newChar = sb[emptyIndex + 3];
                sb[emptyIndex] = newChar;
                sb[emptyIndex + 3] = '*';
                neighbours.Add(sb.ToString());
            }

            return neighbours;
        }

        public static List<string> GeneratePath(Dictionary<string, string> parentMap, string endState)
        {
            List<string> path = new List<string>();
            string parent = endState;
            while (parentMap.ContainsKey(parent))
            {
                path.Add(parent);
                parent = parentMap[parent];
            }
            return path;
        }

        public static string PrintableState(string state)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < state.Length; i++)
            {
                if (i % 3 == 0)
                {
                    sb.AppendLine();
                }
                sb.Append(state[i]).Append(" ");                
            }
            sb.AppendLine();
            return sb.ToString();
        }
        #endregion

        #region Solvers
        public static List<string> BreadthFirstSearch(string initialState)
        {
            if (IsWinCondition(initialState))
            {
                return new List<string> { initialState };
            }

            // Keeps track of visited vertices, 
            // and parents for each vertex
            Dictionary<string, string> visitedMap = new Dictionary<string, string>();

            Queue<string> queue = new Queue<string>();
            queue.Enqueue(initialState);

            while (queue.Count > 0)
            {
                string node = queue.Dequeue();

                foreach (string neighbour in GetNeighbourStates(node))
                {
                    if (!visitedMap.ContainsKey(neighbour))
                    {
                        visitedMap.Add(neighbour, node);
                        queue.Enqueue(neighbour);

                        if (IsWinCondition(neighbour))
                        {
                            return GeneratePath(visitedMap, neighbour);
                        }
                    }
                }
                if (!visitedMap.ContainsKey(node))
                {
                    visitedMap.Add(node, "");
                }
            }
            // No solution found
            return null;
        }

        public static List<string> DepthFirstSearch(string initialState)
        {
            if (IsWinCondition(initialState))
            {
                return new List<string> { initialState };
            }

            // Keeps track of visited vertices, 
            // and parents for each vertex
            Dictionary<string, string> visitedMap = new Dictionary<string, string>();

            Stack<string> stack = new Stack<string>();
            stack.Push(initialState);

            while (stack.Count > 0)
            {
                string node = stack.Pop();

                foreach (string neighbour in GetNeighbourStates(node))
                {
                    if (!visitedMap.ContainsKey(neighbour))
                    {
                        visitedMap.Add(neighbour, node);
                        stack.Push(neighbour);

                        if (IsWinCondition(neighbour))
                        {                          
                            return GeneratePath(visitedMap, neighbour);
                        }
                    }
                }
                if (!visitedMap.ContainsKey(node))
                {
                    visitedMap.Add(node, "");
                }
            }           
            // No solution found
            return null;
        }       
        #endregion              
    }
}
