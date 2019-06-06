using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyFlightGame
{
    /// <summary>
    /// Defines a ring which the player can pass through or miss
    /// </summary>
    public class SFGRing : MonoBehaviour
    {
        // Various variables for easier access
        internal Transform thisTransform;
        internal SFGPlayerControls playerObject;
        internal SFGGameController gameController;
        internal TrailRenderer[] ringEdges;

        [Tooltip("The size of the ring we must pass through")]
        public float ringSize = 3;

        [Tooltip("The rotation speed of the ring object")]
        public float spinSpeed = 100;

        [Tooltip("The speed increase the player gets when it passes through the ring")]
        public float speedBoost = 2;

        [Tooltip("The miss effect that appears when the player passes near a ring, but not through it")]
        public GameObject missEffect;

        [Tooltip("The target position of the ring. When we pass through the ring, it changes position and moves away from the player")]
        internal Vector3 targetPosition;

        [Tooltip("The sound to play when passing through the ring")]
        public AudioClip sound;

        [Tooltip("The tag of the sound source")]
        public string soundSourceTag = "Sound";

        // Use this for initialization
        void Start()
        {
            thisTransform = this.transform;

            // Set the target position of the ring
            targetPosition = thisTransform.position;

            // Find and hold the gamecontroller for quicker access
            gameController = GameObject.FindObjectOfType<SFGGameController>();

            // Find and hold the player object for quicker access
            playerObject = gameController.playerObject.GetComponent<SFGPlayerControls>();

            // Find all the ring edges for quicker access
            //ringEdges = GetComponentsInChildren<TrailRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (playerObject == null && gameController.playerObject ) playerObject = gameController.playerObject.GetComponent<SFGPlayerControls>();

            if (playerObject)
            {
                // Move the ring towards the target position
                thisTransform.position = Vector3.Slerp(thisTransform.position, targetPosition, playerObject.moveSpeed * Time.deltaTime);

                // Rotate the 
                transform.Find("RingEdge").Rotate(Vector3.forward * spinSpeed * Time.deltaTime, Space.Self);

                // Set the 
                //for ( int index = 0; index < ringEdges.Length; index++ )
                //{
                  //  ringEdges[index].time = 1 - Vector3.Distance(thisTransform.position, targetPosition) * 0.05f;

                   // ringEdges[index].startColor = Color.red;

                   // ringEdges[index].endColor = Color.red;

                //}

                // If the player reaches the ring, check if it passes through or misses
                if (playerObject.transform.position.z > targetPosition.z && playerObject.isDead == false )
                {
                    // If the player passes thro
                    if ( Vector3.Distance(playerObject.transform.position, targetPosition) <= ringSize )
                    {
                        // Play the ring sound
                        PlaySound();

                        // Create the next ring
                        gameController.CreateRing();
                    }
                    else //Otherwise, we missed the ring
                    {
                        // Run the miss function
                        playerObject.Miss();

                        // Create the miss effect
                        if (missEffect)
                        {
                            missEffect.SetActive(true);

                            if (missEffect.GetComponent<Animation>()) missEffect.GetComponent<Animation>().Play();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Hides the miss effect
        /// </summary>
        public void HideMissEffect()
        {
            if (missEffect) missEffect.SetActive(false);
        }

        /// <summary>
        /// Plays the sound
        /// </summary>
        public void PlaySound()
        {
            // If there is a sound source tag and audio to play, play the sound from the audio source based on its tag
            if (soundSourceTag != string.Empty && sound)
            {
                // Play the sound
                GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>().PlayOneShot(sound);
            }
        }

    }

}
