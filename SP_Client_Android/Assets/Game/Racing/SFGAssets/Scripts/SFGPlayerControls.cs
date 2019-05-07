using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyFlightGame
{
    /// <summary>
    /// Controls the player object with mouse/keyboard/gamepad/tilt controls, and checks death state
    /// </summary>
    public class SFGPlayerControls : MonoBehaviour
    {
        // Holding variables for quicker access
        internal SFGGameController gameController;
        internal Transform thisTransform;

        //[Tooltip("The camera object that follows the player object")]
        internal Transform cameraObject;

        [Tooltip("The forward movement speed of the player object")]
        public float moveSpeed = 10;

        [Tooltip("The turning speed of the player object")]
        public float turnSpeed = 20;

        [Tooltip("The maximum angle the player object can turn to")]
        public float turnAngle = 30;

        // The turning direction of the player
        internal Vector2 turnDirection = Vector2.zero;

        [Tooltip("The maximum height the player can rise to")]
        public float heightLimit = 15;

        [Tooltip("A list of player limbs that will bend based on the turning direction")]
        public Transform[] limbsThatTurn;

        [Tooltip("The current number of lives the player has. If it reaches 0, it's game over")]
        public int lives = 3;

        [Tooltip("A delay after getting hurt, so we don't get hurt again for a while")]
        public float hurtDelay = 3;
        internal float hurtDelayCount = 0;

        [Tooltip("The effect that appears when the player gets hurt")]
        public ParticleSystem hurtEffect;

        [Tooltip("The effect that appears when the player dies")]
        public GameObject deathEffect;

        [Tooltip("Is the player controlling the player object?")]
        public bool inControl = false;

        // If the player is dead, we can't play
        internal bool isDead = false;

        // The right mobile stick that moves the player in 4 directions
        internal Vector2 rightStickPosition;
        internal bool rightStickDown = false;

        // Use this for initialization
        void Start()
        {
            // Find and hold the gamecontroller from the scene for quicker access
            gameController = GameObject.FindObjectOfType<SFGGameController>();

            // Assign the camera object from the game controller
            if ( gameController && cameraObject == null) cameraObject = gameController.cameraObject;

            // Reset the hurt delay so we don't get hurt again immediately
            hurtDelayCount = hurtDelay;

            // Hold the transform for quicker access
            thisTransform = this.transform;
        }

        // Update is called once per frame
        void Update()
        {
            // If the game is paused, don't continue to the code that checks controls and movement
            if ( gameController == null || gameController.isPaused == true ) return;

            // Move the player in the forward direction it looks
            thisTransform.Translate(new Vector3(turnDirection.x * turnSpeed, turnDirection.y * turnSpeed, moveSpeed) * Time.deltaTime, Space.World);

            // Play the flapping animation every once in a while
            if (Random.value > 0.995f) GetComponent<Animator>().Play("Flap");

            // If the player is not in control, don't continue to the code that turns the player
            if (inControl == false) return;

            // Rotate the player based on turn direction which is controlled by the mouse/keyboard/gamepad/tilt/etc
            thisTransform.localEulerAngles = new Vector3(turnDirection.y * -turnAngle, 0, turnDirection.x * -turnAngle);

            // Update the hurt delay
            if (hurtDelayCount > 0) hurtDelayCount -= Time.deltaTime;
        }

        private void LateUpdate()
        {
            // Go through all the limbs of the player model and turn them as the player rotates. This gives a curvy look to the player as it turns
            for (int index = 0; index < limbsThatTurn.Length; index++)
            {
                limbsThatTurn[index].localEulerAngles = new Vector3(turnDirection.x * -turnSpeed * 0.5f, turnDirection.y * -turnSpeed * 0.5f, turnDirection.x * -turnSpeed * 0.5f);
            }

            if (cameraObject)
            {
                // Make the camera follow the player
                cameraObject.position = transform.position;

                // Make the camera look towards the direction the player is rotating to
                cameraObject.GetChild(0).localEulerAngles = new Vector3(turnDirection.y * -turnSpeed * 0.5f, turnDirection.x * turnSpeed * 0.5f, 0);
            }
        }

        /// <summary>
        /// Loses control of the player
        /// </summary>
        public void LoseControl()
        {
            inControl = false;
        }

        /// <summary>
        /// Regains control of the player
        /// </summary>
        public void GetControl()
        {
            inControl = true;
        }

        /// <summary>
        /// Checks if we started holding the right stick down and registers the touch position
        /// </summary>
        public void RightStickDown()
        {
            rightStickPosition = Input.mousePosition;

            rightStickDown = true;
        }

        /// <summary>
        /// Checks if we released the right stick
        /// </summary>
        public void RightStickUp()
        {
            rightStickDown = false;
        }

        public void OnTriggerEnter(Collider collision)
        {
            // If the player touches any obstacle, it dies
            Die();
        }

        /// <summary>
        /// Misses a ring, and loses the game
        /// </summary>
        public void Miss()
        {
            // If we are still hurt, we can't get hurt again
            if (hurtDelayCount > 0) return;

            // If we have lives, we get hurt, but not die
            if ( lives > 1 )
            {
                // Reduce from lives
                lives--;

                // Update the lives text of the player
                if (gameController.livesText) gameController.livesText.text = lives.ToString();

                // Reset the hurt delay so we don't get hurt again immediately
                hurtDelayCount = hurtDelay;

                // Create the next ring
                gameController.CreateRing();
            }
            else
            {
                // Lose control of the player
                LoseControl();

                // Run the game over function from the gamecontroller
                GameObject.FindObjectOfType<SFGGameController>().SendMessage("GameOver", 1);

                // Stop the camera from following the player
                cameraObject = null;

                // Remove the player object after 2 seconds
                Destroy(gameObject, 2);
            }
        }

        /// <summary>
        /// Dies and ends the game
        /// </summary>
        public void Die()
        {
            // If we are still hurt, we can't get hurt again
            if ( gameController == null || hurtDelayCount > 0 ) return;

            // If we have lives, we get hurt, but not die
            if ( lives > 1 )
            {
                // Reduce from lives
                lives--;

                // Update the lives text of the player
                if ( gameController.livesText) gameController.livesText.text = lives.ToString();

                // Reset the hurt delay so we don't get hurt again immediately
                hurtDelayCount = hurtDelay;

                // Play the hurt effect
                if (hurtEffect) Instantiate(hurtEffect, transform.position, Quaternion.identity);

                // Create the next ring
                gameController.CreateRing();
            }
            else
            {
                // Lose control of the player
                LoseControl();

                // The player is dead, it cannot pass rings anymore
                isDead = true;

                // Play the hurt effect
                if (hurtEffect) Instantiate(hurtEffect, transform.position, Quaternion.identity);

                // If there is a death effect, show it
                if (deathEffect) deathEffect.SetActive(true);

                // Give the player some physical properties to make it fall down
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().isKinematic = false;

                // Throw the player and spin it based on its speed
                GetComponent<Rigidbody>().AddForce(Vector3.forward * 1 * moveSpeed, ForceMode.Impulse);
                GetComponent<Rigidbody>().AddTorque(Vector3.right * 1 * moveSpeed, ForceMode.Impulse);
                GetComponent<Rigidbody>().AddTorque(Vector3.forward * 1 * moveSpeed, ForceMode.Impulse);

                // Play the death animation of the player
                GetComponent<Animator>().Play("Die");

                // Run the game over function from the gamecontroller
                GameObject.FindObjectOfType<SFGGameController>().SendMessage("GameOver", 1);

                // Stop the camera from following the player
                cameraObject = null;

                // Remove the player object after 2 seconds
                Destroy(gameObject, 2);
            }
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(new Vector3(-100, heightLimit, 0), new Vector3(100, heightLimit, 0));
        }
    }
}
