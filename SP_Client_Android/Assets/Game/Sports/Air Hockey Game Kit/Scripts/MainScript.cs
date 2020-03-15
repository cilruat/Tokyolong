
// MAIN SCRIPT 20171215 //

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// "MainScript" controls the main functions of the game.
// It receives and manages the different game and UI events.

// This asset use a single scene for the various game stats (menus, game session, gameover...),
// this avoids loading times and optimise the project, for example if you target low-end mobile devices.

namespace AirHokey
{
    public class MainScript : MonoBehaviour
    {

        // The camera to use
        public Camera cam;

        // Game's current state
        public string gameState = "menu";

        // Specify the game mode to the mallet ("1P" or "2P")
        public string gameMode = "1P";

        // Game options :
		public int scoreToWin;
		public int difficulty;

        // Scores for each player in a game session
        public int scoreP1;
        public int scoreP2;

        // Level Up System values
        public int[] levelling; // Populate in editor with experience values to reach
        public int level = 1;
        public int experience = 0;

        // delete all player preferences at start ? (for debugging purposes - should be set to false before compilation)
        public bool debugDeleteAllPlayerPrefs;

        // References to players (mallets) and their control scripts
        // player 1 = mallet 1, player 2 = mallet 2
        // Each 'player' can be a user or a CPU
        public GameObject player1; // mallet 1
        public MalletCpuScript p1CpuScript;
        public MalletPlayerScript p1PlayerScript;
        public GameObject player2; // mallet 2
        public MalletCpuScript p2CpuScript;
        public MalletPlayerScript p2PlayerScript;

        public GameObject puck;

        public PuckScript puckScript;

        // UI elements
        private GameObject ui;
        private GameObject uiStartScreen;
        private GameObject uiScore;
        private GameObject uiInGame;
        private GameObject uiPause;
        private GameObject uiGameOver;
        public GameObject[] uiGameOverInfos;
        private UiScoreScript uiScoreScript;

        // Used to store camera view value when switching view
        private int cameraViewPrevious;

        // Is the camera actually doing a transition ?
        private bool camTransition = false;

        // Does the camera transition use lerp ?
        private bool camLerp = false;

        // Camera transition values - End positions and euler angles for camera transition lerp
        private Vector3 camPosEnd;
        private Vector3 camEulerAnglesEnd;

        // Used for UI interaction on touch screen device
        public Ray mousePosRay;
        public RaycastHit hit;
        // LayerMask is used in the rayCast, to raycast for layer 14, "UI Layer". raycast is used to determine buttons selection on touch screen devices
        public LayerMask uiLayerMask = 1 << 14;

        // "gameplayPreset" stores the gameplay type performed
        // There are three example presets : "arcade", "balanced", or "simulation"
        // See the function "ChangeGameplayPreset()" for more informations
        private string gameplayPreset = "balanced"; // "arcade", "balanced", or "simulation"

        // Physic materials are used when switching gameplay presets
        public PhysicMaterial[] puckPhysicMat;
        public PhysicMaterial[] malletPhysicMat;


        // GUI vars (OnGUI)
        // Show debug GUI ? (display debug option in Unity native GUI)
        public bool showDebugGUI = true;
        // Reference to mallet player Scripts "smoothedMoves" value
        private bool toggleUserSmoothedMove = true;
        // Reference to mallet player Scripts "targetSpeed" value
        private int userTargetSpeedValue;
        private float userTargetSpeedValueFloat;
        //private string userTargetSpeedValueText;
        // Used to display the debug options
        public Rect debugOptionWindowRect = new Rect(1, 1, 200, 150);

        // Game pause variables
        // Is the game paused ?
        public bool gamePause = false;
        // Is the game allowed to pause ?
        private bool canPause = true;

        void Awake()
        {

            // Set a unique framerate over different devices
            Application.targetFrameRate = 60;

        }

