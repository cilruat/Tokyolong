using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace OnetapSoccer
{
    public class UserInputManager : MonoBehaviour
    {

        /// <summary>
        /// This class handles all user inputs on UI elements and interactions within the game .
        /// </summary>

        public static bool isPaused;            //global flag for pause event
        public static bool enableUserInput;     //when we pause/unpause, we need a slight delay before we let user to interact with the game
        private float savedTimeScale;           //will be used upon unpause event
        public GameObject pausePlane;           //the plane we use to dim the view

        private GameObject AdManagerObject;     //reference to adManager object

        enum Status
        {
            PLAY, PAUSE
        }
        private Status currentStatus = Status.PLAY;


        //*****************************************************************************
        // Init.
        //*****************************************************************************
        void Awake()
        {

            isPaused = false;
            enableUserInput = false;

            StartCoroutine(enableUserInteraction());

            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f;

            if (pausePlane)
                pausePlane.SetActive(false);

            //		AdManagerObject = GameObject.FindGameObjectWithTag("AdManager");
        }


        //*****************************************************************************
        // FSM
        //*****************************************************************************
        void Update()
        {

            //touch controls
            touchManager();

            //optional pause in Editor & Windows (just for debug)
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyUp(KeyCode.Escape))
            {
                //PAUSE THE GAME
                switch (currentStatus)
                {
                    case Status.PLAY:
                        PauseGame();
                        break;
                    case Status.PAUSE:
                        UnPauseGame();
                        break;
                    default:
                        currentStatus = Status.PLAY;
                        break;
                }
            }

            //debug restart
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }


        //*****************************************************************************
        // This function monitors player touches on menu buttons.
        // detects both touch and clicks and can be used with editor, handheld device and 
        // every other platforms at once.
        //*****************************************************************************
        void touchManager()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hitInfo))
                {
                    string objectHitName = hitInfo.transform.gameObject.name;
                    switch (objectHitName)
                    {
                        case "Button-Pause":
                            switch (currentStatus)
                            {
                                case Status.PLAY:
                                    PauseGame();
                                    break;
                                case Status.PAUSE:
                                    UnPauseGame();
                                    break;
                                default:
                                    currentStatus = Status.PLAY;
                                    break;
                            }
                            break;

                        case "Btn-Resume":
                            switch (currentStatus)
                            {
                                case Status.PLAY:
                                    PauseGame();
                                    break;
                                case Status.PAUSE:
                                    UnPauseGame();
                                    break;
                                default:
                                    currentStatus = Status.PLAY;
                                    break;
                            }
                            break;

                        case "Btn-Restart":
                            UnPauseGame();
                            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                            break;

                        case "Btn-Menu":
                            UnPauseGame();
                            Time.timeScale = 1;
                            SceneManager.LoadScene("ArcadeGame");
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// Pause the game
        /// </summary>
        void PauseGame()
        {

            print("Game is Paused...");

            //show an Interstitial Ad when the game is paused
            if (AdManagerObject)
                AdManagerObject.GetComponent<AdManager>().showInterstitial();

            isPaused = true;
            enableUserInput = false;

            savedTimeScale = Time.timeScale;
            Time.timeScale = 0;
            AudioListener.volume = 0;
            if (pausePlane)
                pausePlane.SetActive(true);
            currentStatus = Status.PAUSE;
        }


        /// <summary>
        /// Unpause the game
        /// </summary>
        void UnPauseGame()
        {

            //print("Unpause");

            isPaused = false;
            StartCoroutine(enableUserInteraction());

            Time.timeScale = savedTimeScale;
            AudioListener.volume = 1.0f;
            if (pausePlane)
                pausePlane.SetActive(false);
            currentStatus = Status.PLAY;
        }


        /// <summary>
        /// We need a slight delay between each player's input or he might be able to do strange things like double shoot.
        /// </summary>
        /// <returns>The user interaction.</returns>
        IEnumerator enableUserInteraction()
        {
            yield return new WaitForSeconds(0.25f);
            enableUserInput = true;
        }

    }
}