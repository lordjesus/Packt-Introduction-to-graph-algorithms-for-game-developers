using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_3._5___Grid
{
    class GridSearch
    {
        public static List<Grid.Point> DepthFirstSearch(Grid grid, Grid.Point startPos, Grid.Point endPos)
        {
            if (startPos.Equals(endPos))
            {
                return new List<Grid.Point>() { startPos };
            }

            Dictionary<Grid.Point, Grid.Point> visitedMap = new Dictionary<Grid.Point, Grid.Point>();

            Stack<Grid.Point> stack = new Stack<Grid.Point>();
            stack.Push(startPos);

            while (stack.Count > 0)
            {
                Grid.Point node = stack.Pop();

                foreach (Grid.Point adj in grid.GetAdjacentCells(node))
                {
                    if (!visitedMap.ContainsKey(adj))
                    {
                        visitedMap.Add(adj, node);
                        stack.Push(adj);

                        if (adj.Equals(endPos))
                        {
                            return GeneratePath(visitedMap, adj);
                        }
                    }
                }
                if (!visitedMap.ContainsKey(node))
                {
                    visitedMap.Add(node, null);
                }
            }
            return null;
        }

        public static List<Grid.Point> BreadthFirstSearch(Grid grid, Grid.Point startPos, Grid.Point endPos)
        {
            if (startPos.Equals(endPos))
            {
                return new List<Grid.Point>() { startPos };
            }

            Dictionary<Grid.Point, Grid.Point> visitedMap = new Dictionary<Grid.Point, Grid.Point>();

            Queue<Grid.Point> queue = new Queue<Grid.Point>();
            queue.Enqueue(startPos);

            while (queue.Count > 0)
            {
                Grid.Point node = queue.Dequeue();

                foreach (Grid.Point adj in grid.GetAdjacentCells(node))
                {
                    if (!visitedMap.ContainsKey(adj))
                    {
                        visitedMap.Add(adj, node);
                        queue.Enqueue(adj);

                        if (adj.Equals(endPos))
                        {
                            return GeneratePath(visitedMap, adj);
                        }
                    }
                }
                if (!visitedMap.ContainsKey(node))
                {
                    visitedMap.Add(node, null);
                }
            }
            return null;
        }

        public static List<Grid.Point> Dijkstra(Grid grid, Grid.Point startPos, Grid.Point endPos)
        {
            List<Grid.Point> unfinishedVertices = new List<Grid.Point>();
            Dictionary<Grid.Point, float> distanceMap = new Dictionary<Grid.Point, float>();
            Dictionary<Grid.Point, Grid.Point> visitedMap = new Dictionary<Grid.Point, Grid.Point>();

            unfinishedVertices.Add(startPos);

            distanceMap.Add(startPos, 0);
            visitedMap.Add(startPos, null);

            while (unfinishedVertices.Count > 0)
            {
                Grid.Point vertex = GetClosestVertex(unfinishedVertices, distanceMap);
                unfinishedVertices.Remove(vertex);
                if (vertex.Equals(endPos))
                {
                    return GeneratePath(visitedMap, vertex);
                }
                foreach (Grid.Point adj in grid.GetAdjacentCells(vertex))
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
            return null;
        }

        private static Grid.Point GetClosestVertex(List<Grid.Point> list, Dictionary<Grid.Point, float> distanceMap)
        {
            Grid.Point candidate = list[0];
            foreach (Grid.Point vertex in list)
            {
                if (distanceMap[vertex] < distanceMap[candidate])
                {
                    candidate = vertex;
                }
            }
            return candidate;
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
