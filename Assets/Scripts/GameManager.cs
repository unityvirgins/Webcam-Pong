using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

    static int _p1_score = 0;
    static int _p2_score = 0;

    public Text _text;

	// Use this for initialization
	void Start () {
		UpdateUI();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddPoint(string msg)
    {
        switch (msg)
        {
            case "P1":
                _p1_score++;
                break;
            case "P2":
                _p2_score++;
                break;

        }
        UpdateGameStatus();
        UpdateUI();
    }

    private void UpdateUI()
    {
        _text.text = _p1_score+" - "+_p2_score;
    }

    private void UpdateGameStatus()
    {
        if (_p1_score >= 3)
            Debug.Log("Player 1 won");
        if (_p2_score >= 3)
            Debug.Log("Player 2 won");
    }
}
