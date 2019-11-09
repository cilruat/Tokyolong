using UnityEngine;
using System.Collections;

namespace Bowling
{
public class click : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
	
	}
	public void singleplayer(){
		Application.LoadLevel ("saloon");
	}
	public void Multiplayer(){
		multiplayer.player2score = 0;
		multiplayer.Player1 = true;
		Application.LoadLevel ("saloonmultiplayer");
	}
	public void exit(){
		Application.Quit ();
	}
}
}