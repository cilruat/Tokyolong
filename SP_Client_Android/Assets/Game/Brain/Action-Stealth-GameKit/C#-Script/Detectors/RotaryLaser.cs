using UnityEngine;
using System.Collections;
namespace Stealth
{
public class RotaryLaser : MonoBehaviour {
		
	///*************************************************************************///
	/// Main class for Rotary Laser.
	/// This class rotates the laser, and cast a ray everyframe to check if 
	/// laser detect anything.
	///*************************************************************************///

	public GameObject laserShootPoint;			//Starting poit for raycasting.
	public float direction = 1.0f;				//Raycasting direction (forward or backward)
	public float startingRotationY = 0.0f;		
	public float rotationSpeed = 15.0f;			//Degree per second
	public float laserReach = 15.0f;			//Laser effective distance

	private GameObject player;					//Reference to player game object

	void Awake (){
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Start (){
		transform.eulerAngles = new Vector3(transform.eulerAngles.x,
		                                    startingRotationY,
		                                    transform.eulerAngles.z);
	}
		
	void Update (){
		turnAndCastLaser();
	}


	///*************************************************************************///
	/// Turn the laser body and cast a ray everyframe.
	///*************************************************************************///
	private RaycastHit hitInfo;
	private Vector3 forward;
	private Vector3 accurateHitPointDir;
	private GameObject objectHit;
	private LineRenderer lineRenderer;
	void turnAndCastLaser (){

		transform.eulerAngles += new Vector3(0, rotationSpeed * Time.deltaTime * direction, 0);
		forward = laserShootPoint.transform.TransformDirection(new Vector3(0,0,-1)) * laserReach;
		
		if(Physics.Raycast(laserShootPoint.transform.position, forward, out hitInfo, laserReach)) {
			objectHit = hitInfo.transform.gameObject;
			accurateHitPointDir = laserShootPoint.transform.TransformDirection(new Vector3(0,0,-1)) * hitInfo.distance;

			//print ("objectHit: " + objectHit.name);

	        Debug.DrawRay(laserShootPoint.transform.position, forward, Color.green);
	        Debug.DrawRay(laserShootPoint.transform.position, accurateHitPointDir, Color.red);
	        
	        lineRenderer = GetComponent<LineRenderer>();
	        lineRenderer.SetPosition(0, laserShootPoint.transform.position);
	        lineRenderer.SetPosition(1, hitInfo.point);
	        
	        if(objectHit.tag == "Player") {
				print("Game Over. catch by Rotary Laser ;)");
				StartCoroutine(player.GetComponent<PlayerManager>().reload(0));
			}
		}
	}
	}
}