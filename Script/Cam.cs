using UnityEngine;
using System.Collections;

public class Cam : MonoBehaviour {
	private int mode = 0;
	// 0 toPlayer
	// 1 toBoss
	
	public Transform target;
	public Transform mode1Target;

	// right & left
	private float fixX,fixY;
	private bool turnL,turnR;
	private float destX;
	private float fixRoL; // not fix, change and tmp
	private Vector3 iniRot = new Vector3(3.265761f,62.43021f,0f);
	private Vector3 iniPos = new Vector3(-5.809719f,5f,-4.41862f);
	// rightUp & leftUp
	private bool turnLUp,turnRUp;
	private Vector3 iniRotUp = new Vector3(332.5548f,62.43021f,0f);
	private float destY;
	private float fixRoUp; // not fix, change and tmp
	private bool twoP;

	// Use this for initialization
	void Start () {
		mode = 0;
		destX = -5.8f;
		destY = 1.5f;
		if (GameManager.twoP)
		{
			if(this.name == "1P Cam")
				this.GetComponent<Camera> ().rect = new Rect (0,0,1,0.5f);
			else if(this.name == "2P Cam")
			{
				this.GetComponent<Camera> ().rect = new Rect (0,0.5f,1,1);
				twoP = true;
			}
		}
		else
		{
			this.GetComponent<Camera> ().rect = new Rect (0,0,1,1);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (mode == 0)
		{
			#region Rotate to target
			// Rotate To Player
			if(turnR)
			{
				fixRoL = Vector3.Slerp(new Vector3(fixRoL,0), new Vector3(0,0), 0.1f).x;
				fixRoUp = Vector3.Slerp(new Vector3(fixRoUp,0), new Vector3(0,0), 0.1f).x;
				this.transform.localEulerAngles = new Vector3(iniRot.x - fixRoUp, iniRot.y - fixRoL, iniRot.z);
				if(this.transform.localEulerAngles == iniRot)
					turnR = false;
			}
			else if(turnL)
			{
				fixRoL = Vector3.Slerp(new Vector3(fixRoL,0), new Vector3(iniRot.y + iniRot.y,0), 0.1f).x;
				fixRoUp = Vector3.Slerp(new Vector3(fixRoUp,0), new Vector3(0,0), 0.1f).x;
				this.transform.localEulerAngles = new Vector3(iniRot.x - fixRoUp, iniRot.y - fixRoL, iniRot.z);
				if(this.transform.localEulerAngles == new Vector3(iniRot.x, iniRot.y + iniRot.y, iniRot.z))
					turnL = false;
			}
			else if(turnRUp)
			{
				fixRoL = Vector3.Slerp(new Vector3(fixRoL,0), new Vector3(0,0), 0.1f).x;
				fixRoUp = Vector3.Slerp(new Vector3(fixRoUp,0), new Vector3(30.711f,0), 0.1f).x;
				this.transform.localEulerAngles = new Vector3(iniRot.x - fixRoUp, iniRot.y - fixRoL, iniRot.z);
				if(this.transform.localEulerAngles == iniRotUp)
					turnRUp = false;
			}
			else if(turnLUp)
			{
				fixRoL = Vector3.Slerp(new Vector3(fixRoL,0), new Vector3(iniRot.y + iniRot.y,0), 0.1f).x;
				fixRoUp = Vector3.Slerp(new Vector3(fixRoUp,0), new Vector3(30.711f,0), 0.1f).x;
				this.transform.localEulerAngles = new Vector3(iniRot.x - fixRoUp, iniRot.y - fixRoL, iniRot.z);
				if(this.transform.localEulerAngles == new Vector3(iniRotUp.x, iniRot.y + iniRot.y, iniRotUp.z))
					turnLUp = false;
			}

			if (twoP)
			{
				if(Input.GetKeyDown(KeyCode.Keypad9))
				{
					// See to right
					this.RotaToRight ();
				}
				else if(Input.GetKeyDown(KeyCode.Keypad7))
				{
					// See to Left
					this.RotaToLeft ();
				}
				else if(Input.GetKeyDown(KeyCode.Keypad3))
				{
					// See To Right Up
					this.RotaToRUp ();
				}
				else if(Input.GetKeyDown(KeyCode.Keypad1))
				{
					// See To Left Up
					this.RotaToLUp ();
				}
			}
			else // 1P
			{
				if(Input.GetKeyDown(KeyCode.E))
				{
					// See to right
					this.RotaToRight ();
				}
				else if(Input.GetKeyDown(KeyCode.Q))
				{
					// See to Left
					this.RotaToLeft ();
				}
				else if(Input.GetKeyDown(KeyCode.C))
				{
					// See To Right Up
					this.RotaToRUp ();
				}
				else if(Input.GetKeyDown(KeyCode.Z))
				{
					// See To Left Up
					this.RotaToLUp ();
				}
			}
			#endregion
			
			#region Follow target
			if(target)
			{
				fixX = target.position.x + destX;
				fixY = target.position.y + destY;
				if (this.transform.position != new Vector3 (fixX, fixY, iniPos.z)) {
					this.transform.position = Vector3.Lerp(this.transform.position, new Vector3 (fixX, fixY, iniPos.z), 0.1f);
				}
			}
			else
			{
				if(twoP)
				{
					fixX = GameManager.plr2DPos.x + destX;
					fixY = GameManager.plr2DPos.y + destY;
				}
				else
				{
					fixX = GameManager.plrDeadPos.x + destX;
					fixY = GameManager.plrDeadPos.y + destY;
				}
				if (this.transform.position != new Vector3 (fixX, fixY, iniPos.z)) {
					this.transform.position = Vector3.Lerp(this.transform.position, new Vector3 (fixX, fixY, iniPos.z), 0.1f);
				}
			}
			#endregion
		}
		else if (mode == 1)
		{
			// Rotate Change
			Vector3 relativePos = mode1Target.position - this.transform.position;
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(relativePos), 0.05f);
			// Position Change
			this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(195.32063f,17.33699f,0f), 0.05f);
		}
	}

	public void ChangeMode(int co)
	{
		mode = co;
		if (co == 0)
			this.RotaToRight ();
	}

	void RotaToRight()
	{
		turnR = true;
		turnL = false;
		turnRUp = false;
		turnLUp = false;
		destX = -5.8f;
		destY = 1.5f;
	}

	void RotaToLeft()
	{
		turnL = true;
		turnR = false;
		turnRUp = false;
		turnLUp = false;
		destX = 5.8f;
		destY = 1.5f;
	}

	void RotaToRUp()
	{
		turnRUp = true;
		turnL = false;
		turnR = false;
		turnLUp = false;
		destX = -5.8f;
		destY = -0.25f;
	}

	void RotaToLUp()
	{
		turnLUp = true;
		turnL = false;
		turnR = false;
		turnRUp = false;
		destX = 5.8f;
		destY = -0.25f;
	}
}
