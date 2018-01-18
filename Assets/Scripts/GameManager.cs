using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

    static int _p1_score = 0;
    static int _p2_score = 0;

    public Text _text;

    public Text _introText;

    public Transform ball;
    public Transform ball_spawner;

    bool multiball = false;

	// Use this for initialization
	void Start () {
		UpdateUI();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("newBall"))
        {
            Instantiate(ball, ball_spawner);
            if (_introText.IsActive())
            {
                _introText.gameObject.SetActive(false);
            } else
            {
                multiball = true;
            }
        }
	}

    public void AddPoint(string msg)
    {
        switch (msg)
        {
            case "P1":
                _p2_score++;
                if (!multiball)
                {
                    Instantiate(ball, ball_spawner);
                }
                break;
            case "P2":
                _p1_score++;
                if (!multiball)
                {
                    Instantiate(ball, ball_spawner);
                }
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
