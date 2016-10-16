using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIControl : MonoBehaviour {
	public GameObject stUI;
	public GameObject helpUI;
	public GameObject lodingUI;
	public AudioClip clickSE;
	public Toggle prompt;
	/*
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	*/
	public void OpenHelp ()
	{
		stUI.SetActive (false);
		helpUI.SetActive (true);
		helpUI.transform.GetChild(0).GetComponent<RectTransform> ().localScale = new Vector3 (1,0,1);
	}

	public void OpenStUI ()
	{
		stUI.SetActive (true);
		stUI.transform.GetChild(7).GetComponent<StartBtn> ().ChooseIni ();
		stUI.transform.GetChild (stUI.transform.childCount-1).GetComponent<StartBtn> ().SkipAn ();
		helpUI.SetActive (false);
	}

	public void OpenLoding (bool twoP)
	{
		lodingUI.SetActive (true);
		stUI.SetActive (false);
		GameManager.prompt = prompt.isOn;
		GameManager.twoP = twoP;
		StartCoroutine ("LoadOne");
	}

	IEnumerator LoadOne ()
	{
		yield return new WaitForSeconds (3f);
		Application.LoadLevel(1);
	}

	public void ClickSE ()
	{
		AudioSource.PlayClipAtPoint (clickSE, Vector3.zero, GameManager.clickVol);
	}
}
