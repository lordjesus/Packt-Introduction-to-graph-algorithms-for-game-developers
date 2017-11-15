using System.Collections.Generic;

namespace Video_2._2___BFS
{
    class Program
    {
        static void Main(string[] args)
        {
           

            Vertex<string> v1 = new Vertex<string>("v1");
            Vertex<string> v2 = new Vertex<string>("v2");
            Vertex<string> v3 = new Vertex<string>("v3");
            Vertex<string> v4 = new Vertex<string>("v4");
            Vertex<string> v5 = new Vertex<string>("v5");
            Vertex<string> v6 = new Vertex<string>("v6");
            Vertex<string> v7 = new Vertex<string>("v7");

            List<Vertex<string>> vertices = new List<Vertex<string>> {
                v1, v2, v3, v4, v5, v6, v7
            };

            Graph<string> graph = new Graph<string>(vertices);

            graph.CreateUndirectedEdge(v4, v5);
            graph.CreateUndirectedEdge(v4, v2);
            graph.CreateUndirectedEdge(v4, v1);
            graph.CreateUndirectedEdge(v5, v6);
            graph.CreateUndirectedEdge(v2, v5);
            graph.CreateUndirectedEdge(v2, v7);
            graph.CreateUndirectedEdge(v2, v1);
            graph.CreateUndirectedEdge(v1, v3);
            graph.CreateUndirectedEdge(v1, v7);
            graph.CreateUndirectedEdge(v7, v6);

            System.Console.WriteLine(graph.ToString());

            SearchAlgorithms.BreadthFirstSearch<string>(graph, v4);

            List<Vertex<string>> fromV6 = SearchAlgorithms.GetPathToSource<string>(v6);

            System.Console.WriteLine("Printing path from v6");
            foreach (Vertex<string> vertex in fromV6)
            {
                System.Console.WriteLine(vertex);
            }

            List<Vertex<string>> path = SearchAlgorithms.BreadthFirstSearchWithGoal(graph, v6, v3);
            System.Console.WriteLine("Printing path from v3 to v6");
            foreach (Vertex<string> vertex in path)
            {
                System.Console.WriteLine(vertex);
            }

            while (true) ;
        }        
    }
}
