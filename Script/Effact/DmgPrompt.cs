using UnityEngine;
using System.Collections;

public class DmgPrompt : MonoBehaviour {
	private bool on, alphaUp, alphaDown;
	private float alpha;

	// Use this for initialization
	void Start () {
		this.GetComponent<CanvasRenderer> ().SetColor (new Color (1, 1, 1, 0));
	}
	
	// Update is called once per frame
	void Update () {
		if (on) 
		{
			float flag = Time.deltaTime + Time.deltaTime + Time.deltaTime + Time.deltaTime + Time.deltaTime;
			if (alphaUp)
			{
				if(alpha < 1)
					alpha += flag;
				else
				{
					alpha = 1;
					alphaUp = false;
				}
			}
			else if (alphaDown)
			{
				if(alpha > 0)
					alpha -= flag;
				else
				{
					alpha = 0;
					alphaDown = false;
				}
			}
			this.GetComponent<CanvasRenderer> ().SetColor (new Color (1, 1, 1, alpha));
		}
	}

	void On ()
	{
		alpha = 0f;
		on = true;
		this.GetComponent<CanvasRenderer> ().SetColor (new Color (1, 1, 1, alpha));
	}

	void Off ()
	{
		alpha = 0f;
		on = false;
		this.GetComponent<CanvasRenderer> ().SetColor (new Color (1, 1, 1, alpha));
	}

	void AlphaUp ()
	{
		alphaUp = true;
		alphaDown = false;
	}

	void AlphaDown ()
	{
		alphaDown = true;
		alphaUp = false;
	}
}
