namespace Video_2._2___BFS
{
    public class Vertex<T>
    {
        public int Index { get; set; }

        private T _data;
        public T Data { get { return _data; } }

        // For keeping track of search
        public Vertex<T> Parent { get; set; }
        public bool Visited { get; set; }
        public float Distance { get; set; }

        public Vertex(T data)
        {
            _data = data;
            Index = -1;
        }

        public override string ToString()
        {
            return "{Vertex} " + Data.ToString();
        }
    }
}