using System;
using System.Collections.Generic;
using System.Linq;

namespace Video_4._5___Bi_dir
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

        public static SearchResult BiDirectionalAStarSearch(Grid grid, Grid.Point startPos, Grid.Point endPos)
        {
            // True if opened from the start queue, false if opened by the end queue, not opened if not exists
            Dictionary<Grid.Point, bool> openedBy = new Dictionary<Grid.Point, bool>();

            FakePriorityQueue<Grid.Point> startQueue = new FakePriorityQueue<Grid.Point>();
            FakePriorityQueue<Grid.Point> endQueue = new FakePriorityQueue<Grid.Point>();

            Dictionary<Grid.Point, float> startDistanceMap = new Dictionary<Grid.Point, float>();
            Dictionary<Grid.Point, Grid.Point> startVisitedMap = new Dictionary<Grid.Point, Grid.Point>();
            Dictionary<Grid.Point, float> endDistanceMap = new Dictionary<Grid.Point, float>();
            Dictionary<Grid.Point, Grid.Point> endVisitedMap = new Dictionary<Grid.Point, Grid.Point>();

            startQueue.Enqueue(startPos, 0);
            startDistanceMap[startPos] = 0;
            startVisitedMap[startPos] = null;
            openedBy[startPos] = true;

            endQueue.Enqueue(endPos, 0);
            endDistanceMap[endPos] = 0;
            endVisitedMap[endPos] = null;
            openedBy[endPos] = false;

            while (!startQueue.Empty && !endQueue.Empty)
            {
                // From start
                Grid.Point current = startQueue.Dequeue();
                if (openedBy.ContainsKey(current) && openedBy[current] == false)
                {
                    // Found goal or the frontier from the end queue
                    // Return solution
                    List<Grid.Point> startPath = GeneratePath(startVisitedMap, current);
                    List<Grid.Point> endPath = GeneratePath(endVisitedMap, current);

                    List<Grid.Point> allPath = new List<Grid.Point>(startPath);
                    allPath.AddRange(endPath);

                    List<Grid.Point> allVisited = new List<Grid.Point>(startVisitedMap.Keys);
                    allVisited.AddRange(endVisitedMap.Keys);

                    return new SearchResult
                    {
                        Path = allPath,
                        Visited = allVisited
                    };
                }
                foreach (Grid.Point neighbour in grid.GetAdjacentCells(current))
                {
                    float newCost = startDistanceMap[current] + grid.GetCostOfEnteringCell(neighbour);
                    if (!startDistanceMap.ContainsKey(neighbour) || newCost < startDistanceMap[neighbour])
                    {
                        startDistanceMap[neighbour] = newCost;
                        openedBy[neighbour] = true;

                        float priority = newCost + Heuristic(endPos, neighbour);
                        startQueue.Enqueue(neighbour, priority);

                        startVisitedMap[neighbour] = current;
                    }
                }
                // From end
                current = endQueue.Dequeue();
                if (openedBy.ContainsKey(current) && openedBy[current] == true)
                {
                    // Found goal or the frontier from the start queue
                    // Return solution
                    List<Grid.Point> startPath = GeneratePath(startVisitedMap, current);
                    List<Grid.Point> endPath = GeneratePath(endVisitedMap, current);

                    List<Grid.Point> allPath = new List<Grid.Point>(startPath);
                    allPath.AddRange(endPath);

                    List<Grid.Point> allVisited = new List<Grid.Point>(startVisitedMap.Keys);
                    allVisited.AddRange(endVisitedMap.Keys);

                    return new SearchResult
                    {
                        Path = allPath,
                        Visited = allVisited
                    };
                }
                foreach (Grid.Point neighbour in grid.GetAdjacentCells(current))
                {
                    float newCost = endDistanceMap[current] + grid.GetCostOfEnteringCell(neighbour);
                    if (!endDistanceMap.ContainsKey(neighbour) || newCost < endDistanceMap[neighbour])
                    {
                        endDistanceMap[neighbour] = newCost;
                        openedBy[neighbour] = false;

                        float priority = newCost + Heuristic(startPos, neighbour);
                        endQueue.Enqueue(neighbour, priority);

                        endVisitedMap[neighbour] = current;
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
