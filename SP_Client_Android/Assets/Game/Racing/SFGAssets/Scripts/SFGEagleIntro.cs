using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyFlightGame
{
    /// <summary>
    /// Defines the intro eagle which simply moves forward, flapping its wings periodically
    /// </summary>
    public class SFGEagleIntro : MonoBehaviour
    {
        [Tooltip("The camera object that follows the player")]
        public Transform cameraObject;

        [Tooltip("The forward movement speed of the player")]
        public float moveSpeed = 10;

        [Tooltip("The distance at which the eagle resets its position")]
        public float resetDistance = 500;

        // Update is called once per frame
        void Update()
        {
            // Move forward
            transform.Translate( Vector3.forward * moveSpeed * Time.deltaTime, Space.World);

            // If the eagle reaches the reset distance, move it back to the start
            if ( transform.position.z > resetDistance) transform.Translate(Vector3.forward * -resetDistance, Space.World);

            // Periodically play the flap animation
            if (Random.value > 0.997f) GetComponent<Animator>().Play("Flap");
        }

        private void LateUpdate()
        {
            if (cameraObject) cameraObject.position = transform.position;
        }
    }
}
