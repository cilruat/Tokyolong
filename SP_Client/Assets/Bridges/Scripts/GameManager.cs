using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using SgLib;

#if EASY_MOBILE
using EasyMobile;
#endif

namespace Bridges
{
	public enum GameEvent
	{
	    Start,
	    Paused,
	    PreGameOver,
	    Resumed,
	    GameOver
	}

	public class GameManager : MonoBehaviour
	{

	    public static event System.Action<GameEvent> NewGameEvent = delegate { };

	    public static int gameCountForRewardedAds = 0;

	    [Header("Gameplay Config")]    
	    public int initialPath;
	    public int minPathNumber;
	    public int maxPathNumber;
	    public float rotateBridgeTime;
	    [Range(0f, 1f)]
	    public float goldFrequency;
	    [Header("\"Continue Lost Game\" Feature Config")]
	    public int coinsToRecover = 30;

	    [Header("Object References")]
	    public UIManager uIManager;
	    public PlayerController playerController;
	    public GameObject turnLeftPathManager;
	    public GameObject turnRightPathManager;
	    public GameObject goldPrefab;
	    public GameObject firstPath;
	    public GameObject[] pathArray;
	    public GameObject[] turnLeftPathArray;
	    public GameObject[] turnRightPathArray;
	    [HideInInspector]
	    public bool gameOver;

	    private List<GameObject> listPath = new List<GameObject>();
	    private List<Vector3> listGoldPosition = new List<Vector3>();
	    private Vector3 planeSize;
	    private Vector3 nextPathPosition;
	    private Vector3 fixedPosition;
	    private Vector3 pathCreationDirection;
	    private Vector3 pathCreationRotation;
	    private int turnPathNumberPooled;
	    private int goldNumberPooled;
	    private int pathNumber;
	    private int pathCounter = 0;
	    private int turn = 1;
	    private bool lostGameRecovered;

	    void OnEnable()
	    {
	        PlayerController.PlayerFall += PlayerController_PlayerFall;

	    }

	    void OnDisable()
	    {
	        PlayerController.PlayerFall -= PlayerController_PlayerFall;

	    }

	    // Use this for initialization
	    void Start()
	    {
	        ScoreManager.Instance.Reset();

	        listPath.Add(firstPath);
	        turnPathNumberPooled = initialPath; //Max number of turn path
	        goldNumberPooled = initialPath * 3; //Max number of gold

	        planeSize = LastPlaneOfPath(firstPath).GetComponent<Renderer>().bounds.size; //Get size of plane
	        pathNumber = Random.Range(minPathNumber, maxPathNumber); //Random path number


	        fixedPosition = new Vector3(LastPlaneOfPath(firstPath).transform.position.x,
	            firstPath.transform.position.y,
	            LastPlaneOfPath(firstPath).transform.position.z);

	        pathCreationDirection = Vector3.right; //Directon of path creation
	        pathCreationRotation = new Vector3(0, 0, 0); //Rotation of path creation

	        nextPathPosition = fixedPosition + pathCreationDirection * planeSize.x;

	        CRPrepareGame();
	        StartCoroutine(CheckAndRefeshPath());
			SoundManager.Instance.PlayMusic(SoundManager.Instance.background);
	    }

	    void Update()
	    {
	        // Exit on Android Back button
	        #if UNITY_ANDROID && EASY_MOBILE
	        if (Input.GetKeyUp(KeyCode.Escape))
	        {   

	            NativeUI.AlertPopup alert = NativeUI.ShowTwoButtonAlert(
	                                          "Exit Game",
	                                          "Are you sure you want to exit?",
	                                          "Yes", 
	                                          "No");

	            if (alert != null)
	            {
	                alert.OnComplete += (int button) =>
	                {
	                    switch (button)
	                    {
	                        case 0: // Yes
	                            Application.Quit();
	                            break;
	                        case 1: // No
	                            break;
	                    }
	                };
	            }     
	        }
	        #endif
	    }

	    public void StartGame()
	    {
	        NewGameEvent(GameEvent.Start);	        
	    }

