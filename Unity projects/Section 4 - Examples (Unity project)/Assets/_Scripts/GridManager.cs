using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GridManager : MonoBehaviour {


    [SerializeField]
    private Cell _CellPrefab;

    [SerializeField]
    private int _GridSize = 10;

    [SerializeField]
    private Text _PathText, _VisitedText;

    private Grid _grid;

    private Cell _startCell, _endCell;

    private List<Cell> _cells = new List<Cell>();

    [SerializeField]
    private string _LevelPath = "level";

	// Use this for initialization
	void Start ()
    {
        LoadLevel(_LevelPath);
	}

    public void LoadMazeLevel()
    {
        LoadLevel("level maze");
    }

    public void LoadLevelLevel()
    {
        LoadLevel("level");
    }

    private void LoadLevel(string level)
    {
        _LevelPath = level;
        _PathText.text = "";
        _VisitedText.text = "";
        ReadGridFromFile(_LevelPath);
        CreateGrid(_grid);
    }

    private void ReadGridFromFile(string filename)
    {
        TextAsset levelData = Resources.Load(filename) as TextAsset;

        string[] lines = levelData.text.Split('\n');

        _GridSize = int.Parse(lines[0]);
        _grid = new Grid(_GridSize, _GridSize);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] cells = lines[i].Split(' ');
            for (int j = 0; j < cells.Length; j++)
            {
                string cell = cells[j];
                int val = int.Parse(cell);
                
                _grid[i - 1, j] = int.Parse(cells[j]);
            }
        }
    }

    private void CreateGrid(Grid grid)
    {
        float startX = -1 * grid.Width / 2 + 0.5f;
        float startY = -1 * grid.Height / 2 + 0.5f;
        for (int i = 0; i < grid.Width; i++)
        {
            for (int j = 0; j < grid.Height; j++)
            {
                Cell cell = Instantiate(_CellPrefab) as Cell;
                cell.transform.position = new Vector2(startX + i, startY + j);
                //if (grid[i, j])
                //{
                //    cell.SetState(Cell.CellState.Normal);
                //}
                //else
                //{
                //    cell.SetState(Cell.CellState.Impassable);
                //}
                
                cell.Clicked += Cell_Clicked;
                cell.SetPosition(i, j);
                cell.SetState(Cell.CellState.Normal, grid.GetCostOfEnteringCell(cell.GetPosition()));
                _cells.Add(cell);
            }
        }
    }

    public void Reset()
    {
        _startCell = null;
        _endCell = null;
        _VisitedText.text = "";
        _PathText.text = "";
        for (int i = 0; i < _grid.Width; i++)
        {
            for (int j = 0; j < _grid.Height; j++)
            {
                Grid.Point pos = new Grid.Point(i, j);
                _cells.FirstOrDefault(c => c.GetPosition().Equals(pos)).SetState(Cell.CellState.Normal, _grid.GetCostOfEnteringCell(pos));
            }
        }
    }

    public void ClearPath()
    {
        _PathText.text = "";
        _VisitedText.text = "";
        for (int i = 0; i < _grid.Width; i++)
        {
            for (int j = 0; j < _grid.Height; j++)
            {
                Grid.Point pos = new Grid.Point(i, j);
                Cell cell = _cells.FirstOrDefault(c => c.GetPosition().Equals(pos));
                
                if (cell.State == Cell.CellState.End || cell.State == Cell.CellState.Start)
                {
                    continue;
                }
                cell.SetState(Cell.CellState.Normal, _grid.GetCostOfEnteringCell(pos));
            }
        }
    }

    public void BFS()
    {
        ClearPath();
        var result = GridSearch.BreadthFirstSearch(_grid, _startCell.GetPosition(), _endCell.GetPosition());
        ShowVisited(result.Visited);
        StartCoroutine(ShowPath(result.Path));
    }

    public void DFS()
    {
        ClearPath();
        var result = GridSearch.DepthFirstSearch(_grid, _startCell.GetPosition(), _endCell.GetPosition());
        ShowVisited(result.Visited);
        StartCoroutine(ShowPath(result.Path));
    }

    public void Dijkstra()
    {
        ClearPath();
        var result = GridSearch.DijkstraPriority(_grid, _startCell.GetPosition(), _endCell.GetPosition());
        ShowVisited(result.Visited);
        StartCoroutine(ShowPath(result.Path));
    }

    public void AStar()
    {
        ClearPath();
        var result = GridSearch.AStarSearchPriority(_grid, _startCell.GetPosition(), _endCell.GetPosition());
        ShowVisited(result.Visited);
        StartCoroutine(ShowPath(result.Path));
    }

    public void BestFirst()
    {
        ClearPath();
        var result = GridSearch.BestFirstSearch(_grid, _startCell.GetPosition(), _endCell.GetPosition());
        ShowVisited(result.Visited);
        StartCoroutine(ShowPath(result.Path));
    }

    public void BiAStar()
    {
        ClearPath();
        var result = GridSearch.BiAStarSearch(_grid, _startCell.GetPosition(), _endCell.GetPosition());
        ShowVisited(result.Visited);
        StartCoroutine(ShowPath(result.Path));
    }

    private void ShowVisited(List<Grid.Point> visited)
    {
        _VisitedText.text = "Visited: " + visited.Count;
        foreach (Grid.Point point in visited)
        {
            Cell cell = _cells.FirstOrDefault(c => c.GetPosition().Equals(point));
            if (cell.State == Cell.CellState.Normal)
            {
                cell.SetState(Cell.CellState.Visited);
            }
        }
    }

    private IEnumerator ShowPath(List<Grid.Point> path)
    {
        print("Path");
        path.Reverse();

        float totalWeight = 0;

        for (int i = 1; i < path.Count - 1; i++)
        {
            Grid.Point step = path[i];
            Cell cell = _cells.FirstOrDefault(c => c.GetPosition().Equals(step));

            if (cell.State == Cell.CellState.End || cell.State == Cell.CellState.Start)
            {
                continue;
            }

            cell.SetState(Cell.CellState.Highlight);
            
            totalWeight += _grid.GetCostOfEnteringCell(step);
            _PathText.text = "Total weight: " + totalWeight;
            yield return new WaitForSeconds(0.1f);
        }

        _PathText.text = "Total weight: " + totalWeight;
    }

    private void Cell_Clicked(object sender, System.EventArgs e)
    {
        Cell cell = sender as Cell;
        if (_startCell == null)
        {
            _startCell = cell;
            cell.SetState(Cell.CellState.Start);
            return;
        }
        if (_endCell == null)
        {
            _endCell = cell;
            cell.SetState(Cell.CellState.End);
            return;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
