using UnityEngine;
using System.Collections;

public class CubeBoss : MonoBehaviour {
	#region -- public variable --
	public GameObject tama;
	public GameObject fallIron;
	public GameObject pullUpObj;
	public GameObject enCap;
	public GameObject boom;
	public GameObject fallSE, rdyAtkSE, fireOffSE, esBoomSE, esBurnSE, mkWallSE, liOpenSE, boomSE, rdyBoomSE;
	public Material noseDmgMat;
	public Material noseMat;
	public RectTransform hpWidth;
	public Transform player;
	public Transform player2;
	public Transform enermyStones;
	public Transform aACreators; // absortAble Creators
	public Transform breakAbles;
	public Transform liteCld; // lighting cloud
	public Transform fireOffRan;
	public Transform enStones;
	public Flare smallFlare, bigFlare;
	public Animation dmgPrompt;
	public DarkSpace darkSpace;
	public BgSEFade bgm;

	public bool test;
	#endregion

	#region -- private variable --
	private EllipsoidParticleEmitter dyingPar;
	private Transform bossCube;
	private Transform aAcolRan; // absortAble collect range
	private Transform beam;
	private Transform bigTamaPos;
	private Transform tmpTama;
	private Transform nowTarget;
	private Transform[] aAcolPos = new Transform[4]; // absortAble collect position
	private Transform[] aAObj = new Transform[4];
	private Transform[] breakWall = new Transform[4];
	private GameObject deadPar;
	private GameObject[] fallIrons = new GameObject[3];
	private Renderer nose;
	private Light spLightL,spLightR; // spot light
	private float hp = 1000f;
	private float noseDmgCD;
	private bool sLup; // spot light Up
	private bool eSshow = true; // enermy stone show
	private bool aAcollect,shootStart;
	private bool shootBeam;
	private bool isTamaing; // Tama Atk ing
	private bool sphereOpen;
	private bool bossDead;
	private bool openDS; // is open Dark Space
	private bool twoP;
	private int targetCo;
	private GameObject waitKillSE; // use to kill ready boom se
	private Color iniColor;
	#endregion

	void Start () {
		#region -- initialize --
		bossCube = this.transform.GetChild (0);
		iniColor = bossCube.GetComponent<MeshRenderer> ().material.color;
		nose = this.transform.GetChild (1).GetComponent<MeshRenderer>();
		spLightL = this.transform.GetChild (2).GetComponent<Light>();
		spLightR = this.transform.GetChild (3).GetComponent<Light>();
		aAcolRan = this.transform.GetChild (4);
		aAcolPos[0] = this.transform.GetChild (5).GetChild(0);
		aAcolPos[1] = this.transform.GetChild (5).GetChild(1);
		aAcolPos[2] = this.transform.GetChild (5).GetChild(2);
		aAcolPos[3] = this.transform.GetChild (5).GetChild(3);
		breakWall [0] = breakAbles.GetChild (0);
		breakWall [1] = breakAbles.GetChild (1);
		breakWall [2] = breakAbles.GetChild (2);
		breakWall [3] = breakAbles.GetChild (3);
		beam = nose.transform.GetChild(0);
		bigTamaPos = this.transform.GetChild (6);
		dyingPar = this.transform.GetChild (7).GetComponent<EllipsoidParticleEmitter> ();
		deadPar = this.transform.GetChild (8).gameObject;
		twoP = GameManager.twoP;
		if (player)
		{
			nowTarget = player;
			targetCo = 1;
		}
		else if(twoP && player2)
		{
			nowTarget = player2;
			targetCo = 2;
		}

		dyingPar.emit = true;

		EnermyStone.enCount = 0;
		Enermy.kill = false;
		EnCapsule.dead = false;
		#endregion
	}

