using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using SgLib;
using System;

namespace CrashRacing
{
	public enum GameState
	{
	    Prepare,
	    Playing,
	    Paused,
	    PreGameOver,
	    GameOver
	}

	public class GameManager : MonoBehaviour {
	    public static GameManager Instance { get; private set; }

	    public static event System.Action<GameState, GameState> GameStateChanged;

	    private static bool isRestart;

	    public GameState GameState
	    {
	        get
	        {
	            return _gameState;
	        }
	        set
	        {
	            if (value != _gameState)
	            {
	                GameState oldState = _gameState;
	                _gameState = value;

	                if (GameStateChanged != null)
	                    GameStateChanged(_gameState, oldState);
	            }
	        }
	    }

	    public static int GameCount
	    {
	        get { return _gameCount; }
	        private set { _gameCount = value; }
	    }

	    private static int _gameCount = 0;

	    [Header("Set the target frame rate for this game")]
	    [Tooltip("Use 60 for games requiring smooth quick motion, set -1 to use platform default frame rate")]
	    public int targetFrameRate = 30;

	    [Header("Current game state")]
	    [SerializeField]
	    private GameState _gameState = GameState.Prepare;

	    // List of public variable for gameplay tweaking
	    [Header("Gameplay Config")]
	    public int initialPathNumber;
	    public float maxCarSpeed;
	    public float minCarSpeed;
	    [Range(0f, 1f)]
	    public float treeAndRockFrequency;
	    [Range(0f, 1f)]
	    public float carFrequency;
	    [Range(0f, 1f)]
	    public float reverseCarFrequency;
	    [Range(0f, 1f)]
	    public float goldFrequency;

	    public float initialSpeed = 40;
	    public float playerSpeed = 60;
	    public float increaseSpeedFactor = 150;
	    public float turnTime = 0.3f;
	    public float horizontalThresholdSwipe = 30;
	    public float verticalThresholdSwipe = 50;
	    public float rotateAngle = 15;

	    public bool cameraAlwaysfollowPlayer = true;

	    [Header("Object References")]
	    public PlayerController playerController;
	    public GameObject unactiveCarManager;
	    public GameObject activeCarManager;
	    public GameObject treeAndRockManager;
	    public GameObject firstPath;
	    public GameObject pathPrefab;
	    public GameObject leftLampPrefab;
	    public GameObject rightLampPrefab;
	    public GameObject goldPrefab;
	    public GameObject[] treeAndRockArray;
	    public GameObject[] carArray;

	    private List<GameObject> listPath = new List<GameObject>();
	    private Vector3 previousCarPos = Vector3.zero;
	    private int initialCarNumber = 50;
	    private int initialGoldNumber = 15;
	    private int initialTreeAndRockNumber;
	    private float zPathSize;
	    private int turn = 1;
	    private float lampCounter = 0;
	    private int listPathIndex = 0;
	    private const float PositionBias = 0;

	    void OnEnable()
	    {
	        PlayerController.PlayerDied += PlayerController_PlayerDied;
	    }

	    void OnDisable()
	    {
	        PlayerController.PlayerDied -= PlayerController_PlayerDied;
	    }

	    void Awake()
	    {
	        if (Instance == null)
	        {
	            Instance = this;
	        }
	        else
	        {
	            DestroyImmediate(Instance.gameObject);
	            Instance = this;
	        }
	    }

	    void OnDestroy()
	    {
	        if (Instance == this)
	        {
	            Instance = null;
	        }
	    }

	    void Start()
	    {
	        // Initial setup
	        Application.targetFrameRate = targetFrameRate;
	        ScoreManager.Instance.Reset();

	        PrepareGame();

	        ScoreManager.Instance.Reset();

	        zPathSize = firstPath.transform.GetChild(0).GetComponent<Renderer>().bounds.size.z;
	        initialTreeAndRockNumber = (2 * 3) * (initialPathNumber + 1);

	        firstPath.transform.parent = transform;
	        listPath.Add(firstPath);

	        CRStartGame();

	        List<Vector3> listTreeAndRockLeft = ListTreeAndRockPosition(firstPath);
	        CreateTreeAndRock(listTreeAndRockLeft, firstPath);

	        StartCoroutine(CheckAndDisableMovePath());

			if (SoundManager.Instance.background != null)
				SoundManager.Instance.PlayMusic(SoundManager.Instance.background);
	    }

