using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_2._5___DFS
{
    class SearchAlgorithms
    {
        public static void DepthFirstSearch<T>(Graph<T> graph, Vertex<T> sourceVertex, 
            bool reverseNeighbours = false)
        {
            foreach (Vertex<T> vertex in graph.Vertices)
            {
                vertex.Parent = null;
                vertex.Distance = 0;
                vertex.Visited = false;
            }

            Stack<Vertex<T>> stack = new Stack<Vertex<T>>();
            stack.Push(sourceVertex);

            while (stack.Count > 0)
            {
                Vertex<T> vertex = stack.Pop();

                List<Vertex<T>> neighbours = graph.GetAdjacentVertices(vertex);
                if (reverseNeighbours)
                {
                    neighbours.Reverse();
                }
                foreach (Vertex<T> neighbour in neighbours)
                {
                    if (!neighbour.Visited)
                    {
                        neighbour.Parent = vertex;
                        neighbour.Distance = vertex.Distance + graph.GetEdgeWeight(vertex, neighbour);
                        neighbour.Visited = true;

                        stack.Push(neighbour);
                    }
                }
                vertex.Visited = true;
            }
        }

        public static List<Vertex<T>> DepthFirstSearchWithGoal<T>(
            Graph<T> graph, Vertex<T> sourceVertex, Vertex<T> goalVertex, bool reverseNeighbours = false)
        {
            if (sourceVertex.Equals(goalVertex))
            {
                return new List<Vertex<T>> { sourceVertex };
            }
            foreach (Vertex<T> vertex in graph.Vertices)
            {
                vertex.Parent = null;
                vertex.Distance = 0;
                vertex.Visited = false;
            }

            Stack<Vertex<T>> stack = new Stack<Vertex<T>>();
            stack.Push(sourceVertex);

            while (stack.Count > 0)
            {
                Vertex<T> vertex = stack.Pop();

                List<Vertex<T>> neighbours = graph.GetAdjacentVertices(vertex);
                if (reverseNeighbours)
                {
                    neighbours.Reverse();
                }
                foreach (Vertex<T> neighbour in neighbours)
                {
                    if (!neighbour.Visited)
                    {
                        neighbour.Parent = vertex;
                        neighbour.Distance = vertex.Distance + graph.GetEdgeWeight(vertex, neighbour);
                        neighbour.Visited = true;

                        if (neighbour.Equals(goalVertex))
                        {
                            return GetPathToSource<T>(neighbour);
                        }

                        stack.Push(neighbour);
                    }
                }
                vertex.Visited = true;
            }
            // No path found
            return null;
        }

        public static List<Vertex<T>> GetPathToSource<T>(Vertex<T> from)
        {
            List<Vertex<T>> path = new List<Vertex<T>>();
            Vertex<T> next = from;

            while (next != null)
            {
                path.Add(next);
                next = next.Parent;
            }

            return path;
        }
    }
}