        IEnumerator Start()
        {

            // The only PhysicsManager property that is modified, to get accuracy for real scale air hockey board - (Bounce Threshold default value is 2)
            Physics.bounceThreshold = 0.2f;

            // Uncomment this line if you need a reminder
            //Debug.Log("Physics bounceThreshold set to 0.2", gameObject);

            // Access the CPU and Player scripts for each mallet

            // Get components "MalletCpuScript" from each player and assign them to variables "p1CpuScript" and "p2CpuScript"
            if (p1CpuScript == null) p1CpuScript = player1.GetComponent("MalletCpuScript") as MalletCpuScript;
            if (p2CpuScript == null) p2CpuScript = player2.GetComponent("MalletCpuScript") as MalletCpuScript;
            // Get components "MalletPlayerScript" from each player and assign them to variables "p1PlayerScript" and "p2PlayerScript"
            if (p1PlayerScript == null) p1PlayerScript = player1.GetComponent("MalletPlayerScript") as MalletPlayerScript;
            if (p2PlayerScript == null) p2PlayerScript = player2.GetComponent("MalletPlayerScript") as MalletPlayerScript;

            // Now get the puck's script
            if (puckScript == null) puckScript = puck.GetComponent("PuckScriptCS") as PuckScript;

            // Find User Interface components wich are located inside the camera, and assign them to variables
            ui = GameObject.Find("UI");
            uiStartScreen = GameObject.Find("UI_StartScreen");
            uiScore = GameObject.Find("UI_Score");
            uiInGame = GameObject.Find("UI_InGame");
            uiPause = GameObject.Find("UI_Pause");
            uiGameOver = GameObject.Find("UI_GameOver");

            // Get component "UiScoreScript" from "UI_Score" gameObject - "UiScoreScript" displays the score during game sessions 
            uiScoreScript = uiScore.GetComponent("UiScoreScript") as UiScoreScript;

            /* // DEPRECATED
            // Check screen size and reposition the UI's Z local position so we have a coherent UI size
            float pixelSize = Screen.width * Screen.height;
            //float uiPosZ;
            if (pixelSize <= 153600) ui.transform.localPosition = new Vector3(ui.transform.localPosition.x, ui.transform.localPosition.y, 0.2f); // <= 480*320
            else if (pixelSize <= 384000) ui.transform.localPosition = new Vector3(ui.transform.localPosition.x, ui.transform.localPosition.y, 0.23f); // <= 800*480
            else if (pixelSize <= 614400) ui.transform.localPosition = new Vector3(ui.transform.localPosition.x, ui.transform.localPosition.y, 0.27f); // <= 1024*600
            else ui.transform.localPosition = new Vector3(ui.transform.localPosition.x, ui.transform.localPosition.y, 0.35f); // > 1024*600
            */

            // Setup "userTargetSpeedValue" value for the debug "OnGUI()" function
            userTargetSpeedValue = p1PlayerScript.targetSpeed;
            userTargetSpeedValueFloat = (float)userTargetSpeedValue;

            // Set the game play to the preset notified in the value "gameplayPreset"
            yield return StartCoroutine(SetGameplayPreset());


            // delete all player preferences ? (for debugging purposes - should be set to false before compilation)
            if (debugDeleteAllPlayerPrefs == true) PlayerPrefs.DeleteAll();

            // Load player preferences for Level Up System
            if (PlayerPrefs.HasKey("Player experience")) experience = PlayerPrefs.GetInt("Player experience");
            if (PlayerPrefs.HasKey("Player level")) level = PlayerPrefs.GetInt("Player level");

            // Set camPosEnd and camEulerAnglesEnd to default values
            camPosEnd = cam.transform.localPosition;
            cam.transform.localRotation = cam.transform.parent.rotation;
            camEulerAnglesEnd = cam.transform.localEulerAngles;

            // Launch menu state
            yield return StartCoroutine(InitMenu());

        }

