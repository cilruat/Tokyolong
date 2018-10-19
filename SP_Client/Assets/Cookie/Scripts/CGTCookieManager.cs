using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

public class CGTCookieManager : MonoBehaviour {

    public static CGTCookieManager instance = null;

    [Header("Spawn")]
    public RectTransform spawnLine;

    [Header("Spawn Objects")]
    public GameObject[] spawnCookieObjects;
    private GameObject spawnSmallCookie;
    public GameObject[] spawnBonusObjects;
    private GameObject spawnBonusCookie;

    public GameObject scoreEffect;
    public GameObject scoreBonus;
    public GameObject bonusParticles;

    [Header("Sounds")]
    public AudioClip buttonClickSound;
    public AudioClip gameBonusSound;
    public AudioClip[] gameClickSound;
    public AudioClip[] gameBonusClickSound;


    [Header("Visuals")]
    public Text gameCookiesText;
    public Text gameCpsText;
    public Text gameMaxBonusText;

    public Slider bonusBar;

    [Header("Cursor")]
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    [Header("Menus")]
    public GameObject menuCanvas;
    public GameObject gameCanvas;
    public GameObject pauseCanvas;
    public GameObject gameOverCanvas;

    [Header("Quit")]
    public string gameOverURL = "http://u3d.as/11ta";

    public int clickValue = 1;

    private int cookiesScore = 0;
    //private float cookisBps = 0.0f;
    private float currentBonus;
    private float levelBonus = 10.0f;
    private float levelBonusMulti = 1.0f;
    private int levelBonusCookies = 1;
    private int levelBonusSub = 5;
    private float levelCps = 0.0f;

    private float gameTimer = 0.0f;
     
    private Animator animator;

    internal bool isGameOver = false;
    internal bool isGamePaused = false;

    internal Vector3 aimMousePosition;
    internal Vector3 aimTouchPosition;

    void Awake()
    {
         if (instance == null)
             instance = this;
         else if (instance != this)
             Destroy(gameObject);  

        animator = GetComponent<Animator>();
    }

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

#if UNITY_WEBGL
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.ForceSoftware);
#else
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
#endif
        LoadGameData();
        SetGameData();
        UpdateGameData();
        ShowGamePlayMenu();
    }

    void Update()
    {
        gameTimer += Time.deltaTime;
        
        UpdateInput();
        //UpdateGameData();
        if (gameTimer >= 1)
        { 
            levelCps = 0;
            gameTimer = 0;
            UpdateGameData();

        }
    }

    public void UpdateInput()
    {
        if (!Application.isMobilePlatform)
        {

            if (Input.GetButtonDown("Fire1"))
            {
                aimMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Tap(aimMousePosition);
            }
        }

        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0)
            {
                for (int t = 0; t < Input.touchCount; t++)
                {
                    Touch touch = Input.GetTouch(t);
                    TouchPhase phase = touch.phase;

                    if (phase == TouchPhase.Began)
                    {
                        aimTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                        Tap(aimTouchPosition);
                    }
                }
            }
        }
    }

    public void Tap(Vector3 aimPosition)
    {
        levelCps++;

        
        RaycastHit2D raycastHit2D = Physics2D.Raycast(aimPosition, Vector2.zero);
        if (raycastHit2D.collider != null)
        {
            if (raycastHit2D.collider.tag == "Bonus")
            {
                PlaySound(gameBonusClickSound[Random.Range(0, gameBonusClickSound.Length)]);
                UpdateScore(clickValue * levelBonusSub);
                Vector3 spawnBonusPos = raycastHit2D.collider.gameObject.transform.position;
                GameObject newBonusParticle = (GameObject)(Instantiate(bonusParticles, spawnBonusPos, Quaternion.identity));
                Destroy(newBonusParticle, 1.0f);
                Destroy(raycastHit2D.collider.gameObject);
                GameObject newBonusText = (GameObject)(Instantiate(scoreBonus, spawnBonusPos, Quaternion.identity));
                newBonusText.GetComponent<CGTScoreBonus>().SetScoreValue(clickValue * levelBonusSub);
            }
            else if (raycastHit2D.collider.tag == "Cookie")
            {
                PlaySound(gameClickSound[Random.Range(0, gameClickSound.Length)]);
                CreateSmallCookie();
                animator.SetBool("Tap", true);
                UpdateScore(clickValue);
            }
        }
    }

    public void UpdateScore(int scoreValue)
    {
        cookiesScore += scoreValue;
        UpdateGameData();
        UpdateBonusData(scoreValue);
        SaveGameData();
        CreateTextScore(scoreValue);
    }

    void UpdateBonusData(int scoreValue)
    {
        currentBonus += scoreValue;
       
        if (currentBonus >= levelBonus)
        {
            PlaySound(gameBonusSound);
            for (int i = 0; i < levelBonusCookies; i++)
            {
                CreateBonusCookie();
            }
            currentBonus = currentBonus - levelBonus;
            SetBonusLevel();
            bonusBar.minValue = 0;
            bonusBar.maxValue = levelBonus;  
        }
        bonusBar.value = currentBonus;
    }

    void SetBonusLevel()
    {
        if (levelBonus / levelBonusMulti == 100)
        {
            levelBonusMulti *= 10;
        }
        levelBonus += (10 * levelBonusMulti);
        gameMaxBonusText.text = levelBonus.ToString();
    }

    public void CreateTextScore(int scoreValue)
    {
        float spawnObjectXPos = Random.Range(-1.0f, 1.0f);
        float spawnObjectYPos = Random.Range(-1.0f, 1.0f);
        Vector3 spawnObjectPos = new Vector3(spawnObjectXPos,spawnObjectYPos, 0);
        GameObject newScoreText = (GameObject) (Instantiate(scoreEffect, spawnObjectPos, Quaternion.identity));
        newScoreText.GetComponent<CGTScoreEffect>().SetScoreValue(clickValue);
    }

    public void CreateSmallCookie()
    {
        float spawnObjectXPos = Random.Range(-2.25f, 2.25f);
        Vector3 spawnObjectPos = new Vector3(spawnObjectXPos, spawnLine.position.y, 0);
        spawnSmallCookie = spawnCookieObjects[Random.Range(0, spawnCookieObjects.Length)];
        Instantiate(spawnSmallCookie, spawnObjectPos, Quaternion.identity);
    }

    public void CreateBonusCookie()
    {
        float spawnObjectXPos = Random.Range(-2.0f, 2.0f);
        Vector3 spawnObjectPos = new Vector3(spawnObjectXPos, spawnLine.position.y, 0);
        spawnBonusCookie = spawnBonusObjects[Random.Range(0, spawnBonusObjects.Length)];
        Instantiate(spawnBonusCookie, spawnObjectPos, Quaternion.identity);
    }

    void LoadGameData()
    {
#if UNITY_5_3_OR_NEWER
        // DELETE ALL GAME DATA !!!!! PlayerPrefs.DeleteAll();
        cookiesScore = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "COOKIE_GAMESCORE", 0);
        levelBonus = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "COOKIE_LEVELBONUS", 10.0f);
        levelBonusMulti = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "COOKIE_LEVELBONUSMULTI", 1.0f);
        currentBonus = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "COOKIE_CURRENTBONUS", 0.0f);

       
