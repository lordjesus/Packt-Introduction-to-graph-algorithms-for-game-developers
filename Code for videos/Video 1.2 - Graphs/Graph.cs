using System.Collections.Generic;
using System.Text;

namespace Video_1._2___Graphs
{
    public class Graph<T>
    {
        private List<Vertex<T>> _vertices;
        public List<Vertex<T>> Vertices { get { return _vertices; } }

        private AdjacencyMatrix _adjacencyMatrix;

        public Graph(List<Vertex<T>> vertices)
        {
            _vertices = vertices;
            for (int i = 0; i < _vertices.Count; i++)
            {
                _vertices[i].Index = i;
            }
            _adjacencyMatrix = new AdjacencyMatrix(_vertices.Count);
        }

        public void CreateDirectedEdge(int fromIndex, int toIndex, float weight = 1)
        {
            _adjacencyMatrix.AddDirectedEdge(fromIndex, toIndex, weight);
        }

        public void CreateDirectedEdge(Vertex<T> from, Vertex<T> to, float weight = 1)
        {
            this.CreateDirectedEdge(from.Index, to.Index, weight);
        }

        public void CreateUndirectedEdge(int v1, int v2, float weight = 1)
        {
            _adjacencyMatrix.AddUndirectedEdge(v1, v2, weight);
        }

        public void CreateUndirectedEdge(Vertex<T> v1, Vertex<T> v2, float weight = 1)
        {
            this.CreateUndirectedEdge(v1.Index, v2.Index, weight);
        }

        public List<Vertex<T>> GetAdjacentVertices(int sourceIndex)
        {
            List<int> adjacentIndices = _adjacencyMatrix.GetAdjacencyList(sourceIndex);
            List<Vertex<T>> adjacentVertices = new List<Vertex<T>>();

            foreach (int vertexIndex in adjacentIndices)
            {
                adjacentVertices.Add(_vertices[vertexIndex]);
            }

            return adjacentVertices;
        }

        public List<Vertex<T>> GetAdjacentVertices(Vertex<T> source)
        {
            return GetAdjacentVertices(source.Index);
        }

        public float GetEdgeWeight(Vertex<T> v1, Vertex<T> v2)
        {
            return _adjacencyMatrix.GetEdgeWeight(v1.Index, v2.Index);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Graph:");

            foreach (Vertex<T> vertex in _vertices)
            {
                sb.AppendLine(vertex.Data.ToString());
                sb.Append("\t");
                List<Vertex<T>> adjacentVertices = GetAdjacentVertices(vertex);
                if (adjacentVertices.Count > 0)
                {
                    sb.Append("Edge to: ");
                    foreach (Vertex<T> adjVertex in adjacentVertices)
                    {
                        sb.Append(adjVertex.Data.ToString());
                        sb.Append("(w=");
                        sb.Append(GetEdgeWeight(vertex, adjVertex));
                        sb.Append(") ");
                    }
                }
                else
                {
                    sb.Append("No outgoing edges");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
