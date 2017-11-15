using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    public enum CellState { Normal, Start, End, Highlight, Impassable, Visited }

    public delegate void ClickedEventHandler(object sender, EventArgs e);

    public event ClickedEventHandler Clicked;

    private Color _highlightColor = new Color(243f / 255, 112f / 255, 33f / 255);
    private Color _impassableColor = new Color(51f / 255, 51f / 255, 51f / 255);
    private Color _visitedColor = new Color(66f / 255, 134f / 255, 244f / 255);
    private Color _normalColor = Color.white;

    private CellState _state;
    public CellState State { get { return _state; } }

    [SerializeField]
    private SpriteRenderer _BgSprite;

    [SerializeField]
    private SpriteRenderer _OutlineSprite;

    [SerializeField]
    private TextMesh _Text;

    private BoxCollider2D _collider;

    private int _x, _y;

    // Use this for initialization
    void Start () {
        _collider = GetComponent<BoxCollider2D>();
        //SetState(CellState.Normal);
	}

    protected virtual void OnClicked(EventArgs e)
    {
        if (Clicked != null)
        {
            Clicked(this, e);
        }
    }

    public void SetPosition(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public Grid.Point GetPosition()
    {
        return new Grid.Point(_x, _y);
    }

    public void SetState(CellState state, float weight = 0)
    {
        _state = state;
        _Text.text = "";
        switch (_state)
        {
            case CellState.Normal:
                _BgSprite.color = Color.Lerp(_normalColor, _impassableColor, (weight - 1) / 9f);                
                break;
            case CellState.Highlight:
                _BgSprite.color = _highlightColor;
                break;
            case CellState.Impassable:
                _BgSprite.color = _impassableColor;
                break;
            case CellState.Start:
                _BgSprite.color = _highlightColor;
                _Text.text = "s";
                break;
            case CellState.End:
                _BgSprite.color = _highlightColor;
                _Text.text = "e";
                break;
            case CellState.Visited:
                _BgSprite.color = _visitedColor;
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