	void Update () {

		if (test) 
		{
			StartCoroutine ("BigTamaAtk");
			test = false;
		}

		if (noseDmgCD > 1)
			noseDmgCD -= Time.deltaTime;
		hpWidth.localScale = new Vector3(hp * 0.001f, 0.1f, 1f);

		#region -- Eyes Blink Color --
		if(sLup)
		{
			spLightL.intensity = Mathf.Lerp(spLightL.intensity, 8f, 0.1f);
			spLightR.intensity = spLightL.intensity;
		}
		else
		{
			spLightL.intensity -= Time.deltaTime + Time.deltaTime + Time.deltaTime + Time.deltaTime;
			spLightR.intensity = spLightL.intensity;
		}
		#endregion

		#region -- AbsortAble Collect --
		if(aAcollect)
		{
			// open sphere
			aAcolRan.localScale = Vector3.Slerp (aAcolRan.localScale, new Vector3(6,6,6), 0.05f);
			// collect absortAble
			if(shootStart)
			{
				// 不再收集
				for(int i=0; i<4; i++)
				{
					// 排序依場景擺放順序
					if(aAObj[i] && !aAObj[i].GetComponent<AbsortAble>().isShoot)
					{
						aAObj[i].position = Vector3.Lerp (aAObj[i].position, aAcolPos[i].position, 0.05f);
						aAObj[i].localScale = Vector3.Lerp (aAObj[i].localScale, new Vector3(3,3,3), 0.05f);
					}
				}
			}
			else
			{
				// 收集球
				// 未收集完
				for(int i=0; i<aACreators.childCount; i++)
				{
					// 排序依場景擺放順序
					// 找出未放置的
					int j;
					if(!aAObj[0])
						j=0;
					else if(!aAObj[1])
						j=1;
					else if(!aAObj[2])
						j=2;
					else if(!aAObj[3])
						j=3;
					else
					{
						shootStart = true;
						break;
					}

					// 放置
					if(aAObj[j] = aACreators.GetChild(i).GetComponent<AACreator>().GetAAObj())
					{
						if(!aAObj[j].GetComponent<AbsortAble>().isShoot && !aAObj[j].GetComponent<AbsortAble>().plrGet)
						{
							aAObj[j].tag = "bossAA";
							aAObj[j].position = Vector3.Lerp (aAObj[j].position, aAcolPos[j].position, 0.05f);
							aAObj[j].localScale = Vector3.Lerp (aAObj[j].localScale, new Vector3(3,3,3), 0.05f);
						}
						else
							aAObj[j] = null;
					}
				}
			}
		}
		else if(!sphereOpen)
		{
			aAcolRan.localScale = Vector3.Lerp (aAcolRan.localScale, Vector3.zero, 0.2f);
		}
		#endregion

		#region -- shootBeam --
		if(shootBeam)
		{
			// rotate
			//beam.rotation = Quaternion.Lerp (beam.rotation, Quaternion.LookRotation (player.position - beam.position), 0.1f);
			if (nowTarget)
				beam.LookAt(nowTarget);
			else
				beam.LookAt(GameManager.plrDeadPos);
		}
		#endregion

		#region -- sphere open --
		if (sphereOpen)
		{
			aAcolRan.localScale = Vector3.Slerp (aAcolRan.localScale, new Vector3(6,6,6), 0.05f);
		}
		#endregion

		#region -- Boss Dead --
		if (bossDead) 
		{
			float r = bossCube.GetComponent<Renderer>().material.color.r;
			float g = bossCube.GetComponent<Renderer>().material.color.g;
			float b = bossCube.GetComponent<Renderer>().material.color.b;
			Vector3 rgb = new Vector3(r,g,b);
			rgb = Vector3.Slerp(rgb, new Vector3(0.5f,0.5f,0.5f), 0.01f);
			r = rgb.x;
			g = rgb.y;
			b = rgb.z;
			bossCube.GetComponent<Renderer>().material.color = new Color(r,g,b);

			nose.material.Lerp(nose.material, noseMat, 0.01f);
		}
		#endregion

		#region -- Dmg Color Lerp --
		else if (bossCube.GetComponent<Renderer>().material.color != iniColor)
		{
			bossCube.GetComponent<Renderer>().material.color = Color.Lerp(bossCube.GetComponent<Renderer>().material.color, iniColor, 0.1f);
		}
		else if (nose.material != noseMat)
		{
			nose.material.Lerp(nose.material, noseMat, 0.01f);
		}
		#endregion
	}

	void OnCollisionEnter(Collision collision)
	{
		#region Hit Player
		if (collision.transform.tag == "Player")
		{
			Vector3 atkForce = this.transform.TransformDirection(Vector3.right) * 1000;
			collision.rigidbody.AddForce(atkForce);
			collision.transform.GetComponent<PControl>().GetDmg();
		}
		else if(collision.transform.tag == "enermy")
		{
			collision.gameObject.SendMessage ("GetDmg");
		}
		#endregion

		#region Get Hit
		else if(collision.transform.tag == "absortAble")
		{
			GetDmg(10f);
		}
		#endregion

		#region -- FallDownSE --

		if (this.GetComponent<Rigidbody> ().velocity.y > 10)
			GameManager.PlaySEnearP (fallSE, new Vector3(this.transform.position.x, 0));
		#endregion
	}
	#region -- GetDmg() --
	public void GetDmg(float point)
	{
		if (!bossDead)
		{
			this.hp -= point;
			bossCube.GetComponent<Renderer>().material.color = Color.red;
			bossCube.GetComponent<Animation>().Play("CubeBossGetHit");
			
			// tama cut charge
			if (isTamaing) 
			{
				bigTamaPos.GetChild(0).GetComponent<BigTama>().CutCharge();
				if(tmpTama.GetComponent<BigTama>().chargeCnt < 0)
				{
					isTamaing = false;
					Destroy (tmpTama.gameObject);
					StopCoroutine("BigTamaAtk");
				}
			}
			
			// dead
			if (this.hp <= 0) 
			{
				// 關牆
				for (int i=0; i<4; i++) 
				{
					if(breakWall[i].gameObject.activeSelf)
						breakWall[i].GetComponent<BreakWall> ().RequstClose ();
				}
				// 關雲
				liteCld.GetComponent<LCloud>().Exit ();
				// 關水
				if (beam.gameObject.activeSelf)
				{
					beam.GetComponent<Light> ().enabled = false;
					beam.GetComponent<EllipsoidParticleEmitter> ().emit = false;
					beam.GetComponent<AudioSource> ().Stop();
				}
				// 殺生
				Enermy.kill = true;
				// 殺蛋
				if (tmpTama)
					tmpTama.GetComponent<BigTama> ().Dead ();
				// 殺火
				if (fireOffRan.gameObject.activeSelf)
					fireOffRan.GetComponent<FireOff> ().Dead ();
				// 關氣
				pullUpObj.GetComponent<PullUp> ().Dead ();
				// 殺鐵
				for (int i=0; i<3; i++)
				{
					if (fallIrons[i])
						fallIrons[i].SendMessage ("Boom");
				}
				// 殺膠囊
				EnCapsule.dead = true;
				// 關空間
				this.DarkspOff ();
				// 放音效
				waitKillSE = Instantiate (rdyBoomSE, Vector3.zero, Quaternion.identity) as GameObject;

				StartCoroutine ("Dead");
				GameManager.super = true;
				this.GetComponent<BoxCollider> ().material.bounciness = 0.7f;
				this.hp = 0;
				bossDead = true;
			}
		}
	}

