using UnityEngine;
using System.Collections;

public class DarkSpace : MonoBehaviour {
	public Light worldLight;

	private bool on;
	private bool off;
	
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (off)
		{
			on = false;
			this.GetComponent<MeshRenderer> ().material.color = Color.Lerp (this.GetComponent<MeshRenderer> ().material.color, Color.clear, 0.01f);
			worldLight.color = Color.Lerp(worldLight.color, Color.white, 0.01f);
		}
		else if (on) 
		{
			this.GetComponent<MeshRenderer> ().material.color = Color.Lerp (this.GetComponent<MeshRenderer> ().material.color, Color.black, 0.01f);
			worldLight.color = Color.Lerp(worldLight.color, Color.gray, 0.01f);
		}
	}

	public void On()
	{
		StartCoroutine ("Oning");
	}

	IEnumerator Oning()
	{
		on = true;
		yield return new WaitForSeconds (10f);
		on = false;
		this.GetComponent<MeshRenderer> ().material.color = Color.black;
		worldLight.color = Color.gray;
	}

	public void Off()
	{
		StartCoroutine ("Offing");
	}

	IEnumerator Offing()
	{
		off = true;
		yield return new WaitForSeconds (10f);
		off = false;
		this.GetComponent<MeshRenderer> ().material.color = Color.clear;
		worldLight.color = Color.white;
	}

	void OnDestroy ()
	{
		if(worldLight)
			worldLight.color = Color.white;
	}
}