        // "GamePauseEnable()" and "GamePauseResume()" are called by "Update()" when hitting pause key and by "UiEventButton()" (called itself by the pause button's script)

        IEnumerator GamePauseEnable()
        {

            gamePause = true;

            yield return null;

            // Activate pause UI elements
            uiPause.SetActive(true);

            GameObject[] objects = FindObjectsOfType(typeof(GameObject)) as GameObject[];

            foreach (GameObject go in objects)
            {
                go.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
            }

        }

        IEnumerator GamePauseResume()
        {

            gamePause = false;

            yield return null;

            //UnPause Objects
            GameObject[] objects = FindObjectsOfType(typeof(GameObject)) as GameObject[];

            foreach (GameObject go in objects)
            {
                go.SendMessage("OnResumeGame", SendMessageOptions.DontRequireReceiver);
            }

            // Deactivate pause UI elements
            uiPause.SetActive(false);

        }

        void Update()
        {

            // Exit application on key input if we are in menu
            if (Input.GetKeyDown("escape") && gameState == "menu") Application.Quit();		
            else

            // Game pause on key input
            if (canPause == true && gameState == "game" && (Input.GetKeyDown("p") || Input.GetKeyDown("escape")))
            {
                if (gamePause == false) StartCoroutine(GamePauseEnable());
                else StartCoroutine(GamePauseResume());
            }
            // If user is using a touch screen device, then detect UI buttons with a raycast
            if (gameState == "menu")
            {

                if (Input.touchCount == 1)
                {
                    // Create a ray going from camera through a screen point
                    mousePosRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                    // Draw a mousePosRay reference in the scene view for debugging purpose
                    Vector3 forward = mousePosRay.direction * 10;
                    Debug.DrawRay(mousePosRay.origin, forward, Color.blue);

                    // Check if the ray "mousePosRay" intersects "playingSurface" collider, located in layer 14 "UI Layer" (or any other collider in this layer !)
                    if (Physics.Raycast(mousePosRay, out hit, 10.0f, uiLayerMask))
                    {
                        if (Input.touches[0].phase == TouchPhase.Ended) hit.transform.SendMessage("OnMouseUp");
                    }
                }
            }

            if (cam.transform.localPosition == camPosEnd) camTransition = false;

            if (camTransition == true)
            {
                if (camLerp == true)
                {
                    cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camPosEnd, 3 * Time.deltaTime);
                    cam.transform.localEulerAngles = Vector3.Lerp(cam.transform.localEulerAngles, camEulerAnglesEnd, 5 * Time.deltaTime);
                }

                else // Else if no camera lerp
                {
                    cam.transform.localPosition = camPosEnd;
                    cam.transform.localEulerAngles = camEulerAnglesEnd;
                }
            }

        }

