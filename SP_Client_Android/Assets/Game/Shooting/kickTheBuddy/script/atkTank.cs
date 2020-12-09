using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KickTheBuddy
{ 
public class atkTank : MonoBehaviour {
	public float speed;
	public float posXstop = -1;
	public float atkCD=3;
	public Transform firePoint;
	public float forceCount;
	public int scoreAtk=10;
	public GameObject fxFire;
	public GameObject bullet;
	public int atkNumMax=5;
	private int atkNum;
	private float timeTemp;
	private bool stopDO;
	private Transform playerPoint;
	private Transform tankHead;
	private float posXbegin;
	// Use this for initialization
	void Start () {
		GameObject[] playerPoints = GameObject.FindGameObjectsWithTag ("playerPoint");
		for (int i = 0; i < playerPoints.Length ; i++) {
			if (playerPoints [i].name == "playerPoint (1)") { //Tank attack target point
				playerPoint =playerPoints [i].transform ;
				break;
			}
		}

		tankHead = this.transform.Find ("head");
		posXbegin = transform.position.x;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!stopDO) {
			transform.Translate (Vector3.forward * speed * Time.deltaTime);
			if (transform.position.x > posXstop) {			//Move to target location stop
				stopDO = true;
				timeTemp = Time.time + atkCD;
			} else if (transform.position.x < posXbegin) {			
				Destroy (this.gameObject); //The end of the attack is moved to a certain location and destroyed.
			} 
		} else {
			tankHead.rotation = Quaternion.Slerp (tankHead.rotation, Quaternion.LookRotation (playerPoint.position - tankHead.position), 2 * Time.deltaTime);//Slow steering target
			if(Time.time>timeTemp){
				Instantiate (fxFire, firePoint.position, firePoint.rotation);
				GameObject bulletObj = Instantiate (bullet, firePoint.position, firePoint.rotation)as GameObject;//Create attack shells
				bullet b = bulletObj.GetComponent<bullet> ();
				if (b) {
					b.forceCount = forceCount;
					b.scoreAtk = scoreAtk;
				} 
				GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<cameraJump> ().camera_jump ();//Camera vibration
				Handheld.Vibrate();
				atkNum += 1;
				if (atkNum == atkNumMax) { //Number of attacks
					stopDO = false;
					speed = -speed;

				}
				timeTemp = Time.time + atkCD;
			    }
		    }
	    }
    }
}
