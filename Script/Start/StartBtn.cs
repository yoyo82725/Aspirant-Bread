using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartBtn : MonoBehaviour {
	public AudioClip clickSE;

	private bool canSkip;
	private int keyBoardCh = 0;

	// Use this for initialization
	void Start () {
		GameManager.bossed = false;
		GameManager.fin = false;
		Enermy.kill = false;
		this.transform.parent.GetChild (7).GetComponent<Toggle> ().isOn = GameManager.prompt;
		StartCoroutine ("SkipWait");
	}
	
	// Update is called once per frame
	void Update () {
		if (this.GetComponent<Animation> ().isPlaying) 
		{
			if (canSkip && Input.anyKeyDown) 
			{
				this.SkipAn ();
			}
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
			{
				keyBoardCh = (keyBoardCh+1)%5;
				Choose (keyBoardCh);
			}
			else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
			{
				keyBoardCh--;
				if(keyBoardCh > 4)
					keyBoardCh = 0;
				else if(keyBoardCh < 0)
					keyBoardCh = 4;
				Choose (keyBoardCh);
			}
			else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
			{
				if(keyBoardCh < 2)
					keyBoardCh = 4;
				else
					keyBoardCh = 3;
				Choose (keyBoardCh);
			}
			else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
			{
				if(keyBoardCh == 4)
					keyBoardCh = 0;
				else if (keyBoardCh == 3)
					keyBoardCh = 2;
				Choose (keyBoardCh);
			}
		}
	}

	void Choose(int co)
	{
		switch (co) 
		{
		case 0:
			this.transform.parent.GetChild(3).GetComponent<Button> ().Select ();
			break;
		case 1:
			this.transform.parent.GetChild(4).GetComponent<Button> ().Select ();
			break;
		case 2:
			this.transform.parent.GetChild(5).GetComponent<Button> ().Select ();
			break;
		case 3:
			this.transform.parent.GetChild(6).GetComponent<Button> ().Select ();
			break;
		case 4:
			this.transform.parent.GetChild(7).GetComponent<Toggle> ().Select ();
			break;
		}
		this.ClickSE ();
	}

	public void OnPress (int btnCo)
	{
		switch (btnCo) 
		{
		case 1:
			break;
		case 2:
			break;
		case 3:
			Application.Quit ();
			break;
		case 4:
			break;
		}
		this.ClickSE ();
	}

	public void SkipAn ()
	{
		for (int i=0; i<8; i++)
		{
			switch(i)
			{
			case 0:
				this.transform.parent.GetChild(i).GetComponent<Animation> ()["FadeIn"].normalizedTime = 1;
				break;
			case 1:
				this.transform.parent.GetChild(i).GetComponent<Animation> ()["StPanel"].normalizedTime = 1;
				break;
			case 2:
				this.transform.parent.GetChild(i).GetComponent<Animation> ()["StText"].normalizedTime = 1;
				break;
			case 3:
				this.transform.parent.GetChild(i).GetComponent<Animation> ()["StBtn"].normalizedTime = 1;
				break;
			case 4:
				this.transform.parent.GetChild(i).GetComponent<Animation> ()["StBtn2"].normalizedTime = 1;
				break;
			case 5:
				this.transform.parent.GetChild(i).GetComponent<Animation> ()["StBtn3"].normalizedTime = 1;
				break;
			case 6:
				this.transform.parent.GetChild(i).GetComponent<Animation> ()["StBtn4"].normalizedTime = 1;
				break;
			case 7:
				this.transform.parent.GetChild(i).GetComponent<Animation> ()["StBtn5"].normalizedTime = 1;
				break;
			}
		}
	}

	public void ChooseIni ()
	{
		this.transform.parent.GetChild (3).GetComponent<Button> ().Select ();
		keyBoardCh = 0;
	}

	IEnumerator SkipWait ()
	{
		yield return new WaitForSeconds (2f);
		canSkip = true;
	}

	public void ClickSE ()
	{
		AudioSource.PlayClipAtPoint (clickSE, Vector3.zero, GameManager.clickVol);
	}
}
