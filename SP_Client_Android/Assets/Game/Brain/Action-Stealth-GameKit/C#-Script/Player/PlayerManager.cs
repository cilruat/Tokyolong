using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
namespace Stealth
{
public class PlayerManager : MonoBehaviour {
		
	///*************************************************************************///
	/// Main Player controller class
	/// This class controlls all player related events and routines.
	///*************************************************************************///

	//available control types
	public enum ControlType {
		mouseCtrl,
		touchCtrl,
		keyboardCtrl,
		virtualJoystickCtrl	
	}
	public ControlType controlMode = ControlType.mouseCtrl;	//set the desired control mode by default

	public bool smoothFollow = true;			//movement type
	public float speed = 3.0f;					//movement speed

	public static int totalKeyFound;			//static variable for keeping track of keys found.
												//useful for levels with locked doors
												
	public static bool  playerIsSilent;			//static variable to let other objects know if
												//player is making noise by walking.
			
	private Vector3 inputPos;				//position of player touch/click on screen
	private Vector3 lastPos;				//Last rouched position
	private float speedModifier; 			//movement smoothness modifier (0~1)
	private float minDistanceToTarget;		//minimum distance to target required to stop movement
	private float distanceToTarget;			//current ditance to target
	private float lookDelay;				//rotation smoothness
	private bool isReloading;				//level reload flag (prevents double reload)	
	private bool levelIsFinished;			//has the player beat the level?
	private GameObject fadePlane;			//reference to fadePlane game object.
	private bool halt;						//flag to disable player movement after getting cought.
	private GameObject virtualJoystick;		//reference to virtualJoystick game object.
	private GameObject joystickTarget;		//reference to virtualJoystick target object.

	//AudioClips
	public AudioClip sirenSfx;
	public AudioClip alertSfx;
	public AudioClip hitSfx;
	public AudioClip[] eatKeySfx;

	void Start (){
		speedModifier = 0.0f;
		smoothFollow = true;
		lookDelay = 10.0f;
		minDistanceToTarget = 0.1f;
		lastPos = transform.position;
		totalKeyFound = 0;
		levelIsFinished = false;
		isReloading = false;
		playerIsSilent = true;
		halt = false;
		
		//find the fadePlane
		fadePlane = GameObject.FindGameObjectWithTag("fadePlane");
		
		//find the virtual joystick and it's target
		virtualJoystick = GameObject.FindGameObjectWithTag("vJoystick");
		joystickTarget = GameObject.FindGameObjectWithTag("joystickTarget");
			
	}

	void Update (){
		if(!halt) {
	 		if(controlMode == ControlType.mouseCtrl || controlMode == ControlType.touchCtrl) {
	 		
	 			touchInputManager();
	 			if(virtualJoystick)
	 				virtualJoystick.SetActive(false);
	 				
	 		} else if(controlMode == ControlType.keyboardCtrl) {
	 		
	 			keyboardInputManager();
	 			if(virtualJoystick)
	 				virtualJoystick.SetActive(false);
	 				
	 		} else if(controlMode == ControlType.virtualJoystickCtrl) {
	 			//only if virtual joystich prefab exists in the scene
	 			if(virtualJoystick) {
	 				virtualJoystick.SetActive(true);
	 				followJoystickTarget();
	 			}
	 			else
	 				print("You should place an instance of virtualJoystick in the scene.");
	 		}
	 	}
	}

	//*********************************************************************
	// This function moves the player with keyboard keys.
	//*********************************************************************
	private float turnSpeed = 180;
	private float moveSpeed = 3;
	void keyboardInputManager (){
		//Rotation
		if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
			GetComponent<AnimatedAtlas>().enabled = true;
			playerIsSilent = false;
		} else if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			transform.Rotate(0, turnSpeed * Time.deltaTime * -1, 0);
			GetComponent<AnimatedAtlas>().enabled = true;
			playerIsSilent = false;
		}
		
