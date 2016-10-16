using UnityEngine;
using System.Collections;

public class TeachTrigger : MonoBehaviour {
	private bool isShow;
	public static bool hasOtherOpen;
	
	// Use this for initialization
	void Start () {
		if (!GameManager.prompt)
			Destroy (this.gameObject);
		hasOtherOpen = false;
	}

	/*
	// Update is called once per frame
	void Update () {
	
	}
	*/

	void OnTriggerEnter (Collider other)
	{
		if (!isShow && other.tag == "Player" && !hasOtherOpen) 
		{
			this.transform.GetChild(0).gameObject.SetActive (true);
			this.transform.GetChild(0).GetComponent<FlipPage> ().Init();
			isShow = true;
			hasOtherOpen = true;
		}
	}
}
