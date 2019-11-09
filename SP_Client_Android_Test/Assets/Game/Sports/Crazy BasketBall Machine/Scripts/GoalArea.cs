using UnityEngine;
using System.Collections;

public class GoalArea : MonoBehaviour {

	public ParticleSystem psStar;
	public ParticleSystem psBigStar;
	private GameMgr gamemanagerscript;
	public AudioClip hitnetSound;


	// Use this for initialization
	void Start () {

		gamemanagerscript = GameObject.Find ("GameManager").GetComponent<GameMgr> ();
	}



	void OnTriggerEnter (Collider other) {

		if (other.gameObject.tag=="BasketBall"||other.gameObject.tag=="BasketBallFire") {
			// Goal!!
			AudioSource.PlayClipAtPoint(hitnetSound, gameObject.transform.position);
			gamemanagerscript.addScore();

			if (!gamemanagerscript.islastshot)
				psStar.Play ();
			else
				psBigStar.Play ();
		}
	}

}
