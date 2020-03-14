using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using StackGameTemplate.Types;

namespace StackGameTemplate
{
	/// <summary>
	/// This script displays several levels that can be loaded, and are locked and rated based on our PlayerPrefs score
	/// </summary>
	public class STKStageSelector : MonoBehaviour 
	{
        public Text highScoreText;

        [Tooltip("The current level we are on. The first level is 0, the second 1, etc")]
        public int currentStage = 0;

        public string stagePlayerPrefs = "CurrentStage";

        [Tooltip("A list of levels in the game, their names, the scene they link to, the price to unlock it, and the state of the level ( -1 - locked and can't be played, 0 - unlocked no stars, 1-X the star rating we got on the level")]
        public Stage[] stages;

        internal Transform currentIcon;

        public Button buttonNextStage;

        public Button buttonPrevStage;

        public Button buttonStartGame;
        public Text starTextObject;
        internal string startText;

        public Vector3 stageIconPosition;

        public float stageSpinSpeed = 100;
        internal float stageRotation = 0;

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
            buttonStartGame.onClick.AddListener(delegate { StartCoroutine("StartGame"); });

            if (starBar) starBarSize = starBar.sizeDelta;

            // Get all the highscores from all the stages, using PlayerPrefs
            for ( index = 0; index < stages.Length; index++ )
            {
                stages[index].highscore = PlayerPrefs.GetInt(stages[index].sceneName + "HighScore", 0);

                totalStars += Mathf.Clamp((stages[index].highscore / scoreForStar), 0, maximumStars);
            }
            
            currentStage = PlayerPrefs.GetInt(stagePlayerPrefs, currentStage);

            ChangeStage(0);
        }

        void Update()
        {
            if (currentIcon)
            {
                //stageRotation += 
                currentIcon.Rotate( Vector3.up * stageSpinSpeed * Time.deltaTime, Space.World);
            }

        }

        public void ChangeStage( int changeValue )
        {
            // Change the index of the stage
            currentStage += changeValue;

            // Make sure we don't go out of the list of available stages
            if (currentStage > stages.Length - 1) currentStage = 0;
            else if (currentStage < 0) currentStage = stages.Length - 1;

            PlayerPrefs.SetInt(stagePlayerPrefs, currentStage);

            // Remove the icon of the previous stage
            if (currentIcon)
            {
                stageRotation = currentIcon.eulerAngles.y;

                Destroy(currentIcon.gameObject);
            }

            // Display the icon of the current stage
            if ( stages[currentStage].stageIcon )
            {
                //currentIcon = new Transform();

                currentIcon = Instantiate(stages[currentStage].stageIcon, stageIconPosition, Quaternion.identity) as Transform;

                currentIcon.eulerAngles = Vector3.up * stageRotation;

                if (currentIcon.GetComponent<Animation>()) currentIcon.GetComponent<Animation>().Play();
            }



            if (starBar)
            {
                starBar.sizeDelta = new Vector2( (stages[currentStage].highscore / scoreForStar) * (starBarSize.x/maximumStars), starBarSize.y);
                
                // starBarWidth

                //starBar.sizeDelta = new Vector2( stages[currentStage].highscore % scoreForStar, 128);
            }

            if (highScoreText) highScoreText.text = stages[currentStage].highscore.ToString();
            
            if (totalStars >= stages[currentStage].starsToUnlock)
            {
                buttonStartGame.interactable = true;

                if (starTextObject) starTextObject.text = "GO!";
            }
            else
            {
                buttonStartGame.interactable = false;

                if (starTextObject) starTextObject.text = (stages[currentStage].starsToUnlock - totalStars).ToString();
                

            }

        }

        IEnumerator StartGame()
        {
            if (buttonStartGame.GetComponent<Animation>()) buttonStartGame.GetComponent<Animation>().Play();

            yield return new WaitForSeconds(0.5f);

            if ( stages[currentStage].highscore > -1 )
            {
                SceneManager.LoadScene(stages[currentStage].sceneName);
            }

        }


        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawCube(stageIconPosition, new Vector3(1,0.1f,1));


        }

    }
}