	    // Update is called once per frame
	    void Update()
	    {

	    }

	    // Listens to the event when player dies and call GameOver
	    void PlayerController_PlayerDied()
	    {
	        GameOver();
	    }

	    // Make initial setup and preparations before the game can be played
	    public void PrepareGame()
	    {
	        GameState = GameState.Prepare;

	        // Automatically start the game if this is a restart.
	        if (isRestart)
	        {
	            isRestart = false;
	            StartGame();
	        }
	    }

	    // A new game official starts
	    public void StartGame()
	    {
	        ReplacePlayerMesh();
	        GameState = GameState.Playing;	        
	        StartCoroutine(CountScore());
	    }

	    private void ReplacePlayerMesh()
	    {
	        GameObject currentCharacter = CharacterManager.Instance.characters[CharacterManager.Instance.CurrentCharacterIndex];
	        MeshFilter meshfilter = playerController.GetComponent<MeshFilter>();
	        MeshCollider meshCollider = playerController.GetComponent<MeshCollider>();
	        MeshRenderer meshRenderer = playerController.GetComponent<MeshRenderer>();
	        meshfilter.sharedMesh = currentCharacter.GetComponent<MeshFilter>().sharedMesh;
	        meshCollider.sharedMesh = currentCharacter.GetComponent<MeshFilter>().sharedMesh;
	        meshRenderer.sharedMaterial = currentCharacter.GetComponent<MeshRenderer>().sharedMaterial;
	    }

	    // Called when the player died
	    public void GameOver()
	    {
	        if (SoundManager.Instance.background != null)
	            SoundManager.Instance.StopMusic();

	        SoundManager.Instance.PlaySound(SoundManager.Instance.gameOver);
	        GameState = GameState.GameOver;
	        GameCount++;

	        // Add other game over actions here if necessary
	    }

	    // Start a new game
	    public void RestartGame(float delay = 0)
	    {
	        isRestart = true;
	        StartCoroutine(CRRestartGame(delay));
	    }

	    IEnumerator CRRestartGame(float delay = 0)
	    {
	        yield return new WaitForSeconds(delay);
	        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	    }

	    public void HidePlayer()
	    {
	        if (playerController != null)
	            playerController.gameObject.SetActive(false);
	    }

	    public void ShowPlayer()
	    {
	        if (playerController != null)
	            playerController.gameObject.SetActive(true);
	    }
	    // Use this for initialization
		
	    void CRStartGame()
	    {

	        //Instantiate trees and rocks
	        for (int i = 0; i < initialTreeAndRockNumber; i++)
	        {
	            GameObject treeAndRock = Instantiate(treeAndRockArray[UnityEngine.Random.Range(0, treeAndRockArray.Length)]);
	            treeAndRock.SetActive(false);
	            treeAndRock.transform.parent = treeAndRockManager.transform;
	        }

	        //Instantiate cars
	        for (int i = 0; i < initialCarNumber; i++)
	        {
	            GameObject car = Instantiate(carArray[UnityEngine.Random.Range(0, carArray.Length)]);
	            car.SetActive(false);
	            car.transform.parent = unactiveCarManager.transform;
	        }

	        //Instantiate gold
	        for(int i = 0; i < initialGoldNumber; i++)
	        {
	            GameObject gold = Instantiate(goldPrefab);
	            gold.SetActive(false);
	            gold.transform.parent = CoinManager.Instance.transform;
	        }

	        //Instantiate paths
	        for (int i = 0; i < initialPathNumber; i++)
	        {
	            Vector3 pathPosition = listPath[listPath.Count - 1].transform.position + Vector3.forward * zPathSize;
	            pathPosition.z += PositionBias;
	            GameObject thePath = Instantiate(pathPrefab, pathPosition, Quaternion.identity) as GameObject;
	            CreateLamp(thePath);
	            thePath.transform.parent = transform;
	            listPath.Add(thePath);

	            List<Vector3> listTreeAndRock = ListTreeAndRockPosition(thePath);
	            CreateTreeAndRock(listTreeAndRock, thePath);
	        }     
	    }


