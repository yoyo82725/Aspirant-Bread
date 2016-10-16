using UnityEngine;
using System.Collections;

public class EnermyStone : MonoBehaviour {
	public static int enCount; // enermyCount :Enermy.cs
	public Material iniMat, deadMat;

	public GameObject enermy;
	public GameObject boom;

	private Transform burnSphere;
	private bool sphereDown,sphereAlpha;
	// Use this for initialization
	void Start () {
		burnSphere = this.transform.GetChild (0);
		this.Burn ();
	}
	
	// Update is called once per frame
	void Update () {
		if(sphereDown)
		{
			burnSphere.localScale = Vector3.Lerp (burnSphere.localScale, new Vector3(2.5f,2.5f,2.5f), 0.1f);
		}
		else if(sphereAlpha)
		{
			burnSphere.GetComponent<MeshRenderer>().material.Lerp(burnSphere.GetComponent<MeshRenderer>().material, deadMat, 0.1f);
		}
	}

	public void ToEnermy ()
	{
		Instantiate (boom, this.transform.position, Quaternion.identity);
		Instantiate (enermy, this.transform.position, Quaternion.identity);
		enCount++;
		this.gameObject.SetActive (false);
	}

	public void Burn()
	{
		StartCoroutine ("Burning");
	}

	IEnumerator Burning()
	{
		this.GetComponent<MeshRenderer> ().enabled = false;
		burnSphere.gameObject.SetActive (true);
		burnSphere.localScale = new Vector3 (10, 10, 10);
		burnSphere.GetComponent<MeshRenderer> ().material = iniMat;
		sphereDown = true;
		yield return new WaitForSeconds(1f);

		sphereDown = false;
		this.GetComponent<MeshRenderer> ().enabled = true;
		sphereAlpha = true;
		yield return new WaitForSeconds(1f);

		sphereAlpha = false;
		burnSphere.gameObject.SetActive (false);
	}
}
