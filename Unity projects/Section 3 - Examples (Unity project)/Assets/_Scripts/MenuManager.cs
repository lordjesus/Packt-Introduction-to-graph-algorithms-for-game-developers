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

    public void LoadGraphScene()
    {
        SceneManager.LoadScene("specific graph");
    }

    public void LoadRandomGraphScene()
    {
        SceneManager.LoadScene("graph");
    }

    public void LoadGridScene()
    {
        SceneManager.LoadScene("grid");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
