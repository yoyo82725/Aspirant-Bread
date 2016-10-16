using UnityEngine;
using System.Collections;

public class LightingBall : MonoBehaviour {

	public GameObject boom;
	public GameObject boomSE;


	/*
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	*/

	void OnCollisionEnter(Collision collision)
	{
		Instantiate (boom, this.transform.position, Quaternion.identity);
		GameManager.PlaySEnearP (boomSE, this.transform.position);
		Destroy (this.gameObject);
	}
}
