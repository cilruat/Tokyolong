using UnityEngine;
using System.Collections;
namespace Stealth
{
public class MachineGun : MonoBehaviour {

	///*************************************************************************///
	/// Main Machine Gun Controller class.
	/// This class will handle machine gun's rotation, ray casting and shooting behaviours.
	///*************************************************************************///

	public GameObject gunShootPoint;		//Dummy shoot point
	public GameObject bullet;				//Object to shoot
	public GameObject gunNozzleFire;		//Shoot particles
	public AudioClip fireSfx;				//Shoot sound
	public bool smoothFollow = true;		//Target follow type
	public float detectionRange = 5.0f;		//Minimum distance to target required in order to become active.

	private float distanceToPlayer;
	private GameObject victim;
	private int ammoPerRound = 3;
	private float lookDelay;
	//private float xVelocity = 0.0f;
	//private float zVelocity = 0.0f;

	public static bool isHit;				//Static variable to indicate if a bullet has hit the player or not.

	void Start (){
		isHit = false;
		gunNozzleFire.SetActive(false);
		victim = GameObject.FindGameObjectWithTag("Player");
		smoothFollow = true;
		lookDelay = 0.5f;
	}
		

	///*************************************************************************///
	/// find distance between machine gun and the player
	///*************************************************************************///
	void Update (){
		distanceToPlayer = Vector3.Distance(transform.position, victim.transform.position);
		if(distanceToPlayer <= detectionRange) {
			lookAtVictim();
			StartCoroutine(castLaser());
		}

		//debug
		if(canSee) {
			print ("I can see the player");
		} else {
			print ("I can not see the player");
		}
	}


	///*************************************************************************///
	/// Rotate towards target
	///*************************************************************************///
	private	Quaternion rotation;
	void lookAtVictim (){
		if(smoothFollow) {
			rotation = Quaternion.LookRotation(victim.transform.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * (1 / lookDelay));
			transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
		}
		else
			transform.LookAt(victim.transform);
	}


	///*************************************************************************///
	/// Cast laser to find target's exact location. 
	///*************************************************************************///
	private RaycastHit hitInfo;
	private Vector3 forward;
	private Vector3 accurateHitPointDir;
	private GameObject objectHit;
	private LineRenderer lineRenderer;
	private bool canSee = false;
	private float canSeeTimer;
	private float canSeeTimeLimit = 0.95f;		//If we are locked on player for this amount of time, we are allowed to shoot.
	IEnumerator castLaser (){
		forward = gunShootPoint.transform.TransformDirection(new Vector3(0,0,1)) * detectionRange;
		if(Physics.Raycast(gunShootPoint.transform.position, forward, out hitInfo, detectionRange)) {
			objectHit = hitInfo.transform.gameObject;
			accurateHitPointDir = gunShootPoint.transform.TransformDirection(new Vector3(0,0,1)) * hitInfo.distance;
	        Debug.DrawRay(gunShootPoint.transform.position, forward, Color.green);
	        Debug.DrawRay(gunShootPoint.transform.position, accurateHitPointDir, Color.red);
	      
	        if(objectHit.tag == "Player" && !isHit) {		
				print("Locked on Player ..");
				canSee = true;
				canSeeTimer += Time.deltaTime;
				if(canSeeTimer >= canSeeTimeLimit) {		//shoot at target if we have him locked.
					canSeeTimer = 0.0f;
					gunNozzleFire.SetActive(true);
					playSfx(fireSfx);
					for(float cnt = 0; cnt < ammoPerRound; cnt++) {
						Instantiate(bullet, gunShootPoint.transform.position, transform.rotation);
						yield return new WaitForSeconds(0.15f);
					}
					gunNozzleFire.SetActive(false);
				}
			} else {
				canSee = false;
			}
		}
	}

	///*************************************************************************///
	/// Play AudioClip
	///*************************************************************************///
	void playSfx ( AudioClip _sfx  ){
		GetComponent<AudioSource>().clip = _sfx;
		if (!GetComponent<AudioSource>().isPlaying)
			GetComponent<AudioSource>().Play();
	}
}
}