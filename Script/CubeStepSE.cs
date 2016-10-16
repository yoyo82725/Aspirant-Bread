using UnityEngine;
using System.Collections;

public class CubeStepSE : MonoBehaviour {

	public GameObject ftStepSE;

	/*
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

	}
	*/

	public void FootStepSE ()
	{
		// 在地上才撥
		RaycastHit hit;
		if (Physics.Raycast (this.transform.position, Vector3.down, out hit, 1f) && (hit.transform.tag == "map" || hit.transform.tag == "enermy" || hit.transform.tag == "Player")) 
		{
			GameManager.PlaySEnearP (ftStepSE, this.transform.position);
		}
	}
}
