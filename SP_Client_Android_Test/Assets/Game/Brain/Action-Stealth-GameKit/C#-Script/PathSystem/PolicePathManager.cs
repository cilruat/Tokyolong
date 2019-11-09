using UnityEngine;
using System.Collections;
namespace Stealth
{
public class PolicePathManager : MonoBehaviour {
		
	///*************************************************************************///
	/// Main Police Class.
	/// This class will enable polices to follow indicated paths.
	///*************************************************************************///

	public float startDelay = 0.0f;					//Starting delay at the beginning of the scene
	private bool activeToMove = false;				//will be true, after the above delay is passed.

	private float waypointRadius  = 0.1f;			//Distance to waypoints as a rach threshold.
	private bool loop = true;						//Can follow the paths infinite times?
	public float speed = 1.0f;						//Path following speed
	private bool  faceHeading = true;				//always look forward towards the next waypoint?

	private float damping = 4.0f;					//acceleration/deacceleration on movement
	private Vector3 targetHeading;			
	private Vector3 currentHeading;
	private Vector3 moveDirection;
	private int targetwaypoint;
	private Transform xform;
	public Transform motherHolder; 					//A parent gameObject which holds all the child waypoints inside (as position dummies)
	private Transform[] waypoints; 					//A new array which will be build based on motherHolder's childs
	private int index = 0;
	private Transform child;

	//*********************************************************************
	// Simple Init
	//*********************************************************************	
	void Awake (){

		//put all childs in motherHolder in waypoints array
		waypoints = new Transform[motherHolder.childCount];
		//waypoints = new Array(motherHolder.childCount);
	       
	    foreach(Transform child in motherHolder) 
	    	waypoints[index++] = child as Transform;
		
		//if this object should start with a delay, then apply it
		if(startDelay > 0)
			StartCoroutine(activate());
		else
			activeToMove = true;
	}

	IEnumerator activate (){
		yield return new WaitForSeconds(startDelay);
		activeToMove = true;
	}
	//*********************************************************************
	// Check if there is enough waypoints for this object to build a path to follow?
	//*********************************************************************	
	void Start (){
	    xform = transform;
	    currentHeading = xform.forward;
	    if(waypoints.Length <= 0) {
	        Debug.Log("No waypoints on " + name);
	        enabled = false;
	    }
	    targetwaypoint = 0;
	}
	//*********************************************************************
	// Calculate a new heading for destination
	//*********************************************************************	
	void Update (){
	    if(activeToMove) {
			   
		    moveDirection = waypoints[targetwaypoint].position - xform.position;						
		    moveDirection = moveDirection.normalized * Time.deltaTime * speed;
		    currentHeading = Vector3.Lerp(currentHeading, moveDirection, damping * Time.deltaTime);
		    xform.position += moveDirection;
		    
		    if(faceHeading)
				xform.LookAt(xform.position + currentHeading);
			
		    if(Vector3.Distance(xform.position, waypoints[targetwaypoint].position) <= waypointRadius) {
		        targetwaypoint++;
		        if(targetwaypoint >= waypoints.Length) {
		            targetwaypoint = 0;
		            if(!loop)
		                enabled = false;
		        }
		    }
		}
	}
	}
}