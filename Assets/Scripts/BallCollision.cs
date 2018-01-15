using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour {

    private GameObject GM;


	// Use this for initialization
	void Start () {
        GM = GameObject.Find("Game Manager");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.name)
        {
            case "Goal_P1":
                Debug.Log("But du Joueur 1");
                GM.GetComponent<GameManager>().AddPoint("P1");
                break;
            case "Goal_P2":
                Debug.Log("But du Joueur 2");
                GM.GetComponent<GameManager>().AddPoint("P2");
                break;
        }

        Destroy(this.gameObject);
    }
}
