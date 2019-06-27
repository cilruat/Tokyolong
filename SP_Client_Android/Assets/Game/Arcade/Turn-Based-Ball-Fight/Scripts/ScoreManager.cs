using UnityEngine;
using System.Collections;
namespace Turnbase
{
public class ScoreManager : MonoBehaviour {

	/// <summary>
	/// When player drops one or more of opponents ball out of the board,
	/// we create a 3d text object over the lost ball to show the score to the player.
	/// this class is the main manager for score object which appears in scene.
	/// It will be initiated by the "OpponentColliderManager" class.
	/// </summary>

	internal string myText = "";	//score string - set from "OpponentColliderManager" class
	private bool dieFlag = false;	//flag to destroy this object


	void Start() {
		GetComponent<TextMesh>().text = myText;	//set the text

		//limiters for when the ball is being destroyed outside the scene views
		if(transform.position.z > 9)
			transform.position = new Vector3(transform.position.x, transform.position.y, 9);
		if(transform.position.z < -9)
			transform.position = new Vector3(transform.position.x, transform.position.y, -9);									
	}

	/// <summary>
	/// FSM - Destroy the text object
	/// </summary>
	void Update (){
		if(!dieFlag)
			StartCoroutine(die());
	}

	/// <summary>
	/// Fade this object from white to hollow, then destroy it.
	/// </summary>
	IEnumerator die (){

		dieFlag = true;
		float t = 0.0f;
		
		while(t < 1) {
			t += Time.deltaTime;
			
			if(GetComponent<TextMesh>().characterSize <= 0.85f)
				GetComponent<TextMesh>().characterSize += t / 45;	//animate the text size
		
			if(t >= 1)
				Destroy(gameObject);
				
			yield return 0;
		}
	}
}
}