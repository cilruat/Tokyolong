using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ObjectSmashingGame
{
    public class OSGSmashObject : MonoBehaviour
    {
        internal Transform thisTransform;

        public float moveSpeed = 1.2f;
        
        // A referemce to the Game Controller, which is taken by the first time this script runs, and is remembered across all other scripts of this type
        static OSGGameController gameController;

        // The animated part of the object. By default this is taken from this object
        internal Animation objectAnimation;

        [Tooltip("The health of the object. How many hits it takes to be smashed.")]
        public int health = 1;

        [Tooltip("The bonus we get smashing this object")]
        public int bonus = 1;

        [Tooltip("The tag of the object that can hit this object")]
        public string targetTag = "Player";

        [Tooltip("Hurt the target that touches this object ( ex: the player loses a life when touching this object")]
        public bool hurtTarget = false;

        [Tooltip("The effect created when this object is smashed")]
        public Transform smashEffect;

        // Is the object dead?
        internal bool isDead = false;

        internal int index;

        // Use this for initialization
        void Start()
        {
            thisTransform = this.transform;

            // Hold the gamcontroller object in a variable for quicker access
            if (gameController == null) gameController = GameObject.FindObjectOfType<OSGGameController>();

            // The animator of the object. This holds all the animations and the transitions between them
            if (GetComponent<Animation>()) objectAnimation = GetComponent<Animation>();
        }

        void Update()
        {
            thisTransform.Translate( Vector3.forward * gameController.moveSpeed * Time.deltaTime, Space.Self);

            if ( thisTransform.position.z > gameController.laneLength )
            {
                // If this is a regular object ( doesn't hurt the player ), then it will only hurt the player when it passes the screen without being smashed
                if ( hurtTarget == false )    gameController.ChangeLives(-1);

                gameController.objectsLeft--;

                gameController.UpdateProgress();

                Destroy(gameObject);
            }
        }

        void HitObject( Vector3 hitPosition)
        {
            ChangeHealth(-1);

            // Hurt the target ( make the player lose a life )
            if (hurtTarget) gameController.ChangeLives(-1);
        }

        /// <summary>
        /// Changes the health of the target, and checks if it should die
        /// </summary>
        /// <param name="changeValue"></param>
        public void ChangeHealth(int changeValue)
        {
            // Chnage health value
            health += changeValue;

            if (health > 0)
            {
                // Animate the hit object
                if (objectAnimation)
                {
                    objectAnimation.Stop();
                    objectAnimation.Play();
                }
            }
            else
            {
                // Health reached 0, so the target should die
                Die();
            }
        }

        /// <summary>
        /// Kills the object and gives it a random animation from a list of death animations
        /// </summary>
        public void Die()
        {
            // The object is now dead. It can't move.
            isDead = true;

            // If there is a smash effect, create it at the position of the smashed object
            if ( smashEffect )
            {
                // Create the smash effect
                Instantiate( smashEffect, thisTransform.position, thisTransform.rotation);
            }

            // If there is a bonus effect, create it at the position of the smashed object
            //if (bonusEffect)
            //{
            //    for ( index = 0; index < bonus; index++)
            //    {
            //        // Create the bonus effect
            //        Instantiate(bonusEffect, thisTransform.position, thisTransform.rotation);
            //    }

            //}


            if (bonus != 0)
            {
                gameController.ChangeScore(bonus);

                // If we have a bonus effect
                if (gameController.bonusEffect)
                {
                    // Create a new bonus effect at the hitSource position
                    Transform newBonusEffect = Instantiate(gameController.bonusEffect) as Transform;

                    newBonusEffect.position = transform.position;// + Vector3.up;

                    // Display the bonus value multiplied by a streak
                    newBonusEffect.Find("Text").GetComponent<Text>().text = "+" + bonus.ToString();

                    // Rotate the bonus text slightly
                    //newBonusEffect.eulerAngles = Vector3.forward * Random.Range(-10, 10);
                }
            }

            gameController.objectsLeft--;

            gameController.UpdateProgress();

            Destroy(gameObject);
        }
    }
}
