using UnityEngine;
using System.Collections;
namespace Stealth
{
public class cameraMovement : MonoBehaviour {
		
	///*************************************************************************///
	/// Camera Controller Class
	/// You can control camera to follow the player (For example in a large scene)
	/// or you can set it to be fixed in postion.
	///*************************************************************************///

	//choose this based on the level design.
	public bool canFollowPlayer = false;
	public float followSpeed = 0.2f;

	private GameObject player;	//Reference to player (target to follow)
	private float smoothX;		//Smooth Movement variable
	private float smoothZ;		//Smooth Movement variable

	void Start (){
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update (){
		if(canFollowPlayer)
			followPlayer();
	}

	//*************************************************************************//
	// Follow player with smooth movement.
	//*************************************************************************//
	void followPlayer (){
		smoothX = Mathf.SmoothStep(transform.position.x, player.transform.position.x, followSpeed);
		smoothZ = Mathf.SmoothStep(transform.position.z, player.transform.position.z, followSpeed);
		transform.position = new Vector3(smoothX, transform.position.y, smoothZ);
	}
}
}