        // Change camera position/rotation
        void SetCameraView(int cameraView, bool lerp) // "cameraView" : "0" = menu, "1" = 1P camera, "2" = 2P camera
        {

            if (cameraView == 0) // Set menu view
            {
                // Set the new (local) position
                camPosEnd = cam.transform.localPosition = new Vector3(0.7f, 0.6f, -0.8f);
                // Align the camera rotation with the parent
                cam.transform.localRotation = cam.transform.parent.rotation;
                // Rotate the camera locally to the desired angle
                camEulerAnglesEnd = cam.transform.localEulerAngles = new Vector3(45, 305, 0);
                //cam.fieldOfView = 60; // Set FOV		
            }

            else

            if (cameraView == 1) // Set 1 player mode perspective view
            {
                // Set the new (local) position
                camPosEnd = new Vector3(0, 0.33f, -1.6f);
                // Align the camera rotation with the parent
                cam.transform.localRotation = cam.transform.parent.rotation;
                // Rotate the camera locally to the desired angle
                camEulerAnglesEnd = new Vector3(10, 0, 0);
                //cam.fieldOfView = 60; // Set FOV
            }

            else

            if (cameraView == 2) // Set 1 player mode perspective view
            {
                // Set the new (local) position
                camPosEnd = new Vector3(0, 0.5f, -1.5f);
                // Align the camera rotation with the parent
                cam.transform.localRotation = cam.transform.parent.rotation;
                // Rotate the camera locally to the desired angle
                camEulerAnglesEnd = new Vector3(30, 0, 0);
                //cam.fieldOfView = 60; // Set FOV
            }

            else

            if (cameraView == 3) // Set 1 player mode perspective view
            {
                // Set the new (local) position
                camPosEnd = new Vector3(0, 1.1f, -1.5f);
                // Align the camera rotation with the parent
                cam.transform.localRotation = cam.transform.parent.rotation;
                // Rotate the camera locally to the desired angle
                camEulerAnglesEnd = new Vector3(45, 0, 0);
                //cam.fieldOfView = 60; // Set FOV
            }

            else

            if (cameraView == 4) // Set 2 players mode top-down view
            {
                // Set the new (local) position
                camPosEnd = cam.transform.localPosition = new Vector3(0, 1.4f, 0);
                // Align the camera rotation with the parent
                cam.transform.localRotation = cam.transform.parent.rotation;
                // Rotate the camera locally to the desired angle
                camEulerAnglesEnd = cam.transform.localEulerAngles = new Vector3(90, -90, 0);
                //cam.fieldOfView = 60; // Set FOV
            }

            cameraViewPrevious = cameraView;

            camTransition = true; camLerp = lerp;

        }

        // Called by UI buttons
        public void UiEventButton(int buttonNumber)
        {

            // Uncomment this line if you need to test the UI
            // Debug.Log("UiEventButton called by button number "+buttonNumber, gameObject);

            // If button is '1 Player' start button
            if (buttonNumber == 1)
            {
                gameMode = "1P";
                StartCoroutine(InitGame(gameMode));
            }

            else

            // If button is '2 Players' start button
            if (buttonNumber == 2)
            {
                gameMode = "2P";
                StartCoroutine(InitGame(gameMode));
            }

            // (Buttons 3 'Score to win' and 4 "Difficulty" are handled by their respective UiButtonScript instead of here)

            else

            // If button is 'CAMERA' in-game button
            if (buttonNumber == 5)
            {
                if (cameraViewPrevious == 1)
                {
                    // Save the new preset in Player Preferences
                    PlayerPrefs.SetInt("Game cameraView", 2);
                    SetCameraView(2, true); // Call camera view 2 with lerp transition
                }

                else
                if (cameraViewPrevious == 2)
                {
                    // Save the new preset in Player Preferences
                    PlayerPrefs.SetInt("Game cameraView", 3);
                    SetCameraView(3, true); // Call camera view 3 with lerp transition
                }

                else
                if (cameraViewPrevious == 3)
                {
                    // Save the new preset in Player Preferences - Only if we are not in "2P" gameMode
                    if (gameMode != "2P") PlayerPrefs.SetInt("Game cameraView", 4);
                    SetCameraView(4, false); // Call camera view 4 without lerp transition
                }

                else
                if (cameraViewPrevious == 4)
                {
                    // Save the new preset in Player Preferences
                    PlayerPrefs.SetInt("Game cameraView", 1);
                    SetCameraView(1, false); // Call camera view 1 without lerp transition
                }

            }

            else

            // If button is 'PAUSE' in-game button
            if (buttonNumber == 6)
            {
                if (gamePause == false) StartCoroutine(GamePauseEnable());
                else StartCoroutine(GamePauseResume());
            }

            else

            // If button is 'RESUME' pause button
            if (buttonNumber == 7)
            {
                StartCoroutine(GamePauseResume());
            }

            else

            // If button is 'MENU' pause button
            if (buttonNumber == 8)
            {
                StartCoroutine(InitMenu());
            }

        }


