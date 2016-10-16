using UnityEngine;
using System.Collections;

public class Enermy : MonoBehaviour {
	public static bool kill;
	public GameObject boomObj;
	public GameObject amazingObj;
	public GameObject boomSE;
	public GameObject atkHitSE, amazingSE;
	public Material deadMat;

	// can change
	private float speed = 5f;
	private float srhRange = 15f;
	private float tBTimeIni = 0.55f;
	private float deadRadius = 5f;

	#region can't change var
	private Transform eCube,deadSphere;
	// hit
	private bool hitted;
	// move & direction
	private float emuGA; // emu Get Axis
	private bool move;
	private float refSmCV;
	private bool dirToR;
	private bool turnBack; // use StartCoroutine("TurnBack");
	// Action
	private int rndAct,preAct,repActCnt;
	// Find Player
	private bool searchPlayer,isFindPlayer;
	private bool atking;
	private float tBCoolTime;
	#endregion

	// Use this for initialization
	void Start () {
		dirToR = true;
		preAct = -1;
		eCube = this.transform.GetChild (0);
		deadSphere = this.transform.GetChild (1);

		this.StartAction ();
	}
	
	// Update is called once per frame
	void Update () {
		#region hitted
		if(hitted)
		{
			// count down and close;
			if(this.GetComponent<Rigidbody>().velocity == Vector3.zero && (Physics.Raycast (this.transform.position,Vector3.down,1f) || Physics.Raycast (this.transform.position,new Vector3(0.5f,-1,0),1f) || Physics.Raycast (this.transform.position,new Vector3(-0.5f,-1,0),1f)))
			{
				// Color Blink
				float r = eCube.GetComponent<Renderer>().material.color.r;
				float g = eCube.GetComponent<Renderer>().material.color.g;
				float b = eCube.GetComponent<Renderer>().material.color.b;
				Vector3 rgb = new Vector3(r,g,b);
				rgb = Vector3.Lerp(rgb, new Vector3(0.5f,0.5f,0.5f), 0.1f);
				r = rgb.x;
				g = rgb.y;
				b = rgb.z;
				eCube.GetComponent<Renderer>().material.color = new Color(r,g,b);
				deadSphere.localScale = Vector3.Slerp (deadSphere.localScale ,new Vector3(deadRadius+deadRadius,deadRadius+deadRadius,deadRadius+deadRadius) ,0.05f);
				if(eCube.GetComponent<Renderer>().material.color == Color.gray)
				{
					Collider[] colliders = Physics.OverlapSphere(this.transform.position, deadRadius);
					foreach(Collider hit in colliders)
					{
						if(hit.transform.tag == "enermy" && Physics.Linecast(this.transform.position, hit.transform.position))
						{
							hit.transform.GetComponent<Enermy>().GetDmg();
							hit.GetComponent<Rigidbody>().AddExplosionForce(1000,this.transform.position,deadRadius,500);
						}
						else if(hit.transform.tag == "Player" && Physics.Linecast(this.transform.position, hit.transform.position))
						{
							hit.transform.GetComponent<PControl>().GetDmg();
							hit.GetComponent<Rigidbody>().AddExplosionForce(1000,this.transform.position,deadRadius,500);
						}
						else
						{
							if(hit.transform.name == "BossCube")
							{
								hit.transform.GetComponent<CubeBoss>().GetDmg(5f);
							}
							/*
							if(hit.transform.name == "Nose")
							{
								hit.transform.parent.GetComponent<CubeBoss>().HitNose(25f);
							}
							*/
						}
					}
					hitted = false;
					boomObj.GetComponent<Renderer>().material = eCube.GetComponent<Renderer>().material;
					Instantiate (boomObj, this.transform.position, Quaternion.identity);
					GameManager.PlaySEnearP (boomSE, this.transform.position);
					if(EnermyStone.enCount > 0)
						EnermyStone.enCount--;
					Destroy (this.gameObject);
				}
			}
		}
		#endregion
		else
		{
			#region MOVE+Direction & ATK & Turn
			if(move)
			{
				if(dirToR)
					emuGA = Mathf.SmoothDamp(emuGA,0.5f,ref refSmCV,0.1f);
				else
					emuGA = Mathf.SmoothDamp(emuGA,-0.5f,ref refSmCV,0.1f);
				
				// Move
				this.transform.Translate (emuGA* speed* Time.deltaTime,0,0,Space.World);
				
				// Direction
				if(emuGA > 0)
					this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, Vector3.zero, 0.1f);
				else
					this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, new Vector3(0f,180f,0f), 0.1f);
				
				// Animate
				eCube.GetComponent<Animation>()["CubeRun"].speed = Mathf.Abs(emuGA);
				eCube.GetComponent<Animation>().CrossFade("CubeRun");

				// reflex
				if(tBCoolTime <= 0f)
				{
					RaycastHit reflexHit;
					if (Physics.Raycast (this.transform.position, this.transform.right, out reflexHit, 1f))
					{
						if(reflexHit.transform.tag != "Player")
						{
							tBCoolTime = tBTimeIni;
							this.GetComponent<Rigidbody>().AddForce(new Vector3(0,150,0));
							StartCoroutine("TurnBack",false);
						}
					}
				}
			}
			else if(atking)
			{
				// fix Direction
				if (dirToR)
					this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, Vector3.zero, 0.1f);
				else
					this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, new Vector3(0f,180f,0f), 0.1f);
				// Attack
				eCube.GetComponent<Animation>().CrossFade("CubeAtk");
				if(dirToR)
					emuGA = Mathf.SmoothDamp(emuGA,0.5f,ref refSmCV,0.1f);
				else
					emuGA = Mathf.SmoothDamp(emuGA,-0.5f,ref refSmCV,0.1f);
				this.transform.Translate (emuGA* speed* 6* Time.deltaTime,0,0,Space.World);

				// reflex
				if(tBCoolTime <= 0f)
				{
					RaycastHit reflexHit;
					if (Physics.Raycast (this.transform.position, this.transform.right, out reflexHit, 1f))
					{
						if(reflexHit.transform.tag != "Player")
						{
							if(reflexHit.transform.tag == "Enermy")
								reflexHit.transform.GetComponent<Rigidbody> ().AddForce((reflexHit.transform.position - this.transform.position).normalized * 500);
							GameManager.PlaySEnearP (atkHitSE, this.transform.position);

							tBCoolTime = tBTimeIni;
							this.GetComponent<Rigidbody>().AddForce(new Vector3(0,250,0));
							StartCoroutine("TurnBack",false);
						}
					}
				}
			}
			else
			{
				emuGA = Mathf.SmoothDamp(emuGA,0f,ref refSmCV,0.1f);
				eCube.GetComponent<Animation>().CrossFade("CubeIdle");
			}
			#endregion
			
			#region Turn Back (use StartCoroutine("TurnBack");)
			if (tBCoolTime > 0)
				tBCoolTime -= Time.deltaTime;
			if(turnBack)
			{
				if (dirToR)
					this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, Vector3.zero, 0.1f);
				else
					this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, new Vector3(0f,180f,0f), 0.1f);
				if(!atking)
				{
					//eCube.animation["CubeRun"].speed = 0.1f;
					eCube.GetComponent<Animation>().CrossFade("CubeRun");
				}
			}
			
			#endregion
			
			#region Search Player
			if (searchPlayer)
			{
				RaycastHit srhHit;
				if (Physics.Raycast (this.transform.position, this.transform.right, out srhHit, srhRange) && srhHit.transform.tag == "Player")
				{
					this.StopAction ();
					isFindPlayer = true;
					// Jump
					this.GetComponent<Rigidbody>().AddForce(new Vector3(0,230,0));
					this.Amazing();
					StartCoroutine("Atk");
				}
			}
			
			#endregion

			if (kill) // boss end used
			{
				this.GetDmg ();
			}
		}
	}

	public void GetDmg()
	{
		hitted = true;
		this.tag = "map";
		this.StopAction ();
		this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		eCube.GetComponent<Renderer>().material = deadMat;
	}
	void RndAction()
	{
		searchPlayer = true;
		rndAct = Random.Range (0, 2);
		// limit two times
		if (preAct == rndAct)
		{
			repActCnt++;
		}
		else
		{
			repActCnt = 0;
		}
		if(repActCnt > 1)
		{
			rndAct = (rndAct+1)%2;
		}
		preAct = rndAct;

		// Action
		if (rndAct==0)
		{
			// Move
			move = true;
			if (Random.Range(0,2)==0)
				dirToR = true;
			else
				dirToR = false;
		}
		else
			move = false;
	}
	void StartAction()
	{
		isFindPlayer = false;
		searchPlayer = true;
		InvokeRepeating("RndAction", 2f , 1f);
	}
	void StopAction()
	{
		CancelInvoke ();
		move = false;
		isFindPlayer = false;
		searchPlayer = false;
		atking = false;
		eCube.GetComponent<Animation>().CrossFade("CubeIdle");
	}

	IEnumerator Atk()
	{
		yield return new WaitForSeconds (1f);
		// Jump
		atking = true;
		this.GetComponent<Rigidbody>().AddForce(new Vector3(0,250,0));
		yield return new WaitForSeconds (3f);
		atking = false;
		if (tBCoolTime <= 0f)
		{
			RaycastHit srhHit;
			if (Physics.Raycast (this.transform.position, this.transform.right, out srhHit, srhRange))
			{
				if(srhHit.transform.tag != "Player")
					StartCoroutine ("TurnBack",false);
			}
			else
				StartCoroutine ("TurnBack",false);
		}
		StartAction ();
	}

	IEnumerator TurnBack(bool amazing)
	{
		if (amazing)
			this.Amazing ();
		dirToR = !dirToR;
		turnBack = true;
		yield return new WaitForSeconds (tBTimeIni);
		turnBack = false;
	}

	void OnCollisionStay(Collision collision)
	{
		// collision to player than atk or trun
		if (collision.transform.tag == "Player")
		{
			// Amazing Turn Back
			if(tBCoolTime <= 0f && !atking && !hitted && !isFindPlayer && !turnBack)
			{

				RaycastHit srhHit;
				if(Physics.Raycast (this.transform.position,this.transform.right,out srhHit,srhRange))
				{
					if(srhHit.transform.tag != "Player")
					{
						tBCoolTime = tBTimeIni;
						this.StopAction ();
						StartCoroutine ("TurnBack",true);
						this.StartAction ();
					}else
						searchPlayer = true;
				}
				else
				{
					tBCoolTime = tBTimeIni;
					this.StopAction ();
					StartCoroutine ("TurnBack",true);
					this.StartAction ();
				}
			}
			// Attack Player
			else if(atking)
			{
				Vector3 atkForce = this.transform.TransformDirection(new Vector3(1,0.5f,0)) * 1000;
				collision.rigidbody.AddForce(atkForce);
				this.GetComponent<Rigidbody>().AddForce(atkForce * (-1));
				Instantiate (atkHitSE, Vector3.zero, Quaternion.identity);
				collision.transform.GetComponent<PControl>().GetDmg();
				atking = false;
			}
		}
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.transform.name == "BossCube")
		{
			this.GetDmg ();
		}
	}

	void Amazing()
	{
		GameObject amaTmp = Instantiate (amazingObj, this.transform.position + Vector3.up, Quaternion.identity) as GameObject;
		GameManager.PlaySEnearP (amazingSE, this.transform.position);
		Destroy (amaTmp, 1f);
	}
}
