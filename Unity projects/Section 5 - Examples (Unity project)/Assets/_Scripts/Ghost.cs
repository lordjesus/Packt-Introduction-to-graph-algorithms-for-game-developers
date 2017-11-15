using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

    public delegate void DiedEventHandler(object sender, EventArgs e);

    public event DiedEventHandler Died;

    public enum GhostState { Normal, Scared }

    public enum GhostAIState { Random, TargetingWithLag, Fleeing }

    public GhostAIState AIstate { get; private set; }

    private GhostState _state;
    public GhostState State { get { return _state; } }

    public bool HasBeenScared { get; private set; }

    [SerializeField]
    private GameObject _Normal;

    [SerializeField]
    private GameObject _Scared;

    [SerializeField]
    private float _MoveSpeed = 5f;

    [SerializeField]
    private float _ScaredSpeed = 3.5f;
 
    private float _speed;

    private Vector2 _moveGoal;
    private Point _pointGoal;

    private Grid _grid;

    private Player _player;

    private int _AIrefreshCount;

    private Queue<Point> _pathList;

    private GhostAI _ai;

    // Use this for initialization
    void Start () {
        
        SetState(GhostState.Normal);
	}

    public void SetState(GhostState state)
    {
        _state = state;
        switch (state)
        {
            case GhostState.Normal:
                _Scared.SetActive(false);
                _Normal.SetActive(true);
                _speed = _MoveSpeed;
                break;
            case GhostState.Scared:
                _Normal.SetActive(false);
                _Scared.SetActive(true);
                _speed = _ScaredSpeed;
                HasBeenScared = true;
                break;
        }
    }

    public void Init()
    {
        _moveGoal = transform.position;
    }  

    public void SetAIState(GhostAIState state)
    {
        _ai.SetAIState(state);
        this.AIstate = state;
        _pointGoal = null;
        _pathList = null;
    }

    public void SetGrid(Grid grid)
    {
        _grid = grid;

        _pathList = new Queue<Point>(new List<Point>
        {
            _grid.GhostBeginPosition
        });

        _ai = new GhostAI(_grid);
    }

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    protected virtual void OnDied(EventArgs e)
    {
        if (Died != null)
        {
            Died(this, e);
        }
    }

    private bool EpsilonClose(Vector2 a, Vector2 b)
    {
        float epsilon = 0.05f;
        return Mathf.Abs(a.x - b.x) < epsilon && Mathf.Abs(a.y - b.y) < epsilon;
    }

    public void EatGhost()
    {
        LeanTween.cancel(gameObject);
        OnDied(EventArgs.Empty);
        Destroy(this);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update () {       
        if (_moveGoal == null || EpsilonClose(transform.position, _moveGoal))
        {
            var point = _grid.GetClosestPoint(transform.position);
            Point goalPoint = _ai.GetNextMoveGoal(point, _grid.GetClosestPoint(_player.transform.position));
            _moveGoal = _grid.GridToWorldPosition(goalPoint);
        }

        transform.position = Vector2.MoveTowards(transform.position, _moveGoal, _speed * Time.deltaTime);
    }

    public class GhostAI
    {
        private Grid _grid;
        private Stack<Point> _pathList;
        private GhostAIState _AIState;
        private int _AIrefreshCount = 0;

        public GhostAI(Grid grid)
        {
            _grid = grid;
        }

        public void SetAIState(GhostAIState state)
        {
            _AIState = state;
            _pathList = null;
        }

        public Point GetNextMoveGoal(Point ghostPosition, Point playerPosition)
        {
            if (_AIState == GhostAIState.TargetingWithLag && _AIrefreshCount > 9)
            {
                _AIrefreshCount = 0;
                GetNewPath(ghostPosition, playerPosition);
            }
            if (_pathList == null || _pathList.Count == 0)
            {                
                GetNewPath(ghostPosition, playerPosition);
            }
            if (_pathList != null && _pathList.Count > 0)
            {
                _AIrefreshCount++;
                if (_pathList.Count == 1 && _AIState == GhostAIState.Fleeing)
                {
                    _AIState = GhostAIState.Random;
                    GetNewPath(ghostPosition, playerPosition);
                }
                return _pathList.Pop();
            }            

            return ghostPosition;
        }

        private void GetNewPath(Point ghostPosition, Point playerPosition)
        {

            List<Point> path = null;

            switch (_AIState)
            {
                case GhostAIState.Random:
                    path = GridSearch.BestFirstSearch(_grid, ghostPosition, _grid.GetRandomOpenPoint()).Path;
                    break;
                case GhostAIState.TargetingWithLag:
                    path = GridSearch.AStarSearch(_grid, ghostPosition, playerPosition).Path;
                    break;
                case GhostAIState.Fleeing:

                    var result = GridSearch.DijkstraGeneral(_grid, playerPosition);

                    Point best = null;
                    float largestDist = 0;
                    foreach (Point k in result.DistanceMap.Keys)
                    {
                        if (result.DistanceMap[k] > largestDist)
                        {
                            best = k;
                            largestDist = result.DistanceMap[k];
                        }
                    }

                    Dictionary<Point, float> costMap = InvertCostMap(result.DistanceMap, largestDist);

                    path = GridSearch.AStarSearchWithCost(_grid, ghostPosition, best, costMap).Path;

                    break;
            }

            _pathList = new Stack<Point>(path);
        }

        private Dictionary<Point, float> InvertCostMap(Dictionary<Point, float> map, float largestCost)
        {
            Dictionary<Point, float> inverted = new Dictionary<Point, float>();
            foreach (Point p in map.Keys)
            {
                float cost = largestCost - map[p];
                cost = cost * cost * cost; // cost^3
                inverted.Add(p, cost);
            }
            return inverted;
        }        
    }

    //public class GhostAI
    //{
    //    private Grid _grid;
    //    private Stack<Point> _pathList;
    //    private GhostAIState _AIState;

    //    public GhostAI(Grid grid)
    //    {
    //        _grid = grid;
    //    }

    //    public void SetAIState(GhostAIState state)
    //    {
    //        _AIState = state;
    //        _pathList = null;
    //    }

    //    public Point UpdateMoveGoal(Point ghostPosition, Point playerPosition)
    //    {            
    //        if (_pathList == null || _pathList.Count == 0)
    //        {
    //            GetNewPath(ghostPosition, playerPosition);
    //        }
    //        if (_pathList != null && _pathList.Count > 0)
    //        {
    //            return _pathList.Pop();
    //        }
    //        return ghostPosition;
    //    }

    //    private void GetNewPath(Point ghostPosition, Point playerPosition)
    //    {
    //        List<Point> path = null;

    //        switch (_AIState)
    //        {
    //            case GhostAIState.Random:
    //                path = GridSearch.BestFirstSearch(_grid, ghostPosition, _grid.GetRandomOpenPoint()).Path;
    //                break;               
    //        }

    //        _pathList = new Stack<Point>(path);
    //    }            
    //}
}