		public void ExecuteLoadLevel()
		{
			#if UNITY_5_3 || UNITY_5_3_OR_NEWER
			SceneManager.LoadScene("ArcadeGame");
			#else
			Application.LoadLevel(levelName);
			#endif
		}
        // Called by UI buttons
        public IEnumerator UiOptionModify(int buttonNumber, int optionValue)
        {

            // Uncomment this line if you need to test the UI
            //Debug.Log("UiOptionModify called by button number "+buttonNumber+", received value "+optionValue, gameObject);

            // If button is "Score to win" button
            if (buttonNumber == 3) scoreToWin = 3;

            else

            // If button is "Difficulty" button
            if (buttonNumber == 4)
            {
                difficulty = optionValue;

                yield return true;

                p1CpuScript.speed = p2CpuScript.speed = 1.25f + (float)difficulty; // Quick example of difficulty tweaking
            }
        }

        IEnumerator InitMenu()
        {

            // Change the game's state
            gameState = "menu";

            // Turn off the volume
            AudioListener.volume = 0;

            // Disable mallets script
            p1PlayerScript.enabled = false;
            p2PlayerScript.enabled = false;

            // Enable CPU's script for both mallets (= rolling demo !)
            p1CpuScript.enabled = true;
            p2CpuScript.enabled = true;


            puck.SetActive(true);

            // Enable puck script
            puckScript.enabled = true;

            // Reset scores and score UI
            scoreP1 = scoreP2 = 0;
            uiScoreScript.ResetUIScore();

            // Deactivate the possibly showing gameOver UI
            for (int i = 0; i < uiGameOverInfos.Length; i++) uiGameOverInfos[i].SetActive(false);

            // Deactivate in game UI elements
            uiInGame.SetActive(true);


            // Disallow the game to pause
            canPause = false;
            // Deactivate pause UI elements
            uiPause.SetActive(false);

            yield return null;

            // Resume pause for all scene entities
            StartCoroutine(GamePauseResume());

            yield return null;

            // Set camera menu placement
            SetCameraView(0, false); // Menu view

            p1CpuScript.StartCoroutine("Place");
            p2CpuScript.StartCoroutine("Place");
            puckScript.StartCoroutine("Place", Random.Range(1, 3)); // Place puck at random side

            // Activate Start Screen's UI elements
            uiStartScreen.SetActive(true);

            // Deactivate score UI
            uiScore.SetActive(false);

        }

        IEnumerator InitGame(string gameMode) //game mode is "1P" for 1 player game or "2P" for 2 players touch screen game.
        {

            // Change the game's state
            gameState = "game";

            // Allow the game to pause
            canPause = true;

            // Deactivate Start Screen's UI elements
            uiStartScreen.SetActive(false);

            // Activate score UI
            uiScore.SetActive(true);

            // Stop the possibly active coroutines
            p1CpuScript.StopCoroutine("Place");
            p2CpuScript.StopCoroutine("Place");
            p1CpuScript.StopAllCoroutines();
            p2CpuScript.StopAllCoroutines();

            p1CpuScript.enabled = false;
            p2CpuScript.enabled = false;


            if (gameMode == "1P")
            {

                // Activate in game UI elements - Only in 1 player game mode (pause is still accessible by key press !)
                uiInGame.SetActive(true);


                // Place the camera to a position to be translated from
                cam.transform.localPosition = new Vector3(0, 1.5f, -3);
                cam.transform.localRotation = cam.transform.parent.rotation;
                camEulerAnglesEnd = new Vector3(10, 0, 0);

                // If cameraView has been already stored in Player Preference, set camera accordingly
                if (PlayerPrefs.HasKey("Game cameraView")) SetCameraView(PlayerPrefs.GetInt("Game cameraView"), true); // Set the camera position and rotation
                else SetCameraView(2, true); // Set the camera position and rotation

                p1PlayerScript.enabled = true;
                p2CpuScript.enabled = true;
                puckScript.enabled = true;

                // Notify player script of the actual game mode
                p1PlayerScript.gameMode = gameMode;

                yield return null;

                p1PlayerScript.StartCoroutine("Place");
                p2CpuScript.StartCoroutine("Place");
            }

            else

            if (gameMode == "2P")
            {
                SetCameraView(4, false); // Set top-down view
				uiInGame.SetActive(true);

                p1PlayerScript.enabled = true;
                p2PlayerScript.enabled = true;
                puckScript.enabled = true;

                // Notify player scripts of the actual game mode
                p1PlayerScript.gameMode = p2PlayerScript.gameMode = gameMode;

                yield return true;

                p1PlayerScript.StartCoroutine("Place");
                p2PlayerScript.StartCoroutine("Place");
            }

            puck.SetActive(true);
            puckScript.StartCoroutine("Place", 0); // place the puck at the center of the playing surface
            AudioListener.volume = 1; // Turn on the volume

        }


