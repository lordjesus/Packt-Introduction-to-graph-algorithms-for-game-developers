using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_5._3___Target
{
    public struct SearchResult
    {
        public List<Point> Path { get; set; }
        public List<Point> Visited { get; set; }
        public Dictionary<Point, Point> VisitedMap { get; set; }
        public Dictionary<Point, float> DistanceMap { get; set; }
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

    public class GridSearch
    {

        public static SearchResult BestFirstSearch(Grid grid, Point startPos, Point endPos)
        {
            FakePriorityQueue<Point> queue = new FakePriorityQueue<Point>();
            Dictionary<Point, float> distanceMap = new Dictionary<Point, float>();
            Dictionary<Point, Point> visitedMap = new Dictionary<Point, Point>();

            queue.Enqueue(startPos, 0);
            distanceMap.Add(startPos, 0);
            visitedMap.Add(startPos, null);

            while (!queue.Empty)
            {
                Point current = queue.Dequeue();
                if (current.Equals(endPos))
                {
                    return new SearchResult
                    {
                        Path = GeneratePath(visitedMap, current),
                        Visited = new List<Point>(visitedMap.Keys)
                    };
                }

                foreach (Point neighbour in grid.GetAdjacentCells(current))
                {
                    if (!visitedMap.ContainsKey(neighbour))
                    {
                        float priority = Heuristic(endPos, neighbour);
                        queue.Enqueue(neighbour, priority);
                        visitedMap.Add(neighbour, current);
                        distanceMap.Add(neighbour, distanceMap[current] + grid.GetCostOfEnteringCell(neighbour));
                    }
                }
            }
            return new SearchResult();
        }

        public static SearchResult AStarSearch(Grid grid, Point startPos, Point endPos)
        {
            FakePriorityQueue<Point> queue = new FakePriorityQueue<Point>();
            Dictionary<Point, float> costSoFar = new Dictionary<Point, float>();
            Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();

            queue.Enqueue(startPos, 0);
            costSoFar[startPos] = 0;
            cameFrom[startPos] = null;

            while (!queue.Empty)
            {
                Point current = queue.Dequeue();
                if (current.Equals(endPos))
                {
                    return new SearchResult
                    {
                        Path = GeneratePath(cameFrom, current),
                        Visited = new List<Point>(cameFrom.Keys)
                    };
                }
                foreach (Point neighbour in grid.GetAdjacentCells(current))
                {
                    float newCost = costSoFar[current] + grid.GetCostOfEnteringCell(neighbour);
                    if (!costSoFar.ContainsKey(neighbour) || newCost < costSoFar[neighbour])
                    {
                        costSoFar[neighbour] = newCost;

                        float priority = newCost + Heuristic(endPos, neighbour);
                        queue.Enqueue(neighbour, priority);

                        cameFrom[neighbour] = current;
                    }
                }
            }
            return new SearchResult();
        }

        private static float Heuristic(Point endPos, Point point, bool useManhattan = true)
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

        private static float Manhattan(Point A, Point B)
        {
            return Math.Abs(A.X - B.X) + Math.Abs(A.Y - B.Y);
        }

        private static float Euclidean(Point A, Point B)
        {
            return (float)Math.Sqrt(((A.X - B.X) * (A.X - B.X)) + ((A.Y - B.Y) * (A.Y - B.Y)));
        }

        public static List<Point> GeneratePath(Dictionary<Point, Point> parentMap, Point endState)
        {
            List<Point> path = new List<Point>();
            Point parent = endState;
            while (parent != null && parentMap.ContainsKey(parent))
            {
                path.Add(parent);
                parent = parentMap[parent];
            }
            return path;
        }
    }
}
