using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace Bowling
{
public class score : MonoBehaviour {

	public int scor;
	public Text TE;
	public static int officialscore;
	public float timer = 5f;
	public bool active =true;
	public bool Multiplayer;
	void Start () {
		if (Multiplayer == false) {
			officialscore += scor;
		} else {
			if(multiplayer.Player1 == true){
				officialscore += scor;
			}else{
				multiplayer.player2score += scor;
			}
		}
	}	
	void Update () {

		TE.text = scor.ToString ();
		timer -= Time.deltaTime;
		if (timer <= 0) {
			if(Multiplayer == false){
			Application.LoadLevel ("saloon");
			}else{
				if(multiplayer.Player1 == true){
					multiplayer.Player1 = false;
				}else{
					multiplayer.Player1 = true;
				}
				Application.LoadLevel("saloonmultiplayer");
			}
			savescore.level += 1;
		}
		if (active == true) {
			if (scor == 10) {
				GetComponent<AudioSource> ().Play ();
				active = false;
			}
		}
	}
}
}