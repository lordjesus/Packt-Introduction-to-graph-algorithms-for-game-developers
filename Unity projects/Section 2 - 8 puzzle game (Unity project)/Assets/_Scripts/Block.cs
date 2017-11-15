using UnityEngine;
using System;

public class Block : MonoBehaviour {

    public delegate void ClickedEventHandler(object sender, EventArgs e);

    public event ClickedEventHandler Clicked;

    [SerializeField]
    private int _Value;

    public int Value { get { return _Value; } }
    
    private BoxCollider2D _collider;

    // Use this for initialization
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }
	
    protected virtual void OnClicked(EventArgs e)
    {
        if (Clicked != null)
        {
            Clicked(this, e);
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
