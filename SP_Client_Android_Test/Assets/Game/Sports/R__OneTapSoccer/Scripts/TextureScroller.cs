using UnityEngine;
using System.Collections;

namespace OnetapSoccer
{
public class TextureScroller : MonoBehaviour {

	/// <summary>
	/// This script will scroll the background texture of the game. (Simulate clouds movement)
	/// </summary>

	private float offset;
	private float damper = 0.1f;

	[Range(1, 4)]
	public float coef = 2.0f;
	
	void Update () {
		offset +=  damper * Time.deltaTime * coef * (GetComponent<Renderer>().material.mainTextureScale.x / 1.5f);
		GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", new Vector2(offset, 0));
	}
}
}