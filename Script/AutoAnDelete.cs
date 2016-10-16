using UnityEngine;
using System.Collections;

public class AutoAnDelete : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!this.GetComponent<Animation>().isPlaying)
			Destroy (this.transform.root.gameObject);
	}
}