	    void CreateLamp(GameObject thePath)
	    {
	        lampCounter++;
	        if (lampCounter % 3 == 0)
	        {
	            turn = turn * (-1);
	            if (turn < 0) //Create left lamp
	            {
	                //Find left barrier
	                GameObject leftBarrier = null;
	                for(int i = 0; i < thePath.transform.childCount; i++)
	                {
	                    if (thePath.transform.GetChild(i).CompareTag("LeftBarrier"))
	                    {
	                        leftBarrier = thePath.transform.GetChild(i).gameObject;
	                    }
	                }

	                //Instantiate left lamp base on leftBarrier position
	                Vector3 leftLampPosition = new Vector3(leftBarrier.transform.position.x, 0.5f, leftBarrier.transform.position.z);
	                GameObject leftLamp = Instantiate(leftLampPrefab, leftLampPosition, Quaternion.identity) as GameObject;
	                leftLamp.transform.parent = thePath.transform;

	            }
	            else //Create right lamp
	            {
	                //Find right barrier
	                GameObject rightBarrier = null;
	                for (int i = 0; i < thePath.transform.childCount; i++)
	                {
	                    if (thePath.transform.GetChild(i).CompareTag("RightBarrier"))
	                    {
	                        rightBarrier = thePath.transform.GetChild(i).gameObject;
	                    }
	                }

	                //Instantiate left lamp base on leftBarrier position
	                Vector3 rightLampPosition = new Vector3(rightBarrier.transform.position.x, 0.5f, rightBarrier.transform.position.z);
	                GameObject rightLamp = Instantiate(rightLampPrefab, rightLampPosition, Quaternion.identity) as GameObject;
	                rightLamp.transform.parent = thePath.transform;

	            }
	        }
	    }
	   

	    //Find all positions to create car
	    List<Vector3> ListCarPosition(GameObject thePath)
	    {
	        List<Vector3> listPathPosition = new List<Vector3>();
	        for(int i = 0; i < thePath.transform.childCount; i++)
	        {
	            if (thePath.transform.GetChild(i).CompareTag("Path"))
	            {
	                Vector3 pos = thePath.transform.GetChild(i).transform.position + new Vector3(0, 1f, 0);
	                listPathPosition.Add(pos);
	            }        
	        }
	        return listPathPosition;
	    }

	    //Create tree and rock
	    void CreateTreeAndRock(List<Vector3> listTreeAndRockPos, GameObject thePath)
	    {
	        for(int i = 0; i < listTreeAndRockPos.Count; i++)
	        {
	            if (UnityEngine.Random.value <= treeAndRockFrequency)
	            {
	                GameObject treeAndRock = GetRandomTreeAndRock();
	                treeAndRock.transform.position = listTreeAndRockPos[i];
	                treeAndRock.transform.parent = thePath.transform;
	            }      
	        }
	    }


