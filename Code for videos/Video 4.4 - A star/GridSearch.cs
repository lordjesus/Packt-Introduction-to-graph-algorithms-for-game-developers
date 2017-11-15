using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_4._4___A_star
{
    class GridSearch
    {
        public struct SearchResult
        {
            public List<Grid.Point> Path { get; set; }
            public List<Grid.Point> Visited { get; set; }
        }

        class FakePriorityQueue<T>
        {
            private Dictionary<T, float> _list;

            public bool Empty { get { return _list.Count == 0; } }

            public FakePriorityQueue()
            {
                _list = new Dictionary<T, float>();
            }

            public void Enqueue(T element, float priority)
            {
                _list[element] = priority;
            }

            public T Dequeue()
            {
                if (_list.Count == 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                T best = _list.Keys.First();
                float priority = _list[best];

                foreach (T candidate in _list.Keys)
                {
                    if (_list[candidate] < priority)
                    {
                        best = candidate;
                        priority = _list[candidate];
                    }
                }

                _list.Remove(best);
                return best;
            }
        }

        public static SearchResult Dijkstra(Grid grid, Grid.Point startPos, Grid.Point endPos)
        {
            FakePriorityQueue<Grid.Point> queue = new FakePriorityQueue<Grid.Point>();
            Dictionary<Grid.Point, float> distanceMap = new Dictionary<Grid.Point, float>();
            Dictionary<Grid.Point, Grid.Point> visitedMap = new Dictionary<Grid.Point, Grid.Point>();

            queue.Enqueue(startPos, 0);

            distanceMap.Add(startPos, 0);
            visitedMap.Add(startPos, null);

            while (!queue.Empty)
            {
                Grid.Point current = queue.Dequeue();
                if (current.Equals(endPos))
                {
                    return new SearchResult
                    {
                        Path = GeneratePath(visitedMap, current),
                        Visited = new List<Grid.Point>(visitedMap.Keys)
                    };
                }
                foreach (Grid.Point adj in grid.GetAdjacentCells(current))
                {
                    float newDist = distanceMap[current] + grid.GetCostOfEnteringCell(adj);
                    if (!distanceMap.ContainsKey(adj) || newDist < distanceMap[adj])
                    {
                        distanceMap[adj] = newDist;
                        visitedMap[adj] = current;
                        queue.Enqueue(adj, newDist);
                    }
                }
            }
            return new SearchResult();
        }

        public static SearchResult AStarSearch(Grid grid, Grid.Point startPos, Grid.Point endPos)
        {
            FakePriorityQueue<Grid.Point> queue = new FakePriorityQueue<Grid.Point>();
            Dictionary<Grid.Point, float> distanceMap = new Dictionary<Grid.Point, float>();
            Dictionary<Grid.Point, Grid.Point> visitedMap = new Dictionary<Grid.Point, Grid.Point>();

            queue.Enqueue(startPos, 0);
            distanceMap[startPos] = 0;
            visitedMap[startPos] = null;

            while (!queue.Empty)
            {
                Grid.Point current = queue.Dequeue();
                if (current.Equals(endPos))
                {
                    return new SearchResult
                    {
                        Path = GeneratePath(visitedMap, current),
                        Visited = new List<Grid.Point>(visitedMap.Keys)
                    };
                }
                foreach (Grid.Point neighbour in grid.GetAdjacentCells(current))
                {
                    float newCost = distanceMap[current] + grid.GetCostOfEnteringCell(neighbour);
                    if (!distanceMap.ContainsKey(neighbour) || newCost < distanceMap[neighbour])
                    {
                        distanceMap[neighbour] = newCost;

                        float priority = newCost + Heuristic(endPos, neighbour);
                        queue.Enqueue(neighbour, priority);

                        visitedMap[neighbour] = current;
                    }
                }
            }
            return new SearchResult();
        }

        private static float Heuristic(Grid.Point endPos, Grid.Point point, bool useManhattan = true)
        {
            if (useManhattan)
            {
                return Manhattan(endPos, point);
            }
            else
            {
                return Euclidean(endPos, point);
            }
        }

        private static float Manhattan(Grid.Point A, Grid.Point B)
        {
            return Math.Abs(A.X - B.X) + Math.Abs(A.Y - B.Y);
        }

        private static float Euclidean(Grid.Point A, Grid.Point B)
        {
            return (float)Math.Sqrt(((A.X - B.X) * (A.X - B.X)) + ((A.Y - B.Y) * (A.Y - B.Y)));
        }

        public static List<Grid.Point> GeneratePath(Dictionary<Grid.Point, Grid.Point> parentMap, Grid.Point endState)
        {
            List<Grid.Point> path = new List<Grid.Point>();
            Grid.Point parent = endState;
            while (parent != null && parentMap.ContainsKey(parent))
            {
                path.Add(parent);
                parent = parentMap[parent];
            }
            return path;
        }
    }
}