	public void HitNose(float point)
	{
		if (noseDmgCD <= 1)
		{
			this.GetDmg (point);
			noseDmgCD = 4f;
		}
		else
		{
			this.GetDmg (point*0.4f);
		}
		if (isTamaing) 
		{
			bigTamaPos.GetChild(0).GetComponent<BigTama>().CutCharge();
			bigTamaPos.GetChild(0).GetComponent<BigTama>().CutCharge();
			bigTamaPos.GetChild(0).GetComponent<BigTama>().CutCharge();
			bigTamaPos.GetChild(0).GetComponent<BigTama>().CutCharge();
			if(tmpTama.GetComponent<BigTama>().chargeCnt < 0)
			{
				isTamaing = false;
				Destroy (tmpTama.gameObject);
				StopCoroutine("BigTamaAtk");
			}
		}
		nose.material.Lerp(nose.material, noseDmgMat, 1f);
		bossCube.GetComponent<Animation>().Play("CubeBossNoseHit");
	}
	#endregion

	#region -- Blink() -- this.Blink (Color.red); 3.5f
	void Blink(Color color, Flare flare)
	{
		StartCoroutine ("Blinking",color);
		spLightL.flare = flare;
		spLightR.flare = flare;
	}

	IEnumerator Blinking(Color color)
	{
		if (bossDead) 
		{
			sLup = false;
		}
		else
		{
			spLightL.color = color;
			spLightR.color = color;
			sLup = true;
			Instantiate (rdyAtkSE, Vector3.zero, Quaternion.identity);
			yield return new WaitForSeconds(0.5f);
			sLup = false;
		}
	}
	#endregion

	#region -- Enermy Stone Active -- StartCoroutine ("ESActive"); 2f
	// 招喚石頭或怪
	IEnumerator ESActive()
	{
		if (!bossDead) 
		{
			this.Blink (Color.red, bigFlare);
			if (eSshow) 
			{
				for (int i=0; i<enStones.childCount; i++)
				{
					enStones.GetChild(i).GetComponent<EnermyStone> ().ToEnermy ();
					GameManager.PlaySEnearP (esBoomSE, enStones.GetChild(i).position);
					yield return new WaitForSeconds(0.3f);
					if(bossDead)
						break;
				}
				eSshow = false;
			}
			else
			{
				for (int i=0; i < enermyStones.childCount; i++)
				{
					enermyStones.GetChild(i).gameObject.SetActive(true);
					enermyStones.GetChild(i).GetComponent<EnermyStone>().Burn();
					GameManager.PlaySEnearP (esBurnSE, enStones.GetChild(i).position);
					yield return new WaitForSeconds(0.3f);
					if(bossDead)
						break;
				}
				eSshow = true;
			}
		}
	}
	#endregion

	#region -- absortAble Atk -- StartCoroutine("AAAtk"); 7.5f

	IEnumerator AAAtk()
	{
		if (!bossDead) 
		{
			this.Blink (Color.magenta, bigFlare);
			dmgPrompt.Play ("DmgPrompt RUp");
			// 清空陣列
			for (int i=0; i<4; i++) 
			{
				aAObj[i] = null;
			}

			aAcollect = true;
			yield return new WaitForSeconds (3f);
			shootStart = true;

			// 射出新的
			for (int i=0; i<4; i++) 
			{
				if (bossDead) // 強制中斷
				{
					// 丟掉所有
					for (int j=0; j<4; j++) 
					{
						if(aAObj[j])
							aAObj[j].GetComponent<AbsortAble> ().ReadyShot ();
					}
					// 關掉圓
					aAcollect = false;
					sphereOpen = false;

					break;
				}
				// 攻擊
				else if (i == 3) // 最後一發
				{
					aAcollect = false;
					shootStart = false;
					this.ShootAAObj (3);
				}
				else
				{
					aAcollect = false;
					this.ShootAAObj (i);
					yield return new WaitForSeconds (0.2f);
					aAcollect = true;
					yield return new WaitForSeconds (1.3f);
				}
			}
		}
	}

