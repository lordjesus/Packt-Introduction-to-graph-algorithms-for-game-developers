using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridSearch {

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

    public static SearchResult DijkstraGeneral(Grid grid, Point startPos)
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
            
            foreach (Point adj in grid.GetAdjacentCells(current))
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
        return new SearchResult()
        {
            VisitedMap = visitedMap,
            DistanceMap = distanceMap
        };
    }

    public static SearchResult DijkstraWithCost(Grid grid, Point startPos, Point endPos, Dictionary<Point, float> costMap)
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
            foreach (Point adj in grid.GetAdjacentCells(current))
            {
                float cost = costMap.ContainsKey(adj) ? costMap[adj] : 1;
                float newDist = distanceMap[current] + cost;
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

    public static SearchResult AStarSearchWithCost(Grid grid, Point startPos, Point endPos, Dictionary<Point, float> costMap)
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
                float cost = costMap.ContainsKey(neighbour) ? costMap[neighbour] : 1;
                float newCost = costSoFar[current] + cost;
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

    public static SearchResult DijkstraPriority(Grid grid, Point startPos, Point endPos)
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
            foreach (Point adj in grid.GetAdjacentCells(current))
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

    public static SearchResult Dijkstra(Grid grid, Point startPos, Point endPos)
    {
        List<Point> unfinishedVertices = new List<Point>();
        Dictionary<Point, float> distanceMap = new Dictionary<Point, float>();
        Dictionary<Point, Point> visitedMap = new Dictionary<Point, Point>();

        unfinishedVertices.Add(startPos);

        distanceMap.Add(startPos, 0);
        visitedMap.Add(startPos, null);

        while (unfinishedVertices.Count > 0)
        {
            Point vertex = GetClosestVertex(unfinishedVertices, distanceMap);
            unfinishedVertices.Remove(vertex);
            if (vertex.Equals(endPos))
            {
                return new SearchResult
                {
                    Path = GeneratePath(visitedMap, vertex),
                    Visited = new List<Point>(visitedMap.Keys)
                };
            }
            foreach (Point adj in grid.GetAdjacentCells(vertex))
            {
                if (!visitedMap.ContainsKey(adj))
                {
                    unfinishedVertices.Add(adj);
                }
                float adjDist = distanceMap.ContainsKey(adj) ? distanceMap[adj] : int.MaxValue;
                float vDist = distanceMap.ContainsKey(vertex) ? distanceMap[vertex] : int.MaxValue;
                if (adjDist > vDist + grid.GetCostOfEnteringCell(adj))
                {
                    if (distanceMap.ContainsKey(adj))
                    {
                        distanceMap[adj] = vDist + grid.GetCostOfEnteringCell(adj);
                    }
                    else
                    {
                        distanceMap.Add(adj, vDist + grid.GetCostOfEnteringCell(adj));
                    }
                    if (visitedMap.ContainsKey(adj))
                    {
                        visitedMap[adj] = vertex;
                    }
                    else
                    {
                        visitedMap.Add(adj, vertex);
                    }
                }
            }
        }
        return new SearchResult();
    }

    private static Point GetClosestVertex(List<Point> list, Dictionary<Point, float> distanceMap)
    {
        Point candidate = list[0];
        foreach (Point vertex in list)
        {
            if (distanceMap[vertex] < distanceMap[candidate])
            {
                candidate = vertex;
            }
        }
        return candidate;
    }

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

    public static SearchResult BestFirstSearch2(Grid grid, Point startPos, Point endPos)
    {
        List<Point> openSet = new List<Point>();
        Dictionary<Point, float> distanceMap = new Dictionary<Point, float>();
        Dictionary<Point, float> priorityMap = new Dictionary<Point, float>();
        Dictionary<Point, Point> visitedMap = new Dictionary<Point, Point>();

        openSet.Add(startPos);
        priorityMap.Add(startPos, 0);
        distanceMap.Add(startPos, 0);
        visitedMap.Add(startPos, null);

        while (openSet.Count > 0)
        {
            Point current = GetClosestVertex(openSet, priorityMap);
            openSet.Remove(current);
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
                    openSet.Add(neighbour);
                    visitedMap.Add(neighbour, current);
                    priorityMap.Add(neighbour, priority);
                    distanceMap.Add(neighbour, distanceMap[current] + grid.GetCostOfEnteringCell(neighbour));
                }
            }
        }
        return new SearchResult();
    }

    public static SearchResult BiAStarSearch(Grid grid, Point startPos, Point endPos)
    {
        // True if opened from the start queue, false if opened by the end queue, not opened if not exists
        Dictionary<Point, bool> openedBy = new Dictionary<Point, bool>();

        FakePriorityQueue<Point> startQueue = new FakePriorityQueue<Point>();
        FakePriorityQueue<Point> endQueue = new FakePriorityQueue<Point>();

        Dictionary<Point, float> startCostSoFar = new Dictionary<Point, float>();
        Dictionary<Point, Point> startCameFrom = new Dictionary<Point, Point>();
        Dictionary<Point, float> endCostSoFar = new Dictionary<Point, float>();
        Dictionary<Point, Point> endCameFrom = new Dictionary<Point, Point>();

        startQueue.Enqueue(startPos, 0);
        startCostSoFar[startPos] = 0;
        startCameFrom[startPos] = null;
        openedBy[startPos] = true;

        endQueue.Enqueue(endPos, 0);
        endCostSoFar[endPos] = 0;
        endCameFrom[endPos] = null;
        openedBy[endPos] = false;

        while (!startQueue.Empty && !endQueue.Empty)
        {
            // From start
            Point current = startQueue.Dequeue();
            if (openedBy.ContainsKey(current) && openedBy[current] == false)
            {
                // Found goal or the frontier from the end queue
                // Return solution
                List<Point> startPath = GeneratePath(startCameFrom, current);
                List<Point> endPath = GeneratePath(endCameFrom, current);

                List<Point> allPath = new List<Point>(startPath);
                allPath.AddRange(endPath);

                List<Point> allVisited = new List<Point>(startCameFrom.Keys);
                allVisited.AddRange(endCameFrom.Keys);

                return new SearchResult
                {
                    Path = allPath,
                    Visited = allVisited
                };
            }
            foreach (Point neighbour in grid.GetAdjacentCells(current))
            {
                float newCost = startCostSoFar[current] + grid.GetCostOfEnteringCell(neighbour);
                if (!startCostSoFar.ContainsKey(neighbour) || newCost < startCostSoFar[neighbour])
                {
                    startCostSoFar[neighbour] = newCost;
                    openedBy[neighbour] = true;

                    float priority = newCost + Heuristic(endPos, neighbour);
                    startQueue.Enqueue(neighbour, priority);

                    startCameFrom[neighbour] = current;
                }
            }
            // From end
            current = endQueue.Dequeue();
            if (openedBy.ContainsKey(current) && openedBy[current] == true)
            {
                // Found goal or the frontier from the start queue
                // Return solution
                List<Point> startPath = GeneratePath(startCameFrom, current);
                List<Point> endPath = GeneratePath(endCameFrom, current);

                List<Point> allPath = new List<Point>(startPath);
                allPath.AddRange(endPath);

                List<Point> allVisited = new List<Point>(startCameFrom.Keys);
                allVisited.AddRange(endCameFrom.Keys);

                return new SearchResult
                {
                    Path = allPath,
                    Visited = allVisited
                };
            }
            foreach (Point neighbour in grid.GetAdjacentCells(current))
            {
                float newCost = endCostSoFar[current] + grid.GetCostOfEnteringCell(neighbour);
                if (!endCostSoFar.ContainsKey(neighbour) || newCost < endCostSoFar[neighbour])
                {
                    endCostSoFar[neighbour] = newCost;
                    openedBy[neighbour] = false;

                    float priority = newCost + Heuristic(startPos, neighbour);
                    endQueue.Enqueue(neighbour, priority);

                    endCameFrom[neighbour] = current;
                }
            }
        }    
        return new SearchResult();
    }

    public static SearchResult AStarSearchPriority(Grid grid, Point startPos, Point endPos)
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

    public static SearchResult AStarSearch(Grid grid, Point startPos, Point endPos)
    {
        List<Point> openSet = new List<Point>();
        Dictionary<Point, float> costSoFar = new Dictionary<Point, float>();
        Dictionary<Point, float> priorityMap = new Dictionary<Point, float>();
        Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();

        openSet.Add(startPos);
        priorityMap.Add(startPos, 0);
        costSoFar.Add(startPos, 0);
        cameFrom.Add(startPos, null);

        while (openSet.Count > 0)
        {
            Point current = GetClosestVertex(openSet, priorityMap);
            openSet.Remove(current);
            if (current.Equals(endPos))
            {
                return new SearchResult
                {
                    Path = GeneratePath(cameFrom, current),
                    Visited = new List<Point>(cameFrom.Keys)
                };
            }

            foreach(Point neighbour in grid.GetAdjacentCells(current))
            {
                float newCost = costSoFar[current] + grid.GetCostOfEnteringCell(neighbour);
                if (!costSoFar.ContainsKey(neighbour) || newCost < costSoFar[neighbour])
                {
                    costSoFar[neighbour] = newCost;

                    float priority = newCost + Heuristic(endPos, neighbour);
                    openSet.Add(neighbour);
                    priorityMap[neighbour] = priority;

                    cameFrom[neighbour] = current;                    
                }
            }
        }
        return new SearchResult();
    }

    private static float Heuristic(Point endPos, Point point)
    {
        // Manhattan distance
        return Math.Abs(endPos.X - point.X) + Math.Abs(endPos.Y - point.Y);

        //return Euclidean(endPos, point);
    }

    private static float Euclidean(Point A, Point B)
    {
        return (float)Math.Sqrt(Math.Abs(A.X - B.X) * Math.Abs(A.X - B.X) + Math.Abs(A.Y - B.Y) * Math.Abs(A.Y - B.Y));
    }

    public static SearchResult DepthFirstSearch(Grid grid, Point startPos, Point endPos)
    {
        if (startPos.Equals(endPos))
        {
            return new SearchResult();            
        }

        Dictionary<Point, Point> visitedMap = new Dictionary<Point, Point>();

        Stack<Point> stack = new Stack<Point>();
        stack.Push(startPos);

        while (stack.Count > 0)
        {
            Point node = stack.Pop();

            foreach (Point adj in grid.GetAdjacentCells(node))
            {
                if (!visitedMap.ContainsKey(adj))
                {
                    visitedMap.Add(adj, node);
                    stack.Push(adj);

                    if (adj.Equals(endPos))
                    {
                        return new SearchResult
                        {
                            Path = GeneratePath(visitedMap, adj),
                            Visited = new List<Point>(visitedMap.Keys)
                        };
                    }
                }
            }
            if (!visitedMap.ContainsKey(node))
            {
                visitedMap.Add(node, null);
            }
        }
        return new SearchResult();
    }
	
    public static SearchResult BreadthFirstSearch(Grid grid, Point startPos, Point endPos)
    {
        if (startPos.Equals(endPos))
        {
            return new SearchResult();
        }

        Dictionary<Point, Point> visitedMap = new Dictionary<Point, Point>();

        Queue<Point> queue = new Queue<Point>();
        queue.Enqueue(startPos);
        
        while (queue.Count > 0)
        {
            Point node = queue.Dequeue();

            foreach (Point adj in grid.GetAdjacentCells(node))
            {
                if (!visitedMap.ContainsKey(adj))
                {
                    visitedMap.Add(adj, node);
                    queue.Enqueue(adj);

                    if (adj.Equals(endPos))
                    {
                        return new SearchResult
                        {
                            Path = GeneratePath(visitedMap, adj),
                            Visited = new List<Point>(visitedMap.Keys)
                        };
                    }
                }
            }
            if (!visitedMap.ContainsKey(node))
            {
                visitedMap.Add(node, null);
            }
        }
        return new SearchResult();
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
