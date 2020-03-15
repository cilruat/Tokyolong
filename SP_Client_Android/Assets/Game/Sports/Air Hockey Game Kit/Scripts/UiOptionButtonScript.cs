
// UI BUTTON SCRIPT 20171215 //

using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace AirHokey
{
    public class UiOptionButtonScript : MonoBehaviour
    {

        // Each UI button has a unique ID number
        public int buttonNumber = 0;

        public MainScript mainScript;

        // For options buttons containing numeric value
        //public bool isNumericOptionButton = false;

        // Enabling highlighting effect ?
        public bool highlighting = true;

        // The numeric option's actual value
        public int optionValue;

        // Array of number object meshes
        //public ui
        public Image numericImage;
        public Sprite[] numericOption;


        // WIP IS THE BUTTON ALREADY IN USE ? // CHECK : http://forum.unity3d.com/threads/iphone-touch-phase-began-and-hittest-repeating.77027/
        private bool buttonIsUsed = false;

        void Start()
        {
            // Check for saved values
            if (buttonNumber == 3 && PlayerPrefs.HasKey("Game scoreToWin")) optionValue = PlayerPrefs.GetInt("Game scoreToWin");
            else if (buttonNumber == 3) optionValue = 5;

            if (buttonNumber == 4 && PlayerPrefs.HasKey("Game difficulty")) optionValue = PlayerPrefs.GetInt("Game difficulty");
            else if (buttonNumber == 4) optionValue = 2;

            // Send the option value of the button to the main Script to ensure to have the same value on both
            StartCoroutine(mainScript.UiOptionModify(buttonNumber, optionValue));

            // Init the display of the value
            numericImage.sprite = numericOption[optionValue];
            //UpdateNumericOptionDisplay();	
        }

        void OnEnable()
        {
            // DEPRECATED
            // transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, 0); // Reset the position

        }


        // This function process numeric option button's behavior
        // It manages the UI display, then send is value to "MainScript"
        public void ProcessNumericOption()
        {

            // Performing a debug log with Unity remote could be handy
            Debug.Log("UIButtonScript : " + "ProcessNumericOption()", gameObject);

            // if this object is the "Score to win" option button
            if (buttonNumber == 3)
            {
                // update the score to win option variable
                if (optionValue == 9) optionValue = 3;
                else if (optionValue == 3) optionValue = 5;
                else if (optionValue == 5) optionValue = 7;
                else if (optionValue == 7) optionValue = 9;

                // Save the new preset in Player Preferences
                PlayerPrefs.SetInt("Game scoreToWin", optionValue);

                numericImage.sprite = numericOption[optionValue];

                //Manage the UI display with function "UpdateNumericOptionDisplay"
                //UpdateNumericOptionDisplay ();

                // Call function "UiEventOptionButton" in "MainScript", and send it the option value
                StartCoroutine(mainScript.UiOptionModify(buttonNumber, optionValue));
            }

            else

            // if this object is the "Difficulty" option button
            if (buttonNumber == 4)
            {
                // update the score to win option variable
                if (optionValue == 5) optionValue = 1;
                else optionValue = optionValue + 1;

                // Save the new preset in Player Preferences
                PlayerPrefs.SetInt("Game difficulty", optionValue);

                numericImage.sprite = numericOption[optionValue];

                //Manage the UI display with function "UpdateNumericOptionDisplay"
                //UpdateNumericOptionDisplay ();

                // Call function "UiEventOptionButton" in "MainScript", and send it the option value
                StartCoroutine(mainScript.UiOptionModify(buttonNumber, optionValue));
            }

        }
        /* // DEPRECATED
            // This function manages the button's move effect
            IEnumerator MoveButton (float startPos, float endPos, float time)
            {

                float i = 0;
                float rate = 1.0f/time;

                // Lerp the transform's local position from "startPos" to "endPos" at the given speed "time"
                while (i < 1.0f)
                {
                    i += Time.deltaTime * rate;
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Lerp(startPos, endPos, i));
                    yield return null;
                }

            }
        */
    }

}