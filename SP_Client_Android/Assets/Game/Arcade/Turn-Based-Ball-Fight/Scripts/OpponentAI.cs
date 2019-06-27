using UnityEngine;
using System.Collections;
namespace Turnbase
{
public class OpponentAI : MonoBehaviour {
		
	/// <summary>
	/// Basic understanding of Opponent AI:
	/// first AI have to choose one of it's available balls as the actor
	/// then it should put all available target into an array,
	/// then it should calculate which target is easier to hit,
	/// then it should calculate the power and the direction and perform the shoot!
	/// </summary>

	public GameObject[] availableBalls;			//Available balls (actors)
	public GameObject[] availableTargets;		//Available player balls
	private GameObject target;					//Selected player ball
	private float distanceToTarget;				//distance to selected ball
	private Vector3 directionToTarget;			//direction vector to selected ball

	[Range(5.0f, 9.0f)]
	public float shootPower = 7.0f;				//shoot power (Avoid high values)
	[Range(0.0f, 1.0f)]
	public float shootInaccuracy = 0.5f;		//shoot inaccuracy (the bigger, the less acurate shoot by opponent)
	public static bool canShoot;				//prevent double shot flag

	//AI
	private GameObject actor;					//selected own ball as the actor
	private GameObject gameManager;				//reference to main GameManager object
	public static int scoreQueue;				//incremental scoring system


	void Awake (){
		gameManager = GameObject.FindGameObjectWithTag("GameManager");
	}


	/// <summary>
	/// Init
	/// </summary>
	IEnumerator Start (){
		canShoot = true;
		scoreQueue = 0;
		yield return new WaitForSeconds(1.0f);
	}


	/// <summary>
	/// FSM
	/// </summary>
	void Update (){		
		if(GlobalGameManager.opponentsTurn && canShoot && !GlobalGameManager.gameIsFinished) {
			scoreQueue = 0;		//if its opponent's turn, set the score queue back to zero
			canShoot = false;
			StartCoroutine(shoot());
		}
	}


	/// <summary>
	/// Main shoot routine of the opponent AI
	/// </summary>
	private float distanceCoef;		//used to fine-tune shoot power based on the distance to target ball
	IEnumerator shoot() {

		//wait for a while to fake thinking process
		yield return new WaitForSeconds(1.0f);

		//check if there is enough ball for player and AI in the scene?
		if(fillArrays()) {
			
			target = getTarget();	//select a player ball as the victim
			if(target == null)		//prevent unwanted null reference errors
				yield break;
			
			//This is the selected opponent ball
			actor = getActor();	

			//Calculate direction, distance and power
			distanceToTarget = Vector3.Distance(actor.transform.position, target.transform.position);
			directionToTarget = target.transform.position - actor.transform.position;

			//if we want a little inaccuracy in AI shoots
			directionToTarget += new Vector3(	Random.Range(shootInaccuracy * -1, shootInaccuracy),
												directionToTarget.y,
												Random.Range(shootInaccuracy * -1, shootInaccuracy));
			
			//Basic AI
			//If opponent's actor is very close to our balls, the distance will be very small
			//and shoot power might be too low. 
			//So we should provide some additional power if actor and target are too close
			if(distanceToTarget < 8 && distanceToTarget > 0) 
				distanceCoef = Random.Range(7, 13);
			else
				distanceCoef = Random.Range(0, 3);;

			//calculate and apply the force
			Vector3 force = Vector3.Normalize(directionToTarget) * (distanceToTarget + distanceCoef) * shootPower;
			actor.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

			//let the main GameManager know that the shoot is done, and it changes the turn.
			StartCoroutine(gameManager.GetComponent<GlobalGameManager>().managePostShoot("Opponent"));
		}
	}


	/// <summary>
	/// Monitors available balls for player and AI
	/// </summary>
	bool fillArrays() {
		availableBalls = GameObject.FindGameObjectsWithTag("Opponent");
		availableTargets = GameObject.FindGameObjectsWithTag("Player");
		
		if(availableBalls.Length > 0 && availableTargets.Length > 0) 
			return true;
		else
			return false;
	}


	/// <summary>
	/// select a target from available player balls in the scene
	/// </summary>
	GameObject getTarget() {
		if(availableTargets.Length > 0)
			target = availableTargets[Random.Range(0, availableTargets.Length)];
		else	
			target = null;
			
		return target;
	}


	/// <summary>
	/// Select a ball as an actor
	/// </summary>
	GameObject getActor() {
		actor = availableBalls[Random.Range(0, availableBalls.Length)];
		return actor;
	}	
	}
}