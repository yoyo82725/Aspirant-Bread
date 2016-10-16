using UnityEngine;
using System.Collections;

public class FlipPage : MonoBehaviour {

	public AudioClip clickSE;

	private int index;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Exit
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			this.Exit ();
		}

		// Next Page
		else if(Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			this.Flip ();
		}

		// Prev Page
		else if(index > 0 && Input.GetKeyDown (KeyCode.LeftControl))
		{
			this.Prev ();
		}

	}

	public void Init () // trigger In
	{
		index = 0;
		this.transform.GetChild (index).localScale = new Vector3 (1, 0, 1);
		this.transform.GetChild (index).gameObject.SetActive (true);
	}

	public void Exit () // X Btn Down
	{
		//this.transform.GetChild (index).gameObject.SetActive (false);
		//this.gameObject.SetActive (false);
		TeachTrigger.hasOtherOpen = false;
		AudioSource.PlayClipAtPoint (clickSE, Vector3.zero, GameManager.clickVol);
		Destroy (this.transform.parent.gameObject);
	}

	public void Flip () // Enter Btn Down
	{
		this.transform.GetChild (index).gameObject.SetActive (false);
		index ++;
		if (index < this.transform.childCount) 
		{
			AudioSource.PlayClipAtPoint (clickSE, Vector3.zero, GameManager.clickVol);
			this.transform.GetChild (index).localScale = new Vector3 (1, 0, 1);
			this.transform.GetChild (index).gameObject.SetActive (true);
		}
		else
		{
			index --;
			this.Exit ();
		}
	}

	public void Prev () // Prev Down
	{
		AudioSource.PlayClipAtPoint (clickSE, Vector3.zero, GameManager.clickVol);
		this.transform.GetChild (index).gameObject.SetActive (false);
		index --;
		this.transform.GetChild (index).localScale = new Vector3 (1, 0, 1);
		this.transform.GetChild (index).gameObject.SetActive (true);
	}
}
