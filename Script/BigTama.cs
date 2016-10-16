using UnityEngine;
using System.Collections;

public class BigTama : MonoBehaviour {
	public Material deadMat;
	public float chargeCnt;
	public GameObject chargeSE, hitSE;

	private GameObject waitDelSE;
	private float radiusBns; // radius bonus
	private bool toBig;
	private float chargeTimer;
	private float chargeBuff;
	private float parEmtCnt; // particle emit count
	private bool charging;
	private bool shooting;
	private bool fadeOut;
	private bool hitted;
	private bool dead;

	void Start () {
		this.transform.localScale = Vector3.zero;
		if(!IsInvoking("ChangeSize"))
			InvokeRepeating ("ChangeSize", 0f, 0.5f);
		this.GetComponent<EllipsoidParticleEmitter> ().maxEmission = 0;
		this.GetComponent<EllipsoidParticleEmitter> ().minEmission = 0;
		this.GetComponent<Light> ().range = 0f;
		charging = true;
		waitDelSE = GameManager.PlySEnerPRG (chargeSE, this.transform.position);
	}

	void Update () {
		if (!dead) 
		{
			if (!shooting)
			{
				// change size
				if (toBig) 
				{
					radiusBns += Time.deltaTime;
					if (charging)
						parEmtCnt = Mathf.Lerp(parEmtCnt, 50f, 0.01f);
				}
				else
				{
					radiusBns -= Time.deltaTime;
					if (charging)
						parEmtCnt = Mathf.Lerp(parEmtCnt, 100f, 0.01f);
				}
				chargeBuff = Mathf.Lerp (chargeBuff, chargeCnt, 0.01f);
				float tmpRad = chargeBuff*0.7f+radiusBns;
				this.transform.localScale = new Vector3 (tmpRad, tmpRad, tmpRad);
				this.transform.localPosition = new Vector3 (chargeBuff*0.25f,chargeBuff*0.25f);
				
				if (charging)
				{
					this.GetComponent<EllipsoidParticleEmitter> ().maxEmission = parEmtCnt;
					this.GetComponent<EllipsoidParticleEmitter> ().minEmission = parEmtCnt;
					this.GetComponent<Light> ().range = chargeBuff * 30f;

					if (!waitDelSE)
					{
						waitDelSE = GameManager.PlySEnerPRG (chargeSE, this.transform.position);
					}
					// plus charge
					if (chargeTimer <= 0) 
					{
						chargeCnt++;
						chargeTimer = 2f;
					}
					else
						chargeTimer -= Time.deltaTime;
				}
			}
			else if (fadeOut)
			{
				{
					this.GetComponent<Light>().intensity -= Time.deltaTime*0.1f;
					this.GetComponent<Renderer>().material.Lerp (this.GetComponent<Renderer>().material, deadMat, 0.01f);
				}
			}
		} 
		else
		{
			this.transform.localScale = Vector3.Lerp (this.transform.localScale, Vector3.zero, 0.01f);
		}
	}

	void ChangeSize()
	{
		toBig = !toBig;
	}

	public void ReadyForShoot()
	{
		this.GetComponent<EllipsoidParticleEmitter> ().emit = false;
		this.gameObject.AddComponent<SphereCollider> ();
		this.GetComponent<SphereCollider>().isTrigger = true;
		this.GetComponent<Light> ().flare = null;
		charging = false;
		if (waitDelSE)
			Destroy (waitDelSE.gameObject);
	}

	public void CutCharge()
	{
		chargeCnt -= 0.5f;
		chargeBuff = chargeBuff * 0.5f;
	}

	public void Dead () // 強制中斷
	{
		dead = true;
		this.GetComponent<EllipsoidParticleEmitter> ().emit = false;
		this.GetComponent<Light> ().flare = null;
		StartCoroutine("Hit");
	}

	public void Shooting()
	{
		shooting = true;
	}

	/*
	void OnCollisionEnter(Collision collision)
	{
		if (!charging) 
		{
			if (collision.transform.tag == "map" || collision.transform.tag == "enermy" || collision.transform.tag == "Player")
			{
				StartCoroutine("Hit");
			}
		}
	}
	*/

	void OnTriggerEnter(Collider other)
	{
		if (!hitted && shooting)
		{
			StartCoroutine ("Hit");
			hitted = true;
		}
		else
		{
			if (other.tag == "Player" || other.tag == "enermy")
			{
				other.GetComponent<Rigidbody>().AddExplosionForce (1500 ,this.transform.position,this.transform.localScale.x*0.5f ,1f);
				other.SendMessage("GetDmg");
				GameManager.PlaySEnearP (hitSE, other.transform.position);
			}
			else if(other.tag == "absortAble")
			{
				Destroy (other.gameObject);
				GameManager.PlaySEnearP (hitSE, other.transform.position);
			}
			else if(other.tag == "map")
			{
				if(other.gameObject.layer == 8)// SpWall
				{
					other.GetComponent<BreakWall> ().RequstClose ();
					GameManager.PlaySEnearP (hitSE, other.transform.position);
				}
				else if(other.gameObject.layer == 9) // FallIron
					other.SendMessage ("Boom");
			}
		}
	}

	IEnumerator Hit()
	{
		yield return new WaitForSeconds(5f);
		fadeOut = true;
		yield return new WaitForSeconds(10f);
		Destroy (this.gameObject);
	}
}