	    //Find positions to create tree and rock
	    List<Vector3> ListTreeAndRockPosition(GameObject thePath)
	    {
	        List<Vector3> listTreeAndRock = new List<Vector3>();

	        GameObject leftLand = null;
	        for(int i = 0; i < thePath.transform.childCount; i++)
	        {
	            if (thePath.transform.GetChild(i).CompareTag("LeftLand"))
	            {
	                leftLand = thePath.transform.GetChild(i).gameObject;
	                break;
	            }
	        }

	        GameObject rightLand = null;
	        for (int i = 0; i < thePath.transform.childCount; i++)
	        {
	            if (thePath.transform.GetChild(i).CompareTag("RightLand"))
	            {
	                rightLand = thePath.transform.GetChild(i).gameObject;
	                break;
	            }
	        }

	        Vector3 pos_1 = leftLand.transform.position;
	        //Vector3 pos_2 = leftLand.transform.position + Vector3.right * 10f;
	        //Vector3 pos_3 = leftLand.transform.position + Vector3.right * 20f;
	        Vector3 pos_4 = leftLand.transform.position + Vector3.right * -10f;
	        Vector3 pos_5 = leftLand.transform.position + Vector3.right * -20f;

	        Vector3 pos_6 = rightLand.transform.position;
	        Vector3 pos_7 = rightLand.transform.position + Vector3.right * 10f;
	        Vector3 pos_8 = rightLand.transform.position + Vector3.right * 20f;
	        //Vector3 pos_9 = rightLand.transform.position + Vector3.right * -10f;
	        //Vector3 pos_10 = rightLand.transform.position + Vector3.right * -20f;


	        listTreeAndRock.Add(pos_1);
	        //listTreeAndRock.Add(pos_2);
	        //listTreeAndRock.Add(pos_3);
	        listTreeAndRock.Add(pos_4);
	        listTreeAndRock.Add(pos_5);
	        listTreeAndRock.Add(pos_6);
	        listTreeAndRock.Add(pos_7);
	        listTreeAndRock.Add(pos_8);
	        //listTreeAndRock.Add(pos_9);
	        //listTreeAndRock.Add(pos_10);

	        return listTreeAndRock;
	    }


	    //Get random car from CarManager
	    GameObject GetRandomCar()
	    {
	        GameObject car = unactiveCarManager.transform.GetChild(UnityEngine.Random.Range(0, unactiveCarManager.transform.childCount)).gameObject;
	        car.transform.parent = null;
	        car.SetActive(true);
	        return car;
	    }

	    //Get random gold
	    GameObject GetRandomGold()
	    {
	        if (CoinManager.Instance.transform.childCount > 0)
	        {
	            GameObject gold = CoinManager.Instance.transform.GetChild(0).gameObject;
	            gold.SetActive(true);
	            return gold;
	        }
	        else
	        {
	            return null;
	        }
	    }

