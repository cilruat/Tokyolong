using UnityEngine;
using System.Collections;
namespace Bowling
{
public class multiplayer : MonoBehaviour {
	public GameObject p1text;
	public GameObject p2text;
	public static bool Player1 = true;
	public static int player2score;
	public int levelgame;
	public GameObject p1win;
	public GameObject p2win;
	public GameObject equal;
	void Start () {
		if (Player1 == true) {
			p1text.SetActive (true);
		} else
			p2text.SetActive (true);
	}
	
	void Update () {
		if (savescore.level == levelgame) {
			if(score.officialscore > player2score){
				Destroy(gameObject);
				p1win.SetActive(true);
			}
			if(score.officialscore < player2score){	
				Destroy(gameObject);
				p2win.SetActive(true);
			}
            if(score.officialscore == player2score){
				Destroy(gameObject);
				equal.SetActive(true);
		}
	
	}
	}
}
}