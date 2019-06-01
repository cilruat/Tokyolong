using UnityEngine;
using System.Collections;
namespace Stealth
{
public class PressMachine : MonoBehaviour {

	///*************************************************************************///
	/// Main Press Machine class.
	/// This class simulates a press machine with two moving wings and an invisible 
	/// body which can be used to trigger some events. It moves the wings to sides
	/// and then quickly closes them to trap the player inside. you can control the
	/// speed and delay of each movement via inspector.
	///*************************************************************************///

	public GameObject innerPressDetector;			//reference to invisible trigger (inside machine)
	public GameObject[] outerPressDetector;			//reference to invisible triggers (outside machine)

	public GameObject leftWing;						//Left wing game object
	public GameObject rightWing;					//Right wing game object
	public bool isAutomatic = true;					//cycle the press procedure infinite times
	private bool isPressingFlag = false;			//flag used to prevent calling the function twice

	//attributes
	public float startDelay = 0.0f;					//first run starting delay
	private bool startFlag = false;
	public float closeSpeed = 4.0f;					//speed of closing the wings
	public float openSpeed = 0.2f;					//speed of closopening the wings
	public float delayOfNextPress = 2.0f; 			//in second	. delay between each press.
	private float distanceOfWings;
	private float wingWidth = 0.5f; 				//meter
	private float leftPressPoint;
	private float rightPressPoint;
	private float leftWingOrigin;
	private float rightWingOrigin;
	private bool halt;
	private bool isClosing;

	//do not touch these. used to get informed by playerManager.
	public bool isPlayerBetweenMyWings;
	public bool isPlayerOutsideMyWings;

	private GameObject player;	//reference to player game object

	void Awake (){
		isPlayerBetweenMyWings = false;
		isPlayerOutsideMyWings = false;
		halt = false;
		isClosing = true;
		player = GameObject.FindGameObjectWithTag("Player");
	}	

	IEnumerator Start (){
		distanceOfWings = Vector3.Distance(leftWing.transform.localPosition, rightWing.transform.localPosition);
		leftPressPoint = leftWing.transform.localPosition.z - (distanceOfWings / 2) + (wingWidth / 2);
		rightPressPoint = rightWing.transform.localPosition.z + (distanceOfWings / 2) - (wingWidth / 2);
		leftWingOrigin = leftWing.transform.localPosition.z;
		rightWingOrigin = rightWing.transform.localPosition.z;
		//if there is some startDelay set, then apply it
		yield return new WaitForSeconds(startDelay);
		startFlag = true;
	}

	void Update (){
		if(isAutomatic) {
			if(!isPressingFlag && startFlag) {
				isPressingFlag = true;
				StartCoroutine(press());
			}
		}
		
		calculateWingsDitance();
		
		updateTriggersStatus();
	}

	///*************************************************************************///
	/// Make the detectors (triggers) enabled/disabled when needed.
	///*************************************************************************///
	void updateTriggersStatus (){
		if(isClosing) { //if we are pressing
			innerPressDetector.SetActive(true);	//ser the inner trigger as active
			foreach(GameObject item in outerPressDetector) { //disable outer triggers
				item.SetActive(false);
			}
		} else { //if we are closing (returning to initial state)
			innerPressDetector.SetActive(false); //ser the inner trigger as inactive
			foreach(GameObject item in outerPressDetector) { //enable outer triggers
				item.SetActive(true);
			}
		}
	}


	///*************************************************************************///
	/// Calculate the distance between two pressing wings, and if player is
	/// inside these wings, and has no way out, ring the alarm.
	///*************************************************************************///
	void calculateWingsDitance (){
		distanceOfWings = Vector3.Distance(leftWing.transform.localPosition, rightWing.transform.localPosition);
		print(distanceOfWings);
		
		if(distanceOfWings <= 1.2f && isPlayerBetweenMyWings && isClosing) {
			print("Game Over. Player is Pressed to death.");
			halt = true;
			StartCoroutine(player.GetComponent<PlayerManager>().reload(0));
		}
		
		if(distanceOfWings >= 1.7f && isPlayerOutsideMyWings && !isClosing) {
			print("Game Over. Player is Pressed to death.");
			halt = true;
			StartCoroutine(player.GetComponent<PlayerManager>().reload(0));
		}
	}


	///*************************************************************************///
	/// Close the wings and then open them again, and again.
	///*************************************************************************///
	IEnumerator press (){
		float t = 0.0f;
		while(t < 1.0f && !halt) {
			t += Time.deltaTime * closeSpeed;
			leftWing.transform.localPosition = new Vector3(leftWing.transform.localPosition.x,
			                                               leftWing.transform.localPosition.y,
			                                               Mathf.SmoothStep(leftWingOrigin, leftPressPoint, t));
			rightWing.transform.localPosition = new Vector3(rightWing.transform.localPosition.x,
			                                                rightWing.transform.localPosition.y,
			                                                Mathf.SmoothStep(rightWingOrigin, rightPressPoint, t));
			isClosing = true;
			yield return 0;
		}
		if(t >= 1.0f) {
			yield return new WaitForSeconds(0.75f);
			float t2 = 0.0f;
			while(t2 < 1.0f && !halt) {
				t2 += Time.deltaTime * openSpeed;
				leftWing.transform.localPosition = new Vector3(leftWing.transform.localPosition.x,
				                                               leftWing.transform.localPosition.y,
				                                               Mathf.SmoothStep(leftPressPoint, leftWingOrigin, t2));
				rightWing.transform.localPosition = new Vector3(rightWing.transform.localPosition.x,
				                                                rightWing.transform.localPosition.y,
				                                                Mathf.SmoothStep(rightPressPoint, rightWingOrigin, t2));
				isClosing = false;
				yield return 0;
			}
			if(t2 >= 1.0f) {
				yield return new WaitForSeconds(delayOfNextPress);
				isPressingFlag = false;
			}
		}
	}
	}
}