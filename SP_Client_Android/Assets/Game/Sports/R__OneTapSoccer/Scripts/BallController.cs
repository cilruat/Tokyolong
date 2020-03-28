using UnityEngine;
using System.Collections;

namespace OnetapSoccer
{
public class BallController : MonoBehaviour {

	/// <summary>
	/// This is the main ball controller. It handles all ball status including force management,
	/// movements, collisions, sounds, visual effects, etc.
	/// </summary>

	private GameObject gc;						//Reference to GameController Object
	private float ballMaxSpeed = 25.0f;			//Ball should not move faster than this value
	private float shootDirection;				//will be in 0 ~ 90 degree range
	public static bool hasBeenShot;				//will be true if this ball has been shot
	private bool goalHappened;					//will be true if the shood led to a goal

	[Range(1.0f, 5.0f)]
	public float shootDirectionTurnSpeed = 1;	//how fast the shoot direction arrow must turn
	[Range(1.5f, 2.5f)]
	public float ballLifetime = 2.0f;			//how many seconds we let the ball live after shot (then it gets resetted)


	public AudioClip goal;						//Audio when player scores a goal
	public AudioClip ballHitGround;				//Audio when ball hits floor
	public AudioClip ballHitMiddlePole;			//Audio when ball hits the pole
	public AudioClip ballHitPlayer;				//Audio when ball hits the player

	private Vector3 ballStartingPosition;		//initial position of the ball
	private GameObject player;					//Reference to Player Object
	private float shootTime;					//save the time of shooting
	private float shootDamper = 10;				//we need to adjust shoot power (and make it controllable/viewable)
	private float remainingLife;				//remaining ball's lifetime after each shoot

	public GameObject ballSpeedDebug;			//debug object to show ball's speed at all times
	public GameObject hitEffect;				//visual effect for contact points
	public GameObject ballShadow;				//shadow object that follows the ball
	public GameObject ballArrowPivot;			//arrow object that shows ball shoot direction
	public GameObject goalGfx;					//Goal gfx object


	void Awake () {
		//find and cache references to important gameobjects
		player			 = GameObject.FindGameObjectWithTag("PlayerHead");
		gc				 = GameObject.FindGameObjectWithTag("GameController");
	}


	void Start () {
		//set ball starting position
		ballStartingPosition = new Vector3(-5.2f, -2.5f, 0);
		transform.position = ballStartingPosition;
		shootDirection = 0;
		hasBeenShot = false;
		goalHappened = false;
		shootTime = 0;
		remainingLife = 0;
		ballArrowPivot.SetActive(true);

		resetRound ();
	}
	

	/// <summary>
	/// FSM
	/// </summary>
	void FixedUpdate () {

		//Rotate the shoot arrow up and down
		manageBallDirection ();
		
		//move ball's shadow object
		manageBallShadow();

		//debug - show ball's speed
		if(ballSpeedDebug)
			ballSpeedDebug.GetComponent<TextMesh>().text = "Speed: " + GetComponent<Rigidbody>().velocity.magnitude.ToString();

		//limit ball's maximum speed
		if(GetComponent<Rigidbody>().velocity.magnitude > ballMaxSpeed)
			GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * ballMaxSpeed;
	}


	/// <summary>
	/// Main shoot function. It disables the pre-shoot calculations and starts new coroutines to check if the 
	/// shoot leds to goal or is lost.
	/// </summary>
	public void shootBall() {

		if (hasBeenShot)
			return;
		hasBeenShot = true;

		//sfx
		playSfx(ballHitPlayer);

		//Hide the direction arrow
		ballArrowPivot.SetActive(false);

		//deduct 1 from player available balls
		GameController.playerCurrentBalls--;

		//save the exact shoot time, so we can track the ball's lifetime
		shootTime = Time.timeSinceLevelLoad;
		StartCoroutine (CheckBallLifetime (shootTime));

		//add force to the ball to make it act like being shot
		GetComponent<Rigidbody>().AddForce(new Vector3( Mathf.Abs(shootDirection - 90) / shootDamper, shootDirection / shootDamper, 0), ForceMode.Impulse);
	}


	/// <summary>
	/// Monitor the balls remaining life to be able to tell if the shoot has led to a goal or was lost
	/// </summary>
	IEnumerator CheckBallLifetime(float t) {
		
		while (Time.timeSinceLevelLoad < t + ballLifetime && !goalHappened) {
			remainingLife = (t + ballLifetime) - Time.timeSinceLevelLoad;
			//print ("Ball's remaining life: " + remainingLife);

			//if goal happened, we need to reset the timer
			if (goalHappened) {
				remainingLife = 0;
				yield break;
			}

			//if no goal happened, the shoot is lost
			if (remainingLife <= 0.25f) {
				resetRound ();
				remainingLife = 0;
				GameController.playerGoalStreak = 0;
				yield break;
			}

			yield return 0;
		}
	}


