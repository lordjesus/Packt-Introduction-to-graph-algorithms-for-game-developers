using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GraphManager : MonoBehaviour {

    //[SerializeField]
    //private Node[] _Nodes;

    private List<Vertex<Node>> _vertices;

    [SerializeField]
    private int _MaxConnections = 4;

    [SerializeField]
    private int _MinConnections = 1;

    [SerializeField]
    private NodeEdge _NodeEdgePrefab;

    [SerializeField]
    private Node _StartNode;

    private Vertex<Node> _startVertex;

    [SerializeField]
    private Node _EndNode;

    private Vertex<Node> _endVertex;

    private Graph<Node> _graph;

    private List<NodeEdge> _edges = new List<NodeEdge>();

    [SerializeField]
    private Text _PathText;

    [SerializeField]
    private Node[] _Nodes;

  	// Use this for initialization
	void Start () {
        _PathText.text = "";
        if (_Nodes == null || _Nodes.Length == 0)
        {
            RandomReset();
        }
        else
        {
            CreateSpecificGraph(_Nodes);
        }
    }   

    private void CreateSpecificGraph(Node[] nodes)
    {
        initGraph(nodes);
        _edges = new List<NodeEdge>();

        CreateEdge(_vertices[2], _vertices[4]);
        CreateEdge(_vertices[0], _vertices[1]);
        CreateEdge(_vertices[1], _vertices[5]);
        CreateEdge(_vertices[0], _vertices[2]);
        CreateEdge(_vertices[2], _vertices[3]);
        CreateEdge(_vertices[3], _vertices[4]);
        CreateEdge(_vertices[4], _vertices[5]);
        CreateEdge(_vertices[3], _vertices[5]);


        //_startVertex = _vertices[0];
        //_endVertex = _vertices[5];
        //_startVertex.Data.SetNodeState(Node.NodeState.Start);
        //_endVertex.Data.SetNodeState(Node.NodeState.End);
    }

    private void initGraph(Node[] nodes)
    {
        _startVertex = null;
        _endVertex = null;
        _vertices = new List<Vertex<Node>>();
        int i = 1;
        foreach (Node node in nodes)
        {
            node.Name = "V" + i++;
            node.Clicked -= Node_Clicked;
            node.Clicked += Node_Clicked;
            _vertices.Add(new Vertex<Node>(node));
            node.SetNodeState(Node.NodeState.Normal);
        }
        _graph = new Graph<Node>(_vertices);
    }

    private void Node_Clicked(object sender, System.EventArgs e)
    {
        Node node = sender as Node;
        if (_startVertex == null)
        {
            node.SetNodeState(Node.NodeState.Start);
            _startVertex = _vertices.First(a => a.Data.Equals(node));
            return;
        }
        if (_endVertex == null)
        {
            node.SetNodeState(Node.NodeState.End);
            _endVertex = _vertices.First(a => a.Data.Equals(node));
        }
    }

    public void Search()
    {
        Clear();
        var path = SearchAlgorithms.BreadthFirstSearchWithGoal(_graph, _startVertex, _endVertex);

        StartCoroutine(ShowPath(path));

    }

    public void DFS()
    {
        Clear();
        var path = SearchAlgorithms.DepthFirstSearchWithGoal(_graph, _startVertex, _endVertex);

        StartCoroutine(ShowPath(path));
    }

    public void Dijkstra()
    {
        Clear();
        var path = SearchAlgorithms.DijkstraWithGoal(_graph, _startVertex, _endVertex);

        StartCoroutine(ShowPath(path));
    }

    private IEnumerator ShowPath(List<Vertex<Node>> path)
    {       
        if (path == null)
        {
            _PathText.text = "No path";
        }
        else
        {
            path.Reverse();
            float totalWeight = 0;
            for (int i = 1; i < path.Count; i++)
            {
                var v2 = path[i];
                var v1 = path[i - 1];

                totalWeight += _graph.GetEdgeWeight(v1, v2);

                var edge = _edges.FirstOrDefault(e => e.StartVertex.Equals(v1) && e.EndVertex.Equals(v2));
                edge.SetHighlight(true);
                yield return new WaitForSeconds(0.2f);
                if (i < path.Count - 1)
                {
                    v2.Data.SetNodeState(Node.NodeState.Highlight);
                }
                _PathText.text = string.Format("Path weight: {0:0.00}", totalWeight);
                yield return new WaitForSeconds(0.2f);
            }
            _PathText.text = string.Format("Path weight: {0:0.00}", totalWeight);
        }
    }

    public void Clear()
    {
        foreach (var vertex in _vertices)
        {
            if (vertex == _startVertex || vertex == _endVertex)
            {
                continue;
            }
            vertex.Data.SetNodeState(Node.NodeState.Normal);
        }
        foreach (var edge in _edges)
        {
            edge.SetHighlight(false);
        }
        _PathText.text = "";
    }

    public void RandomReset()
    {
        initGraph(GameObject.FindObjectsOfType<Node>());
        CreateConnectedRandomGraph();
        _PathText.text = "";
    }

    public void CreateRandomEdges()
    {
        if (_edges != null && _edges.Count > 0)
        {
            foreach (NodeEdge edge in _edges)
            {
                Destroy(edge);
            }
        }

        int rangeMin = _vertices.Count / 2;
        int rangeMax = _vertices.Count * 2;

        int numEdges = Random.Range(rangeMin, rangeMax);
        _edges = new List<NodeEdge>();

        for (int i = 0; i < numEdges; i++)
        {
            int v1 = Random.Range(0, _vertices.Count);
            int v2 = Random.Range(0, _vertices.Count);
            if (v1 != v2)
            {
                float weight = CreateEdge(_vertices[v1], _vertices[v2]);
                print("Create edge from " + v1 + " to " + v2 + " with weight: " + weight);
            }        
        }
    }

    public void CreateConnectedRandomGraph()
    {
       
        if (_edges != null && _edges.Count > 0)
        {
            foreach (NodeEdge edge in _edges)
            {
                Destroy(edge.gameObject);
               
            }
        }
       
        _edges = new List<NodeEdge>();

        List<Vertex<Node>> randomNodes = new List<Vertex<Node>>(_vertices);
        System.Random rand = new System.Random();
        foreach (Vertex<Node> node in _vertices)
        {
            randomNodes = randomNodes.OrderBy(a => rand.Next()).ToList();
            int numConnections = rand.Next(_MinConnections, _MaxConnections);
            for (int i = 0; i < numConnections; i++)
            {
                if (randomNodes[i].Equals(node))
                {
                    continue;
                }
                float weight = CreateEdge(node, randomNodes[i]);
            }
        }
        print(_graph);
       
    }

    // Create an edge and returns the length
    private float CreateEdge(Vertex<Node> node1, Vertex<Node> node2)
    {      
        NodeEdge edge = Instantiate(_NodeEdgePrefab);
        float length = edge.Init(node1, node2);       

        _edges.Add(edge);

        _graph.CreateDirectedEdge(node1, node2, length);

        return length;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
