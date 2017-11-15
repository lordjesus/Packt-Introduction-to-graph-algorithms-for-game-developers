using System.Collections.Generic;

namespace Video_2._2___BFS
{
    public class AdjacencyMatrix
    {
        private float[,] _matrix;
        private int _size;
        public int Size { get { return _size; } }

        public AdjacencyMatrix(int size)
        {
            _matrix = new float[size, size];
            _size = size;
        }

        public void AddDirectedEdge(int from, int to, float weight)
        {
            _matrix[from, to] = weight;
        }

        public void AddUndirectedEdge(int v1, int v2, float weight)
        {
            _matrix[v1, v2] = weight;
            _matrix[v2, v1] = weight;
        }

        public float GetEdgeWeight(int x, int y)
        {
            return _matrix[x, y];
        }

        public List<int> GetAdjacencyList(int sourceIndex)
        {
            List<int> adjacencyList = new List<int>();
            for (int i = 0; i < _size; i++)
            {
                if (_matrix[sourceIndex, i] != 0)
                {
                    adjacencyList.Add(i);
                }
            }
            return adjacencyList;
        }
    }
}