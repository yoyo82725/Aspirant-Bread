using UnityEngine;
using System.Collections;

public class FallDown : MonoBehaviour {

	public GameObject boom;
	public GameObject boomSE;

	private bool firstHit = false;

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
		if (this.GetComponent<Rigidbody> ().velocity.y < -15) 
		{
			if(collision.transform.tag == "Player" || collision.transform.tag == "enermy")
			{
				collision.gameObject.SendMessage ("GetDmg");
			}
			GameManager.PlaySEnearP (boomSE, this.transform.position);
		}
		else if(!firstHit)
		{
			if(collision.transform.tag == "map")
				GameManager.PlaySEnearP (boomSE, this.transform.position);
			firstHit = true;
		}
	}

	void Boom ()
	{
		Instantiate (boom, this.transform.position, Quaternion.identity);
		GameManager.PlaySEnearP (boomSE, this.transform.position);
		Destroy (this.gameObject);
	}
}