	void ShootAAObj(int i)
	{
		if(aAObj[i])
		{
			aAObj[i].GetComponent<AbsortAble> ().ReadyShot ();
			if(nowTarget)
				aAObj[i].GetComponent<Rigidbody>().AddForce((nowTarget.position-aAObj[i].position)*50);
			else
				aAObj[i].GetComponent<Rigidbody>().AddForce((GameManager.plrDeadPos-aAObj[i].position)*50);
			if (breakWall[0].gameObject.activeSelf)
				Physics.IgnoreCollision (aAObj[i].GetComponent<Collider>(), breakWall[0].GetComponent<Collider>());
			if (breakWall[1].gameObject.activeSelf)
				Physics.IgnoreCollision (aAObj[i].GetComponent<Collider>(), breakWall[1].GetComponent<Collider>());
			if (breakWall[2].gameObject.activeSelf)
				Physics.IgnoreCollision (aAObj[i].GetComponent<Collider>(), breakWall[2].GetComponent<Collider>());
			if (breakWall[3].gameObject.activeSelf)
				Physics.IgnoreCollision (aAObj[i].GetComponent<Collider>(), breakWall[3].GetComponent<Collider>());
		}
	}
	#endregion

	#region -- call break wall -- StartCoroutine("MKWall"); 1.5f

	IEnumerator MKWall()
	{
		if (!bossDead) 
		{
			this.Blink (Color.cyan, smallFlare);
			dmgPrompt.Play ("DmgPrompt R");

			for (int i=0; i<4; i++)
			{
				if (bossDead) // 強制中斷
				{
					break;
				}
				// 行為
				else if (i == 3) // last
				{
					if(!breakWall [3].gameObject.activeSelf)
					{
						breakWall [3].gameObject.SetActive (true);
						breakWall [3].GetComponent<BreakWall> ().Action ();
						GameManager.PlaySEnearP (mkWallSE, breakWall [3].position);
					}
				}
				else
				{
					if(!breakWall [i].gameObject.activeSelf)
					{
						breakWall [i].gameObject.SetActive (true);
						breakWall [i].GetComponent<BreakWall> ().Action ();
						GameManager.PlaySEnearP (mkWallSE, breakWall [i].position);
						yield return new WaitForSeconds (0.5f);
					}
				}
			}
		}
	}

	#endregion

	#region -- lighting -- StartCoroutine("Lighting"); 7f
	IEnumerator Lighting()
	{
		if (!bossDead)
		{
			this.Blink (Color.blue, bigFlare);
			dmgPrompt.Play ("DmgPrompt Up");

			GameManager.PlaySEnearP (liOpenSE, liteCld.position);
			liteCld.GetComponent<LCloud>().Action ();
			yield return new WaitForSeconds(7f);
			if (!bossDead) // 已在GetDmg()關
				liteCld.GetComponent<LCloud>().Exit ();
		}
	}
	#endregion

	#region -- BeamAtk -- StartCoroutine("BeamAtk"); 9f
	IEnumerator BeamAtk()
	{
		if (!bossDead) 
		{
			this.Blink (Color.cyan, bigFlare);
			dmgPrompt.Play ("DmgPrompt RUp");

			yield return new WaitForSeconds(2f);

			if (!bossDead) 
			{
				beam.gameObject.SetActive (true);
				beam.GetComponent<Light> ().enabled = true;
				beam.GetComponent<AudioSource> ().Play();
				beam.GetComponent<EllipsoidParticleEmitter> ().emit = true;
				shootBeam = true;
				
				yield return new WaitForSeconds(7f);
				beam.GetComponent<EllipsoidParticleEmitter> ().emit = false;
				beam.GetComponent<Light> ().enabled = false;
				yield return new WaitForSeconds(1f);
				beam.GetComponent<AudioSource> ().Stop();
				yield return new WaitForSeconds(1f);
				shootBeam = false;
				beam.gameObject.SetActive (false);
			}
			else
				beam.GetComponent<Light> ().enabled = false;
		}
	}
	#endregion

