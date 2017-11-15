using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public enum NodeState { Normal, Start, End, Highlight }

    public delegate void ClickedEventHandler(object sender, EventArgs e);

    public event ClickedEventHandler Clicked;

    public string Name;

    [SerializeField]
    private GameObject _NormalSprite;

    [SerializeField]
    private GameObject _StartSprite;

    [SerializeField]
    private GameObject _EndSprite;

    [SerializeField]
    private GameObject _HighlightedSprite;

    private NodeState _state;
    public NodeState State { get { return _state; } }



    private CircleCollider2D _collider;

	// Use this for initialization
	void Start () {
        _collider = GetComponent<CircleCollider2D>();
        SetNodeState(NodeState.Normal);
	}

    protected virtual void OnClicked(EventArgs e)
    {
        if (Clicked != null)
        {
            Clicked(this, e);
        }
    }

    public void SetNodeState(NodeState state)
    {
        _state = state;
        _NormalSprite.SetActive(false);
        _StartSprite.SetActive(false);
        _EndSprite.SetActive(false);
        _HighlightedSprite.SetActive(false);
        switch (state)
        {
            case NodeState.Normal:
                _NormalSprite.SetActive(true);
                break;
            case NodeState.Start:
                _StartSprite.SetActive(true);
                break;
            case NodeState.End:
                _EndSprite.SetActive(true);
                break;
            case NodeState.Highlight:
                _HighlightedSprite.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hitCollider = _collider.OverlapPoint(mousePosition);

            if (hitCollider)
            {

                OnClicked(EventArgs.Empty);
            }
        }
    }
}
