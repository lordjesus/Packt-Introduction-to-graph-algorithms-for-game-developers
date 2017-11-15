using System.Collections.Generic;
using System.Linq;

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

            foreach (Vertex<T> neighbour in graph.GetAdjacentVertices(vertex))
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

    public static void Dijkstra<T>(Graph<T> graph, Vertex<T> source)
    {
        List<Vertex<T>> unfinishedVertices = new List<Vertex<T>>();
        foreach (Vertex<T> vertex in graph.Vertices)
        {
            vertex.Distance = int.MaxValue;
            vertex.Parent = null;
            unfinishedVertices.Add(vertex);
        }
        source.Distance = 0;        
    
        while (unfinishedVertices.Count > 0)
        {
            Vertex<T> vertex = GetClosestVertex(unfinishedVertices);           
            foreach (Vertex<T> adjVertex in graph.GetAdjacentVertices(vertex))
            {
                if (adjVertex.Distance > vertex.Distance + graph.GetEdgeWeight(vertex, adjVertex))
                {
                    adjVertex.Distance = vertex.Distance + graph.GetEdgeWeight(vertex, adjVertex);
                    adjVertex.Parent = vertex;
                }
            }
        }
    }

    public static List<Vertex<T>> DijkstraWithGoal<T>(Graph<T> graph, Vertex<T> source, Vertex<T> goal)
    {
        List<Vertex<T>> unfinishedVertices = new List<Vertex<T>>();
        foreach (Vertex<T> vertex in graph.Vertices)
        {
            vertex.Distance = int.MaxValue;
            vertex.Parent = null;
            unfinishedVertices.Add(vertex);
        }
        source.Distance = 0;

        while (unfinishedVertices.Count > 0)
        {
            Vertex<T> vertex = GetClosestVertex(unfinishedVertices);
            unfinishedVertices.Remove(vertex);
            if (vertex.Equals(goal))
            {
                return GetPathToSource(vertex);
            }
            foreach (Vertex<T> adjVertex in graph.GetAdjacentVertices(vertex))
            {
                if (adjVertex.Distance > vertex.Distance + graph.GetEdgeWeight(vertex, adjVertex))
                {
                    adjVertex.Distance = vertex.Distance + graph.GetEdgeWeight(vertex, adjVertex);
                    adjVertex.Parent = vertex;
                }
            }
        }
        return null;
    }

    private static Vertex<T> GetClosestVertex<T>(List<Vertex<T>> list)
    {
        Vertex<T> candidate = list[0];
        foreach (Vertex<T> vertex in list)
        {
            if (vertex.Distance < candidate.Distance)
            {
                candidate = vertex;
            }
        }
        return candidate;
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
