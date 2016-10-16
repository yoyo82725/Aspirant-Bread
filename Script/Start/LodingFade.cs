using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LodingFade : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<Image> ().color = Color.clear;
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<CanvasRenderer> ().SetAlpha (Mathf.Lerp (this.GetComponent<Image> ().color.a, 1, 0.1f));
	}
}