		//Movement
		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
			GetComponent<AnimatedAtlas>().enabled = true;
			playerIsSilent = false;
		} else if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
			GetComponent<AnimatedAtlas>().enabled = true;
			playerIsSilent = false;
		}
		
		if(!Input.anyKey) {
			GetComponent<AnimatedAtlas>().enabled = false;
			playerIsSilent = true;
		}
	}
	//*********************************************************************
	// This function moves the player with the Virtual Joystick.
	//*********************************************************************
	void followJoystickTarget (){
		if(Input.touches.Length > 0 || Input.GetMouseButton(0)) {
			inputPos = joystickTarget.transform.position;
			inputPos = new Vector3(inputPos.x, 0.5f, inputPos.z);
			if((transform.position - inputPos).sqrMagnitude > 0.05f) {
				lookAtTarget();
				followTarget();
				GetComponent<AnimatedAtlas>().enabled = true;
				playerIsSilent = false;
			} else {
				//********* Smooth start/stop movement ************//
				speedModifier -= Time.deltaTime * 6.0f;
				if(speedModifier < 0.0f) speedModifier = 0.0f;
				transform.Translate(Vector3.forward * Time.deltaTime * speed * speedModifier);
				//********* Smooth start/stop movement ************//
				GetComponent<AnimatedAtlas>().enabled = false;
				playerIsSilent = true;
				GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, 0);
			}
		}else {
			//print("NO touch");
			//********* Smooth start/stop movement ************//
			speedModifier -= Time.deltaTime * 6.0f;
			if(speedModifier < 0.0f) speedModifier = 0.0f;
			transform.Translate(Vector3.forward * Time.deltaTime * speed * speedModifier);
			//********* Smooth start/stop movement ************//
			inputPos = lastPos;
			GetComponent<AnimatedAtlas>().enabled = false;
			playerIsSilent = true;
			GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, 0);
			
			//move target to player position, so the next movement starts as a normal 
			joystickTarget.transform.position = transform.position;
		}
		//always face direct to camera
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
		transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
	}
		
	//*********************************************************************
	// This function Manages all user inputs: Touch, click, drag, etc...
	//*********************************************************************
	private RaycastHit hitInfo;
	private Ray ray;
	void touchInputManager (){
		if(Input.touches.Length == 1 || Input.GetMouseButton(0)) {
			inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			inputPos.y = 0.5f; //required offset
			if((transform.position - inputPos).sqrMagnitude > minDistanceToTarget) {
				lookAtTarget();
				followTarget();
				GetComponent<AnimatedAtlas>().enabled = true;	//enable texture animation
				playerIsSilent = false;	//player is making noise by walking
			} else {
				//********* Smooth start/stop movement ************//
				speedModifier -= Time.deltaTime * 6.0f;
				if(speedModifier < 0) speedModifier = 0.0f;
				transform.Translate(Vector3.forward * Time.deltaTime * speed * speedModifier);
				//********* Smooth start/stop movement ************//
				GetComponent<AnimatedAtlas>().enabled = false;			//disable texture animation
				playerIsSilent = true; //player is not walking and is not making any noise
				GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, 0);	//set correct texture for stop stance
			}
		} else {
			//print("NO touch");
			
			//********* Smooth start/stop movement ************//
			speedModifier -= Time.deltaTime * 6.0f;
			if(speedModifier < 0.0f) speedModifier = 0.0f;
			transform.Translate(Vector3.forward * Time.deltaTime * speed * speedModifier);
			//********* Smooth start/stop movement ************//
			inputPos = lastPos;
			GetComponent<AnimatedAtlas>().enabled = false;
			playerIsSilent = true; //player is not walking and is not making any noise
			GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, 0);
		}

		//always look towards camera
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
		transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
	}

	//*********************************************************************
	// This function moves the player to touch/click position
	//*********************************************************************
	private Vector3 moveDirection;
	void followTarget (){
		speedModifier += Time.deltaTime * 3.0f; //accelerated movement
		if(speedModifier > 1.0f) 
			speedModifier = 1.0f;
		
		if(smoothFollow) {
			transform.Translate(Vector3.forward * Time.deltaTime * speed * speedModifier);
		} else {
			moveDirection = inputPos - transform.position;
		    moveDirection = moveDirection.normalized * Time.deltaTime * speed;
		    transform.position += moveDirection;
		}
	}

	//*********************************************************************
	// This function rotates the player towards touch/click position.
	//*********************************************************************
	private	Quaternion rotation;
	void lookAtTarget (){
		if(smoothFollow) {
			rotation = Quaternion.LookRotation(inputPos - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * lookDelay);
		} else
			transform.LookAt(inputPos);
	}
		
	//*********************************************************************
	// This function Manages all player collisions.
	//*********************************************************************
	public string nextLevelToLoad = "Menu";
	void OnTriggerStay ( Collider other  ){
		switch(other.gameObject.tag) { 
				
			case "exitdoor":
				print("Level is Finished. Next level ---->>>");
				if(!levelIsFinished) {
					//Application.LoadLevel(nextLevelToLoad);
					SceneManager.LoadScene ("EscapeSuccess");
				}
				break;
			
			case "police":
				print("game over. collided like a fool with a police");
				StartCoroutine(reload(0));
				break;	
			
			case "staticLaser":
				print("GameOver. detected by staticLaser.");
				StartCoroutine(reload(0));
				break;
				
			case "circleDetector":
				//only if we are moving (and making noise)
				if(!playerIsSilent) {
					print("GameOver. detected by Noise Detector.");
					StartCoroutine(reload(0));
				}
				break;
			
			case "pressWing":
				print("GameOver. pressed to death by pressMachine Wings");
				StartCoroutine(reload(0));
				break;
			
			case "innerPressWingDetector":
				//print("Collided with PressWingDetector.");
				other.gameObject.transform.parent.GetComponent<PressMachine>().isPlayerBetweenMyWings = true;
				break;
			
			case "outerPressWingDetector":
				//print("Collided with PressWingDetector.");
				other.gameObject.transform.parent.GetComponent<PressMachine>().isPlayerOutsideMyWings = true;
				break;
			
			//**** KEYS ***//
			case "key":
				print("Found a Key");
				Destroy(other.gameObject);
				totalKeyFound += 1;
				playSfx(eatKeySfx[Random.Range(0, 2)]);
				break;
		}
	}

	void OnTriggerExit ( Collider other  ){
		if(other.gameObject.tag == "innerPressWingDetector")
			other.gameObject.transform.parent.GetComponent<PressMachine>().isPlayerBetweenMyWings = false;
		
		if(other.gameObject.tag == "outerPressWingDetector")
			other.gameObject.transform.parent.GetComponent<PressMachine>().isPlayerOutsideMyWings = false;
	}

	//*********************************************************************
	// This function reloads current level
	//*********************************************************************
	public IEnumerator reload ( int _state  ){
		if(isReloading)
			yield break;
			
		isReloading = true;
		
		halt = true;
		GetComponent<AnimatedAtlas>().enabled = false;
		GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, 0);
		
		switch(_state) {
			case 0:
				playSfx(alertSfx);
				break;	
			case 1:
				playSfx(sirenSfx);
				break;
			case 2:
				playSfx(hitSfx);
				break;
		}
		
		//print("fading...");
		fadePlane.GetComponent<MeshCollider>().enabled = true;
		fadePlane.transform.position = new Vector3(fadePlane.transform.position.x,
		                                           3,
		                                           fadePlane.transform.position.z);
		fadePlane.GetComponent<Renderer>().material.color = new Color(0.6f,
		                                              fadePlane.GetComponent<Renderer>().material.color.g,
		                                              fadePlane.GetComponent<Renderer>().material.color.b,
		                                              0);
		
		//fade to white
		float t = 0.0f;
		while(t < 1.0f) {
			//t += Time.deltaTime * 0.65f;
			t += Time.deltaTime * 1.1f;
			fadePlane.GetComponent<Renderer>().material.color = new Color(fadePlane.GetComponent<Renderer>().material.color.r,
			                                              fadePlane.GetComponent<Renderer>().material.color.g,
			                                              fadePlane.GetComponent<Renderer>().material.color.b,
			                                              t);
			//print(t);
			yield return 0;
		}
		if(t >= 1.0f) {
			yield return new WaitForSeconds(0.5f);
			isReloading = false;
			//Application.LoadLevel(Application.loadedLevelName);
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		}
	}

	//*********************************************************************
	// This function plays AudioClips.
	//*********************************************************************
	void playSfx ( AudioClip _sfx  ){
		GetComponent<AudioSource>().clip = _sfx;
		if (!GetComponent<AudioSource>().isPlaying)
			GetComponent<AudioSource>().Play();
	}
	}
}