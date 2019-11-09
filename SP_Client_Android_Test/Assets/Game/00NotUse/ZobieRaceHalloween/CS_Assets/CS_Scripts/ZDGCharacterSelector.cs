using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ZombieDriveGame
{
	/// <summary>
	/// This script displays several levels that can be loaded, and are locked and rated based on our PlayerPrefs score
	/// </summary>
	public class ZDGCharacterSelector : MonoBehaviour 
	{
        [Tooltip("The current level we are on. The first level is 0, the second 1, etc")]
        public int currentCharacter = 0;

        [Tooltip("A list of levels in the game, their names, the scene they link to, the price to unlock it, and the state of the level ( -1 - locked and can't be played, 0 - unlocked no stars, 1-X the star rating we got on the level")]
        public ZDGPlayer[] characters;

        [Tooltip("The icon object that displays the current character")]
        public Transform currentIcon;
        internal Transform previousIcon;

        public Button buttonNext;

        public Button buttonPrev;

        internal int index;

        //[Tooltip("The/texture of the icon when it is locked ( The black color on locked 3d models )")]
        //public Texture lockedTexture;

        void Start()
        {
            // Don't remove this object until we find a GameController and assign the selected character to it
            DontDestroyOnLoad(gameObject);

            // Listen for a click to change to the next character
            buttonNext.onClick.AddListener(delegate { ChangeCharacter(1); });

            // Listen for a click to change to the next character
            buttonPrev.onClick.AddListener(delegate { ChangeCharacter(-1); });

            ChangeCharacter(0);
        }

        public void Update()
        {
            // If there is a gamecontroller in the scene, assign the current character as the player
            if (FindObjectOfType<ZDGGameController>())
            {
                FindObjectOfType<ZDGGameController>().AssignCurrentPlayer(characters[currentCharacter]);

                // Remove this character selector
                Destroy(gameObject);

                return;
            }
        }
        
        public void ChangeCharacter( int changeValue )
        {
            // Change the index of the character
            currentCharacter += changeValue;

            // Make sure we don't go out of the list of available characters
            if (currentCharacter > characters.Length - 1) currentCharacter = 0;
            else if (currentCharacter < 0) currentCharacter = characters.Length - 1;

            // Display the icon of the current character
            if ( characters[currentCharacter].transform )
            {
                //currentIcon = new Transform();
                previousIcon = currentIcon;

                // Create a new icon and place it in the scene
                currentIcon = Instantiate(characters[currentCharacter].transform, currentIcon.position, currentIcon.rotation) as Transform;

                // If this icon has an animation, stop it
                if (currentIcon.GetComponent<Animator>()) currentIcon.GetComponent<Animator>().Play("Bounce");

                // Remove the previous icon
                if (previousIcon) Destroy(previousIcon.gameObject);
            }
        }
    }
}