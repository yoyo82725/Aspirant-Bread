using UnityEngine;
using System.Collections;

public class SEDelete : MonoBehaviour {
	/*
	// Use this for initialization
	void Start () {
	
	}
	*/
	// Update is called once per frame
	void Update () {
		if (!this.GetComponent<AudioSource> ().isPlaying) 
		{
			Destroy (this.gameObject);
		}
	}
}
