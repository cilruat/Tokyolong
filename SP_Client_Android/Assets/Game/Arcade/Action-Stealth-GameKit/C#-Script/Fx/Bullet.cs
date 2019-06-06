using UnityEngine;
using System.Collections;
namespace Stealth
{
public class Bullet : MonoBehaviour {
		
	///*************************************************************************///
	/// Main Machine Gun Bullet class.
	///*************************************************************************///

	public GameObject hitFx;				//Hit Particle
	private float speed = 10.0f;			//Movement speed
	private GameObject player;				//Reference to player game object

	void Awake (){
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Start (){
		transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
	}

	void Update (){
		transform.Translate(Vector3.forward * Time.deltaTime * speed);
	}

	///*************************************************************************///
	/// If this bullet hit something...
	///*************************************************************************///
	IEnumerator OnCollisionEnter ( Collision collision  ){
		if(collision.gameObject.tag == "Player" && !MachineGun.isHit) {
			print("GameOver. shot by MACHINE GUN");
			MachineGun.isHit = true;
			StartCoroutine(player.GetComponent<PlayerManager>().reload(2));
		}
		
		GetComponent<Renderer>().enabled = false;
		GetComponent<BoxCollider>().enabled = false;
		yield return new WaitForSeconds(3);
		Destroy(gameObject);
	}
	}
}