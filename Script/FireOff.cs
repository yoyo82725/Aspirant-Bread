using UnityEngine;
using System.Collections;

public class FireOff : MonoBehaviour {
	private Vector3 maxScale = new Vector3 (31.2722f, 31.2722f, 31.2722f);
	private bool toBig;
	private bool kill;
	private float rotateBuf;
	private bool dead;

	// Use this for initialization
	void Start () {
		this.transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		if (!dead) 
		{
			if (toBig) 
			{
				this.transform.localScale = Vector3.Lerp (this.transform.localScale, maxScale, 0.1f);
				if(this.transform.localScale.x > 31)
				{
					rotateBuf = Mathf.Lerp (rotateBuf, 1f, 0.01f);
					this.transform.Rotate (0,rotateBuf,0);
				}
			}
			else if (kill)
			{
				if(this.transform.localScale.x > 2f)
					this.transform.localScale = Vector3.Lerp (this.transform.localScale, Vector3.zero, 0.01f);
				else
					this.transform.localScale = Vector3.Lerp (this.transform.localScale, Vector3.zero, 0.1f);
				rotateBuf = Mathf.Lerp (rotateBuf, 7f, 0.01f);
				this.transform.Rotate (0,rotateBuf,0);
				if(this.transform.localScale.x < 0.1f)
				{
					this.End ();
				}
			}
		}
		else
		{
			this.transform.localScale = Vector3.Lerp (this.transform.localScale, Vector3.zero, 0.01f);
			if (this.transform.localScale.x < 0.3f)
				this.End ();
		}
	}

	public void ToBig ()
	{
		toBig = true;
	}

	public void Kill ()
	{
		kill = true;
		toBig = false;
	}

	public void Dead ()
	{
		dead = true;
	}

	void End ()
	{
		kill = false;
		this.transform.localScale = Vector3.zero;
		rotateBuf = 0;
		this.gameObject.SetActive (false);
	}

	void OnTriggerStay(Collider other)
	{
		if (kill && (other.tag == "Player" || other.tag == "enermy")) 
		{
			other.GetComponent<Rigidbody> ().AddForce ((other.transform.position - this.transform.position).normalized * -1000);
			other.SendMessage("GetDmg");
			other.transform.GetChild(0).GetComponent<Animation>().CrossFade ("CubeDeadRota");
		}
	}
}
