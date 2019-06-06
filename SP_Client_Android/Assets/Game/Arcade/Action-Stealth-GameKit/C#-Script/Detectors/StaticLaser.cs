using UnityEngine;
using System.Collections;
namespace Stealth
{
public class StaticLaser : MonoBehaviour {
		
	///*************************************************************************///
	/// Main Static Laser class.
	/// This class will control both static and locked laser. 
	/// Static laser has an on/off state, while locked lasers are always on, 
	/// unless you find enough keys to bypass their security.
	///*************************************************************************///

	public bool requireKeyToShutdown = false;		//if this is a static laser or a locked laser which requires key to shutdown
	public int howManyKeyRequired = 0;				//if this is a locked laser, how many key does it reuqire to shutdown?
	public GameObject laserRay;						//Main laser object
	public float laserOnTime = 1.0f;				//seconds which laser is active
	public float laserOffTimeMin = 3.0f;			//minimum seconds which laser is inactive
	public float laserOffTimeMax = 5.0f;			//maximum seconds which laser is inactive
	public float startDelay = 0.5f;					//first start after scene loads.

	private float laserMainOfftime;
	private bool canLaserStart;
	private bool isOff;

	//Audioclip
	public AudioClip openSfx;

	//Particle system for disabled lasers.
	public GameObject[] openFx;

	void Awake (){
		foreach(GameObject fx in openFx) {
			fx.SetActive(false); //disable all particles by default.
		}
	}

	///*************************************************************************///
	/// Simple Init
	///*************************************************************************///
	IEnumerator Start (){
		canLaserStart = false;
		isOff = false;
		laserRay.GetComponent<Renderer>().material.color = new Color(laserRay.GetComponent<Renderer>().material.color.r,
		                                             laserRay.GetComponent<Renderer>().material.color.g,
		                                             laserRay.GetComponent<Renderer>().material.color.b,
		                                             0);
		laserRay.GetComponent<Collider>().enabled = false;
		yield return new WaitForSeconds(startDelay);
		canLaserStart = true;
		
		laserMainOfftime = Random.Range(laserOffTimeMin, laserOffTimeMax);
	}

	///*************************************************************************///
	/// If this is a locked laser, then check it player has enough keys to open it.
	/// otherwise, enable laser's default on/off behaviour.
	///*************************************************************************///
	void Update (){
		if(requireKeyToShutdown) {
			if(PlayerManager.totalKeyFound >= howManyKeyRequired && !isOff) {
				laserRay.GetComponent<Collider>().enabled = false;
				laserRay.GetComponent<Renderer>().material.color = new Color(laserRay.GetComponent<Renderer>().material.color.r,
				                                             laserRay.GetComponent<Renderer>().material.color.g,
				                                             laserRay.GetComponent<Renderer>().material.color.b,
				                                             0);
				isOff = true;
				playSfx(openSfx);
				foreach(GameObject fx in openFx) {
					fx.SetActive(true);
					StartCoroutine(deactiveFX());
				}
			} else {
				if(canLaserStart) {
					StartCoroutine(laserManager());
					canLaserStart = false;
				}
			}
		} else {
			if(canLaserStart) {
				StartCoroutine(laserManager());
				canLaserStart = false;
			}
		}
	}

	///*************************************************************************///
	/// Disable particles after X seconds.
	///*************************************************************************///
	IEnumerator deactiveFX (){
		yield return new WaitForSeconds(10);
		foreach(GameObject fx in openFx) {
			fx.SetActive(false);
		}
	}

	///*************************************************************************///
	/// Turn laser on/off.
	///*************************************************************************///
	IEnumerator laserManager (){
		//On
		StartCoroutine(fade("in"));
		laserRay.GetComponent<Collider>().enabled = true;
		yield return new WaitForSeconds(laserOnTime);
		//Off
		StartCoroutine(fade("out"));
		laserRay.GetComponent<Collider>().enabled = false;
		yield return new WaitForSeconds(laserMainOfftime);
		canLaserStart = true;    
	}
		
	///*************************************************************************///
	/// Fade different laser's state.
	///*************************************************************************///
	IEnumerator fade ( string _state  ){
		float a;
		float b;
		//float dest;
		if(_state == "in") {
			a = 0.0f;
			b = 1.0f;
			//dest = 1;
		} else {
			a = 1.0f;
			b = 0.0f;
			//dest = -1;
		}
		
		float t = 0.0f;
		while(t <= 1.0f) {
			t += Time.deltaTime * 4.0f;
			laserRay.GetComponent<Renderer>().material.color = new Color(laserRay.GetComponent<Renderer>().material.color.r,
			                                               laserRay.GetComponent<Renderer>().material.color.g,
			                                               laserRay.GetComponent<Renderer>().material.color.b,
			                                               Mathf.Lerp(a, b, t));
			yield return 0;
		}
	}
	///*************************************************************************///
	/// Play audio clips
	///*************************************************************************///
	void playSfx ( AudioClip _sfx  ){
		GetComponent<AudioSource>().clip = _sfx;
		if (!GetComponent<AudioSource>().isPlaying)
			GetComponent<AudioSource>().Play();
	}
	}
}