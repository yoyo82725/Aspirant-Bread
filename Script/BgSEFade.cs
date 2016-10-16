using UnityEngine;
using System.Collections;

public class BgSEFade : MonoBehaviour {
	public AudioClip iniMu;

	private bool iniFadeIn;
	private bool fadeOut ,fadeIn;

	// Use this for initialization
	void Start () {
		iniFadeIn = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (iniFadeIn) 
		{
			if (this.GetComponent<AudioSource> ().volume < 1)
				this.GetComponent<AudioSource> ().volume += Time.deltaTime;
			else
			{
				this.GetComponent<AudioSource> ().volume = 1;
				iniFadeIn = false;
				this.GetComponent<BgSEFade> ().enabled = false;
			}
		}
		else
		{
			if (fadeOut)
			{
				if (this.GetComponent<AudioSource> ().volume > 0)
					this.GetComponent<AudioSource> ().volume -= Time.deltaTime*0.1f;
				else
				{
					this.GetComponent<AudioSource> ().volume = 0;
					fadeOut = false;
					fadeIn = true;
					this.GetComponent<AudioSource> ().clip = iniMu;
					this.GetComponent<AudioSource> ().Play ();
				}
			}
			else if(fadeIn)
			{
				if (this.GetComponent<AudioSource> ().volume < 1)
					this.GetComponent<AudioSource> ().volume += Time.deltaTime*0.1f;
				else
				{
					this.GetComponent<AudioSource> ().volume = 1;
					fadeIn = false;
					this.GetComponent<BgSEFade> ().enabled = false;
				}
			}
		}
	}

	public void FadOutAndPlay () // fade out and play
	{
		iniFadeIn = false;
		fadeOut = true;
	}
	
}
