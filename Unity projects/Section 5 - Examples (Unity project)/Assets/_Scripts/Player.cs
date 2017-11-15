using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void PickedUpEventHandler(object sender, EventArgs e);

    public event PickedUpEventHandler CoinPickedUp;

    public event PickedUpEventHandler PowerupPickedUp;

    public delegate void DiedEventHandler(object sender, EventArgs e);

    public event DiedEventHandler Died;

    

    [SerializeField]
    private float _MoveSpeed = 1f;

    [SerializeField]
    private GameObject _Graphic;

    private MoveDirection _currentDirection = MoveDirection.Left;
    private MoveDirection _nextDirection;

    private Vector2 _moveGoal;
    private Point _pointGoal;

    private Grid _grid;

    public bool CanMove { get; set; }

   

    private Vector2 ConvertDirection(MoveDirection direction)
    {

        switch (direction)
        {
            case MoveDirection.Down:
                return Vector2.down;
            case MoveDirection.Up:
                return Vector2.up;
            case MoveDirection.Left:
                return Vector2.left;
            case MoveDirection.Right:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }

    public void SetGrid(Grid grid)
    {
        _grid = grid;
    }

    // Use this for initialization
    void Start()
    {
        ChangeDirection(MoveDirection.Left);
    }

    private void ChangeDirection(MoveDirection newDirection)
    {
        _nextDirection = newDirection;
        if (Mathf.Abs((int)newDirection - (int)_currentDirection) == 2)
        {
            _pointGoal = null;
        }
    }

    protected virtual void OnDied(EventArgs e)
    {
        if (Died != null)
        {
            Died(this, e);
        }
    }

    private void HandleInput()
    {
        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                ChangeDirection(MoveDirection.Up);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                ChangeDirection(MoveDirection.Down);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                ChangeDirection(MoveDirection.Left);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                ChangeDirection(MoveDirection.Right);
            }
        }
    }

    private bool CanMoveInDirection(MoveDirection direction)
    {
        return true;
    }

    private void UpdateDirection(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.Right:
                _Graphic.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case MoveDirection.Left:
                _Graphic.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case MoveDirection.Up:
                _Graphic.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case MoveDirection.Down:
                _Graphic.transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
        }
    }

    private bool EpsilonClose(Vector2 a, Vector2 b)
    {
        float epsilon = 0.05f;
        return Mathf.Abs(a.x - b.x) < epsilon && Mathf.Abs(a.y - b.y) < epsilon;
    }

    private void Teleport(Point from)
    {
        Point to = from;
        if (from.X == 0)
        {
            to.X = _grid.Width - 1;
        } else
        {
            to.X = 0;
        }
        Vector2 pos = _grid.GridToWorldPosition(to);
        transform.position = pos;
        _pointGoal = null;
    }

    private void UpdateMoveGoal(MoveDirection direction)
    {
        var point = _grid.GetClosestPoint(transform.position);

       

        if (_pointGoal == null || point.Equals(_pointGoal) && EpsilonClose(transform.position, _grid.GridToWorldPosition(_pointGoal)))
        {
            if (point.X == 0 && direction == MoveDirection.Left || point.X == _grid.Width - 1 && direction == MoveDirection.Right)
            {
                Teleport(point);
                return;
            }
            var adjacents = _grid.GetAdjacentCells(point);
            var available = AvailableDirections(adjacents, point);
            if (available.Keys.Contains(_nextDirection))
            {
                _currentDirection = _nextDirection;
                UpdateDirection(_currentDirection);

                _pointGoal = available[_currentDirection];
                _moveGoal = _grid.GridToWorldPosition(_pointGoal);
            }
            else if (available.Keys.Contains(direction))
            {
                _pointGoal = available[direction];
                _moveGoal = _grid.GridToWorldPosition(_pointGoal);
            }
        }
    }

    private Dictionary<MoveDirection, Point> AvailableDirections(List<Point> adjacents, Point current)
    {
        Dictionary<MoveDirection, Point> list = new Dictionary<MoveDirection, Point>();

        foreach (var p in adjacents)
        {
            if (p.X == current.X - 1)
            {
                list.Add(MoveDirection.Left, p);
            }
            if (p.X == current.X + 1)
            {
                list.Add(MoveDirection.Right, p);
            }
            if (p.Y == current.Y - 1)
            {
                list.Add(MoveDirection.Up, p);
            }
            if (p.Y == current.Y + 1)
            {
                list.Add(MoveDirection.Down, p);
            }
        }

        return list;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanMove)
        {
            return;
        }
        HandleInput();

        UpdateMoveGoal(_currentDirection);

        transform.position = Vector2.MoveTowards(transform.position, _moveGoal, _MoveSpeed * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ghost = collision.gameObject.GetComponent<Ghost>();
        if (ghost)
        {            
            if (ghost.State == Ghost.GhostState.Normal)
            {
                OnDied(EventArgs.Empty);
                return;
            }
            if (ghost.State == Ghost.GhostState.Scared)
            {
                ghost.EatGhost();
                return;
            }
        }
        
        Pickupable p = collision.gameObject.GetComponent<Pickupable>();
        if (p)
        {           
            Destroy(p.gameObject);
            if (p is Coin)
            {
                OnCoinPickedUp(EventArgs.Empty);
            }
            else if (p is Powerup)
            {
                OnPowerupPickedUp(EventArgs.Empty);
            }
        }
    }

    protected virtual void OnCoinPickedUp(EventArgs e)
    {
        if (CoinPickedUp != null)
        {
            CoinPickedUp(this, e);
        }
    }

    protected virtual void OnPowerupPickedUp(EventArgs e)
    {
        if (PowerupPickedUp != null)
        {
            PowerupPickedUp(this, e);
        }
    }
}