        // "SetGameplayPreset()" changes the type of gameplay, from arcade to realistic
        // This function is called by GUI button (located in "OnGUI" function)
        IEnumerator SetGameplayPreset()
        {

            if (gameplayPreset == "arcade")
            {
                // Setup arcade preset
                puckScript.maxVelocity = 3;
                puckScript.SetMaxVelocity(puckScript.maxVelocity);
                puck.GetComponent<Rigidbody>().mass = 0.01f;
                puck.GetComponent<Rigidbody>().drag = 0;
                puck.GetComponent<Rigidbody>().angularDrag = 0.5f;

                // Change the physic materials of the mallets and puck
                puck.GetComponent<Collider>().material = puckPhysicMat[0];
                player1.GetComponent<Collider>().material = player2.GetComponent<Collider>().material = malletPhysicMat[0];

                // Enable player scripts "smoothedMoves" and set "targetSpeed" value
                // ("toggleUserSmoothedMove" and "userTargetSpeedValue" are their references in debug gui window)
                p1PlayerScript.smoothedMoves = p2PlayerScript.smoothedMoves = toggleUserSmoothedMove = true;
                p1PlayerScript.targetSpeed = p2PlayerScript.targetSpeed = userTargetSpeedValue = 15;
                userTargetSpeedValueFloat = (float)userTargetSpeedValue;

                yield return 0;
            }

            else

            if (gameplayPreset == "balanced")
            {
                // Setup balanced preset
                puckScript.maxVelocity = 4.15f;
                puckScript.SetMaxVelocity(puckScript.maxVelocity);
                puck.GetComponent<Rigidbody>().mass = 0.03f;
                puck.GetComponent<Rigidbody>().drag = 0.1f;
                puck.GetComponent<Rigidbody>().angularDrag = 0.5f;

                // Change the physic materials of the mallets and puck
                puck.GetComponent<Collider>().material = puckPhysicMat[1];
                player1.GetComponent<Collider>().material = player2.GetComponent<Collider>().material = malletPhysicMat[1];

                // Enable player scripts "smoothedMoves" and set "targetSpeed" value
                p1PlayerScript.smoothedMoves = p2PlayerScript.smoothedMoves = toggleUserSmoothedMove = true;
                p1PlayerScript.targetSpeed = p2PlayerScript.targetSpeed = userTargetSpeedValue = 25;
                userTargetSpeedValueFloat = (float)userTargetSpeedValue;

                yield return 0;
            }

            else

            if (gameplayPreset == "simulation")
            {
                // Setup simulation preset
                puckScript.maxVelocity = 12;
                puckScript.SetMaxVelocity(puckScript.maxVelocity);
                puck.GetComponent<Rigidbody>().mass = 0.06f;
                puck.GetComponent<Rigidbody>().drag = puck.GetComponent<Rigidbody>().angularDrag = 0.5f;

                // Change the physic materials of the mallets and puck
                puck.GetComponent<Collider>().material = puckPhysicMat[2];
                player1.GetComponent<Collider>().material = player2.GetComponent<Collider>().material = malletPhysicMat[2];

                // Enable player scripts "smoothedMoves" and set "targetSpeed" value
                p1PlayerScript.smoothedMoves = p2PlayerScript.smoothedMoves = toggleUserSmoothedMove = true;
                p1PlayerScript.targetSpeed = p2PlayerScript.targetSpeed = userTargetSpeedValue = 35;
                userTargetSpeedValueFloat = (float)userTargetSpeedValue;

                yield return null;
            }

        }


