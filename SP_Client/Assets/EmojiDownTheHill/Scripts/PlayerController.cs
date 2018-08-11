using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Emoji2
{
    public class PlayerController : MonoBehaviour
    {
        public static event System.Action PlayerDied;

        public SpawnCubes spawner;
        public GameObject theWall;
        public GameObject theBallSprite;
        // the sprite
        public GameObject movingBall;
        // obsolete , but still can be re used to check the collision
        public AnimationClip jumpAnim;

        [HideInInspector]   
        public float speedY;
        [HideInInspector] 
        public bool isJumping = false;

        Animator ballAnimator;

        float direction = -1;
        float jumpDirection;
        float prevYpositionOfTheGround;
        Vector2 newWavePosition;
        // aka the move up speed;
        bool firstClick = true;
        bool isReadyToPlay = false;
        bool isJumpingToTheRight = true;
        bool isReadyForNextJump = false;
        bool isReadyForNextLoop = false;
    	
        [HideInInspector]
        public bool isreadyForBigJump = false;
        //this variable is used when the ball meets a spring
        bool isReadyToLeap = false;
        float t = 0;
        float jumpTime;

        const float nearlyHalfTheSpriteLength = 0.4375f;
        const float threeFourthTheLengthOfTheSprite = 0.74f;

        [HideInInspector]
        public bool isCheckingCollision = false;
    	
        float temporaryXposition = 0;

        void OnEnable()
        {
            GameManager.GameStateChanged += GameManager_GameStateChanged;
        }

        void GameManager_GameStateChanged(GameState newState, GameState oldState)
        {
            if (newState == GameState.Playing && oldState == GameState.Prepare)
            {
                if (firstClick)
                {
                    firstClick = false;
                    isReadyToPlay = true;
                    isReadyForNextJump = true;
                    isJumpingToTheRight = true;
                }
            }
        }


        // Use this for initialization
        void Start()
        {

            ballAnimator = theBallSprite.GetComponent<Animator>();
            jumpTime = jumpAnim.length;
            newWavePosition = theWall.transform.position;
    		
            // Set the sprite to the selected character
            if (CharacterManager.Instance != null)
            {
                theBallSprite.GetComponent<SpriteRenderer>().sprite = CharacterManager.Instance.character;
            }
        }

        // Update is called once per frame
        void Update()
        {

            if (GameManager.Instance.GameState == GameState.Playing && Input.GetMouseButtonDown(0))
            {
                // Change direction
                if (!firstClick)
                {
                    direction *= -1;
                    isJumpingToTheRight = !isJumpingToTheRight;
                }
            }


            if (isReadyToPlay)
            {		
                Leap();
            }
        }

        public float MoveUpSpeed(float timeT, float distanceS)
        {
            return (distanceS / timeT);
        }

        public float MoveTheBallSpeed(float timeT, float distanceS)
        {
            return (distanceS / timeT);
        }

        public void Leap()
        {

            // Start jumping!
            if (!isCheckingCollision)
            {
                if (isReadyForNextJump)
                {
                    temporaryXposition = transform.position.x;
                    jumpDirection = direction;

                    isJumping = true;   // ball starts jumping
    				
                    if (isreadyForBigJump)
                    {
                        isReadyForNextJump = false;
                        isReadyToLeap = true;
                        isReadyForNextLoop = false;
                        if (jumpDirection == 1)
                        {
                            ballAnimator.SetBool("LeapRight", true);
                        }
                        else
                        {
                            ballAnimator.SetBool("LeapLeft", true);
                        }
                    }
                    else
                    {
                        if (jumpDirection == 1)
                        {
                            ballAnimator.SetBool("JumpRight", true);
                        }
                        else
                        {
                            ballAnimator.SetBool("JumpLeft", true);
                        }
                        isReadyForNextLoop = true;
                        isReadyForNextJump = false;
                    }	

                    movingBall.transform.position = new Vector2(temporaryXposition, movingBall.transform.position.y);
                }
            }

            if (isReadyForNextLoop)
            {
    				
                speedY = MoveUpSpeed(jumpTime, GameManager.Instance.boundsY * threeFourthTheLengthOfTheSprite);
                newWavePosition.y += (speedY) * Time.deltaTime;
                theWall.transform.position = newWavePosition;
                t += Time.deltaTime;

                // When jump animation finishes
                if (t >= jumpTime)
                {
                    transform.position = new Vector2(transform.position.x + jumpDirection * (GameManager.Instance.boundsX / 2), transform.position.y);
                    ballAnimator.SetBool("JumpRight", false);
                    ballAnimator.SetBool("JumpLeft", false);
                    movingBall.transform.position = transform.position;
    		
                    isJumping = false;
                    isReadyForNextJump = true;
                    isReadyForNextLoop = false;
                    isCheckingCollision = true;
                    t = 0;
                }
            }

            if (isReadyToLeap)
            {
                isreadyForBigJump = false;
                isReadyForNextLoop = false;

                speedY = MoveUpSpeed(jumpTime, GameManager.Instance.boundsY * threeFourthTheLengthOfTheSprite);
                newWavePosition.y += (speedY) * Time.deltaTime;
                theWall.transform.position = newWavePosition;

                t += Time.deltaTime;

                // When double jump finishes
                if (t >= jumpTime * 2)
                {
                    temporaryXposition += 0;
                    temporaryXposition = transform.position.x;

                    transform.position = new Vector2(transform.position.x + jumpDirection * (GameManager.Instance.boundsX / 2) * 2, transform.position.y);
                    ballAnimator.SetBool("LeapRight", false);
                    ballAnimator.SetBool("LeapLeft", false);

                    movingBall.transform.position = transform.position;
                    isJumping = false;
                    isReadyForNextJump = true;
                    isReadyForNextLoop = false;
                    isReadyToLeap = false;
                    t = 0;
                    isCheckingCollision = true;
                }
            }
        }

        public void Die()
        {
            if (GameManager.Instance.GameState != GameState.GameOver)
            {
                PlayerDied();
            }
        }
    }
}