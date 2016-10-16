using UnityEngine;
using System.Collections;

public class ExitBtn : MonoBehaviour {
	public UIControl uiControl;

	/*
	// Use this for initialization
	void Start () {
	
	}
	*/
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown (KeyCode.Space))
		{
			uiControl.OpenStUI ();
			uiControl.GetComponent<UIControl> ().ClickSE ();
		}
	}
}
