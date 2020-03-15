
// PUCK SCRIPT 20171215 //

using UnityEngine;
using System.Collections;


namespace AirHokey
{
    public class PuckScript : MonoBehaviour
    {

        // the maximum physic velocity allowed for the puck
        public float maxVelocity = 10.00f;

        // Simple boolean to check if puck is allowed to move
        public bool canMove = true;

        // in "myRb" and "myTr" we will assign the gameObject rigidbody and transform components for optimisation.
        private Rigidbody myRb;
        private Transform myTr;

        //private var clampedVelocity : Vector3;
        private float sqrMaxVelocity;
        private Vector3 curVelocity;


        // Save velocity properties to restore them on resume pause
        private Vector3 pausedVelocity;
        private Vector3 pausedAngularVelocity;

        // is the game paused ?
        bool gamePause = false;

        void Start()
        {

            // We assign transform and rigidbody to the variables bellow for optimisation.
            myTr = transform; // We will now use "myTr" instead of "transform"
            myRb = GetComponent<Rigidbody>(); // and "myRb" instead of "rigidbody"

            // Call "SetMaxVelocity()" function whoes compute "sqrMaxVelocity" value based on "maxVelocity";
            SetMaxVelocity(maxVelocity);

        }

        // "OnPauseGame()" and "OnResumeGame()" are called by "MainScript" when hitting the pause key or button

        void OnPauseGame()
        {

            pausedVelocity = myRb.velocity;
            pausedAngularVelocity = myRb.angularVelocity;
            gamePause = true;
            myRb.isKinematic = true;
            myTr.GetComponent<Collider>().enabled = false;

        }

        void OnResumeGame()
        {

            gamePause = false;
            myRb.isKinematic = false;
            myTr.GetComponent<Collider>().enabled = true;
            myRb.velocity = pausedVelocity;
            myRb.angularVelocity = pausedAngularVelocity;

        }


        // Called by "MainScript" -Should be called when puck's velocity has to be changed !
        public void SetMaxVelocity(float maxVelocityValue)
        {

            this.maxVelocity = maxVelocityValue;
            sqrMaxVelocity = maxVelocity * maxVelocity;

        }

        void Update()
        {

            // If the puck is not allowed to move, stop the function
            if (canMove == false || gamePause == true) return;

            /*
            // Prevent the puck to go out of the playing surface																																																						
            if (myTr.localPosition.x < -0.6)	myTr.localPosition = new Vector3 (-0.5f, myTr.localPosition.y, myTr.localPosition.z);		// set minimum X position
            else
            if (myTr.localPosition.x > 0.6)	myTr.localPosition = new Vector3 (0.5f, myTr.localPosition.y, myTr.localPosition.z);			// set maximum X position

            if (myTr.localPosition.z < -1.2)	myTr.localPosition = new Vector3 (myTr.localPosition.x, myTr.localPosition.y, -1.1f);		// set minimum Z position
            else
            if (myTr.localPosition.z > 1.2)	myTr.localPosition = new Vector3 (myTr.localPosition.x, myTr.localPosition.y, 1.1f);			// set maximum Z position
            */

        }

        void FixedUpdate()
        {

            // If the puck is not allowed to move, stop the function
            if (canMove == false || gamePause == true) return;

            // Get current velocity
            curVelocity = myRb.velocity;

            // Check if rigidbody's velocity exceed max velocity and correct it
            if (curVelocity.sqrMagnitude > sqrMaxVelocity && myRb.isKinematic == false)
            {
                myRb.velocity = curVelocity.normalized * maxVelocity;
            }

        }

        // Place the puck in the notified zone - serviceSide : "0" = middle, "1" = mallet 1 service zone, "2" = mallet 2 service zone
        public IEnumerator Place(int serviceSide)
        {

            while (gamePause == true)
            {
                yield return null;
            }

            // Set rigidbody to kinematic
            myRb.isKinematic = true;

            myTr.localEulerAngles = Vector3.zero;

            if (serviceSide == 0) myTr.localPosition = new Vector3(0, myTr.localPosition.y, 0);
            else
            if (serviceSide == 1) myTr.localPosition = new Vector3(0, myTr.localPosition.y, -0.6f);
            else
            if (serviceSide == 2) myTr.localPosition = new Vector3(0, myTr.localPosition.y, 0.6f);

            yield return null;

            myRb.isKinematic = false;

            // Stop the velocity
            myRb.velocity = myRb.angularVelocity = Vector3.zero;

        }

        void OnCollisionEnter(Collision collisionInfo)
        {

            // Check the collision object's tag to detect if we hit the bounds
            if (collisionInfo.gameObject.tag != "Mallet" && gameObject.activeInHierarchy == true) GetComponent<AudioSource>().Play();

        }

    }
}