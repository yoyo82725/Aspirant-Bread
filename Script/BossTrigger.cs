using UnityEngine;
using System.Collections;

public class BossTrigger : MonoBehaviour {
	public GameObject front;
	public GameObject boss;
	public GameObject player;
	public GameObject player2;
	public GameObject bossAppear;
	public GameObject bossNameCan;
	public GameObject bossHpBar;
	public GameObject dmgPrompt;
	public AudioSource bgm;
	public AudioClip bossBgm;
	public Camera onePCam,twoPCam;

	private bool canSkip;
	/*
	// Use this for initialization
	void Start () {
	
	}
	*/
	// Update is called once per frame
	void Update () {
		if (canSkip)
		{
			if(Input.anyKeyDown)
			{
				this.End ();
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
			StartCoroutine ("BossIn");
		if (GameManager.twoP) 
		{
			player.transform.position = other.transform.position;
			player2.transform.position = other.transform.position;
		}
	}

	IEnumerator BossIn()
	{
		GameManager.bossed = true;
		Destroy (front.gameObject);
		if (player) 
		{
			player.SetActive (false);
			if(player.transform.GetChild(1).localScale != new Vector3(10,10,10))
				player.transform.GetChild(1).localScale = Vector3.one;
		}
		onePCam.GetComponent<Cam> ().ChangeMode (1);
		if (GameManager.twoP) 
		{
			if (player2)
			{
				player2.SetActive(false);
				if(player2.transform.GetChild(1).localScale != new Vector3(10,10,10))
					player2.transform.GetChild(1).localScale = Vector3.one;
			}
			twoPCam.enabled = false;
			onePCam.rect = new Rect(0,0,1,1);
		}
		boss.SetActive (true);
		bossNameCan.SetActive (true);
		bgm.Stop ();
		yield return new WaitForSeconds(1f);
		bgm.clip = bossBgm;
		bgm.volume = 0.7f;
		bgm.Play ();
		yield return new WaitForSeconds(2f);
		canSkip = true;
		yield return new WaitForSeconds(5f);
		this.End ();
	}

	void End()
	{
		onePCam.GetComponent<Cam> ().ChangeMode (0);
		if(player)
			player.SetActive (true);
		if (GameManager.twoP) 
		{
			if (player2)
				player2.SetActive(true);
			twoPCam.enabled = true;
			onePCam.rect = new Rect (0,0,1,0.5f);
		}
		bossAppear.SetActive (true);
		bossHpBar.SetActive (true);
		dmgPrompt.SetActive (true);
		boss.GetComponent<CubeBoss> ().StartAI ();
		Destroy (this.gameObject);
	}
}
