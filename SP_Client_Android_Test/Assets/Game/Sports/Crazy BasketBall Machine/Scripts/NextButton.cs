using UnityEngine;
using System.Collections;

public class NextButton : MonoBehaviour {

	private GameMgr gamemanagerscript;

	// Use this for initialization
	void Start () {
	
		gamemanagerscript = GameObject.Find ("GameManager").GetComponent<GameMgr> ();
	}
	
	public void startnext()
	{
		gamemanagerscript.StartNextLevel ();
	}
}
