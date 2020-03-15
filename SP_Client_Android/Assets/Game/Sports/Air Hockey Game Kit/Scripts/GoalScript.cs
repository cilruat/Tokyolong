
// GOAL SCRIPT 20171215 //

using UnityEngine;
using System.Collections;

namespace AirHokey
{
    public class GoalScript : MonoBehaviour
    {


        public int goalNumber = 1;  // the goal number (side 1 or 2) 
        public Animation goalIndicatorAnim;
        //public GameObject puck; // unused
        public MainScript mainScript;

        // is the game paused ?
        public bool gamePause = false;

        // is the puck already colliding the goal ? - Meant to prevent multiple 'OnTriggerEnter()' calls
        public bool isColliding = false;

        // "OnPauseGame()" and "OnResumeGame()" are called by "MainScript" when hitting the pause key or button

        void OnPauseGame()
        {

            gamePause = true;

        }

        void OnResumeGame()
        {

            gamePause = false;

        }



        IEnumerator Goal()
        {

            while (gamePause == true)
            {
                yield return null;
            }

            GetComponent<AudioSource>().Play();

            // Play the fading texture animation attached to the children object "Goal Indicator" 
            goalIndicatorAnim.Play();

            // notify "MainScript" that there is a score update
            mainScript.StartCoroutine(mainScript.UpdateScore(goalNumber));

            yield return 1;

            isColliding = false; // used by 'OnTriggerEnter()' to prevent multiple calls
        }


        void OnTriggerEnter(Collider other)
        {
            if (isColliding) return; // If there is already a collision, abort the function
            isColliding = true; // notify the script that there is actually a collision

            StartCoroutine(Goal());

        }
    }
}