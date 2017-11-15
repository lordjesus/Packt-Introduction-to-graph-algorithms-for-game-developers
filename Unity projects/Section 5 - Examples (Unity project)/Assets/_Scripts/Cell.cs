using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{   
    public delegate void ClickedEventHandler(object sender, EventArgs e);

    public event ClickedEventHandler Clicked;

    private Color _highlightColor = new Color(243f / 255, 112f / 255, 33f / 255);
    private Color _impassableColor = new Color(51f / 255, 51f / 255, 51f / 255);
    private Color _visitedColor = new Color(66f / 255, 134f / 255, 244f / 255);
    private Color _normalColor = Color.white;

    private CellType _state;
    public CellType State { get { return _state; } }

    [SerializeField]
    private SpriteRenderer _BgSprite;   
   
   // private BoxCollider2D _collider;

    private int _x, _y;

    // Use this for initialization
    void Start()
    {
       // _collider = GetComponent<BoxCollider2D>();
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

    public Point GetPosition()
    {
        return new Point(_x, _y);
    }

    public void SetState(CellType state, float weight = 0)
    {
        _state = state;

        if (Grid.IsCellPassable(state))
        {
            _BgSprite.color = _normalColor;
        }
        else
        {
            _BgSprite.color = _impassableColor;
        }       
    }


    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    var hitCollider = _collider.OverlapPoint(mousePosition);

        //    if (hitCollider)
        //    {

        //        OnClicked(EventArgs.Empty);
        //    }
        //}
    }
}
