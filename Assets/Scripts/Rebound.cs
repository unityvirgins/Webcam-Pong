using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebound : MonoBehaviour {

    public float speed = 20f;
    public float factor = 1;

	// Use this for initialization
	void Start () {
        int randomInt = Random.Range(0, 2) * 2 - 1;
        int randomInt2 = Random.Range(0, 2) * 2 - 1;
        this.GetComponent<Rigidbody>().velocity = new Vector3(randomInt * 5, randomInt2 * 2, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (this.GetComponent<Rigidbody>().velocity.x * this.GetComponent<Rigidbody>().velocity.y < 20)
        {
            this.GetComponent<Rigidbody>().velocity = speed * (this.GetComponent<Rigidbody>().velocity.normalized);
        }

        float dot = Vector3.Dot(GetComponent<Rigidbody>().velocity.normalized, Vector3.up);
        if (dot > 0.99f || dot < -0.99f)
        {
            //Vector3 v = Random.insideUnitCircle * factor;
            //GetComponent<Rigidbody>().velocity += v;
            if(GetComponent<Rigidbody>().velocity.x < 0)
            {
                GetComponent<Rigidbody>().velocity -= new Vector3(2.0f, 0, 0);
            } else
            {
                GetComponent<Rigidbody>().velocity += new Vector3(2.0f, 0, 0);
            }
            
        }

    }

    /*void OnCollisionEnter(Collision coll)
    {
        Debug.Log("bump");
        this.GetComponent<Rigidbody>().velocity = new Vector3(-this.GetComponent<Rigidbody>().velocity.x, -this.GetComponent<Rigidbody>().velocity.y, 0);
        Debug.Log(this.GetComponent<Rigidbody>().velocity.y);
    }*/
}
