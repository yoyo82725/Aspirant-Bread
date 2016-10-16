using UnityEngine;
using System.Collections;

public class AtkRangeEff : MonoBehaviour {
	public Material alpha;

	private float lifeTime;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (lifeTime > 1.5f)
		{
			//float alpha = this.renderer.material.color.a;
			//alpha -= Time.deltaTime;
			this.GetComponent<Renderer>().material.Lerp (this.GetComponent<Renderer>().material, alpha, 0.1f);

		}
		else
		{
			this.transform.localScale = Vector3.Lerp (this.transform.localScale, new Vector3 (20, 20, 20), 0.1f);
			lifeTime += Time.deltaTime;
		}
	}
}