        public IEnumerator UpdateScore(int goalNumber)
        {

            while (gamePause == true)
            {
                yield return null;
            }

            // If we are not in game
            if (gameState == "menu")
            {
                StartCoroutine(SetGameService(goalNumber));
            }

            else

            if (goalNumber == 1)
            {
                // Update player 2 score
                scoreP2++;

                // Update the score UI ("UI_Score")
                uiScoreScript.UpdateUIScore(2, scoreP2);

                // Did we reach the score to win ?
                if (scoreP2 == scoreToWin) StartCoroutine(GameOver(2));

                // Else launch the new service, player 1 has the service
                else StartCoroutine(SetGameService(1));
            }

            else // If "goalNumber" = 2

            {
                // Update player 1 score
                scoreP1++;

                // Update the score UI ("UI_Score")
                uiScoreScript.UpdateUIScore(1, scoreP1);

                // Did we reach the score to win ?
                if (scoreP1 == scoreToWin)
                {
                    // Update experience points (10 points for winning)
                    experience = experience + 10;

                    // Save "experience" value to player preferences
                    PlayerPrefs.SetInt("Player experience", experience);

                    print("experience = " + experience);

                    // Then check for level up
                    LevellingCheck();

                    StartCoroutine(GameOver(1));
                }

                // Else launch the new service, player 2 has the service
                else
                {
                    // Update experience points (5 points for scoring)
                    experience = experience + 5;

                    // Save "experience" value to player preferences
                    PlayerPrefs.SetInt("Player experience", experience);

                    print("experience = " + experience);

                    // Then check for level up
                    LevellingCheck();

                    StartCoroutine(SetGameService(2));
                }
            }

        }

        // Place the mallets and puck
        IEnumerator SetGameService(int serviceSide)
        {

            while (gamePause == true)
            {
                yield return null;
            }

            if (gameState == "menu")
            {
                p1CpuScript.StartCoroutine("Place");
                p2CpuScript.StartCoroutine("Place");
            }

            else if (gameMode == "1P")
            {
                p1PlayerScript.StartCoroutine("Place");
                p2CpuScript.StartCoroutine("Place");
            }

            else if (gameMode == "2P")
            {
                p1PlayerScript.StartCoroutine("Place");
                p2PlayerScript.StartCoroutine("Place");
            }

            // Place the puck according to the service side
            puckScript.StartCoroutine("Place", serviceSide);

            yield return null;

        }

