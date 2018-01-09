using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebound : MonoBehaviour {

    public float speed = 20f;

	// Use this for initialization
	void Start () {
        this.GetComponent<Rigidbody>().velocity = new Vector3(-2, 5, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (this.GetComponent<Rigidbody>().velocity.x * this.GetComponent<Rigidbody>().velocity.y < 20)
        {
            this.GetComponent<Rigidbody>().velocity = speed * (this.GetComponent<Rigidbody>().velocity.normalized);
        }
    }

    /*void OnCollisionEnter(Collision coll)
    {
        Debug.Log("bump");
        this.GetComponent<Rigidbody>().velocity = new Vector3(-this.GetComponent<Rigidbody>().velocity.x, -this.GetComponent<Rigidbody>().velocity.y, 0);
        Debug.Log(this.GetComponent<Rigidbody>().velocity.y);
    }*/
}
