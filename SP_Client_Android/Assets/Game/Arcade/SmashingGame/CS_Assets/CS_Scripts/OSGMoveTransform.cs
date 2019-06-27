using System.Collections;
using UnityEngine;

namespace ObjectSmashingGame
{
	/// <summary>
	/// This script animates the UI while the game is paused
	/// </summary>
	public class OSGMoveTransform : MonoBehaviour
	{
        internal Transform thisTransform;

        // A referemce to the Game Controller, which is taken by the first time this script runs, and is remembered across all other scripts of this type
        static OSGGameController gameController;

        [Tooltip("The relative movement speed of this")]
        public float relativeSpeed = 1;

        // Use this for initialization
        void Start()
        {
            thisTransform = this.transform;

            // Hold the gamcontroller object in a variable for quicker access
            if (gameController == null) gameController = GameObject.FindObjectOfType<OSGGameController>();
        }

        void Update()
        {
            thisTransform.Translate(Vector3.forward * gameController.moveSpeed * Time.deltaTime, Space.Self);
        }
    }
}

