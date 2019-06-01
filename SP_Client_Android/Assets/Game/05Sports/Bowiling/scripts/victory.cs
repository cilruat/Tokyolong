using UnityEngine;
using System.Collections;

namespace Bowling
{
public class victory : MonoBehaviour {
	public int levelgame;
	public int winscore;
	public GameObject wintext;
	public GameObject losttext;
	void Start () {
	
	}
	
	void Update () {
		if (savescore.level == levelgame) {
			if (score.officialscore >= winscore) {

				Destroy(gameObject);
				wintext.SetActive(true);
			} else {
				losttext.SetActive(true);
				Destroy(gameObject);
			}
		}
	}
}
}