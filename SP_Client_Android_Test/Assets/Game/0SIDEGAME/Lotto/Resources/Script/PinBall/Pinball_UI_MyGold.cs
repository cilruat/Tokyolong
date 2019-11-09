using UnityEngine;
using System.Collections;

/// <summary>
// UI MyGold
/// </summary>
public class Pinball_UI_MyGold : MonoBehaviour {
	public TextMesh text;

	// Update My Gold!
	void Update () {
		text.text = ""+PlayerMeta.GetGold();		
	}
}
