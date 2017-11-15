using System.Collections.Generic;

namespace Video_2._2___BFS
{
    class SearchAlgorithms
    {
        public static void BreadthFirstSearch<T>(Graph<T> graph, Vertex<T> sourceVertex)
        {
            foreach (Vertex<T> vertex in graph.Vertices)
            {
                vertex.Parent = null;
                vertex.Distance = 0;
                vertex.Visited = false;
            }

            Queue<Vertex<T>> queue = new Queue<Vertex<T>>();
            queue.Enqueue(sourceVertex);

            while (queue.Count > 0)
            {
                Vertex<T> vertex = queue.Dequeue();

                foreach(Vertex<T> neighbour in graph.GetAdjacentVertices(vertex))
                {
                    if (!neighbour.Visited)
                    {
                        neighbour.Parent = vertex;
                        neighbour.Distance = vertex.Distance + graph.GetEdgeWeight(vertex, neighbour);                        
                        neighbour.Visited = true;

                        queue.Enqueue(neighbour);
                    }
                }
                vertex.Visited = true;
            }
        }        

        public static List<Vertex<T>> BreadthFirstSearchWithGoal<T>(
            Graph<T> graph, Vertex<T> sourceVertex, Vertex<T> goalVertex)
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

            Queue<Vertex<T>> queue = new Queue<Vertex<T>>();
            queue.Enqueue(sourceVertex);

            while (queue.Count > 0)
            {
                Vertex<T> vertex = queue.Dequeue();

                foreach (Vertex<T> neighbour in graph.GetAdjacentVertices(vertex))
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

                        queue.Enqueue(neighbour);
                    }
                }
                vertex.Visited = true;
            }
            // No path to goal vertex exists
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
