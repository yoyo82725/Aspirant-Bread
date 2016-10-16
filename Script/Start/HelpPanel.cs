using UnityEngine;
using System.Collections;

public class HelpPanel : MonoBehaviour {

	/*
	// Use this for initialization
	void Start () {

	}
	*/
	// Update is called once per frame
	void Update () {
		this.transform.localScale = Vector3.Slerp (this.transform.localScale, new Vector3 (1, 0.8f, 1), 0.1f);
	}
}