	/// <summary>
	/// Change ball's shoot direction with time, to make the player choose an angle for the shoot.
	/// </summary>
	void manageBallDirection() {

		//no rotation when the ball is already been shot
		if (hasBeenShot)
			return;

		shootDirection = Mathf.PingPong (Time.time * 90 * shootDirectionTurnSpeed * GameController.rotationSpeedBooster, 90);
		//print ("shootDirection: " + shootDirection);
		ballArrowPivot.transform.rotation = Quaternion.Euler (0, 0, shootDirection);
	}


	/// <summary>
	/// Resets the round and make the game ready for a new shoot
	/// </summary>
	void resetRound() {

		//destroy available pole in scene
		GameObject pole = GameObject.FindGameObjectWithTag("Pole");
		if(pole)
			Destroy (pole);

		//reset round parameters
		StartCoroutine(resetBallPosition());
		player.GetComponent<PlayerController>().resetPosition();
		hasBeenShot = false;
		ballArrowPivot.SetActive(true);

		//create a new pole
		gc.GetComponent<GameController> ().createPole ();

	}


	/// <summary>
	/// Make shadow object follow ball's movements
	/// </summary>
	void manageBallShadow() {
		if(!ballShadow)
			return;

		ballShadow.transform.position = new Vector3(transform.position.x, -2.9f, 0);
		ballShadow.transform.localScale = new Vector3(1.5f, 0.75f, 0.001f);
	}


	/// <summary>
	/// Manages collision events
	/// </summary>
	void OnCollisionEnter (Collision other) {

		if(goalHappened)
			return;
		
		if(other.gameObject.tag == "Field") {
			playSfx(ballHitGround);
			createHitGfx();
		}

		if(other.gameObject.tag == "PlayerHead") {
			playSfx(ballHitPlayer);
			createHitGfx();
			StartCoroutine(player.GetComponent<PlayerController> ().changeFaceStatus ());
		}

		if(other.gameObject.tag == "MiddlePole") {
			playSfx(ballHitMiddlePole);
			createHitGfx();
		}
	}


	/// <summary>
	/// Manages Trigger events (Goal checking...)
	/// </summary>
	void OnTriggerEnter (Collider c) {

		if(goalHappened)
			return;

		if(c.gameObject.tag == "Gate") {
			goalHappened = true;
			StartCoroutine(manageGoalEvent ());
		}
	}


	/// <summary>
	/// Creates a small visual object to show the contact point between ball and other objects
	/// </summary>
	void createHitGfx() {
		GameObject hitGfx = Instantiate(hitEffect, 
					                    transform.position + new Vector3(0, -0.4f, -1), 
					                    Quaternion.Euler(0, 180, 0)) as GameObject;
		hitGfx.name = "hitGfx";
	}


	/// <summary>
	/// Handle post-goal events
	/// </summary>
	IEnumerator manageGoalEvent() {

		//increase player goals by 1
		GameController.playerGoals++;

		//increase player streak by 1
		//GameController.playerGoalStreak++;

		//create goal gfx
		GameObject g = Instantiate(goalGfx, transform.position + new Vector3(0, 0, -1.5f), Quaternion.Euler(0, 180, 0)) as GameObject;
		g.name = "GoalGfx";

		/*if (GameController.playerGoalStreak > 1) {
			//if player is consecutively scoring goals, we will grant him additional balls as prize!
			GameController.playerCurrentBalls += GameController.playerGoalStreak - 1;
			print ("Received " + (GameController.playerGoalStreak - 1).ToString() + " additional balls.");
		}*/

		playSfx(goal);
		createHitGfx();
		yield return new WaitForSeconds(1.0f);

		resetRound ();
	}


	/// <summary>
	/// Move the ball to the starting position after a goal happened
	/// </summary>
	public IEnumerator resetBallPosition() {

		//back to starting position
		transform.position = ballStartingPosition;

		//freeze the ball for a while
		GetComponent<Rigidbody>().Sleep();
		GetComponent<Rigidbody>().isKinematic = true;

		yield return new WaitForEndOfFrame();

		//unfreeze the ball
		goalHappened = false;
		GetComponent<Rigidbody>().isKinematic = false;
	}


	//*****************************************************************************
	// Play sound clips
	//*****************************************************************************
	void playSfx ( AudioClip _clip  ){
		GetComponent<AudioSource>().clip = _clip;
		if(!GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().Play();
		}
	}
}
}