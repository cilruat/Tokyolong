using UnityEngine;
using System.Collections;
namespace Stealth
{
public class MovingLaser : MonoBehaviour {
		
	///*************************************************************************///
	/// Main moving laser class.
	/// This class moves a laser between two points and at the same time,
	/// casts a ray to detect any intrusion.
	///*************************************************************************///

	public GameObject laserShootPoint;				//Starting point for ray casting
	public Transform dummyStartPos;					//dummy position to indicate the starting point of movement
	public Transform dummyEndPos;					//dummy position to indicate the ending point of movement
	public float laserEffectiveDistance = 20.0f; 	//Ray's effective distance

	private GameObject player;	//reference to player game object

	void Awake (){
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Start (){
		transform.position = dummyStartPos.position;
	}
		
	void Update (){
		moveAndCastLaser();
	}
	///*************************************************************************///
	/// Move the laser and cast a ray everyframe to see if it hits player
	///*************************************************************************///
	private RaycastHit hitInfo;
	private Vector3 forward;
	private Vector3 accurateHitPointDir;
	private GameObject objectHit;
	private LineRenderer lineRenderer;
	void moveAndCastLaser (){
		//move from side to side
		moveSides();
		//cast ray
		forward = laserShootPoint.transform.TransformDirection(new Vector3(0,0,-1)) * laserEffectiveDistance;
		if(Physics.Raycast(laserShootPoint.transform.position, forward, out hitInfo, laserEffectiveDistance)) {
		
			objectHit = hitInfo.transform.gameObject;
			accurateHitPointDir = laserShootPoint.transform.TransformDirection(new Vector3(0,0,-1)) * hitInfo.distance;
			
	        Debug.DrawRay(laserShootPoint.transform.position, forward, Color.green);
	        Debug.DrawRay(laserShootPoint.transform.position, accurateHitPointDir, Color.red);
	        
	        lineRenderer = GetComponent<LineRenderer>();
	        lineRenderer.SetPosition(0, laserShootPoint.transform.position);
	        lineRenderer.SetPosition(1, hitInfo.point);
	        
	        if(objectHit.tag == "Player") {
				print("Game Over. catch by Moving Laser ;)");
				StartCoroutine(player.GetComponent<PlayerManager>().reload(0));
			}
		}
	}


	private Vector3 moveDirection;
	public float speed = 1.0f;			//Movement speed
	private float dir = 1.0f;
	void moveSides (){
		if(Vector3.Distance(transform.position, dummyEndPos.position) < 0.2f) 
			dir = -1.0f;
		else if(Vector3.Distance(transform.position, dummyStartPos.position) < 0.2f)
			dir = 1.0f;
		
		moveDirection =  dummyEndPos.position - dummyStartPos.position;
	    moveDirection = moveDirection.normalized * speed * dir;
	    transform.position += moveDirection * Time.deltaTime;
	}
	}
}