	    //Get random tree and rock from Tree&RockManager
	    GameObject GetRandomTreeAndRock()
	    {
	        GameObject treeAndRock = treeAndRockManager.transform.GetChild(UnityEngine.Random.Range(0, treeAndRockManager.transform.childCount)).gameObject;
	        treeAndRock.SetActive(true);
	        return treeAndRock;
	    }

	   
	    //Check move path to next position
	    IEnumerator CheckAndDisableMovePath()
	    {
	        while (true)
	        {
	            float destroyDistance = Camera.main.transform.position.z - 15f;

	            if (listPath[listPathIndex].transform.position.z < destroyDistance)
	            {
	                Vector3 nextPathPosition = TheFarestPath().transform.position + Vector3.forward * zPathSize; //Find next position
	                nextPathPosition.z += PositionBias;
	                listPath[listPathIndex].transform.position = nextPathPosition; //Move the path

	                ClearPath(ListObjectPooling(listPath[listPathIndex])); //Clear the path

	                //Create tree and rock for the path
	                List<Vector3> listTreeAndRock = ListTreeAndRockPosition(listPath[listPathIndex]); 
	                CreateTreeAndRock(listTreeAndRock, listPath[listPathIndex]);


	                List<Vector3> listCarPosition = ListCarPosition(listPath[listPathIndex]);
	                //Create car
	                if (UnityEngine.Random.value <= carFrequency)
	                {                    
	                    Vector3 carPosition = listCarPosition[UnityEngine.Random.Range(0, listCarPosition.Count)];
	                    while (carPosition.x == previousCarPos.x)
	                    {
	                        carPosition = listCarPosition[UnityEngine.Random.Range(0, listCarPosition.Count)];
	                    }
	                    previousCarPos = carPosition;
	                    GameObject car = GetRandomCar();
	                    car.transform.position = carPosition;
	                    CarController carControl = car.GetComponent<CarController>();
	                    carControl.speed = UnityEngine.Random.Range(minCarSpeed, maxCarSpeed);
	                    carControl.turnTime = turnTime * 2f;
	                    if (UnityEngine.Random.value <= reverseCarFrequency)
	                    {
	                        carControl.isReverseCar = true;
	                        car.transform.rotation = Quaternion.Euler(0, 180f, 0);
	                    }                

	                    car.transform.parent = activeCarManager.transform;
	                }

	                if (UnityEngine.Random.value <= goldFrequency)
	                {
	                    Vector3 goldPosition = listCarPosition[UnityEngine.Random.Range(0, listCarPosition.Count)];
	                    while (goldPosition == previousCarPos)
	                    {
	                        goldPosition = listCarPosition[UnityEngine.Random.Range(0, listCarPosition.Count)];
	                    }

	                    GameObject gold = GetRandomGold();
	                    if (gold)
	                    {
	                        gold.transform.position = new Vector3(goldPosition.x, 0.7f, goldPosition.z);
	                        gold.transform.rotation = Quaternion.Euler(45, 0, 0);
	                        gold.transform.parent = listPath[listPathIndex].transform;
	                    }
	                }

	                listPathIndex++;

	                if (listPathIndex == listPath.Count)
	                {
	                    listPathIndex = 0;
	                }              
	            }

	            //Find all car run out of camera and reset it 
	            for (int i = 0; i < activeCarManager.transform.childCount; i++)
	            {
	                if (activeCarManager.transform.GetChild(i).transform.position.z < destroyDistance)
	                {
	                    GameObject theCar = activeCarManager.transform.GetChild(i).gameObject;
	                    theCar.gameObject.SetActive(false);
	                    theCar.GetComponent<CarController>().stopMoving = false;               
	                    theCar.transform.parent = unactiveCarManager.transform;
	                }
	            }
	            yield return null;
	        }
	    }

	  
	    //Find the farest path
	    GameObject TheFarestPath()
	    {
	        GameObject thisPath = listPath[0];

	        for(int i = 0; i < listPath.Count; i++)
	        {
	            if (listPath[i].transform.position.z > thisPath.transform.position.z)
	            {
	                thisPath = listPath[i];
	            }
	        }

	        return thisPath;
	    }


	    //Find all tree and rock of the path that run out of camera
	    List<GameObject> ListObjectPooling(GameObject thePath)
	    {
	        List<GameObject> listObjectNeededRefesh = new List<GameObject>();
	        for(int i = 0; i < thePath.transform.childCount; i++)
	        {
	            if (thePath.transform.GetChild(i).CompareTag("TreeAndRock") || thePath.transform.GetChild(i).CompareTag("Gold"))
	            {
	                listObjectNeededRefesh.Add(thePath.transform.GetChild(i).gameObject);
	            }
	        }
	        return listObjectNeededRefesh;
	    }

	    //Clear the path
	    void ClearPath(List<GameObject> listObjectPooling)
	    {
	        for(int i = 0; i < listObjectPooling.Count; i++)
	        {
	            listObjectPooling[i].SetActive(false);
	            if (listObjectPooling[i].CompareTag("TreeAndRock"))
	            {
	                listObjectPooling[i].transform.parent = treeAndRockManager.transform;
	            }
	            else if (listObjectPooling[i].CompareTag("Gold"))
	            {
	                listObjectPooling[i].transform.parent = CoinManager.Instance.transform;
	            }           
	        }
	    }

	    IEnumerator CountScore()
	    {
	        while (true)
	        {
	            yield return new WaitForSeconds(0.5f);
	            if (GameState.Equals(GameState.Playing))
	                ScoreManager.Instance.AddScore(1);
	            else
	                yield break;
	        }
	    }

	}
}