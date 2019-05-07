using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockJumpRabbit
{

public class AIController:MonoBehaviour
{
    public float AISpeedChase = 1.0f;
    public float AISpeedRuning = 0.3f;
    public GameObject chatText;
    private float currentSpeed;

    private Transform player;
    private Queue<Platform> queuePlaformAIKnow;
    private PlayerController playerController;

    private Platform platformFront01;
    private Platform platformFront02;

    private float realWidthScreen;

    private bool stop;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        queuePlaformAIKnow = new Queue<Platform>();
        playerController = GetComponent<PlayerController>();

        currentSpeed = AISpeedRuning;

        realWidthScreen = Screen.width * Camera.main.orthographicSize * 2 / Screen.height;
    }

    public void EnQueueAIPath(Platform plaform)
    {
        queuePlaformAIKnow.Enqueue(plaform);
    }
		
//	void Update(){
//		print (player.name);
//	}

     /// <summary>
     /// Chase Player
     /// </summary>
    IEnumerator ChasePlayer()
    {

        yield return new WaitForSeconds(UnityEngine.Random.Range(0.0f, 3.0f));              // Wait for random time to chase player

        while (transform.position.x < -1)
        {
            if (stop)
                yield break;

            playerController.Jump(PlayerController.TypeJump.DoubleJump);

            currentSpeed = (transform.position.x + realWidthScreen / 2 + 3.0f > player.transform.position.x) ? AISpeedChase : AISpeedRuning;

            while (playerController.IsJump)
                yield return null;

            yield return new WaitForSeconds(currentSpeed);
        }

		yield return new WaitForSeconds(currentSpeed);

        while (queuePlaformAIKnow.Count > 1)
        {
            if (stop)
                yield break;

            platformFront01 = queuePlaformAIKnow.Dequeue();
            platformFront02 = queuePlaformAIKnow.Dequeue();

			// Process with platform snake02
			if(platformFront02.typePlatform == TypePlatform.Snake) {
				playerController.Jump(PlayerController.TypeJump.DoubleJump);

                while (playerController.IsJump)
                    yield return null;

                yield return new WaitForSeconds(currentSpeed);
				playerController.Jump(PlayerController.TypeJump.DoubleJump);
				queuePlaformAIKnow.Dequeue();											// when jump snake 02 , remove mid element snake


			}else if (platformFront02.typePlatform == TypePlatform.Empty || platformFront01.typePlatform == TypePlatform.Snake)
            {
                playerController.Jump(PlayerController.TypeJump.NormalJump);

                while (playerController.IsJump)
                    yield return null;

                yield return new WaitForSeconds(currentSpeed);

                playerController.Jump(PlayerController.TypeJump.DoubleJump);

				if (platformFront01.typePlatform != TypePlatform.Snake) {
					Platform platformFront03 = queuePlaformAIKnow.Dequeue ();
					if(platformFront03.typePlatform  == TypePlatform.Snake) {

                        while (playerController.IsJump)
                            yield return null;

                        yield return new WaitForSeconds(currentSpeed);
						playerController.Jump(PlayerController.TypeJump.DoubleJump);
						queuePlaformAIKnow.Dequeue ();
					}
				}
            }
            else
            {
				if(player.transform.position.x < transform.position.x + GameController.DISTANCE_OBJ*2) {

					// Guaranteed ai hit player
					playerController.Jump (PlayerController.TypeJump.NormalJump);

                    while (playerController.IsJump)
                        yield return null;

                    yield return new WaitForSeconds (currentSpeed);
					playerController.Jump (PlayerController.TypeJump.NormalJump);

				}else
                	playerController.Jump(PlayerController.TypeJump.DoubleJump);
            }

            currentSpeed = (transform.position.x + realWidthScreen / 2 + 3.0f > player.transform.position.x) ? AISpeedChase : AISpeedRuning;


            while (playerController.IsJump)
                yield return null;

            yield return new WaitForSeconds(currentSpeed);
        }
    }

    public void IntialAIChasePlayer(Vector2 posInital)
    {
		StopCoroutine (ChasePlayer());
        playerController.StopAllCoroutines();    
        transform.position = posInital;    
        StartCoroutine(ChasePlayer());
        stop = false;
    }

    #region GameEvent
    public void StopMove()
    {
        queuePlaformAIKnow.Clear();
        StopAllCoroutines();
        stop = true;
    }
    #endregion

    void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Player") {
			other.GetComponent<PlayerController> ().WolfEatPlayer ();
			stop = true;
		}
	}

    #region Debug
    [ContextMenu("Start AI Move")]
    void StartAIMove()
    {
        StartCoroutine(ChasePlayer());
    }
    #endregion
}
}
