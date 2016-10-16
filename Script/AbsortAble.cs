using UnityEngine;
using System.Collections;

public class AbsortAble : MonoBehaviour {
	public GameObject colBoom; // collisoin boom
	public float chiMaxEmit;
	public bool chiEmit;
	public bool isShoot;
	public bool plrGet;

	public GameObject boomSE;

	// can change
	/*
	// Use this for initialization
	void Start () {
		
	}
	*/
	// Update is called once per frame
	void Update () {
		if (this.transform.childCount > 0)
		{
			this.transform.GetChild (0).GetComponent<ParticleEmitter>().maxEmission = chiMaxEmit;
			this.transform.GetChild (0).GetComponent<ParticleEmitter>().emit = chiEmit;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		// Aready fired
		if (!this.GetComponent<Collider>().isTrigger) {
			#region particle
			if(collision.transform.GetComponent<Renderer>())
				colBoom.GetComponent<ParticleRenderer>().material = collision.transform.GetComponent<MeshRenderer>().material;
			else
				colBoom.GetComponent<ParticleRenderer>().material = this.GetComponent<MeshRenderer>().material;
			Instantiate(colBoom, this.transform.position, Quaternion.identity);
			#endregion
			#region hit
			if(collision.transform.tag == "enermy")
			{
				collision.rigidbody.AddForce(this.GetComponent<Rigidbody>().velocity*50);
				collision.transform.GetComponent<Enermy>().GetDmg();
			}
			else
			{
				if(collision.transform.name == "Nose")
				{
					collision.transform.parent.GetComponent<CubeBoss>().HitNose(40f);
				}
				else if(collision.transform.name == "BossCube")
				{
					collision.transform.GetComponent<CubeBoss>().GetDmg(6f);
				}
			}
			#endregion
			#region affect range
			Collider[] colliders = Physics.OverlapSphere(this.transform.position, 10f);
			foreach(Collider hit in colliders)
			{
				if(hit.transform.tag == "enermy")
				{
					hit.transform.GetComponent<Enermy>().GetDmg();
					hit.GetComponent<Rigidbody>().AddExplosionForce(1000,this.transform.position,10f,500);
				}
				else if(hit.transform.tag == "Player")
				{
					hit.transform.GetComponent<PControl>().GetDmg();
					hit.GetComponent<Rigidbody>().AddExplosionForce(1000,this.transform.position,10f,500);
				}
				else if(hit.gameObject.layer == 8)
				{
					hit.GetComponent<BreakWall> ().RequstClose ();
				}
				else
				{
					if(hit.transform.name == "BossCube")
					{
						hit.transform.GetComponent<CubeBoss>().GetDmg(3f);
					}
					/*
					if(hit.transform.name == "Nose")
					{
						hit.transform.parent.GetComponent<CubeBoss>().HitNose(25f);
					}
					*/
				}
			}
			#endregion

			if(collision.transform.tag != "absortAble")
				GameManager.PlaySEnearP (boomSE, this.transform.position);
			Destroy(this.gameObject);
		}
	}

	public void ReadyShot()
	{
		this.GetComponent<AbsortAble>().isShoot = true;
		this.GetComponent<Rigidbody>().useGravity = true;
		this.GetComponent<Collider>().isTrigger = false;
	}
}
