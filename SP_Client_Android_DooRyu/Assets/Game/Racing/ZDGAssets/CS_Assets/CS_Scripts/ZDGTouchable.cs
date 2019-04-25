using UnityEngine;

namespace ZombieDriveGame
{
    /// <summary>
    /// This script defines an object which can interact with the player in various ways. A touchable may be a rock or a railing that
    /// bounces the player back, or it can be a zombie that the player can run over, or it can be an item that can be collected.
    /// </summary>
    public class ZDGTouchable : MonoBehaviour
    {
        [Tooltip("The tag of the object that can touch this block")]
        public string touchTargetTag = "Player";

        [Tooltip("The function that runs when this object is touched by the target")]
        public string touchFunction = "ChangeScore";

        [Tooltip("The parameter that will be passed with the function")]
        public float functionParameter = 100;

        [Tooltip("The target object that the function will play from")]
        public string functionTarget = "GameController";

        [Tooltip("The effect that is created at the location of this object when it is touched")]
        public Transform touchEffect;

        [Tooltip("A random rotation given to the object only on the Y axis")]
        public Vector2 rotationRange = new Vector2(0,360);
        
        void Start()
        {
            transform.eulerAngles = Vector3.up * Random.Range( rotationRange.x, rotationRange.y);
        }

        /// <summary>
        /// Is executed when this obstacle touches another object with a trigger collider
        /// </summary>
        /// <param name="other"><see cref="Collider"/></param>
        void OnTriggerEnter(Collider other)
        {
            // Check if the object that was touched has the correct tag
            if (other.tag == touchTargetTag)
            {
                // Check that we have a target tag and function name before running
                if (touchFunction != string.Empty)
                {
                    // Run the function
                    GameObject.FindGameObjectWithTag(functionTarget).SendMessage(touchFunction, functionParameter);
                }

                // If there is a touch effect, create it
                if (touchEffect) Instantiate(touchEffect, transform.position, transform.rotation);
                
                // Remove the object from the game
                Destroy(gameObject);
            }
        }
    }
}