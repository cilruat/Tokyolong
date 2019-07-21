using UnityEngine;
using System.Collections;
namespace Stealth
{
public class CcCam : MonoBehaviour {
		
	///*************************************************************************///
	/// Main CcCamera script.
	/// This class is used for both CcCameras and Police flashlights.
	///*************************************************************************///

	public bool isStatic = false; 			//if ccCams can rotate of not?
	public float startingRotation = 0.0f;	//Starting rotation in degree. Must be greater than 0.
	public float rotationAngle = 60.0f; 	// # degrees to left, then # degrees to right , and so on...
	public float ccCamRotateSpeed = 15.0f; 	//rotation speed
	public float sideReachWait = 1.0f; 		//if ccCam should wait for a while when reaching an end

	//Private vars
	private bool  isRotatingFlag; 			//is rotating? (prevents double calls)
	private bool  canTalk;					//Used when two moving police are near, so they can talk with each other.

	//Audio
	public AudioClip sirenSfx;

	//talk
	public AudioClip[] talkSfx;				//used solely for moving police's flashlights.

	//player
	private GameObject player;				//reference to main player game object.

	void Awake (){
		player = GameObject.FindGameObjectWithTag("Player");
		canTalk = true;
	}

	void Start (){
		isRotatingFlag = false;
	}

	void Update (){

		//Cast some rays to detect any intrusion. If an object is blocking this camera's view, user can safely cover behind that object.
		castRay();
		
		if(isStatic)
			return;
		else {
			if(!isRotatingFlag)
				StartCoroutine(ccCamRotate());
		}
	}

	//*********************************************************************
	// This function casts some rays and then check if they intersect 
	// with anything.
	//*********************************************************************	
	private RaycastHit hitInfoF;
	private RaycastHit hitInfoL;
	private RaycastHit hitInfoR;
	private Vector3 origin;
	private Vector3 forward;
	private Vector3 left;
	private Vector3 right;
	private string[] objectHitTags;
	public float rayLength = 3.7f; 		//Important 3.7f for Cameras and 2.1f for polices
	public float yOffset = 0.5f; 		//for polices this should be zero
	void castRay (){
			
			objectHitTags = new string[3];
			
			origin = transform.position - new Vector3(0, yOffset, 0);						//starting point for raycasting
			forward = transform.TransformDirection(new Vector3(1,0,0)) * rayLength;			//ray in the middle
			left = transform.TransformDirection(new Vector3(1, 0, 0.35f)) * rayLength;		//left ray
			right = transform.TransformDirection(new Vector3(1, 0, -0.35f)) * rayLength;	//right ray
			
			Debug.DrawRay(origin, forward, Color.green);
	        Debug.DrawRay(origin, left, Color.blue);
	        Debug.DrawRay(origin, right, Color.white);
				
			//check if we are hitting something...
			if(Physics.Raycast(origin, forward, out hitInfoF, rayLength)) 
				objectHitTags[0] = hitInfoF.transform.gameObject.tag;
			if(Physics.Raycast(origin, left, out hitInfoL, rayLength)) 
				objectHitTags[1] = hitInfoL.transform.gameObject.tag;
	 		if(Physics.Raycast(origin, right, out hitInfoR, rayLength)) 
	 			objectHitTags[2] = hitInfoR.transform.gameObject.tag;

			
			foreach(string item in objectHitTags) {
				//print("#" + item);
				
				if(item == "Player") {
					if(gameObject.name == "ccCamMain") {
						print("gameOver. catch by ccCam");
						playSfx(sirenSfx);
						StartCoroutine(player.GetComponent<PlayerManager>().reload(1));
					} else if(gameObject.name == "PoliceFlashlightBody") {
						print("gameOver. catch by Moving Police");
						playSfx(sirenSfx);
						StartCoroutine(player.GetComponent<PlayerManager>().reload(0));
					}
				}
				
				if(item == "police") {										//if two moving police saw each other.
					if(gameObject.name == "PoliceFlashlightBody" && canTalk) {
						//print("Police greetings :) ");
						canTalk = false;
						StartCoroutine(activateTalk());
						playSfx(talkSfx[Random.Range(0, talkSfx.Length)]);
					}
				}
		}
	}

	//*********************************************************************
	// Let polices talk again.
	//*********************************************************************
	IEnumerator activateTalk (){
		yield return new WaitForSeconds(5);
		canTalk = true;
	}

	//*********************************************************************
	// Rotate CcCamera's rays.
	//*********************************************************************
	IEnumerator ccCamRotate (){
		isRotatingFlag = true;
		float t = 0.0f;
		while(t < 1.0f) {
			t += Time.deltaTime * (ccCamRotateSpeed / 90);
			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
			                                         Mathf.SmoothStep(startingRotation, startingRotation + rotationAngle, t),
			                                         transform.localEulerAngles.z);
			yield return 0;
		}
		
		if( Mathf.Abs(transform.localEulerAngles.y - (startingRotation + rotationAngle)) <= 0.10f || 
			Mathf.Abs(transform.localEulerAngles.y - (startingRotation + 360 + rotationAngle)) <= 0.10f) {
			
			yield return new WaitForSeconds(sideReachWait);
			float t2 = 0.0f;
			while(t2 < 1.0f) {
				t2 += Time.deltaTime * (ccCamRotateSpeed / 90);
				transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
				                                         Mathf.SmoothStep(startingRotation + rotationAngle, startingRotation, t2),
				                                         transform.localEulerAngles.z);
				yield return 0;
			}
			if(Mathf.Abs(transform.localEulerAngles.y - startingRotation) <= 0.10f) {
				yield return new WaitForSeconds(sideReachWait);
				isRotatingFlag = false;
				yield break;
			}
		}
	} 
		
		
	//*********************************************************************
	// Play AudioClips
	//*********************************************************************
	void playSfx ( AudioClip _sfx  ){
		GetComponent<AudioSource>().clip = _sfx;
		if (!GetComponent<AudioSource>().isPlaying)
			GetComponent<AudioSource>().Play();
	}
	}
}