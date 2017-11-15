using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

public class PuzzleManager : MonoBehaviour {

    [SerializeField]
    private Block[] _Blocks;

    private string _state;

   	// Use this for initialization
	void Start () {
        foreach (Block block in _Blocks)
        {
            block.Clicked += Block_Clicked;
        }
        _state = PuzzleSolver.GenerateRandomSolvableState();

        setBlocksToState(_state);      
    }

    public void ShufflePuzzle()
    {
        _state = PuzzleSolver.GenerateRandomSolvableState();

        setBlocksToState(_state);
    }

    public void SolvePuzzle()
    {
        StartCoroutine(doSolve());        
    }

    private IEnumerator doSolve()
    {
        Stopwatch s = new Stopwatch();

        s.Start();
        List<string> longPath = PuzzleSolver.DepthFirstSearch(_state);
        s.Stop();
        print("DFS took " + s.ElapsedMilliseconds + " ms");
        print("States: " + longPath.Count);

        s.Reset();

        s.Start();
        List<string> path = PuzzleSolver.BreadthFirstSearch(_state);
        s.Stop();
        path.Reverse();
        print("BFS took " + s.ElapsedMilliseconds + " ms");
        print("States: " + path.Count);

        print("Start state: " + _state);
        StartCoroutine(animatePath(path));
        yield return null;
    }

    private void Block_Clicked(object sender, System.EventArgs e)
    {
        Block clickedBlock = (Block)sender;
        print("Clicked on " + clickedBlock.Value);
        _state = getNewState(clickedBlock.Value, _state);
        setBlocksToState(_state);
    }

    private string getNewState(int blockValue, string state)
    {
        int index = state.IndexOf(blockValue.ToString());
        int emptyIndex = state.IndexOf('*');

        int xPos = index % 3;
        int yPos = index / 3;

        int emptyX = emptyIndex % 3;
        int emptyY = emptyIndex / 3;

        if ((Mathf.Abs(xPos - emptyX) == 1 && Mathf.Abs(yPos - emptyY) == 0) || (Mathf.Abs(xPos - emptyX) == 0 && Mathf.Abs(yPos - emptyY) == 1))
        {
            StringBuilder sb = new StringBuilder(state);
            char newChar = sb[index];
            sb[emptyIndex] = newChar;
            sb[index] = '*';
            return sb.ToString();
        }
        return state;
    }    

    private IEnumerator animatePath(List<string> path, float delay = 0.5f)
    {
        foreach (string p in path)
        {
            setBlocksToState(p);
            yield return new WaitForSeconds(delay);
        }
    }

    private Vector3 indexToWorldPosition(int index)
    {
        float startX = -2;
        float startY = 2;
        float cellWidth = 2;
        return new Vector3(startX + index % 3 * cellWidth, startY - index / 3 * cellWidth);
    }

    private void setBlocksToState(string state)
    {
        for (int i = 0; i < state.Length; i++)
        {
            char c = state[i];
            if (c == '*')
            {
                continue;
            }
            int index = (int)char.GetNumericValue(c) - 1;
            LeanTween.move(_Blocks[index].gameObject, indexToWorldPosition(i), 0.25f); 
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