	#region -- BigTamaAtk -- StartCoroutine("BigTamaAtk"); 39f
	IEnumerator BigTamaAtk()
	{
		if (!bossDead) 
		{
			this.Blink (Color.yellow, smallFlare);
			dmgPrompt.Play ("DmgPrompt RUp");
			yield return new WaitForSeconds(1f);
			// 開始集氣
			if (!bossDead)
			{
				tmpTama = (Instantiate (tama, Vector3.zero, Quaternion.identity) as GameObject).transform;
				tmpTama.SetParent (bigTamaPos);
				tmpTama.localPosition = Vector3.zero;
				isTamaing = true;
				yield return new WaitForSeconds(30f);
			}
			// 集氣完
			if (!bossDead)
			{
				tmpTama.GetComponent<BigTama> ().ReadyForShoot ();
				Physics.IgnoreCollision (tmpTama.GetComponent<Collider> (), this.GetComponent<Collider> ());
				isTamaing = false;
				yield return new WaitForSeconds(3f);
			}
			// 張開球
			if (!bossDead)
			{
				this.Blink (Color.magenta, bigFlare);
				sphereOpen = true;
				yield return new WaitForSeconds(5f);
			}
			// 發射蛋
			if (!bossDead)
			{
				sphereOpen = false;
				tmpTama.GetComponent<BigTama> ().Shooting ();
				tmpTama.parent = null;
				//tmpTama.localScale = new Vector3(tmpTama.localScale.x*50, tmpTama.localScale.y*50, tmpTama.localScale.z*50);
				if(nowTarget)
					tmpTama.GetComponent<Rigidbody> ().AddForce ((nowTarget.position - tmpTama.position).normalized*800);
				else
					tmpTama.GetComponent<Rigidbody> ().AddForce ((GameManager.plrDeadPos - tmpTama.position).normalized*800);
			}
		}
	}
	#endregion

	#region -- Fire Off Atk -- StartCoroutine("FireOffAtk"); 5f
	IEnumerator FireOffAtk()
	{
		if (!bossDead) 
		{
			this.Blink (Color.yellow, smallFlare);
			dmgPrompt.Play ("DmgPrompt Cen");
			fireOffRan.gameObject.SetActive (true);
			// toBig
			fireOffRan.GetComponent<FireOff> ().ToBig ();
			// wait
			yield return new WaitForSeconds (5f);
			// rotate & scale down (kill player)
			if (!bossDead)
			{
				this.Blink (Color.yellow, bigFlare);
				fireOffRan.GetComponent<FireOff> ().Kill ();
				Instantiate (fireOffSE, Vector3.zero, Quaternion.identity);
			}
		}
	}
	#endregion

	#region -- Fall Iron -- StartCoroutine("FallIron"); 2f
	IEnumerator FallIron()
	{
		if (!bossDead)
		{
			// blink
			this.Blink (Color.white, smallFlare);
			dmgPrompt.Play ("DmgPrompt Up");
			// Destory before
			if (fallIrons [2]) 
			{
				for (int i=0; i<3; i++)
				{
					fallIrons[i].SendMessage ("Boom");
					yield return new WaitForSeconds (0.3f);
				}
			}

			// make 1
			if (!bossDead)
			{
				fallIrons [0] = Instantiate(fallIron, new Vector3 (259.8f, 73.6f, 0f), new Quaternion (0, 0, 0.22f, 0.97f)) as GameObject;
				// wait
				yield return new WaitForSeconds (0.5f);
			}
			// make 2
			if (!bossDead)
			{
				fallIrons [1] = Instantiate(fallIron, new Vector3 (237.04f, 73.6f, 0f), new Quaternion (0, 0, 0.22f, 0.97f)) as GameObject;
				// wait
				yield return new WaitForSeconds (0.5f);
			}
			if (!bossDead)
			{
				fallIrons [2] = Instantiate(fallIron, new Vector3 (214.2f, 73.6f, 0f), new Quaternion (0, 0, 0.22f, 0.97f)) as GameObject;
			}
			// make 3
		}
	}
	#endregion

	#region -- Pull Up -- StartCoroutine ("PullUp"); 1f
	IEnumerator PullUp()
	{
		if (!bossDead) 
		{
			// blink
			this.Blink (Color.red, smallFlare);
			dmgPrompt.Play ("DmgPrompt Dwn");
			yield return new WaitForSeconds (1f);
			// open pull up
			pullUpObj.GetComponent<PullUp> ().Action ();
		}
	}
	#endregion

	#region -- Enermy Capsule -- StartCoroutine ("EnCapAtk"); 1f
	IEnumerator EnCapAtk ()
	{
		if (!bossDead)
		{
			this.Blink (Color.red, smallFlare);
			dmgPrompt.Play ("DmgPrompt RUp");
			yield return new WaitForSeconds (1f);
			if (!bossDead)
				Instantiate (enCap, new Vector3 (290.8f, 56.8f, 0), new Quaternion(0, 0, -0.707f, -0.707f));
		}
	}
	#endregion

	#region -- DarkSpace -- this.DarkspOn (); this.DarkspOff ();
	void DarkspOn ()
	{
		this.Blink (Color.gray ,smallFlare);
		openDS = true;
		dyingPar.emit = true;
		darkSpace.On ();
	}

	void DarkspOff ()
	{
		dyingPar.emit = false;
		darkSpace.Off ();
	}
	#endregion

	public void StartAI ()
	{
		dyingPar.emit = false;
		StartCoroutine ("NormalAI");
	}

	void ChTarget () // change target
	{
		if (targetCo == 1 && player2)
		{
			nowTarget = player2;
			targetCo = 2;
		}
		else if (targetCo == 2 && player)
		{
			nowTarget = player;
			targetCo = 1;
		}
	}

