using System;
using System.Collections.Generic;

namespace Video_1._2___Graphs
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Video 1.2");

            Vertex<string> v1 = new Vertex<string>("v1");
            Vertex<string> v2 = new Vertex<string>("v2");
            Vertex<string> v3 = new Vertex<string>("v3");
            Vertex<string> v4 = new Vertex<string>("v4");

            List<Vertex<string>> vertices = new List<Vertex<string>> {
                v1, v2, v3, v4
            };

            Graph<string> graph = new Graph<string>(vertices);

            graph.CreateDirectedEdge(v1, v2, 3);
            graph.CreateDirectedEdge(v4, v1, 1);
            graph.CreateDirectedEdge(v2, v3, 1);
            graph.CreateDirectedEdge(v2, v4, -5);

            Console.WriteLine(graph.ToString());

            Console.WriteLine("End of video 1.2");

            while (true) ;
        }
    }
}
