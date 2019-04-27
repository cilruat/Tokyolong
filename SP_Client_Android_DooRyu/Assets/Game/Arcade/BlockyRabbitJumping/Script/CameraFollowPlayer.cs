using UnityEngine;
using System.Collections;

namespace BlockJumpRabbit
{

public class CameraFollowPlayer : MonoBehaviour {

    public Transform player;
    public GameController gameController;
    public float speed;
	public BackgroundParallax backgroundParallax;

    private Vector3 vel;

	void Awake(){
		transform.position = new Vector3(-4 * GameController.DISTANCE_OBJ, transform.position.y, transform.position.z);
	}

    void LateUpdate()
    {
        if (player.transform.position.x > transform.position.x && gameController.stateGame == StateGame.GamePlay)
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(player.position.x, transform.position.y,transform.position.z), ref vel, Time.deltaTime*speed);
    }

	// Reset postion camera
    public void Reset()
    {
		// Set distance view camera when start gameplay
		transform.position = new Vector3(-4 * GameController.DISTANCE_OBJ, transform.position.y, transform.position.z);
		backgroundParallax.Reset ();
    }

//	IEnumerator ResetParallax(){
//		backgroundParallax.enabled = false;
//		yield return new WaitForSeconds (1.0f);
//		backgroundParallax.enabled = true;
//	}
	}
}
