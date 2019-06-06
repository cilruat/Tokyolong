using UnityEngine;
using System.Collections;
namespace Turnbase
{
public class OpponentColliderManager : MonoBehaviour {

	/// <summary>
	/// Calculate the score and destroys the gameObject, 
	/// when player drops opponent ball(s) out of the board.
	/// </summary>


	public AudioClip dieSfx;			//Sfx to play when this object is getting destroyed
	public GameObject scoreDisplay;		//reference to scoreManager prefab to show the score


	void Start() {

	}


	/// <summary>
	/// play the Sfx and destroy the ball upon colliding with the trigger (Borders)
	/// </summary>
	void OnTriggerEnter(Collider other) {

		//decide when to destroy the ball object
		if(other.tag == "Border") {
			//player won't get a score for opponent's mistakes. 
			//They only receive score when they drop opponent's balls out.
			if(GlobalGameManager.playersTurn) {
				OpponentAI.scoreQueue += 1;		//incremental score system
				GameObject myScorePresentor = Instantiate(scoreDisplay, transform.position, transform.rotation) as GameObject;
				myScorePresentor.name = "ScoreManager";
				myScorePresentor.transform.localEulerAngles = new Vector3(90,0,0);
				myScorePresentor.transform.position = new Vector3(	myScorePresentor.transform.position.x,
																	3,
																	myScorePresentor.transform.position.z);
				int score = OpponentAI.scoreQueue * GlobalGameManager.setCoef;	
				GlobalGameManager.score += score;
				myScorePresentor.GetComponent<ScoreManager>().myText = score.ToString();;
			}
			
			GetComponent<AudioSource>().clip = dieSfx;
			if(!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().Play();

			GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
			GetComponent<Rigidbody>().drag = 1000;
			gameObject.tag = "Untagged";
			gameObject.GetComponent<SphereCollider>().enabled = false;
			StartCoroutine(fadeout());
		}
	}


	/// <summary>
	/// Hide and destroy the ball
	/// </summary>
	IEnumerator fadeout (){
		float t = 0.0f;
		while(t < 1) {
			t += Time.deltaTime * 5.0f;
			GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r,
												GetComponent<Renderer>().material.color.g,
												GetComponent<Renderer>().material.color.b,
												1 - t);
			yield return 0;
		}

		if(t >= 1)
			Destroy(gameObject);
	}
	}
}