using UnityEngine;
using System.Collections;

public class PControl : MonoBehaviour {

	public static Vector3 p1Pos, p2Pos;

	public GameObject jumpParticle;
	public GameObject plrBoomObj;
	public GameObject finCan;
	public GameObject gOver;
	public GameObject jumpSE, boomSE, atkSE;
	public Transform sortRanSph;
	public Transform absortCon;
	public Transform iniPos, bossdPos;
	public Transform otherP;
	public bool hitted;

	// can change
	private float speed = 5f;
	private float jumpForce = 500f;
	private float atkPow = 3000f;
	private float reflaxPow = 25f;

	// can't change
	private Transform absortConObj; 
	private Transform playerCube;
	private Collider[] absortRanCol;
	private Vector3 absortRefV;
	private int downCnt;
	private bool twoJump;
	private bool absortBack,openAbsort;
	private bool havAbsToShoot;
	private bool twoRun;
	private bool twoP;

	// Use this for initialization
	void Start () {
		playerCube = this.transform.GetChild (0);
		GameManager.super = false;
		GameManager.fin = false;
		if (GameManager.bossed)
			this.transform.position = bossdPos.position;
		else
			this.transform.position = iniPos.position;

		if (this.name == "Player2") 
			twoP = true;
	}
	
	// Update is called once per frame
	void Update () {
		#region -- 2P --

		if (twoP) 
		{
			p2Pos = this.transform.position;

			#region Hitted
			if (hitted)
			{
				if(playerCube.GetComponent<Animation>() ["CubeAtkBack"].speed > 0.01f)
				{
					playerCube.GetComponent<Animation>() ["CubeAtkBack"].speed -= 0.01f;
				}
				#region absortAble process
				sortRanSph.localScale = Vector3.Slerp(sortRanSph.localScale, Vector3.one, 0.1f);
				if(absortConObj)
				{
					int i=0;
					if (absortRanCol[0])
					{
						foreach(Collider hit in absortRanCol)
						{
							// shoot
							if(hit && hit.tag == "absortAble")
							{
								Physics.IgnoreCollision(this.GetComponent<Collider>(),hit);
								// 忽略和其他顆
								for(int j=0;j<i;j++)
									Physics.IgnoreCollision(absortRanCol[j].GetComponent<Collider>(),hit);
								
								hit.GetComponent<AbsortAble> ().ReadyShot ();
								i++;
							}
						}
						absortRanCol = null;
					}
					Destroy(absortConObj.gameObject);
				}
				#endregion
				// count down and close;
				if((this.GetComponent<Rigidbody>().velocity.x < 0.1f && this.GetComponent<Rigidbody>().velocity.y < 0.1f && this.GetComponent<Rigidbody>().velocity.z < 0.1f) || Physics.Raycast (this.transform.position,Vector3.down,1f))
				{
					// Color Blink
					float r = playerCube.GetComponent<Renderer>().material.color.r;
					float g = playerCube.GetComponent<Renderer>().material.color.g;
					float b = playerCube.GetComponent<Renderer>().material.color.b;
					Vector3 rgb = new Vector3(r,g,b);
					rgb = Vector3.Slerp(rgb, new Vector3(0.5f,0.5f,0.5f), 0.1f);
					r = rgb.x;
					g = rgb.y;
					b = rgb.z;
					playerCube.GetComponent<Renderer>().material.color = new Color(r,g,b);
					if(playerCube.GetComponent<Renderer>().material.color == Color.gray)
					{
						hitted = false;
						plrBoomObj.GetComponent<ParticleRenderer>().material = playerCube.GetComponent<MeshRenderer>().material;
						Instantiate (plrBoomObj, this.transform.position, Quaternion.identity);
						Instantiate (boomSE, Vector3.zero, Quaternion.identity);
						GameManager.plr2DPos = this.transform.position;
						gOver.SetActive (true);
						gOver.GetComponent<GOControl> ().TwoPDead ();
						Destroy (this.gameObject);
					}
				}
			}
			#endregion

			else
			{
				#region MOVE & Direction & Animate
				if (Input.GetAxis ("Vertical") != 0)
				{
					// TwoRun (State)
					if((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Keypad6)) && Mathf.Abs(Input.GetAxis ("Vertical")) > 0.05f)
					{
						twoRun = true;
					}
					// Move
					if(twoRun)
						this.transform.Translate (Input.GetAxis ("Vertical")* speed* 1.5f* Time.deltaTime,0,0,Space.World);
					else
						this.transform.Translate (Input.GetAxis ("Vertical")* speed* Time.deltaTime,0,0,Space.World);	
					
					// Direction
					if(Input.GetAxis ("Vertical") > 0)
						this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, Vector3.zero, 0.1f);
					else
						this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, new Vector3(0f,180f,0f), 0.1f);
					
					// Animate
					if(Physics.Raycast (this.transform.position,Vector3.down,1f))
					{
						if(twoRun)
							playerCube.GetComponent<Animation>()["CubeRun"].speed = Mathf.Abs (Input.GetAxis ("Vertical")) * 1.5f;
						else
							playerCube.GetComponent<Animation>()["CubeRun"].speed = Mathf.Abs (Input.GetAxis ("Vertical"));
					}
					else
					{
						if(twoRun)
							playerCube.GetComponent<Animation>()["CubeRun"].speed = Mathf.Abs (Input.GetAxis ("Vertical"));
						else
							playerCube.GetComponent<Animation>()["CubeRun"].speed = Mathf.Abs (Input.GetAxis ("Vertical")) * 0.5f;
					}
					playerCube.GetComponent<Animation>().CrossFade("CubeRun");
					// reflex
					RaycastHit hit;
					if(Physics.Raycast (this.transform.position,this.transform.right,out hit,0.5f) && hit.transform.tag == "map")
					{
						this.GetComponent<Rigidbody>().AddForce(Vector3.down*Mathf.Abs(Input.GetAxis ("Vertical"))* reflaxPow);
						twoJump = false;
					}
				}
				else
				{
					twoRun = false;
					playerCube.GetComponent<Animation>().CrossFade("CubeIdle");
				}
				#endregion
				
