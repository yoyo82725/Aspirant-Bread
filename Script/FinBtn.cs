using UnityEngine;
using System.Collections;

public class FinBtn : MonoBehaviour {

	public AudioClip clickSE;

	/*
	// Use this for initialization
	void Start () {

	}
	*/

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			this.Exit ();
		}
	}

	public void Exit ()
	{
		if (!this.GetComponent<Animation> ().isPlaying) 
		{
			AudioSource.PlayClipAtPoint (clickSE, Vector3.zero, GameManager.clickVol);
			Application.LoadLevel(0);
		}
	}
}
