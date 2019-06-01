using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Bowling
{
public class savescore : MonoBehaviour {
	public Text gameobj;
	public Text ttextt;
	public Text p2text;
	public static int level;
	public bool multip;
	void Start () {
	}
	
	void Update () {

		gameobj.text = score.officialscore.ToString ();
		ttextt.text = level.ToString ();
		if (multip == true) {
			p2text.text = multiplayer.player2score.ToString();
		}
	}
	public void onclick(){

		level = 0;
		score.officialscore = 0;
		Application.LoadLevel ("menu");
	}
}
}