using UnityEngine;
using System.Collections;

public class StopPlayer : MonoBehaviour {

	/*
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	*/

	void OnCollisionEnter (Collision collision)
	{

		if (collision.transform.tag == "Player") {
			if (this.name == "Back")
			{
				collision.transform.position = new Vector3 (200,collision.transform.position.y,0);
				collision.transform.GetComponent<Rigidbody> ().AddForce (0,-1500,0);
			}
			else if(this.name == "Up")
			{
				collision.transform.position = new Vector3 (collision.transform.position.x,137,0);
				collision.transform.GetComponent<Rigidbody> ().AddForce (0,-1500,0);
			}
			else if(this.name == "Middle")
			{
				collision.transform.position = new Vector3 (205,17,0);
				collision.transform.GetComponent<Rigidbody> ().AddForce (0,-1500,0);
			}
			else if(this.name == "Bottom")
				collision.transform.position = new Vector3 (collision.transform.position.x,15,0);
			else if(this.name == "Front")
				collision.transform.position = new Vector3 (348,15,0);
		}

	}

}
