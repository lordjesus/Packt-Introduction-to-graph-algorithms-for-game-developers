using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEdge : MonoBehaviour {

    [SerializeField]
    private GameObject _EdgeGraphic;

    [SerializeField]
    private GameObject _EdgeArrow;

    private Color _normalColor = new Color(243f / 255, 112f / 255, 33f / 255);
    private Color _highlightColor = new Color(51f / 255, 51f / 255, 51f / 255);

    private Vertex<Node> _startVertex;
    public Vertex<Node> StartVertex { get { return _startVertex; } }
    private Vertex<Node> _endVertex;
    public Vertex<Node> EndVertex { get { return _endVertex; } }

    public void SetLength(float length)
    {
        _EdgeGraphic.transform.localScale = new Vector3(length * 5, 1);
    }

    public float Init(Vertex<Node> startVertex, Vertex<Node> endVertex)
    {
        Vector3 p1 = startVertex.Data.transform.position;
        Vector3 p2 = endVertex.Data.transform.position;

        _startVertex = startVertex;
        _endVertex = endVertex;

        float length = Vector3.Distance(p1, p2);

        // calc mid point
        Vector3 midPoint = (p1 + p2) / 2;

        this.SetLength(length);

        this.SetHighlight(false);

        transform.position = midPoint;
        transform.right = p2 - p1;

        _EdgeArrow.transform.position = p2 - (p2 - p1).normalized * 0.55f;

        return length;
    }

    public void SetHighlight(bool highlight)
    {
        Color color = highlight ? _highlightColor : _normalColor;
        _EdgeGraphic.GetComponent<SpriteRenderer>().color = color;
        _EdgeArrow.GetComponent<SpriteRenderer>().color = color;

        _EdgeGraphic.GetComponent<SpriteRenderer>().sortingOrder = highlight ? 1 : 0;
        _EdgeArrow.GetComponent<SpriteRenderer>().sortingOrder = highlight ? 1 : 0;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
