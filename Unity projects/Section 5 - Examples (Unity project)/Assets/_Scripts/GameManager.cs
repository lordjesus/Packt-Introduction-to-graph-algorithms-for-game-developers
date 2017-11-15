using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private Cell _CellPrefab;

    [SerializeField]
    private GameObject _CoinPrefab;

    [SerializeField]
    private GameObject _PowerupPrefab;

    [SerializeField]
    private Player _Player;

    [SerializeField]
    private Ghost _GhostPrefab;

    [SerializeField]
    private float _ScaredTime = 5f;

    [SerializeField]
    private GameObject _GameOverPanel;

    [SerializeField]
    private GameObject _WinPanel;

    [SerializeField]
    private Ghost.GhostAIState _AIState = Ghost.GhostAIState.Random;

    [SerializeField]
    private bool _LockAIState;

    [SerializeField]
    private bool _DisablePlayer;

    [SerializeField]
    private int _NumberOfGhosts = 4;

    [SerializeField]
    private string _LevelName = "pacman level";

    [SerializeField]
    private Image _EdibleTimer;
   
    private float _edibleTimeLeft = 0;


    Grid _grid;

    private int _totalCoins = 0;
    private int _coinsEaten = 0;

    private int _score;

    private Player _player;

    [SerializeField]
    private Text _ScoreLabel;

    private List<Ghost> _ghosts = new List<Ghost>();

    bool _isSpawning;

	// Use this for initialization
	void Start () {
        TextAsset levelData = Resources.Load(_LevelName) as TextAsset;
        _grid = Grid.LoadPacmanLevel(levelData.text);
        InstantiateGrid(_grid);
        SpawnGhosts();
        _ScoreLabel.text = "0";
        if (_EdibleTimer)
        {
            _EdibleTimer.fillAmount = 0;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (!_isSpawning && _ghosts.Count < 4)
        {
            StartCoroutine(DoSpawn());
        }

        if (_edibleTimeLeft > 0)
        {
            _edibleTimeLeft -= Time.deltaTime;
            if (_edibleTimeLeft < 0)
            {
                _edibleTimeLeft = 0;
            }
            _EdibleTimer.fillAmount = _edibleTimeLeft / _ScaredTime;
        }
	}

    private void InstantiateGrid(Grid grid)
    {
        float startX = -1 * grid.Width / 2 + 0.5f;
        float startY = 1 * grid.Height / 2 + 0.5f;
        for (int i = 0; i < grid.Width; i++)
        {
            for (int j = 0; j < grid.Height; j++)
            {
                Cell cell = Instantiate(_CellPrefab) as Cell;
                cell.transform.position = new Vector2(startX + i, startY - j);
                CellType type = grid[i, j];
                cell.SetState(type);
                
                if (type == CellType.Coin)
                {
                    GameObject coin = Instantiate(_CoinPrefab) as GameObject;
                    coin.transform.position = new Vector2(startX + i, startY - j);
                    _totalCoins++;
                } else if (type == CellType.Powerup)
                {
                    GameObject powerup = Instantiate(_PowerupPrefab) as GameObject;
                    powerup.transform.position = new Vector2(startX + i, startY - j);
                } else if (type == CellType.Start)
                {
                    _player = Instantiate(_Player) as Player;
                    _player.transform.position = new Vector2(startX + i, startY - j);
                    _player.SetGrid(grid);
                    print("Start at " + i + ", " + j);
                    _player.CoinPickedUp += Player_CoinPickedUp;
                    _player.PowerupPickedUp += Player_PowerupPickedUp;
                    _player.Died += Player_Died;

                    _player.CanMove = !_DisablePlayer;
                } 
            }
        }
    }

    private void Player_Died(object sender, System.EventArgs e)
    {
        _GameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void SpawnGhosts()
    {
        _isSpawning = true;
        LeanTween.delayedCall(2f, () =>
        {
            StartCoroutine(DoSpawn());
        });

    }

    private IEnumerator DoSpawn()
    {
        int ghostCount = _ghosts.Count;
        
        _isSpawning = true;        
        for (int i = 0; i < _NumberOfGhosts - ghostCount; i++)
        {
            Ghost ghost = Instantiate(_GhostPrefab) as Ghost;
            ghost.transform.position = _grid.GridToWorldPosition(_grid.GhostBeginPosition);
            ghost.SetGrid(_grid);
            ghost.SetPlayer(_player);
            ghost.SetAIState(_AIState);
            _ghosts.Add(ghost);
            ghost.Died += Ghost_Died;

            ghost.Init();


            yield return new WaitForSeconds(2f);
        }
        _isSpawning = false;
    }

    private void Ghost_Died(object sender, System.EventArgs e)
    {
        print("Ghost died!");
        Ghost ghost = sender as Ghost;
        _ghosts.Remove(ghost);

        UpdateScore(50);
    }

    private void Player_PowerupPickedUp(object sender, System.EventArgs e)
    {
        if (_LockAIState)
        {
            return;
        }
        foreach (Ghost ghost in _ghosts)
        {            
            if (ghost.AIstate == Ghost.GhostAIState.TargetingWithLag)
            {
                ghost.SetAIState(Ghost.GhostAIState.Fleeing);
            }
            else
            {
                ghost.SetAIState(Ghost.GhostAIState.Random);
            }
            ghost.SetState(Ghost.GhostState.Scared);
        }
        _edibleTimeLeft = _ScaredTime;
        LeanTween.delayedCall(_ScaredTime, () => {

            foreach (Ghost ghost in _ghosts)
            {
                ghost.SetState(Ghost.GhostState.Normal);
                // Surviving ghosts are angry now!
                if (ghost.HasBeenScared)
                {
                    ghost.SetAIState(Ghost.GhostAIState.TargetingWithLag);
                }
            }
        });
    }

    private void Player_CoinPickedUp(object sender, System.EventArgs e)
    {
        _coinsEaten++;
        UpdateScore(1);

        if (_coinsEaten == _totalCoins)
        {
            _WinPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void UpdateScore(int scoreAdd)
    {
        _score += scoreAdd;
        _ScoreLabel.text = _score.ToString();
    }
}

public static class GridExtensions
{
    public static Vector2 GridToWorldPosition(this Grid grid, int x, int y)
    {
        float startX = -1 * grid.Width / 2 + 0.5f;
        float startY = 1 * grid.Height / 2 + 0.5f;

        return new Vector2(startX + x, startY - y);
    }

    public static Vector2 GridToWorldPosition(this Grid grid, Point p)
    {
        return GridToWorldPosition(grid, p.X, p.Y);
    }

    public static Point GetClosestPoint(this Grid grid, Vector2 position)
    {
        float startX = -1 * grid.Width / 2 + 0.5f;
        float startY = 1 * grid.Height / 2 + 0.5f;

        float x = position.x - startX;
        float y = startY - position.y;

        return new Point(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
    }
}