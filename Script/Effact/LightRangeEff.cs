using UnityEngine;
using System.Collections;

public class LightRangeEff : MonoBehaviour {
	public Material alpha;
	
	//private float lifeTime;

	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, 1.5f);
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Renderer>().material.Lerp (this.GetComponent<Renderer>().material, alpha, 0.03f);
		this.transform.localScale = Vector3.Lerp (this.transform.localScale, new Vector3 (10, 10, 10), 0.1f);
		this.GetComponent<Light> ().intensity -= Time.deltaTime;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" || other.tag == "enermy" || other.gameObject.layer == 9) 
		{
			Vector3 lookDir = other.transform.position - this.transform.position;
			if (other.gameObject.layer == 9)
				other.GetComponent<Rigidbody>().AddForce(lookDir.normalized*500);
			else
				other.GetComponent<Rigidbody>().AddForce(lookDir.normalized*1000);

			if (other.gameObject.layer != 9)
			{
				RaycastHit hit;
				//lookDir = new Vector3(lookDir.x,0,0);
				if(Physics.Raycast (other.transform.position,lookDir,out hit,1f) && hit.transform.tag == "map")
				{
					other.SendMessage ("GetDmg");
				}
			}
		}
	}
}
