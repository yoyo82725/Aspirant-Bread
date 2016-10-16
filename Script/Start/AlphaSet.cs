using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AlphaSet : MonoBehaviour {

	/*
	// Use this for initialization
	void Start () {
	
	}
	*/
	// Update is called once per frame
	void Update () {
		this.GetComponent<CanvasRenderer> ().SetAlpha (this.GetComponent<Image> ().color.a);
	}
}
