using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace OnefallGames
{
	public enum GameState
	{
	    Prepare,
	    Playing,
	    Pause,
	    Revive,
	    GameOver,
	}

	public class GameManager : MonoBehaviour {

	    public static GameManager Instance { private set; get; }
	    public static event System.Action<GameState> GameStateChanged = delegate { };
	    public static bool isRestart = false;

	    public GameState GameState
	    {
	        get
	        {
	            return gameState;
	        }
	        private set
	        {
	            if (value != gameState)
	            {
	                gameState = value;
	                GameStateChanged(gameState);
	            }
	        }
	    }

	    [Header("Gameplay Config")]
	    [SerializeField] private int firstPillarNumber = 4;
	    [SerializeField] private int firstMountainNumber = 3;
	    [SerializeField] private float pillarSpace = 10f;
	    [SerializeField] private float mountainSpace = 15f;
	    [SerializeField] private int scoreToUpdateMountainColor = 10;
	    [SerializeField] private float lerpingColorTime = 0.3f;
	    [SerializeField] private float changingColorDelay = 0.3f;
	    [SerializeField] private float reviveWaitTime = 4f;
	    [SerializeField] private Color[] mountainColors;


	    [Header("Gameplay References")]
	    [SerializeField] private GameObject firstPillar;
	    [SerializeField] private GameObject firstMountain;
	    [SerializeField] private GameObject mountainPrefab;
	    [SerializeField] private GameObject bluePillarPrefab;
	    [SerializeField] private GameObject greenPillarPrefab;
	    [SerializeField] private GameObject yellowPillarPrefab;
	    [SerializeField] private GameObject pinkPillarPrefab;
	    
	    public Color CurrentMountainColor { private set; get; }
	    public float PillarSpace { private set; get; }
	    public float ReviveWaitTime { private set; get; }
	    public bool IsRevived { private set; get; }
	    public bool IsPause { private set; get; }
	    

	    private GameState gameState = GameState.GameOver;

	    private List<GameObject> listBluePillar = new List<GameObject>();
	    private List<GameObject> listGreenPillar = new List<GameObject>();
	    private List<GameObject> listYellowPillar = new List<GameObject>();
	    private List<GameObject> listPinkPillar = new List<GameObject>();
	    private List<GameObject> listMountain = new List<GameObject>();
	    private Vector2 nextPillarPos = Vector2.zero;
	    private Vector2 mountainPos = Vector2.zero;
	    private int previousScore = 0;
	    private int mountainColorIndex = 0;
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

	    // Use this for initialization
	    void Start () {

	        Application.targetFrameRate = 60;
	        ScoreManager.Instance.Reset();
	        PrepareGame();
		}

	    public void PrepareGame()
	    {
	        //Fire event
	        GameState = GameState.Prepare;
	        gameState = GameState.Prepare;

	        //Add another actions here

	        //Assign variables
	        CurrentMountainColor = mountainColors[mountainColorIndex];
	        PillarSpace = pillarSpace;
	        ReviveWaitTime = reviveWaitTime;
	        IsRevived = false;
	        IsPause = false;

	        //Assign next pillar position and next mountain position
	        nextPillarPos = (Vector2)firstPillar.transform.position + Vector2.right * pillarSpace;
	        mountainPos = (Vector2)firstMountain.transform.position + Vector2.right * mountainSpace;

	        //Add the first mountain to the pos
	        listMountain.Add(firstMountain);

	        for (int i = 0; i < firstPillarNumber; i++)
	        {
	            CreatePillar();
	        }
	        for(int i = 0; i < firstMountainNumber; i++)
	        {
	            CreateMountain();
	        }

	        if (isRestart)
	            PlayingGame();
	    }


	    /// <summary>
	    /// Actual start the game
	    /// </summary>
	    public void PlayingGame()
	    {
	        //Fire event
	        GameState = GameState.Playing;
	        gameState = GameState.Playing;

	        //Add another actions here
	        StartBackgroundMusic(0.5f);
	        StartCoroutine(UpdateMountainColor());

	        //Reset IsPause variable
	        if (IsPause)
	        {
	            StartCoroutine(ResetIsPauseVariable());
	        }
	    }


	    /// <summary>
	    /// Pause the game
	    /// </summary>
	    public void PauseGame()
	    {
	        //Fire event
	        GameState = GameState.Pause;
	        gameState = GameState.Pause;

	        //Add another actions here
	        StopBackgroundMusic(0.5f);
	        IsPause = true;
	    }

	    /// <summary>
	    /// Call Pre_GameOver event
	    /// </summary>
	    public void Revive()
	    {
	        //Fire event
	        GameState = GameState.Revive;
	        gameState = GameState.Revive;

	        //Add another actions here
	        StopBackgroundMusic(0.5f);
	    }


	    /// <summary>
	    /// Call GameOver event
	    /// </summary>
	    public void GameOver()
	    {
	        //Fire event
	        GameState = GameState.GameOver;
	        gameState = GameState.GameOver;

	        //Add another actions here
	        StopBackgroundMusic(0.5f);
	        isRestart = true;
	    }


	    public void LoadScene(string sceneName, float delay)
	    {
	        StartCoroutine(LoadingScene(sceneName, delay));
	    }

	    IEnumerator LoadingScene(string sceneName, float delay)
	    {
	        yield return new WaitForSeconds(delay);
	        SceneManager.LoadScene(sceneName);
	    }

	    void StartBackgroundMusic(float delay)
	    {
	        StartCoroutine(PlayBackgroundMusic(delay));
	    }

	    IEnumerator PlayBackgroundMusic(float delay)
	    {
	        yield return new WaitForSeconds(delay);
	        if (SoundManager.Instance.background != null)
	            SoundManager.Instance.PlayMusic(SoundManager.Instance.background);
	    }

	    void StopBackgroundMusic(float delay)
	    {
	        StartCoroutine(PauseBackgroundMusic(delay));
	    }

	    IEnumerator PauseBackgroundMusic(float delay)
	    {
	        yield return new WaitForSeconds(delay);
	        if (SoundManager.Instance.background != null)
	            SoundManager.Instance.StopMusic();
	    }

	    //Get an inactive blue pillar 
	    private GameObject GetBluePillar()
	    {
	        //Find on the list
	        foreach(GameObject o in listBluePillar)
	        {
	            if (!o.activeInHierarchy)
	                return o;
	        }

	        //Didn't find one -> create new one
	        GameObject bluePillar = Instantiate(bluePillarPrefab, Vector2.zero, Quaternion.identity);
	        listBluePillar.Add(bluePillar);
	        bluePillar.SetActive(false);
	        return bluePillar;
	    }

	    //Get an inactive green pillar 
	    private GameObject GetGreenPillar()
	    {
	        //Find on the list
	        foreach (GameObject o in listGreenPillar)
	        {
	            if (!o.activeInHierarchy)
	                return o;
	        }

	        //Didn't find one -> create new one
	        GameObject greenPillar = Instantiate(greenPillarPrefab, Vector2.zero, Quaternion.identity);
	        listGreenPillar.Add(greenPillar);
	        greenPillar.SetActive(false);
	        return greenPillar;
	    }

	    //Get an inactive yellow pillar 
	    private GameObject GetYellowPillar()
	    {
	        //Find on the list
	        foreach (GameObject o in listYellowPillar)
	        {
	            if (!o.activeInHierarchy)
	                return o;
	        }

	        //Didn't find one -> create new one
	        GameObject yellowPillar = Instantiate(yellowPillarPrefab, Vector2.zero, Quaternion.identity);
	        listYellowPillar.Add(yellowPillar);
	        yellowPillar.SetActive(false);
	        return yellowPillar;
	    }

	    //Get an inactive pink pillar 
	    private GameObject GetPinkPillar()
	    {
	        //Find on the list
	        foreach (GameObject o in listPinkPillar)
	        {
	            if (!o.activeInHierarchy)
	                return o;
	        }

	        //Didn't find one -> create new one
	        GameObject pinkPillar = Instantiate(pinkPillarPrefab, Vector2.zero, Quaternion.identity);
	        listPinkPillar.Add(pinkPillar);
	        pinkPillar.SetActive(false);
	        return pinkPillar;
	    }

	    private GameObject GetMountain()
	    {
	        foreach(GameObject o in listMountain)
	        {
	            if (!o.activeInHierarchy)
	                return o;
	        }

	        GameObject mountain = Instantiate(mountainPrefab, Vector2.zero, Quaternion.identity);
	        listMountain.Add(mountain);
	        mountain.SetActive(false);
	        return mountain;
	    }


	    //Update mountain color
	    private IEnumerator UpdateMountainColor()
	    {
	        while (gameState == GameState.Playing)
	        {
	            int currentScore = ScoreManager.Instance.Score;
	            if (currentScore != previousScore && currentScore % scoreToUpdateMountainColor == 0)
	            {
	                previousScore = currentScore;
	                mountainColorIndex++;
	                if (mountainColorIndex == mountainColors.Length)
	                    mountainColorIndex = 0;

	                CurrentMountainColor = mountainColors[mountainColorIndex];

	                //Arrange the mountains
	                List<MountainController> listMountainControl = FindObjectsOfType<MountainController>().ToList();
	                List<MountainController> arrangedList = new List<MountainController>();
	                while (listMountainControl.Count > 0)
	                {
	                    int index = 0;
	                    float minX = listMountainControl[0].transform.position.x;
	                    for(int i = 1; i < listMountainControl.Count; i++)
	                    {
	                        float currentX = listMountainControl[i].transform.position.x;
	                        if (currentX < minX)
	                        {
	                            minX = currentX;
	                            index = i;
	                        }
	                    }

	                    arrangedList.Add(listMountainControl[index]);
	                    listMountainControl.Remove(listMountainControl[index]);
	                    yield return null;
	                }

	                //Change color for each mountain
	                for(int i = 0; i < arrangedList.Count; i++)
	                {
	                    arrangedList[i].ChangeColor(mountainColors[mountainColorIndex], lerpingColorTime);
	                    yield return new WaitForSeconds(changingColorDelay);
	                }
	            }
	            yield return null;
	        }
	    }

	    private IEnumerator HandleGameState(float delay)
	    {
	        yield return new WaitForSeconds(delay);
	        if (IsRevived)//Player already revived
	        {
	            GameOver();
	        }
	        else //Player isn't revive yet
	        {
	            if (AdManager.Instance.IsRewardedVideoAdReady())
	            {
	                Revive();
	            }
	            else
	            {
	                GameOver();
	            }
	        }
	    }


	    //Reset IsPause variable back to false
	    private IEnumerator ResetIsPauseVariable()
	    {
	        yield return new WaitForSeconds(0.5f);
	        IsPause = false;
	    }

	    //////////////////////////////////////Publish functions
	    
	    
	    /// <summary>
	    /// Handle GameState base on IsRevived variable.
	    /// If player already revived -> call GameOver state.
	    /// Otherwise:
	    /// If rewarded video ads was loaded -> call Revive state.
	    /// Else -> call GameOver state.
	    /// </summary>
	    /// <param name="delay"></param>
	    public void HandlePlayerDieState(float delay)
	    {
	        StartCoroutine(HandleGameState(delay));
	    }

	    /// <summary>
	    /// Create next pillar
	    /// </summary>
	    public void CreatePillar()
	    {
	        //Get a random pillar
	        GameObject pillar = null;
	        //Random.InitState(System.Environment.TickCount);
	        int value = Random.Range(0, 4);
	        if (value == 0)
	            pillar = GetBluePillar();
	        else if (value == 1)
	            pillar = GetGreenPillar();
	        else if (value == 2)
	            pillar = GetYellowPillar();
	        else
	            pillar = GetPinkPillar();

	        //Set position
	        pillar.transform.position = nextPillarPos;

	        //Active that pillar
	        pillar.SetActive(true);

	        //Assign next pillar pos
	        nextPillarPos = (Vector2)pillar.transform.position + Vector2.right * pillarSpace;     
	    }

	    /// <summary>
	    /// Create next mountain
	    /// </summary>
	    public void CreateMountain()
	    {
	        GameObject mountain = GetMountain(); //Get an inactive mountain
	        mountain.SetActive(true); //Active that mountain
	        mountain.transform.position = mountainPos; //Set position
	        mountainPos = (Vector2)mountain.transform.position + Vector2.right * mountainSpace; //Assign next mountain position
	    }

	    /// <summary>
	    /// Continue the game
	    /// </summary>
	    public void SetContinueGame()
	    {
	        IsRevived = true;
	        PlayingGame();
	    }
	}
}