        IEnumerator GameOver(int winner)
        {

            while (gamePause == true)
            {
                yield return null;
            }

            gameState = "gameOver";
            Debug.Log("Player " + winner + " WIN ! (game mode = " + gameMode + ")");

            puck.SetActive(false);

            // If game mode = 1 player, and player win
            if (gameMode == "1P" && winner == 1)
            {
                // Display UI message : "YOU WIN !"
                uiGameOverInfos[0].SetActive(true);
                p1PlayerScript.canMove = false;
                p2CpuScript.canMove = false;
            }

            else

            // If game mode = 1 player, and player loose
            if (gameMode == "1P" && winner == 2)
            {
                // Display UI message : "YOU LOOSE !"
                uiGameOverInfos[1].SetActive(true);
                p1PlayerScript.canMove = false;
                p2CpuScript.canMove = false;
            }

            else

            // If game mode = 2 players, and player 1 win
            if (gameMode == "2P" && winner == 1)
            {
                // Display UI message : "P1 WIN !"
                uiGameOverInfos[2].SetActive(true);
                p1PlayerScript.canMove = false;
                p2PlayerScript.canMove = false;
            }

            else

            // If game mode = 2 players, and player 2 win
            if (gameMode == "2P" && winner == 2)
            {
                // Display UI message : "P2 WIN !"
                uiGameOverInfos[3].SetActive(true);
                p1PlayerScript.canMove = false;
                p2PlayerScript.canMove = false;
            }

            // Play the rotating message animation
            uiGameOver.GetComponent<Animation>().Play();

            p1PlayerScript.enabled = false;
            p2PlayerScript.enabled = false;
            p1CpuScript.enabled = false;
            p2CpuScript.enabled = false;

            yield return new WaitForSeconds(3);
            StartCoroutine(InitMenu());

        }

        // Check for level up
        void LevellingCheck()
        {
            bool levelUp = false;

            // loop through "levelling" array
            for (int i = 0; i < levelling.Length; i++)
            {
                // Do we reach a new level ?
                if (i + 1 >= level && levelling[i] <= experience)
                {
                    levelUp = true;
                    // increment "level" value
                    level = level + 1;
                }
            }

            // Save "level" value to player preferences
            PlayerPrefs.SetInt("Player level", level);

            if (levelUp == true) print("Level up ! You reached level " + level);

        }

        // All the following code is optional and used to show the GUI ////////////////////////////////////////////

        void OnGUI()
        {

            if (showDebugGUI == true)
                // Display the debug options window
                debugOptionWindowRect = GUI.Window(0, debugOptionWindowRect, GuiDebugWindowFunction, "Debug Options");

        }

        // This function is called in "OnGUI()" to display the debug options
        void GuiDebugWindowFunction(int windowID)
        {

            // Create the button for gameplay preset selection
            if (GUI.Button(new Rect(40, 30, 120, 50), "Gameplay : \n" + gameplayPreset))
            {
                // Switch the variable "gameplayPreset" when button is clicked
                if (gameplayPreset == "simulation") { gameplayPreset = "arcade"; }
                else
                if (gameplayPreset == "arcade") { gameplayPreset = "balanced"; }
                else
                if (gameplayPreset == "balanced") { gameplayPreset = "simulation"; }

                // Call the function that changes the preset
                StartCoroutine(SetGameplayPreset());
            }

            toggleUserSmoothedMove = GUI.Toggle(new Rect(22, 80, 200, 30), toggleUserSmoothedMove, "User smoothed moves");
            p1PlayerScript.smoothedMoves = p2PlayerScript.smoothedMoves = toggleUserSmoothedMove;

            // if user smoothed moves is enabled, display a slider to control player scripts' "targetSpeed" value
            if (toggleUserSmoothedMove == true)
            {
                // display "Target speed :" label
                GUI.Label(new Rect(25, 100, 150, 30), "Target speed :");

                // display the horizontal slider
                //userTargetSpeedValueFloat = (float)userTargetSpeedValue;
                userTargetSpeedValueFloat = GUI.HorizontalSlider(new Rect(15, 120, 150, 30), userTargetSpeedValueFloat, 1.0f, 100.0f);
                p1PlayerScript.targetSpeed = p2PlayerScript.targetSpeed = (int)userTargetSpeedValueFloat;

                // display "userTargetSpeedValue" in a text label next to the slider
                //userTargetSpeedValueText = ""+userTargetSpeedValue;
                GUI.Label(new Rect(165, 115, 150, 30), "" + (int)userTargetSpeedValueFloat);

                // Expand the window height
                debugOptionWindowRect = new Rect(1, 1, 200, 150);

            }

            else
            {
                // Shorten the window height
                debugOptionWindowRect = new Rect(1, 1, 200, 110);
            }

        }
    }
}