	    void PlayerController_PlayerFall()
	    {
	        SoundManager.Instance.StopMusic();

	        bool haveEnoughCoins = false;
	        bool canWatchAd = false;

	        // Need to have enough coins
	        haveEnoughCoins = CoinManager.Instance.Coins >= coinsToRecover;

	        // Or ad is ready to watch
	        #if EASY_MOBILE
	        if (PremiumFeaturesManager.Instance.enablePremiumFeatures)
	        {
	            canWatchAd = AdDisplayer.Instance.CanShowRewardedAd();
	        }   
	        #endif

	        // Must not recover before and score must have some score already
	        /*bool canRecoverGame = (haveEnoughCoins || canWatchAd) && !lostGameRecovered && (ScoreManager.Instance.Score > 0);

	        if (canRecoverGame)
	        {
	            PreGameOver();
	            StartCoroutine(ShowPreGameOverUI(1f, haveEnoughCoins, canWatchAd));
	        }
	        else*/
	        {
	            GameOver();
	        }
	    }

	    void PreGameOver()
	    {
	        NewGameEvent(GameEvent.PreGameOver);
	    }

	    IEnumerator ShowPreGameOverUI(float delay, bool canUseCoins, bool canWatchAd)
	    {
	        yield return new WaitForSeconds(delay);
	        uIManager.ShowContinueLostGameUI(canUseCoins, canWatchAd);
	    }

	    public void RecoverLostGame(bool useCoins)
	    {
	        if (useCoins)
	        {
	            CoinManager.Instance.RemoveCoins(coinsToRecover);
	        }

	        playerController.BackToLastPlane();
	        lostGameRecovered = true;
	    }

	    void CRPrepareGame()
	    {
	    
	        //Instantitate golds
	        for (int i = 0; i < goldNumberPooled; i++)
	        {
	            GameObject gold = Instantiate(goldPrefab);
	            gold.SetActive(false);
	            gold.transform.parent = CoinManager.Instance.transform;
	        }

	        int turnLeftPathIndex = -1;
	        //Instantitate turn left paths
	        for (int i = 0; i < turnPathNumberPooled; i++)
	        {
	            turnLeftPathIndex = (turnLeftPathIndex == turnLeftPathArray.Length - 1) ? (0) : (turnLeftPathIndex + 1);
	            GameObject turnLeftPath = Instantiate(turnLeftPathArray[turnLeftPathIndex]);
	            turnLeftPath.SetActive(false);
	            turnLeftPath.transform.parent = turnLeftPathManager.transform;
	        }

	        int turnRightPathIndex = -1;
	        //Instantitate turn right paths
	        for (int i = 0; i < turnPathNumberPooled; i++)
	        {
	            turnRightPathIndex = (turnRightPathIndex == turnRightPathArray.Length - 1) ? (0) : (turnRightPathIndex + 1);
	            GameObject turnRightPath = Instantiate(turnRightPathArray[turnRightPathIndex]);
	            turnRightPath.SetActive(false);
	            turnRightPath.transform.parent = turnRightPathManager.transform;
	        }

	        //Create path
	        for (int i = 0; i < initialPath; i++)
	        {
	            CreatePath();
	        }
	    }

	    public void GameOver()
	    {
	        //Fire game event
	        NewGameEvent(GameEvent.GameOver);
	    }

	    public void RestartGame(float delay)
	    {
	        StartCoroutine(CRRestart(delay));
	    }

	    IEnumerator CRRestart(float delay = 0)
	    {
	        yield return new WaitForSeconds(delay);
	        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	    }

	    //Get rangdom turn left path
	    GameObject GetTurnLeftPath()
	    {
	        GameObject turnPath = turnLeftPathManager.transform.GetChild(Random.Range(0, turnLeftPathManager.transform.childCount)).gameObject;
	        turnPath.transform.parent = null;
	        turnPath.SetActive(true);
	        return turnPath;
	    }

	    //Get rangdom turn right path
	    GameObject GetTurnRightPath()
	    {
	        GameObject turnPath = turnRightPathManager.transform.GetChild(Random.Range(0, turnRightPathManager.transform.childCount)).gameObject;
	        turnPath.transform.parent = null;
	        turnPath.SetActive(true);
	        return turnPath;
	    }

	    //Get gold
	    GameObject GetGold()
	    {
	        GameObject gold = CoinManager.Instance.transform.GetChild(0).gameObject;
	        gold.transform.parent = null;
	        gold.SetActive(true);
	        return gold;
	    }

	    //Find the last plane of the given path
	    GameObject LastPlaneOfPath(GameObject path)
	    {
	        GameObject lastPlane = null;
	        for (int i = 0; i < path.transform.childCount; i++)
	        {
	            if (path.transform.GetChild(i).CompareTag("LastPlane"))
	            {
	                lastPlane = path.transform.GetChild(i).gameObject;
	                break;
	            }
	        }
	        return lastPlane;
	    }

