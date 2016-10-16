using UnityEngine;
using System.Collections;

public class TwoPDel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!GameManager.twoP)
			Destroy (this.gameObject);
	}

	/*
	// Update is called once per frame
	void Update () {
	
	}
	*/

}
