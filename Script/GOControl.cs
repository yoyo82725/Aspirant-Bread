using UnityEngine;
using System.Collections;

public class GOControl : MonoBehaviour {

	public AudioClip clickSE;

	// Use this for initialization
	void Start () {
		this.transform.GetChild (0).localScale = new Vector3 (1,0,1);
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.super) // game clear
			this.gameObject.SetActive (false);
		else 
		{
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				this.Back ();
			}
			else if (Input.GetKeyDown(KeyCode.Escape))
			{
				this.Exit ();
			}
		}
	}

	public void OnePDead ()
	{
		this.transform.GetChild (0).gameObject.SetActive (true);
		this.transform.GetChild (0).localScale = new Vector3 (1,0,1);
	}

	public void TwoPDead ()
	{
		this.transform.GetChild (1).gameObject.SetActive (true);
		this.transform.GetChild (1).localScale = new Vector3 (1,0,1);
	}

	public void Back () // Press Back Btn
	{
		AudioSource.PlayClipAtPoint (clickSE, Vector3.zero, GameManager.clickVol);
		Application.LoadLevel(1);
	}

	public void Exit () // Press Exit Btn
	{
		AudioSource.PlayClipAtPoint (clickSE, Vector3.zero, GameManager.clickVol);
		Application.LoadLevel(0);
	}
}