	#region -- Normal AI --
	IEnumerator NormalAI ()
	{
		yield return new WaitForSeconds (5f);
		int twiceFlag = 0;
		bool p5first = false;
		while (true) 
		{
			float waitTime = 0;
			if (this.hp < 100) // hp 0~100
			{
				#region -- phase 6 -- AA+Fi+Be+Li+Bi+PU | MW+FI+EA+EC
				if (!openDS)
				{
					this.DarkspOn ();
					yield return new WaitForSeconds(5f);
				}
				float waitTimeBuf = 4f;
				int act = Random.Range (0, 6);
				int act2 = Random.Range (0, 4);
				if (twiceFlag == 2)
				{
					switch (act2)
					{
					case 0:
						StartCoroutine ("MKWall");
						yield return new WaitForSeconds (waitTimeBuf);
						StartCoroutine ("ESActive");
						waitTime = 5f + waitTimeBuf;
						break;
					case 1:
						StartCoroutine ("ESActive");
						yield return new WaitForSeconds (waitTimeBuf);
						StartCoroutine ("AAAtk");
						waitTime = 8f + waitTimeBuf;
						break;
					case 2:
						StartCoroutine ("FallIron");
						yield return new WaitForSeconds (waitTimeBuf);
						StartCoroutine ("BigTamaAtk");
						waitTime = 6f + waitTimeBuf;
						break;
					case 3:
						StartCoroutine ("EnCapAtk");
						yield return new WaitForSeconds (waitTimeBuf);
						StartCoroutine ("ESActive");
						waitTime = 5f + waitTimeBuf;
						break;
					}
				}
				else
				{
					switch (act)
					{
					case 0:
						StartCoroutine ("AAAtk");
						yield return new WaitForSeconds (waitTimeBuf);
						StartCoroutine ("Lighting");
						waitTime = 8f + waitTimeBuf;
						break;
					case 1:
						StartCoroutine ("Lighting");
						yield return new WaitForSeconds (waitTimeBuf);
						StartCoroutine ("BeamAtk");
						waitTime = 9f + waitTimeBuf;
						break;
					case 2:
						StartCoroutine ("BeamAtk");
						yield return new WaitForSeconds (waitTimeBuf);
						StartCoroutine ("BigTamaAtk");
						waitTime = 6f + waitTimeBuf;
						break;
					case 3:
						StartCoroutine ("FireOffAtk");
						yield return new WaitForSeconds (waitTimeBuf);
						StartCoroutine ("BeamAtk");
						waitTime = 9f + waitTimeBuf;
						break;
					case 4:
						StartCoroutine ("BigTamaAtk");
						yield return new WaitForSeconds (waitTimeBuf);
						StartCoroutine ("FireOffAtk");
						waitTime = 6f + waitTimeBuf;
						break;
					case 5:
						StartCoroutine ("PullUp");
						yield return new WaitForSeconds (waitTimeBuf);
						StartCoroutine ("AAAtk");
						waitTime = 8f + waitTimeBuf;
						break;
					}
				}
				twiceFlag = (twiceFlag+1) % 3;
				
				// atk end wait
				yield return new WaitForSeconds (waitTime);
				while(isTamaing)
				{
					act = Random.Range (0, 2);
					switch (act)
					{
					case 0:
						StartCoroutine ("Lighting");
						waitTime = 8f + waitTimeBuf;
						break;
					case 1:
						StartCoroutine ("FireOffAtk");
						waitTime = 6f + waitTimeBuf;
						break;
					}
					yield return new WaitForSeconds (waitTime);
				}
				
				// 進入下段
				if (this.hp <= 0)
				{
					break;
				}
				#endregion
			}
			else if (this.hp < 300) // hp 100~300
			{
				#region -- phase 5 -- AA+Fi+Be+Li+Bi+PU | MW+FI+EA+EC
				if (!openDS)
				{
					this.DarkspOn ();
					yield return new WaitForSeconds(5f);
				}

				float waitTimeBuf = 5f;
				int act = Random.Range (0, 6);
				int act2 = Random.Range (0, 4);
				if (!p5first)
				{
					p5first = true;
					StartCoroutine ("ESActive");
					waitTime = 5f + waitTimeBuf;
				}
				else
				{
					if (twiceFlag == 2)
					{
						switch (act2)
						{
						case 0:
							StartCoroutine ("MKWall");
							yield return new WaitForSeconds (waitTimeBuf);
							StartCoroutine ("ESActive");
							waitTime = 5f + waitTimeBuf;
							break;
						case 1:
							StartCoroutine ("ESActive");
							yield return new WaitForSeconds (waitTimeBuf);
							StartCoroutine ("AAAtk");
							waitTime = 8f + waitTimeBuf;
							break;
						case 2:
							StartCoroutine ("FallIron");
							yield return new WaitForSeconds (waitTimeBuf);
							StartCoroutine ("BigTamaAtk");
							waitTime = 6f + waitTimeBuf;
							break;
						case 3:
							StartCoroutine ("EnCapAtk");
							waitTime = 5f + waitTimeBuf;
							break;
						}
					}
					else
					{
						switch (act)
						{
						case 0:
							StartCoroutine ("AAAtk");
							waitTime = 8f + waitTimeBuf;
							break;
						case 1:
							StartCoroutine ("Lighting");
							yield return new WaitForSeconds (waitTimeBuf);
							StartCoroutine ("BeamAtk");
							waitTime = 9f + waitTimeBuf;
							break;
						case 2:
							StartCoroutine ("BeamAtk");
							yield return new WaitForSeconds (waitTimeBuf);
							StartCoroutine ("BigTamaAtk");
							waitTime = 6f + waitTimeBuf;
							break;
						case 3:
							StartCoroutine ("FireOffAtk");
							yield return new WaitForSeconds (waitTimeBuf);
							StartCoroutine ("BeamAtk");
							waitTime = 9f + waitTimeBuf;
							break;
						case 4:
							StartCoroutine ("BigTamaAtk");
							yield return new WaitForSeconds (waitTimeBuf);
							StartCoroutine ("FireOffAtk");
							waitTime = 6f + waitTimeBuf;
							break;
						case 5:
							StartCoroutine ("PullUp");
							yield return new WaitForSeconds (waitTimeBuf);
							StartCoroutine ("AAAtk");
							waitTime = 8f + waitTimeBuf;
							break;
						}
					}
					twiceFlag = (twiceFlag+1) % 3;
				}

				// atk end wait
				yield return new WaitForSeconds (waitTime);
				while(isTamaing)
				{
					act = Random.Range (0, 3);
					switch (act)
					{
						case 0:
							StartCoroutine ("BeamAtk");
							waitTime = 9f + waitTimeBuf;
						break;
						case 1:
							StartCoroutine ("ESActive");
							waitTime = 5f + waitTimeBuf;
						break;
						case 2:
							StartCoroutine ("FallIron");
							waitTime = 5f + waitTimeBuf;
						break;
					}
					yield return new WaitForSeconds (waitTime);
				}
				
				// 進入下段
				if (this.hp < 100)
				{
					StartCoroutine ("Lighting");
					yield return new WaitForSeconds (4f);
					StartCoroutine ("BigTamaAtk");
					yield return new WaitForSeconds (5f);
					StartCoroutine ("Lighting");
					yield return new WaitForSeconds (4f);
					twiceFlag = 0;
				}
				#endregion
			}
			else if (this.hp < 500) // hp 300~500
			{
				#region -- phase 4 -- AA+Fi+Be+Li+Bi+PU | MW+FI+EA+EC
				float waitTimeBuf = 6f;
				int act = Random.Range (0, 6);
				int act2 = Random.Range (0, 4);
				if (twiceFlag == 2)
				{
					switch (act2)
					{
					case 0:
						StartCoroutine ("MKWall");
						waitTime = 5f + waitTimeBuf;
						break;
					case 1:
						StartCoroutine ("ESActive");
						waitTime = 5f + waitTimeBuf;
						break;
					case 2:
						StartCoroutine ("FallIron");
						waitTime = 5f + waitTimeBuf;
						break;
					case 3:
						StartCoroutine ("EnCapAtk");
						waitTime = 5f + waitTimeBuf;
						break;
					}
				}
				else
				{
					switch (act)
					{
					case 0:
						StartCoroutine ("AAAtk");
						waitTime = 8f + waitTimeBuf;
						break;
					case 1:
						StartCoroutine ("Lighting");
						waitTime = 8f + waitTimeBuf;
						break;
					case 2:
						StartCoroutine ("BeamAtk");
						waitTime = 9f + waitTimeBuf;
						break;
					case 3:
						StartCoroutine ("FireOffAtk");
						waitTime = 6f + waitTimeBuf;
						break;
					case 4:
						StartCoroutine ("BigTamaAtk");
						waitTime = 6f + waitTimeBuf;
						break;
					case 5:
						StartCoroutine ("PullUp");
						waitTime = waitTimeBuf;
						break;
					}
				}
				twiceFlag = (twiceFlag+1) % 3;
				
				// atk end wait
				yield return new WaitForSeconds (waitTime);
				while(isTamaing)
					yield return new WaitForSeconds (10f);
				
				// 進入下段
				if (this.hp < 300)
				{
					if (!openDS)
					{
						this.DarkspOn ();
						yield return new WaitForSeconds(5f);
					}
					StartCoroutine ("PullUp");
					yield return new WaitForSeconds (4f);
					StartCoroutine ("FallIron");
					yield return new WaitForSeconds (4f);
					StartCoroutine ("MKWall");
					yield return new WaitForSeconds (4f);
					StartCoroutine ("BigTamaAtk");
					yield return new WaitForSeconds (20f);
					twiceFlag = 0;
				}
				#endregion
			}
			else if (this.hp < 700) // hp 500~700
			{
				#region -- phase 3 -- AA+Fi+Be+Li+Bi | MW+FI+EA
				float waitTimeBuf = 7f;
				int act = Random.Range (0, 5);
				int act2 = Random.Range (0, 3);
				if (twiceFlag == 2)
				{
					switch (act2)
					{
					case 0:
						StartCoroutine ("MKWall");
						waitTime = 5f + waitTimeBuf;
						break;
					case 1:
						StartCoroutine ("ESActive");
						waitTime = 5f + waitTimeBuf;
						break;
					case 2:
						StartCoroutine ("FallIron");
						waitTime = 5f + waitTimeBuf;
						break;
					}
				}
				else
				{
					switch (act)
					{
						case 0:
							StartCoroutine ("AAAtk");
							waitTime = 8f + waitTimeBuf;
							break;
						case 1:
							StartCoroutine ("Lighting");
							waitTime = 8f + waitTimeBuf;
							break;
						case 2:
							StartCoroutine ("BeamAtk");
							waitTime = 9f + waitTimeBuf;
							break;
						case 3:
							StartCoroutine ("FireOffAtk");
							waitTime = 6f + waitTimeBuf;
							break;
						case 4:
							StartCoroutine ("BigTamaAtk");
							waitTime = 6f + waitTimeBuf;
							break;
					}
				}
				twiceFlag = (twiceFlag+1) % 3;
				
				// atk end wait
				yield return new WaitForSeconds (waitTime);
				while(isTamaing)
					yield return new WaitForSeconds (10f);
				
				// 進入下段
				if (this.hp < 500)
				{
					StartCoroutine ("PullUp");
					yield return new WaitForSeconds (4f);
					StartCoroutine ("EnCapAtk");
					yield return new WaitForSeconds (4f);
					twiceFlag = 0;
				}
				#endregion
			}
			else if (this.hp < 900) // hp 700~900
			{
				#region -- phase 2 -- AA+Fi+Be+Li | FI+EA
				float waitTimeBuf = 8f;
				int act = Random.Range (0, 4);
				if (twiceFlag == 2)
				{
					switch (act)
					{
						case 0: case 1:
							StartCoroutine ("ESActive");
							waitTime = 5f + waitTimeBuf;
							break;
						case 2: case 3:
							StartCoroutine ("FallIron");
							waitTime = 5f + waitTimeBuf;
							break;
					}
				}
				else
				{
					switch (act)
					{
					case 0:
						StartCoroutine ("AAAtk");
						waitTime = 8f + waitTimeBuf;
						break;
					case 1:
						StartCoroutine ("Lighting");
						waitTime = 8f + waitTimeBuf;
						break;
					case 2:
						StartCoroutine ("BeamAtk");
						waitTime = 9f + waitTimeBuf;
						break;
					case 3:
						StartCoroutine ("FireOffAtk");
						waitTime = 6f + waitTimeBuf;
						break;
					}
				}
				twiceFlag = (twiceFlag+1) % 3;
				
				// atk end wait
				yield return new WaitForSeconds (waitTime);
				
				// 進入下段
				if (this.hp < 700)
				{
					StartCoroutine ("MKWall");
					yield return new WaitForSeconds (12f);
					StartCoroutine ("BigTamaAtk");
					yield return new WaitForSeconds (17f);
					twiceFlag = 0;
				}
				#endregion
			}
			else // hp 900~1000
			{
				#region -- phase 1 -- AA+Be+Fi | EA
				float waitTimeBuf = 10f;
				int act = Random.Range (0, 3);
				if (twiceFlag == 2)
				{
					StartCoroutine ("ESActive");
					waitTime = 5f + waitTimeBuf;
				}
				else
				{
					switch (act)
					{
					case 0:
						StartCoroutine ("AAAtk");
						waitTime = 8f + waitTimeBuf;
						break;
					case 1:
						StartCoroutine ("BeamAtk");
						waitTime = 9f + waitTimeBuf;
						break;
					case 2:
						StartCoroutine ("FireOffAtk");
						waitTime = 6f + waitTimeBuf;
						break;
					}
				}
				twiceFlag = (twiceFlag+1) % 3;

				// atk end wait
				yield return new WaitForSeconds (waitTime);

				// 進入下段
				if (this.hp < 900)
				{
					StartCoroutine ("Lighting");
					yield return new WaitForSeconds (17f);
					StartCoroutine ("FallIron");
					yield return new WaitForSeconds (10f);
					twiceFlag = 0;
				}
				#endregion
			}

			// player dead
			if (!player && !player2)
				break;
			else if (twoP)
				this.ChTarget (); // change target
		}
	}
	#endregion

	#region -- Dead -- StartCoroutine ("Dead");

	IEnumerator Dead ()
	{
		this.DarkspOff ();
		deadPar.SetActive (true);
		yield return new WaitForSeconds (12f);
		this.DarkspOn ();
		yield return new WaitForSeconds (8f);
		GameManager.fin = true;
		Instantiate (boom, this.transform.position, Quaternion.identity);
		Instantiate (boomSE, Vector3.zero, Quaternion.identity);
		Destroy (waitKillSE.gameObject);
		bgm.enabled = true;
		bgm.FadOutAndPlay ();
		Destroy (darkSpace.gameObject);
		Destroy (this.gameObject);
	}

	#endregion
}
