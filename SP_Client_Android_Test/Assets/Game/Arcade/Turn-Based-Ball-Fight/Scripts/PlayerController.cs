using UnityEngine;
using System.Collections;
namespace Turnbase
{
public class PlayerController : MonoBehaviour {

	/// <summary>
	/// This the main controller for player balls. It handles selecting balls to shoot,
	/// dragging, shoot calculations, showing helper arrow object and Sfx playing.
	/// </summary>
		
	public GameObject selectionCircle;		//selectionCircle child object	
	private GameObject target;		 		//mouse/rouch target object which draws debug/physics lines
	private GameObject arrowPlane; 			//arrow plane which is used to show shootPower
	private GameObject gameManager;			//reference to main GameManager object
	private float currentDistance;			//current distance between ball and players input
	private float distMax = 3;				//maximum allowed distance to shoot
	private float safeDistance; 			//set the safe distance for final shoot formula
	private float pwr;						//final shoot power
	private Vector3 shootDirectionVector;	//direction of the shoot
	public static bool canShoot = true;		//shoot flag to prevent double shooting

	public AudioClip hitSfx;				//Audioclip for balls colliding with each other
	public AudioClip dieSfx;				//Audioclip for ball when dropping out of the board


	/// <summary>
	/// Init
	/// </summary>
	void Awake() {
		//find and cache neccesary GameObjects
		target = GameObject.FindGameObjectWithTag("InputFollower");
		arrowPlane = GameObject.FindGameObjectWithTag("ArrowPlane");		
		gameManager = GameObject.FindGameObjectWithTag("GameManager");
	}


	void Start() {
		pwr = 0.1f;
		currentDistance = 0;
		shootDirectionVector = new Vector3(0,0,0);
		canShoot = true;		
		arrowPlane.GetComponent<Renderer>().enabled = false;	//hide arrowPlane
	}


	/// <summary>
	/// FSM
	/// </summary>
	void Update() {		
		showSelectionCircle();
	}
		
		
	/// <summary>
	/// Shows the selection circle aroun dplayer balls, when this is player's turn.
	/// </summary>
	void showSelectionCircle() {
		if(GlobalGameManager.playersTurn)
			selectionCircle.GetComponent<Renderer>().enabled = true;
		else
			selectionCircle.GetComponent<Renderer>().enabled = false;
	}


	/// <summary>
	/// Drag and shoot routine.
	/// </summary>
	void OnMouseDrag() {

		//if we are allowed to shoot (haven't shoot before in this turn)
		//if this is our turn, and if game is not finished!
		if(canShoot && GlobalGameManager.playersTurn && !GlobalGameManager.gameIsFinished) {
			currentDistance = Vector3.Distance(target.transform.position, transform.position);
			//limit the distance between player's input and selected ball
			if(currentDistance <= distMax)
				safeDistance = currentDistance;
			else
				safeDistance = distMax;

			//calculate the power
			pwr = Mathf.Abs(safeDistance) * 40;

			//show and rotate the indicator power arrow
			manageArrowTransform();		
			
			//draw debug lines
			Debug.DrawLine(transform.position, target.transform.position, Color.red);
			Debug.DrawLine(transform.position, arrowPlane.transform.position, Color.blue);

			//calculate the shoot direction
			shootDirectionVector = Vector3.Normalize(target.transform.position - transform.position);
		}
	}


