using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadNoGhosts()
    {
        LoadLevel("NoGhosts");
    }

    public void LoadRandomMovement()
    {
        LoadLevel("RandomMovement");
    }

    public void LoadPathfindingWithLag()
    {
        LoadLevel("Pathfinding");
    }

    public void LoadFleeingFromPlayer()
    {
        LoadLevel("Fleeing");
    }

    public void LoadTheFinalGame()
    {
        LoadLevel("Main");
    }
}
