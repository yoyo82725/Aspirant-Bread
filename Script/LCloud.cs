using UnityEngine;
using System.Collections;

public class LCloud : MonoBehaviour {
	public GameObject ball;
	
	private Transform launcher;
	private float timer;
	private bool active,close;

	// Use this for initialization
	void Start () {
		launcher = this.transform.GetChild (0);
	}
	
	// Update is called once per frame
	void Update () {
		if (close) 
		{
			active = false;
			this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.zero, 0.1f);
		}
		else if (active) 
		{
			if(this.transform.localScale == new Vector3(10,10,10))
			{
				if(timer <= 0)
				{
					// position
					launcher.localPosition = new Vector3(Random.Range(-4f,5f),0,0);
					// launcher
					Instantiate (ball, launcher.position, Quaternion.identity);
					// cool down
					timer = 0.2f;
				}
				else
					timer -= Time.deltaTime;
			}else
				this.transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(10,10,10), 0.2f);
		}
	}

	public void Action()
	{
		active = true;
		timer = 0f;
	}

	public void Exit()
	{
		StartCoroutine ("Close");
	}

	IEnumerator Close()
	{
		close = true;
		yield return new WaitForSeconds(1f);
		close = false;
	}
}
