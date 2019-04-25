using UnityEngine;

namespace ZombieDriveGame
{
    /// <summary>
    /// This script defines an explosion of gibs. Each gib must have a rigidbody attached to it.
    /// </summary>
    public class ZDGGibExplode : MonoBehaviour
    {
        [Tooltip("A list of the objects that will be exploded")]
        public Rigidbody[] gibs;

        [Tooltip("The explosion power. The objects fly randomly")]
        public float explodePower = 100;

        internal int index;
        
        public void Start()
        {
            Explode();
        }

        public void Explode()
        {
            // Go through all the gibs, and throw them in a random direction
            for ( index = 0; index < gibs.Length; index++ )
            {
                // Throw the object in a random direction
                gibs[index].AddForce(new Vector3(Random.Range(-explodePower, explodePower), Random.Range(explodePower * 2, explodePower * 3), Random.Range(-explodePower, explodePower)));

                // Give the object a random rotation force
                gibs[index].AddTorque(new Vector3(Random.Range(-explodePower, explodePower), Random.Range(-explodePower, explodePower), Random.Range(-explodePower, explodePower)));
            }
        }
    }
}