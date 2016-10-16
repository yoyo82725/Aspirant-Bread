using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour {

	/*
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	*/

	void OnParticleCollision(GameObject other)
	{
		if (other.tag == "Player" || other.tag == "enermy" || other.layer == 9) 
		{
			Vector3 lookDir = other.transform.position - this.transform.position;
			if (other.layer == 9)
				other.GetComponent<Rigidbody>().AddForce(lookDir.normalized*50);
			else
				other.GetComponent<Rigidbody>().AddForce(lookDir.normalized*100);

			if (other.layer != 9)
			{
				RaycastHit hit;
				lookDir = new Vector3(lookDir.x,0,0);
				if(Physics.Raycast (other.transform.position,lookDir,out hit,1f))
				{
					if(hit.transform.tag == "map")
						other.SendMessage ("GetDmg");
				}
			}
		}
	}

}
