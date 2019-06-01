using UnityEngine;
using System.Collections;


namespace Bowling
{
public class collisioncheck : MonoBehaviour {
	public Transform player;
    bool active = false;
	public float sounddistance;
    void Update(){
		if (player == null) {
			active = true;
		}
			if (active == false) {
				if (Vector3.Distance (transform.position, player.transform.position) < sounddistance) {
					GetComponent<AudioSource> ().Play ();
					active = true;
				}
			}
}
}
	
}