				#region JUMP
				if (Input.GetKeyDown (KeyCode.Keypad8) || Input.GetKeyDown (KeyCode.UpArrow))
				{
					RaycastHit hit;
					if (this.GetComponent<Rigidbody>().velocity.y == 0)
					{
						this.GetComponent<Rigidbody>().AddForce(0,jumpForce,0);
						Instantiate(jumpParticle,this.transform.position,Quaternion.identity);
						// Animate
						playerCube.GetComponent<Animation>().Play("CubeJump");
						Instantiate (jumpSE, Vector3.zero, Quaternion.identity);
						twoJump = false;
					}
					else if(!twoJump)
					{
						if(Physics.Raycast (this.transform.position,this.transform.right,out hit,0.5f) && (hit.transform.tag == "map" || hit.transform.tag == "enermy" || hit.transform.tag == "Player"))
						{
							Vector3 twoJumpF = new Vector3(-Input.GetAxis ("Vertical")*500f,jumpForce*0.7f,0);
							this.GetComponent<Rigidbody>().AddForce(twoJumpF);
							if(hit.transform.tag == "enermy" || hit.transform.tag == "Player")
								hit.rigidbody.AddForce(twoJumpF * (-1));
						}
						else
						{
							this.GetComponent<Rigidbody>().AddForce(0,jumpForce*0.7f,0);
						}
						Instantiate(jumpParticle,this.transform.position,Quaternion.identity);
						// Animate
						playerCube.GetComponent<Animation>().Play("CubeJump");
						Instantiate (jumpSE, Vector3.zero, Quaternion.identity);
						twoJump = true;
					}
					else if((Physics.Raycast (this.transform.position,Vector3.down,out hit,1f)
					         || Physics.Raycast (this.transform.position,new Vector3(-0.5f,-1,0),out hit,1f)
					         || Physics.Raycast (this.transform.position,new Vector3(0.5f,-1,0),out hit,1f)
					         || Physics.Raycast (this.transform.position,new Vector3(0.5f,-1,0),out hit,1f)
					         || Physics.Raycast (this.transform.position,new Vector3(0,-1,0.5f),out hit,1f)
					         || Physics.Raycast (this.transform.position,new Vector3(0,-1,-0.5f),out hit,1f))					        
					        && (hit.transform.tag == "map" || hit.transform.tag == "enermy" || hit.transform.tag == "Player"))
					{
						this.GetComponent<Rigidbody>().AddForce(0,jumpForce,0);
						Instantiate(jumpParticle,this.transform.position,Quaternion.identity);
						if(hit.transform.tag == "enermy" || hit.transform.tag == "Player")
							hit.rigidbody.AddForce(new Vector3(0,-jumpForce,0));
						playerCube.GetComponent<Animation>().Play("CubeJump");
						Instantiate (jumpSE, Vector3.zero, Quaternion.identity);
						twoJump = false;
					}
				}
				#endregion

