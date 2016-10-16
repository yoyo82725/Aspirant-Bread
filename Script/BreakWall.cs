using UnityEngine;
using System.Collections;

public class BreakWall : MonoBehaviour {
	public Material iniMat,deadMat;
	private bool open,close;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(close)
		{
			open = false;
			this.GetComponent<MeshRenderer> ().material.Lerp (this.GetComponent<MeshRenderer> ().material, deadMat, 0.1f);
			//this.transform.localScale = Vector3.Lerp (this.transform.localScale, Vector3.zero, 0.1f);
		}
		else if (open) 
		{
			this.transform.localScale = Vector3.Lerp (this.transform.localScale, new Vector3 (1, 60, 60), 0.1f);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "absortAble" && collision.transform.GetComponent<AbsortAble> ().isShoot) 
		{
			StartCoroutine("Close");
		}
	}

	public void Action()
	{
		StartCoroutine("Open");
	}

	public void RequstClose()
	{
		StartCoroutine("Close");
	}

	IEnumerator Open()
	{
		this.GetComponent<MeshRenderer> ().material = iniMat;
		open = true;
		yield return new WaitForSeconds(1f);
		open = false;
	}

	IEnumerator Close()
	{
		if (!close) 
		{
			close = true;
			yield return new WaitForSeconds (1f);
			this.transform.localScale = Vector3.zero;
			close = false;
			this.gameObject.SetActive (false);
		}
	}
}
