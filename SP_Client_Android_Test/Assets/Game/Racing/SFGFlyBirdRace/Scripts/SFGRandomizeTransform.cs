using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyFlightGame
{
    /// <summary>
    /// Randomizes the rotation of an object, and it horizontal position
    /// </summary>
    public class SFGRandomizeTransform : MonoBehaviour
    {
        [Tooltip("Random rotation range")]
        public float randomRotation = 360;

        [Tooltip("The maximum height the player can rise to")]
        public float randomHorizontal = 2;

        public void Start()
        {
            // Set the random rotation
            transform.eulerAngles += Vector3.up * Random.Range(0, 360);

            // Set the random position
            transform.position += Vector3.right * Random.Range(-randomHorizontal, randomHorizontal);
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(new Vector3(transform.position.x - randomHorizontal, transform.position.y + 1, transform.position.z), new Vector3(transform.position.x + randomHorizontal, transform.position.y + 1, transform.position.z));

        }
    }
}
