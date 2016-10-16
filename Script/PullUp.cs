using UnityEngine;
using System.Collections;

public class PullUp : MonoBehaviour {
	private float timer;
	private bool emitOn;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (emitOn) 
		{
			if (timer > 0) 
			{
				if (!this.GetComponent<EllipsoidParticleEmitter> ().emit)
					this.GetComponent<EllipsoidParticleEmitter> ().emit = true;
				timer -= Time.deltaTime;
			}
			else
			{
				this.GetComponent<EllipsoidParticleEmitter> ().emit = false;
				emitOn = false;
			}
		}
	}

	void OnParticleCollision (GameObject other)
	{
		if (other.tag == "Player" || other.tag == "enermy" || other.layer == 9) 
		{
			if(other.layer == 9)
				other.GetComponent<Rigidbody> ().AddForce (0,150,0);
			else
				other.GetComponent<Rigidbody> ().AddForce (0,300,0);

			if(other.tag == "Player")
				other.GetComponent<PControl> ().DownCntRst ();
		}
	}

	public void Action ()
	{
		emitOn = true;
		timer = 30f;
	}

	public void Dead ()
	{
		this.GetComponent<EllipsoidParticleEmitter> ().emit = false;
		emitOn = false;
		timer = 0;
	}
}