				#region Air Jump
				if(Input.GetKey(KeyCode.Keypad4) || Input.GetKey(KeyCode.LeftArrow))
				{
					if((Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.RightArrow)) && !this.IsGround ())
					{
						this.GetComponent<Rigidbody> ().AddForce (this.transform.TransformDirection(-300,-150,0));
						playerCube.GetComponent<Animation>().Play("CubeDown");
						Instantiate (jumpSE, Vector3.zero, Quaternion.identity);
						Instantiate(jumpParticle,this.transform.position + Vector3.left,new Quaternion (0,0,-0.5f,0.866f));
					}
				}
				if(Input.GetKey(KeyCode.Keypad6) || Input.GetKey(KeyCode.RightArrow))
				{
					if((Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.LeftArrow)) && !this.IsGround ())
					{
						this.GetComponent<Rigidbody> ().AddForce (this.transform.TransformDirection(-300,-150,0));
						playerCube.GetComponent<Animation>().Play("CubeDown");
						Instantiate (jumpSE, Vector3.zero, Quaternion.identity);
						Instantiate(jumpParticle,this.transform.position + Vector3.right,new Quaternion (0,0,-0.866f,0.5f));
					}
				}
				#endregion
				
				#region Down
				if ((Input.GetKeyDown (KeyCode.Keypad5) || Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.Keypad2)) && downCnt < 3)
				{
					if(!Physics.Raycast (this.transform.position,Vector3.down,1f))
					{
						this.GetComponent<Rigidbody>().AddForce(0,-jumpForce,0);
						// Animate
						playerCube.GetComponent<Animation>().Play("CubeDown");
						Instantiate (jumpSE, Vector3.zero, Quaternion.identity);
						downCnt++;
					}
				}
				if((this.GetComponent<Rigidbody>().velocity.y == 0) || (downCnt > 0 && (Physics.Raycast (this.transform.position,Vector3.down,1f)
				                                                                        || Physics.Raycast (this.transform.position,new Vector3(-0.5f,-1,0),1f)
				                                                                        || Physics.Raycast (this.transform.position,new Vector3(0.5f,-1,0),1f)
				                                                                        || Physics.Raycast (this.transform.position,new Vector3(0,-1,0.5f),1f)
				                                                                        || Physics.Raycast (this.transform.position,new Vector3(0,-1,-0.5f),1f))))
				{
					downCnt = 0;
				}
				#endregion
				
				#region ATK

				if((Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Keypad0)) && !absortBack && !openAbsort)
				{
					openAbsort = true;
					absortConObj = Instantiate(absortCon, this.transform.position + this.transform.TransformDirection(new Vector3(1.2f,0.2f,0f)), Quaternion.identity) as Transform;
					absortConObj.parent = this.transform;
					Instantiate (jumpSE, Vector3.zero, Quaternion.identity);
				}
				else if((Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Keypad0)) && !absortBack && absortConObj)
				{
					sortRanSph.localScale = Vector3.Slerp(sortRanSph.localScale, new Vector3(10f,10f,10f), 0.1f);
					absortRanCol = Physics.OverlapSphere(this.transform.position,sortRanSph.localScale.x*0.5f);
					foreach(Collider hit in absortRanCol)
					{
						if(hit.tag == "absortAble" && !hit.GetComponent<AbsortAble>().isShoot)
						{
							//hit.transform.position = Vector3.SmoothDamp(hit.transform.position, absortConObj.position, ref absortRefV, 0.1f);
							hit.transform.position = Vector3.Lerp(hit.transform.position, absortConObj.position, 0.1f);
							hit.GetComponent<AbsortAble>().plrGet = true;
						}
					}
				}
				else if((Input.GetKeyUp(KeyCode.KeypadPlus) || Input.GetKeyUp(KeyCode.Keypad0)) && !absortBack)
				{
					absortBack = true;
					if(absortConObj)
					{
						int i=0;
						if (absortRanCol[0])
						{
							Collider[] waitIgon = new Collider[30];
							foreach(Collider hit in absortRanCol)
							{
								// shoot
								if(hit.tag == "absortAble")
								{
									Physics.IgnoreCollision(this.GetComponent<Collider>(),hit);
									// 忽略和其他顆
									for(int j=0;j<i;j++)
										Physics.IgnoreCollision(waitIgon[j],hit);
									
									hit.GetComponent<AbsortAble> ().ReadyShot ();
									hit.GetComponent<Rigidbody>().AddForce(this.transform.TransformDirection(new Vector3(1f,0.1f,0f))*atkPow);
									havAbsToShoot = true;

									waitIgon[i] = hit;
									i++;
								}
							}
							absortRanCol = null;
						}
						
						if(havAbsToShoot)
						{
							if (i > 5)
								i=5;
							this.GetComponent<Rigidbody>().AddForce(this.transform.TransformDirection(new Vector3(1f,0.1f,0f))*(-atkPow*0.5f)*i);
							Instantiate (atkSE, Vector3.zero, Quaternion.identity);
							// Animate
							playerCube.GetComponent<Animation>().CrossFade("CubeAtkBack");
						}else
							Instantiate (jumpSE, Vector3.zero, Quaternion.identity);
						Destroy(absortConObj.gameObject);
					}
				}
				else if(absortBack)
				{
					if(havAbsToShoot)
						playerCube.GetComponent<Animation>().CrossFade("CubeAtkBack");
					sortRanSph.localScale = Vector3.Slerp(sortRanSph.localScale, Vector3.one, 0.2f);
					if(sortRanSph.localScale == Vector3.one)
					{
						openAbsort = false;
						absortBack = false;
						havAbsToShoot = false;
					}
				}

				#endregion
			}
			
			if (!otherP && GameManager.fin) 
			{
				StartCoroutine ("Fin");
				GameManager.fin = false;
			}
		}

		#endregion

		#region -- 1P --

		else
		{
			p1Pos = this.transform.position;

			#region Hitted
			if (hitted)
			{
				if(playerCube.GetComponent<Animation>() ["CubeAtkBack"].speed > 0.01f)
				{
					playerCube.GetComponent<Animation>() ["CubeAtkBack"].speed -= 0.01f;
				}
				#region absortAble process
				sortRanSph.localScale = Vector3.Slerp(sortRanSph.localScale, Vector3.one, 0.1f);
				if(absortConObj)
				{
					int i=0;
					if (absortRanCol[0])
					{
						foreach(Collider hit in absortRanCol)
						{
							// shoot
							if(hit && hit.tag == "absortAble")
							{
								Physics.IgnoreCollision(this.GetComponent<Collider>(),hit);
								// 忽略和其他顆
								for(int j=0;j<i;j++)
									Physics.IgnoreCollision(absortRanCol[j].GetComponent<Collider>(),hit);
								
								hit.GetComponent<AbsortAble> ().ReadyShot ();
								i++;
							}
						}
						absortRanCol = null;
					}
					Destroy(absortConObj.gameObject);
				}
				#endregion
				// count down and close;
				if((this.GetComponent<Rigidbody>().velocity.x < 0.1f && this.GetComponent<Rigidbody>().velocity.y < 0.1f && this.GetComponent<Rigidbody>().velocity.z < 0.1f) || Physics.Raycast (this.transform.position,Vector3.down,1f))
				{
					// Color Blink
					float r = playerCube.GetComponent<Renderer>().material.color.r;
					float g = playerCube.GetComponent<Renderer>().material.color.g;
					float b = playerCube.GetComponent<Renderer>().material.color.b;
					Vector3 rgb = new Vector3(r,g,b);
					rgb = Vector3.Slerp(rgb, new Vector3(0.5f,0.5f,0.5f), 0.1f);
					r = rgb.x;
					g = rgb.y;
					b = rgb.z;
					playerCube.GetComponent<Renderer>().material.color = new Color(r,g,b);
					if(playerCube.GetComponent<Renderer>().material.color == Color.gray)
					{
						hitted = false;
						plrBoomObj.GetComponent<ParticleRenderer>().material = playerCube.GetComponent<MeshRenderer>().material;
						Instantiate (plrBoomObj, this.transform.position, Quaternion.identity);
						Instantiate (boomSE, Vector3.zero, Quaternion.identity);
						GameManager.plrDeadPos = this.transform.position;
						gOver.SetActive (true);
						gOver.GetComponent<GOControl> ().OnePDead ();
						Destroy (this.gameObject);
					}
				}
			}
			#endregion

			else
			{
				#region MOVE & Direction & Animate
				if (Input.GetAxis ("Horizontal") != 0)
				{
					// TwoRun (State)
					if((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) && Mathf.Abs(Input.GetAxis ("Horizontal")) > 0.05f)
					{
						twoRun = true;
					}
					// Move
					if(twoRun)
						this.transform.Translate (Input.GetAxis ("Horizontal")* speed* 1.5f* Time.deltaTime,0,0,Space.World);
					else
						this.transform.Translate (Input.GetAxis ("Horizontal")* speed* Time.deltaTime,0,0,Space.World);	
					
					// Direction
					if(Input.GetAxis ("Horizontal") > 0)
						this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, Vector3.zero, 0.1f);
					else
						this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, new Vector3(0f,180f,0f), 0.1f);
					
					// Animate
					if(Physics.Raycast (this.transform.position,Vector3.down,1f))
					{
						if(twoRun)
							playerCube.GetComponent<Animation>()["CubeRun"].speed = Mathf.Abs (Input.GetAxis ("Horizontal")) * 1.5f;
						else
							playerCube.GetComponent<Animation>()["CubeRun"].speed = Mathf.Abs (Input.GetAxis ("Horizontal"));
					}
					else
					{
						if(twoRun)
							playerCube.GetComponent<Animation>()["CubeRun"].speed = Mathf.Abs (Input.GetAxis ("Horizontal"));
						else
							playerCube.GetComponent<Animation>()["CubeRun"].speed = Mathf.Abs (Input.GetAxis ("Horizontal")) * 0.5f;
					}
					playerCube.GetComponent<Animation>().CrossFade("CubeRun");
					// reflex
					RaycastHit hit;
					if(Physics.Raycast (this.transform.position,this.transform.right,out hit,0.5f) && hit.transform.tag == "map")
					{
						this.GetComponent<Rigidbody>().AddForce(Vector3.down*Mathf.Abs(Input.GetAxis ("Horizontal"))* reflaxPow);
						twoJump = false;
					}
				}
				else
				{
					twoRun = false;
					playerCube.GetComponent<Animation>().CrossFade("CubeIdle");
				}
				#endregion
				
				#region JUMP
				if (Input.GetKeyDown (KeyCode.W))
				{
					RaycastHit hit;
					if (this.GetComponent<Rigidbody>().velocity.y == 0)
					{
						this.GetComponent<Rigidbody>().AddForce(0,jumpForce,0);
						Instantiate(jumpParticle,this.transform.position,Quaternion.identity);
						// Animate
						playerCube.GetComponent<Animation>().Play("CubeJump");
						Instantiate (jumpSE, Vector3.zero, Quaternion.identity);
						twoJump = false;
					}
					else if(!twoJump)
					{
						if(Physics.Raycast (this.transform.position,this.transform.right,out hit,0.5f) && (hit.transform.tag == "map" || hit.transform.tag == "enermy" || hit.transform.tag == "Player"))
						{
							Vector3 twoJumpF = new Vector3(-Input.GetAxis ("Horizontal")*500f,jumpForce*0.7f,0);
							this.GetComponent<Rigidbody>().AddForce(twoJumpF);
							if(hit.transform.tag == "enermy" || hit.transform.tag == "Player")
								hit.rigidbody.AddForce(twoJumpF * (-1));
						}
						else
						{
							this.GetComponent<Rigidbody>().AddForce(0,jumpForce*0.7f,0);
						}
						Instantiate(jumpParticle,this.transform.position,Quaternion.identity);
						// Animate
						playerCube.GetComponent<Animation>().Play("CubeJump");
						Instantiate (jumpSE, Vector3.zero, Quaternion.identity);
						twoJump = true;
					}
					else if((Physics.Raycast (this.transform.position,Vector3.down,out hit,1f)
					         || Physics.Raycast (this.transform.position,new Vector3(-0.5f,-1,0),out hit,1f)
					         || Physics.Raycast (this.transform.position,new Vector3(0.5f,-1,0),out hit,1f)
					         || Physics.Raycast (this.transform.position,new Vector3(0.5f,-1,0),out hit,1f)
					         || Physics.Raycast (this.transform.position,new Vector3(0,-1,0.5f),out hit,1f)
					         || Physics.Raycast (this.transform.position,new Vector3(0,-1,-0.5f),out hit,1f))					        
					         && (hit.transform.tag == "map" || hit.transform.tag == "enermy" || hit.transform.tag == "Player"))
					{
						this.GetComponent<Rigidbody>().AddForce(0,jumpForce,0);
						Instantiate(jumpParticle,this.transform.position,Quaternion.identity);
						if(hit.transform.tag == "enermy" || hit.transform.tag == "Player")
							hit.rigidbody.AddForce(new Vector3(0,-jumpForce,0));
						playerCube.GetComponent<Animation>().Play("CubeJump");
						Instantiate (jumpSE, Vector3.zero, Quaternion.identity);
						twoJump = false;
					}
				}
				#endregion

				#region Air Jump
				if(Input.GetKey(KeyCode.A))
				{
					if(Input.GetKeyDown(KeyCode.D) && !this.IsGround ())
					{
						this.GetComponent<Rigidbody> ().AddForce (this.transform.TransformDirection(-300,-150,0));
						playerCube.GetComponent<Animation>().Play("CubeDown");
						Instantiate (jumpSE, Vector3.zero, Quaternion.identity);
						Instantiate(jumpParticle,this.transform.position + Vector3.left,new Quaternion (0,0,-0.5f,0.866f));
					}
				}
				if(Input.GetKey(KeyCode.D))
				{
					if(Input.GetKeyDown(KeyCode.A) && !this.IsGround ())
					{
						this.GetComponent<Rigidbody> ().AddForce (this.transform.TransformDirection(-300,-150,0));
						playerCube.GetComponent<Animation>().Play("CubeDown");
						Instantiate (jumpSE, Vector3.zero, Quaternion.identity);
						Instantiate(jumpParticle,this.transform.position + Vector3.right,new Quaternion (0,0,-0.866f,0.5f));
					}
				}
				#endregion

				#region Down
				if (Input.GetKeyDown (KeyCode.S) && downCnt < 3)
				{
					if(!Physics.Raycast (this.transform.position,Vector3.down,1f))
					{
						this.GetComponent<Rigidbody>().AddForce(0,-jumpForce,0);
						// Animate
						playerCube.GetComponent<Animation>().Play("CubeDown");
						Instantiate (jumpSE, Vector3.zero, Quaternion.identity);
						downCnt++;
					}
				}
				if((this.GetComponent<Rigidbody>().velocity.y == 0) || (downCnt > 0 && (Physics.Raycast (this.transform.position,Vector3.down,1f)
				                                                                        || Physics.Raycast (this.transform.position,new Vector3(-0.5f,-1,0),1f)
				                                                                        || Physics.Raycast (this.transform.position,new Vector3(0.5f,-1,0),1f)
				                                                                        || Physics.Raycast (this.transform.position,new Vector3(0,-1,0.5f),1f)
				                                                                        || Physics.Raycast (this.transform.position,new Vector3(0,-1,-0.5f),1f))))
				{
					downCnt = 0;
				}
				#endregion
				
				#region ATK
				if(Input.GetKey(KeyCode.Space) && !absortBack && !openAbsort)
				{
					openAbsort = true;
					absortConObj = Instantiate(absortCon, this.transform.position + this.transform.TransformDirection(new Vector3(1.2f,0.2f,0f)), Quaternion.identity) as Transform;
					absortConObj.parent = this.transform;
					Instantiate (jumpSE, Vector3.zero, Quaternion.identity);
				}
				else if(Input.GetKey(KeyCode.Space) && !absortBack && absortConObj)
				{
					sortRanSph.localScale = Vector3.Slerp(sortRanSph.localScale, new Vector3(10f,10f,10f), 0.1f);
					absortRanCol = Physics.OverlapSphere(this.transform.position,sortRanSph.localScale.x*0.5f);
					foreach(Collider hit in absortRanCol)
					{
						if(hit.tag == "absortAble" && !hit.GetComponent<AbsortAble>().isShoot)
						{
							//hit.transform.position = Vector3.SmoothDamp(hit.transform.position, absortConObj.position, ref absortRefV, 0.1f);
							hit.transform.position = Vector3.Lerp(hit.transform.position, absortConObj.position, 0.1f);
							hit.GetComponent<AbsortAble>().plrGet = true;
						}
					}
				}
				else if(Input.GetKeyUp(KeyCode.Space) && !absortBack)
				{
					absortBack = true;
					if(absortConObj)
					{
						int i=0;
						if (absortRanCol[0])
						{
							Collider[] waitIgon = new Collider[30];
							foreach(Collider hit in absortRanCol)
							{
								// shoot
								if(hit.tag == "absortAble")
								{
									Physics.IgnoreCollision(this.GetComponent<Collider>(),hit);
									// 忽略和其他顆
									for(int j=0;j<i;j++)
										Physics.IgnoreCollision(waitIgon[j],hit);
									
									hit.GetComponent<AbsortAble> ().ReadyShot ();
									hit.GetComponent<Rigidbody>().AddForce(this.transform.TransformDirection(new Vector3(1f,0.1f,0f))*atkPow);
									havAbsToShoot = true;
									
									waitIgon[i] = hit;
									i++;
								}
							}
							absortRanCol = null;
						}
						
						if(havAbsToShoot)
						{
							if (i > 5)
								i=5;
							this.GetComponent<Rigidbody>().AddForce(this.transform.TransformDirection(new Vector3(1f,0.1f,0f))*(-atkPow*0.5f)*i);
							Instantiate (atkSE, Vector3.zero, Quaternion.identity);
							// Animate
							playerCube.GetComponent<Animation>().CrossFade("CubeAtkBack");
						}
						else
							Instantiate (jumpSE, Vector3.zero, Quaternion.identity);
						Destroy(absortConObj.gameObject);
					}
				}
				else if(absortBack)
				{
					if(havAbsToShoot)
						playerCube.GetComponent<Animation>().CrossFade("CubeAtkBack");
					sortRanSph.localScale = Vector3.Slerp(sortRanSph.localScale, Vector3.one, 0.2f);
					if(sortRanSph.localScale == Vector3.one)
					{
						openAbsort = false;
						absortBack = false;
						havAbsToShoot = false;
					}
				}
				#endregion
			}
			
			if (GameManager.fin) 
			{
				StartCoroutine ("Fin");
				GameManager.fin = false;
			}
		}

		#endregion
	}

	public void GetDmg()
	{
		if (!GameManager.super) 
		{
			this.tag = "Untagged";
			playerCube.GetComponent<Animation>().CrossFade ("CubeAtkBack");
			hitted = true;
		}
	}

	public void DownCntRst () // down count reset
	{
		downCnt = 0;
		twoJump = false;
	}

	IEnumerator Fin()
	{
		yield return new WaitForSeconds (10f);
		finCan.SetActive (true);
	}

	void OnCollisionEnter (Collision collision)
	{
		// 落地音
		if (hitted) {
			if (Mathf.Abs (this.GetComponent<Rigidbody> ().velocity.y) > 2 && Physics.Raycast (this.transform.position, Vector3.down, 1f)) {
				Instantiate (jumpSE, Vector3.zero, Quaternion.identity);
			}
		}
	}

	bool IsGround ()
	{
		RaycastHit hit;
		return (Physics.Raycast (this.transform.position, Vector3.down, out hit, 1f)
			|| Physics.Raycast (this.transform.position, new Vector3 (-0.5f, -1, 0), out hit, 1f)
			|| Physics.Raycast (this.transform.position, new Vector3 (0.5f, -1, 0), out hit, 1f)
			|| Physics.Raycast (this.transform.position, new Vector3 (0.5f, -1, 0), out hit, 1f)
			|| Physics.Raycast (this.transform.position, new Vector3 (0, -1, 0.5f), out hit, 1f)
			|| Physics.Raycast (this.transform.position, new Vector3 (0, -1, -0.5f), out hit, 1f))					        
			&& (hit.transform.tag == "map" || hit.transform.tag == "enermy" || hit.transform.tag == "Player");
	}
}
