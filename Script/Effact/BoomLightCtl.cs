using UnityEngine;
using System.Collections;

public class BoomLightCtl : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Light>().intensity -= Time.deltaTime;
	}
}
