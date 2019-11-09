using UnityEngine;
using System.Collections;

namespace OnetapSoccer
{
public class PlayerController : MonoBehaviour {

	/// <summary>
	/// Main player controller. It lets the player shoot the ball whenever possible, manages player shoot animation 
	/// and change player's face (avatar) when hit by ball or other colliders.
	/// </summary>

	public StatusImage[] availableAvatars;			//different avatar images for player character
	private float startingX = -6.6f;				//normally you don't need to change this.
	private Vector3 startingPosition;

	//reference to child game objects
	public GameObject myShoe;
	public GameObject myHead;
	private GameObject ball;						//reference to the ball game object


	/// <summary>
	/// init
	/// </summary>
	void Awake () {

		//reset player's position
		startingPosition = new Vector3(startingX, -1.5f, 0);
		transform.position = startingPosition;

		ball = GameObject.FindGameObjectWithTag ("Ball");

		//change player avatar image based on the selection made from the Menu scene
		myHead.GetComponent<Renderer>().material.mainTexture = availableAvatars[PlayerPrefs.GetInt("selectedAvatar", 0)].normal;
	}
		

	/// <summary>
	/// FSM
	/// </summary>
	void LateUpdate () {
		if(!GameController.gameIsFinished && !BallController.hasBeenShot)
			manageShoots ();
	}
		

	/// <summary>
	/// Let the player shoot the ball whenever possible
	/// </summary>
	void manageShoots() {

		if(Input.GetKeyUp(KeyCode.Space) || (Input.GetMouseButtonUp (0) && UserInputManager.enableUserInput)) {
			ball.GetComponent<BallController> ().shootBall ();
			StartCoroutine(animFoot ());
		}

	}


	/// <summary>
	/// Simple animation for player foot
	/// You can replace this with better animations.
	/// </summary>
	private bool isAnimating = false;
	IEnumerator animFoot() {

		if (isAnimating)
			yield break;
		isAnimating = true;

		float t = 0;
		while (t < 1) {
			t += Time.deltaTime * 5.0f;

			//move forward
			myShoe.transform.localRotation = Quaternion.Euler (0, Mathf.SmoothStep(0, -35, t), 180);
			myShoe.transform.localScale = new Vector3(Mathf.SmoothStep(1, 2, t), 0.01f, Mathf.SmoothStep(0.6f, 1.2f, t));
			myShoe.transform.localPosition = new Vector3(Mathf.SmoothStep(0, 0.5f, t), 0.1f, Mathf.SmoothStep(-0.85f, -0.5f, t));

			if (t >= 1) {
				//reset all values back to initial - move back
				myShoe.transform.localRotation = Quaternion.Euler (0, 0, 180);
				myShoe.transform.localScale = new Vector3(1, 0.01f, 0.6f);
				myShoe.transform.localPosition = new Vector3(0, 0.1f, -0.85f);
				isAnimating = false;
			}

			yield return 0;
		}

	}


	/// <summary>
	/// Change avatar face when we are hit by ball or other colliders
	/// </summary>
	public IEnumerator changeFaceStatus() {
		myHead.GetComponent<Renderer>().material.mainTexture = availableAvatars[PlayerPrefs.GetInt("selectedAvatar", 0)].hit;
		yield return new WaitForSeconds(0.3f);
		myHead.GetComponent<Renderer>().material.mainTexture = availableAvatars[PlayerPrefs.GetInt("selectedAvatar", 0)].normal;
	}
		

	/// <summary>
	/// take the object to its initial position, while also resetting its physics properties.
	/// </summary>
	public void resetPosition() {
		transform.position = startingPosition;
		GetComponent<Rigidbody>().sleepThreshold = 0.005f;
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero; 
	}

}
}