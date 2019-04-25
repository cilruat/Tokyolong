using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SpeederRunGame.Types;

namespace SpeederRunGame
{
	/// <summary>
	/// This script controls the game, starting it, following game progress, and finishing it with game over.
	/// </summary>
	public class SRGStageSelector : MonoBehaviour 
	{
        public Text highScoreText;

        [Tooltip("The current level we are on. The first level is 0, the second 1, etc")]
        public int currentStage = 0;

        public string stagePlayerPrefs = "CurrentStage";

        [Tooltip("A list of levels in the game, their names, the scene they link to, the price to unlock it, and the state of the level ( -1 - locked and can't be played, 0 - unlocked no stars, 1-X the star rating we got on the level")]
        public Stage[] stages;

        public Button buttonNextStage;

        public Button buttonPrevStage;

        public Button buttonStartGame;
        public Text starTextObject;
        internal string startText;

        [Tooltip("Get a star for every amount of score in the stage. Example: Get a star at 100 score, and another at 200, and another at 300")]
        public int scoreForStar = 100;

        [Tooltip("The maximum number of stars you can get in any stage. These will be displayed in the stars bar")]
        public int maximumStars = 3;

        // The total number of stars we have, calculated from all the highscores in all the stages
        internal int totalStars;

        [Tooltip("The UI bar that displays the number of stars we have")]
        public RectTransform starBar;
        internal Vector2 starBarSize;

        internal int index;

        //[Tooltip("The/texture of the icon when it is locked ( The black color on locked 3d models )")]
        //public Texture lockedTexture;

        void Start()
        {
            if (starTextObject) starTextObject.text = startText;

            // Listen for a click to change to the next stage
            buttonNextStage.onClick.AddListener(delegate { ChangeStage(1); });

            // Listen for a click to change to the next stage
            buttonPrevStage.onClick.AddListener(delegate { ChangeStage(-1); });

            // Listen for a click to start the game in the current stage
            buttonStartGame.onClick.AddListener(delegate { StartGame(); });

            if (starBar) starBarSize = starBar.sizeDelta;

            // Get all the highscores from all the stages, using PlayerPrefs
            for ( index = 0; index < stages.Length; index++ )
            {
                stages[index].highscore = PlayerPrefs.GetFloat(stages[index].sceneName + "HighScore", 0);

                totalStars += Mathf.Clamp((Mathf.FloorToInt(stages[index].highscore / scoreForStar)), 0, maximumStars);
            }
            
            currentStage = PlayerPrefs.GetInt(stagePlayerPrefs, currentStage);

            ChangeStage(0);
        }
        
        public void ChangeStage( int changeValue )
        {
            // Change the index of the stage
            currentStage += changeValue;

            // Make sure we don't go out of the list of available stages
            if (currentStage > stages.Length - 1) currentStage = 0;
            else if (currentStage < 0) currentStage = stages.Length - 1;

            PlayerPrefs.SetInt(stagePlayerPrefs, currentStage);

            if (starBar)
            {
                int barSize = Mathf.Clamp((Mathf.FloorToInt(stages[currentStage].highscore / scoreForStar)), 0, maximumStars);

                starBar.sizeDelta = new Vector2(barSize * (starBarSize.x/maximumStars), starBarSize.y);
            }

            if (highScoreText) highScoreText.text = stages[currentStage].highscore.ToString();
            
            if (totalStars >= stages[currentStage].starsToUnlock)
            {
                buttonStartGame.interactable = true;

                if (starTextObject) starTextObject.text = stages[currentStage].stageName;
            }
            else
            {
                buttonStartGame.interactable = false;

                if (starTextObject) starTextObject.text = "GET " + (stages[currentStage].starsToUnlock - totalStars).ToString() + " STARS";
            }

        }

        public void StartGame()
        {
            if ( stages[currentStage].highscore > -1 )
            {
                SceneManager.LoadScene(stages[currentStage].sceneName);
            }

        }

        

    }
}