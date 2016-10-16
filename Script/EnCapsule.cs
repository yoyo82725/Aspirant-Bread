using UnityEngine;
using System.Collections;

public class EnCapsule : MonoBehaviour {
	public static bool dead = false;
	public GameObject boom;
	public GameObject enermy;
	public GameObject boomSE;

	private float timer = 1f;

	// Use this for initialization
	void Start () {
		this.GetComponent<Rigidbody> ().AddForce (-1000, 500, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (dead) 
		{
			Instantiate (boom, this.transform.position, Quaternion.identity);
			Destroy (this.gameObject);
		}
		else
		{
			this.transform.Rotate (0, 0, 5f);
			if (timer > 0) 
			{
				timer -= Time.deltaTime;
			}
			else
			{
				timer = 0.5f;
				this.Burn ();
				EnermyStone.enCount ++;
			}
		}
	}

	void OnCollisionEnter (Collision collision)
	{
		this.Burn ();
		if (collision.gameObject.layer == 9) 
		{
			collision.gameObject.GetComponent<Rigidbody> ().AddForce ((collision.transform.position - this.transform.position).normalized * 500);
		}
		if (collision.transform.tag == "Player" || collision.transform.tag == "enermy") 
		{
			collision.gameObject.SendMessage ("GetDmg");
		}
		Destroy (this.gameObject);
	}

	void Burn()
	{
		Instantiate (boom, this.transform.position, Quaternion.identity);
		GameObject tmpEn;
		tmpEn = Instantiate (enermy, this.transform.position + new Vector3(0,2,0), Quaternion.identity) as GameObject;
		tmpEn.GetComponent<Rigidbody> ().AddForce (0,500,0);
		Physics.IgnoreCollision (this.GetComponent<Collider> (), tmpEn.GetComponent<Collider> ());
		GameManager.PlaySEnearP (boomSE, this.transform.position);
	}
}