	/// <summary>
	/// show and rotate the shoot arrow.
	/// </summary>
	void manageArrowTransform() {

		arrowPlane.GetComponent<Renderer>().enabled = true;		//show arrowPlane
		
		//calculate correct position.
		if(currentDistance <= distMax) {
			arrowPlane.transform.position = new Vector3((2 * transform.position.x) - target.transform.position.x,
			                                            2.5f,
			                                            (2 * transform.position.z) - target.transform.position.z);
		} else {
			Vector3 dxy = target.transform.position - transform.position;
			float diff = dxy.magnitude;
			arrowPlane.transform.position = transform.position + (dxy / diff) * distMax * -1;
			arrowPlane.transform.position = new Vector3(arrowPlane.transform.position.x,
			                                            2.5f,
			                                            arrowPlane.transform.position.z);
		}

		//calculate correct rotation
		Vector3 dir = target.transform.position - transform.position;
		float outRotation; //between 0 ~ 360
		
		if(Vector3.Angle(dir, transform.forward) > 90) 
			outRotation = Vector3.Angle(dir, transform.right);
		else
			outRotation = Vector3.Angle(dir, transform.right) * -1;
			
		arrowPlane.transform.eulerAngles = new Vector3(0, outRotation, 0);
		
		//calculate correct scale based on the distance (shoot power)
		float scaleCoefX = Mathf.Log(1 + safeDistance/10, 2);
		float scaleCoefZ = Mathf.Log(1 + safeDistance/20, 2);
		arrowPlane.transform.localScale = new Vector3(0.2f + scaleCoefX, 2.5f, 0.1f + scaleCoefZ);
	}


	/// <summary>
	/// shoot the ball with the calculated parameters, when player releases the touch/mouse inout
	/// </summary>
	void OnMouseUp (){

		//give the player a second chance to choose another ball if drag on the unit is too low
		//print("currentDistance: " + currentDistance);
		if(currentDistance < 0.75f) {
			arrowPlane.GetComponent<Renderer>().enabled = false;
			return;
		}
		
		//But if player wants to shoot anyway:

		//prevent double shooting in a round
		if(!canShoot)
			return;
		
		//set the flag
		canShoot = false;
		
		//hide power Arrow
		arrowPlane.GetComponent<Renderer>().enabled = false;
		
		//physics calculations to shoot the ball 
		Vector3 outPower = shootDirectionVector * pwr * -1;
		//print(outPower);

		//make the player move only in x-z plane and not on the y direction	
		GetComponent<Rigidbody>().AddForce(outPower.x, 0, outPower.z, ForceMode.Impulse);

		//let the main GameManager know that the shoot is done, and it changes the turn.
		StartCoroutine(gameManager.GetComponent<GlobalGameManager>().managePostShoot("Player"));
		
		//reset shoot variables
		currentDistance = 0;
		pwr = 0.1f;	//setting it to 0 might cause physics problem.
		shootDirectionVector = new Vector3(0, 0, 0);
	}


	/// <summary>
	/// play the Sfx and destroy the ball upon colliding with the trigger (Borders)
	/// </summary>
	void OnTriggerEnter(Collider other) {
		//decide when to destroy the ball object
		if(other.tag == "Border") {					
			GetComponent<AudioSource>().clip = dieSfx;
			if(!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().Play();

			GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);	//Stop the ball
			GetComponent<Rigidbody>().drag = 1000;						//Helps to stop the ball
			gameObject.tag = "Untagged";								//Un-tag the ball before destroying it
			gameObject.GetComponent<SphereCollider>().enabled = false;	//Remove the collider
			selectionCircle.SetActive(false);							//Hide the selectionCircle child object
			StartCoroutine(fadeout());									//Hide and destroy the ball
		}
	}
		

	/// <summary>
	/// Hide and destroy the ball
	/// </summary>
	IEnumerator fadeout() {
		float t = 0.0f;
		while(t < 1) {
			t += Time.deltaTime * 3.0f;
			GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r,
												GetComponent<Renderer>().material.color.g,
												GetComponent<Renderer>().material.color.b,
												1 - t);
			yield return 0;
		}

		if(t >= 1) {
			yield return new WaitForSeconds(3.0f);	
			Destroy(gameObject);
		}
	}

	
	/// <summary>
	/// Play hit sfx when ball collides with other colliders (Opponent Balls)
	/// </summary>
	void OnCollisionEnter(Collision other) {
		if(other.gameObject.tag == "Opponent") {
			GetComponent<AudioSource>().clip = hitSfx;
			if(!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().Play();
		}
	}
	}
}