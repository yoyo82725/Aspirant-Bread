using UnityEngine;
using System.Collections;

public class AACreator : MonoBehaviour {
	public GameObject abAObj;

	private Transform nowAbAObj; // now AbsortAble Obj
	private bool createEnd;
	// Use this for initialization
	void Start () {
		//this.CreateAAObj ();
		this.CreateAAObj ();
	}
	
	// Update is called once per frame
	void Update () {
		if (createEnd)
		{
			if(!nowAbAObj)
			{
				//this.CreateAAObj ();
				StartCoroutine ("Creator");
				createEnd = false;
			}
		}
		else if(nowAbAObj)
		{
			if (!nowAbAObj.GetComponent<Animation>().isPlaying)
			{
				nowAbAObj.tag = "absortAble";
				createEnd = true;
				Destroy(nowAbAObj.GetChild(0).gameObject);
			}
		}
	}

	void CreateAAObj()
	{
		nowAbAObj = (Instantiate (abAObj, this.transform.position, Quaternion.identity) as GameObject).transform;
	}

	IEnumerator Creator()
	{
		// cool down
		yield return new WaitForSeconds (2.5f);
		this.CreateAAObj ();
	}

	void OnDrawGizmos ()
	{
		Gizmos.DrawSphere (this.transform.position, 1f);
	}

	public Transform GetAAObj()
	{
		if (createEnd)
			return this.nowAbAObj;
		else
			return null;
	}
}
