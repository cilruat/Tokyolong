using UnityEngine;
using System.Collections;

namespace Bowling
{
public class pin : MonoBehaviour {
	bool Q;
	score sco;
	public GameObject text;
	void Start () {
		sco = text.GetComponent<score> ();
	}
	
	void Update () {
		if (Q == false) {
			if (transform.rotation.eulerAngles.z > 5) {
				Q = true;
				sco.scor += 1;
			}
		}
	}
}
}