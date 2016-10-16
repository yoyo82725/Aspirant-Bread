using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static Vector3 plrDeadPos,plr2DPos; // player dead position
	public static bool super = false;
	public static bool fin = false;
	public static bool prompt = true;
	public static bool bossed = false;
	public static bool twoP = false;
	public static float clickVol = 0.3f;

	/* 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	*/

	// play se near player
	public static void PlaySEnearP (GameObject se, Vector3 sourcePos)
	{
		if(GameManager.twoP)
		{
			Vector3 newV = sourcePos - PControl.p1Pos;
			float p1dis = Mathf.Abs(newV.x) + Mathf.Abs(newV.y) + Mathf.Abs(newV.z);
			Vector3 newV2 = sourcePos - PControl.p2Pos;
			float p2dis = Mathf.Abs(newV2.x) + Mathf.Abs(newV2.y) + Mathf.Abs(newV2.z);

			Vector3 editPos = sourcePos - PControl.p1Pos;
			editPos.z = 0;

			if (p1dis < p2dis)
				Instantiate (se, editPos, Quaternion.identity);
			else
				Instantiate (se, editPos, Quaternion.identity);
		}
		else
			Instantiate (se, sourcePos - PControl.p1Pos, Quaternion.identity);
	}

	// play SE near player return gameObject
	public static GameObject PlySEnerPRG (GameObject se, Vector3 sourcePos)
	{
		if(GameManager.twoP)
		{
			Vector3 newV = sourcePos - PControl.p1Pos;
			float p1dis = Mathf.Abs(newV.x) + Mathf.Abs(newV.y) + Mathf.Abs(newV.z);
			Vector3 newV2 = sourcePos - PControl.p2Pos;
			float p2dis = Mathf.Abs(newV2.x) + Mathf.Abs(newV2.y) + Mathf.Abs(newV2.z);
			
			Vector3 editPos = sourcePos - PControl.p1Pos;
			editPos.z = 0;
			
			if (p1dis < p2dis)
				return Instantiate (se, editPos, Quaternion.identity) as GameObject;
			else
				return Instantiate (se, editPos, Quaternion.identity) as GameObject;
		}
		else
			return Instantiate (se, sourcePos - PControl.p1Pos, Quaternion.identity) as GameObject;
	}
}