#else
		// DELETE ALL GAME DATA !!!!! PlayerPrefs.DeleteAll();
		cookiesScore = PlayerPrefs.GetInt(Application.loadedLevelName + "COOKIE_GAMESCORE", 0);
        levelBonus = PlayerPrefs.GetFloat(Application.loadedLevelName + "COOKIE_LEVELBONUS", 10.0f);
        levelBonusMulti = PlayerPrefs.GetFloat(Application.loadedLevelName + "COOKIE_LEVELBONUSMULTI", 1.0f);
        currentBonus = PlayerPrefs.GetFloat(Application.loadedLevelName + "COOKIE_CURRENTBONUS", 0.0f);
	
#endif
    }

    void SetGameData()
    {
        bonusBar.minValue = 0;
        bonusBar.maxValue = levelBonus;
        gameMaxBonusText.text = levelBonus.ToString();
        bonusBar.value = currentBonus;
    }

    void UpdateGameData()
    {
        if (!isGameOver)
        {
            gameCookiesText.text = cookiesScore.ToString() + " Cookies!";
            gameCpsText.text = levelCps.ToString("N1") + " per second";
        }
        else
        {
           /* gameOverScoreText.text = "Score: " + lastGameScore.ToString();
            gameOverHighScoreText.text = "High Score: " + highGameScore.ToString(); */
        }
    }

    void SaveGameData()
    {
#if UNITY_5_3_OR_NEWER
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "COOKIE_GAMESCORE", cookiesScore);
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "COOKIE_LEVELBONUS", levelBonus);
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "COOKIE_LEVELBONUSMULTI", levelBonusMulti);
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "COOKIE_CURRENTBONUS", currentBonus);
#else
		PlayerPrefs.SetInt(Application.loadedLevelName + "COOKIE_GAMESCORE", cookiesScore);
        PlayerPrefs.SetFloat(Application.loadedLevelName + "COOKIE_LEVELBONUS", levelBonus);
        PlayerPrefs.SetFloat(Application.loadedLevelName + "COOKIE_LEVELBONUSMULTI", levelBonusMulti);
        PlayerPrefs.SetFloat(Application.loadedLevelName + "COOKIE_CURRENTBONUS", currentBonus);
#endif
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            CGTSoundManager.instance.PlaySound(clip);
    }

    void ButtonSound()
    {
        if (buttonClickSound != null)
            CGTSoundManager.instance.PlaySound(buttonClickSound); 
    }

    public void TapFalse()
    {
        animator.SetBool("Tap", false);
    }

    #region --------------- MENUS AND GAME CONTROL ---------------

    public void ShowGamePlayMenu()
    {
        gameCanvas.SetActive(true);
        gameOverCanvas.SetActive(false);
        /*pauseCanvas.SetActive(false);
        menuCanvas.SetActive(false);*/

        //XXXX DisplayBanner(false); 
    }

    public void ShowGameOverMenu()
    {
        gameOverCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        /*gameCanvas.SetActive(false);
        menuCanvas.SetActive(false);*/

        //XXXX DisplayBanner(true);
    }

    public void GameOver()
    {
        ButtonSound();
        ShowGameOverMenu();
    }

    public void GameQuitNow()
    {
        ButtonSound();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
            Application.OpenURL(gameOverURL);
#else
            Application.Quit();
#endif   
    }
    #endregion
}
