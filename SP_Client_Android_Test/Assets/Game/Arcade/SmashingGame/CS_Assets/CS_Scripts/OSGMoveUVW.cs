using System.Collections;
using UnityEngine;

namespace ObjectSmashingGame
{
	/// <summary>
	/// This script animates the UI while the game is paused
	/// </summary>
	public class OSGMoveUVW : MonoBehaviour
	{
        internal Renderer thisRenderer;

        // A referemce to the Game Controller, which is taken by the first time this script runs, and is remembered across all other scripts of this type
        static OSGGameController gameController;

        [Tooltip("The relative movement speed of this UVW texture. This is always affected by the move speed in the gamecontroller")]
        public float relativeSpeed = 0.5f;

        // Use this for initialization
        void Start()
        {
            thisRenderer = GetComponent<Renderer>();

            // Hold the gamcontroller object in a variable for quicker access
            if (gameController == null) gameController = GameObject.FindObjectOfType<OSGGameController>();
        }

        void Update()
        {
            thisRenderer.material.mainTextureOffset = new Vector2(0, gameController.moveSpeed * relativeSpeed * Time.time);
        }
    }
}

