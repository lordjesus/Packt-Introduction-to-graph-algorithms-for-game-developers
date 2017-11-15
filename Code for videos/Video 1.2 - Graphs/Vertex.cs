namespace Video_1._2___Graphs
{
    public class Vertex<T>
    {
        public int Index { get; set; }

        private T _data;
        public T Data { get { return _data; } }        

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