	    //Create the path
	    void CreatePath()
	    {
	        pathCounter++;

	        //pathCounter == pathNumber -> this is the last path, change direction right here
	        if (pathCounter == pathNumber)
	        {        
	            //Reset pathCounter and random pathNumber for next one
	            pathCounter = 0;
	            pathNumber = Random.Range(minPathNumber, maxPathNumber);
	            turn = turn * (-1);

	            pathCreationDirection = (turn < 0) ? Vector3.forward : Vector3.right; //Indentify direction
	            pathCreationRotation = (turn < 0) ? (new Vector3(0, 270, 0)) : (new Vector3(0, 0, 0)); //Indentify rotation

	            //Create path and gold
	            GameObject turnPath = (turn < 0) ?
	                                GetTurnLeftPath() :
	                                GetTurnRightPath();

	            turnPath.GetComponent<PathController>().GetFirstRotation();
	            turnPath.transform.position = nextPathPosition;
	            turnPath.GetComponent<PathController>().isTurnPath = true;
	            CreateGold(turnPath);

	            //Update postion for next path
	            fixedPosition = new Vector3(LastPlaneOfPath(turnPath).transform.position.x,
	                turnPath.transform.position.y,
	                LastPlaneOfPath(turnPath).transform.position.z);
	            nextPathPosition = fixedPosition + pathCreationDirection * planeSize.x;
	            listPath.Add(turnPath);
	        }
	        else //This isn't the last path
	        {
	            GameObject path = Instantiate(pathArray[Random.Range(0, pathArray.Length - 1)], nextPathPosition, Quaternion.identity) as GameObject;
	            path.GetComponent<PathController>().GetFirstRotation();
	            path.transform.eulerAngles = pathCreationRotation;
	            CreateGold(path);
	            fixedPosition = new Vector3(LastPlaneOfPath(path).transform.position.x,
	                path.transform.position.y,
	                LastPlaneOfPath(path).transform.position.z);
	            nextPathPosition = fixedPosition + pathCreationDirection * planeSize.x;
	            listPath.Add(path);
	        }      
	    }

	    //Create gold base on the given path
	    void CreateGold(GameObject path)
	    {
	        listGoldPosition.Clear();

	        for (int i = 0; i < path.transform.childCount; i++)
	        {
	            GameObject ob = path.transform.GetChild(i).gameObject;
	            if (!ob.CompareTag("Bridge"))
	            {
	                listGoldPosition.Add(ob.transform.position);
	            }
	        }

	        foreach (Vector3 pos in listGoldPosition)
	        {
	            if (Random.value <= goldFrequency)
	            {
	                GameObject gold = GetGold();
	                gold.transform.position = new Vector3(pos.x, 1, pos.z);
	                gold.transform.parent = path.transform;
	            }
	        }
	    }

	    //Rotate all bridge
	    public void RotateAllBridge()
	    {
	        for (int i = 0; i < listPath.Count; i++)
	        {
	            listPath[i].GetComponent<PathController>().RotateBridges();
	        }
	    }

	    //Check and refesh path
	    IEnumerator CheckAndRefeshPath()
	    {
	        while (!gameOver)
	        {
	            foreach(GameObject path in listPath)
	            {
	                Vector3 viewPortPos = Camera.main.WorldToViewportPoint(path.transform.position);
	                if (viewPortPos.y < -1f)
	                {                  
	                    if (path.CompareTag("TurnLeftPath"))
	                    {
	                        path.transform.parent = turnLeftPathManager.transform;
	                        path.GetComponent<PathController>().ResetRotation();
	                        path.transform.eulerAngles = Vector3.zero;
	                        path.SetActive(false);
	                    }
	                    else if (path.CompareTag("TurnRightPath"))
	                    {
	                        path.transform.parent = turnRightPathManager.transform;
	                        path.GetComponent<PathController>().ResetRotation();
	                        path.transform.eulerAngles = Vector3.zero;
	                        path.SetActive(false);
	                    }
	                    else
	                    {
	                        Destroy(path);
	                    }
	                    listPath.Remove(path);
	                    CreatePath();
	                    break;
	                }
	            }            
	            yield return null;
	        }
	    }
	}
}