using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BlockJumpRabbit
{
public enum StateGame
{
	Loading,
    Home,
    GamePlay,
    GameOver
}

public enum Season
{
    Spring = 0,
    Summer = 1,
    Autumn = 2,
    Winter = 3
}

public class GameController : MonoBehaviour {

	public const float DISTANCE_OBJ = 1.35f;

    public StateGame stateGame = StateGame.Home;
    public PlayerController player;
    public AIController ai;
    public int currentScore { get; private set; }
    public int highestScore { get; private set; }
    public bool firstPlay { get; set; }
	public GameObject particleRain;
	public GameObject particleSnow;

    private UIManager uiManager;
    private SpawnObjectManger spawnObjManager;
	private SoundController soundController;
    private int scoreNextLevel;
    public int currentLevel { get; private set; }

    private List<GradientColorChangeObject> listCreatures;
    private Season currentSeason;

    void Awake()
    {
        currentScore = 0;
        currentLevel = 1;

        uiManager = GetComponent<UIManager>();
        spawnObjManager = GetComponent<SpawnObjectManger>();
		soundController = GetComponent<SoundController> ();

        scoreNextLevel = UnityEngine.Random.Range(10, 15) + currentScore;

        uiManager.ChangeScoreText(currentScore);

		// Start Loading screen
		stateGame = StateGame.Loading;
		player.transform.position = new Vector2 (-8 * DISTANCE_OBJ, player.transform.position.y);
		StartCoroutine( Loading ());

        // Check play first time to show tutorial
        firstPlay = CheckFirstPlay();

        // Get list obj change gradien to control
        listCreatures = new List<GradientColorChangeObject>();
        GradientColorChangeObject[] array = FindObjectsOfType<GradientColorChangeObject>();
        foreach(GradientColorChangeObject obj in array)
            listCreatures.Add(obj);
        currentSeason = Season.Spring;
    }

    public void AddScore(int scoreAdd)
    {
        currentScore += scoreAdd;
        uiManager.ChangeScoreText(currentScore);

        if (currentScore > scoreNextLevel)
        {
            ChangeLevel();
            scoreNextLevel = UnityEngine.Random.Range(20, 80) + currentScore;
        }
    }

	IEnumerator Loading() {
		while (player.transform.position.x < -0.1f) {
			if (player.transform.position.x > 0) {
				player.transform.position = new Vector2 (0, transform.position.y);
				//StopAllCoroutines ();
				break;
			}
			player.Jump (PlayerController.TypeJump.NormalJump);

			// player only jump when hit ground
			while (player.IsJump) {
				yield return null;
			}				

			yield return new WaitForSeconds(0.2f);
		}

		uiManager.CallHome ();
		stateGame = StateGame.Home;
	}

    public void ChangeLevel()
    {
        currentLevel++;

        switch (currentLevel)
        {
		case 2:
				spawnObjManager.Level2ChangePriorList ();
				ChangeSeason (Season.Summer);

                break;
            case 3:
				particleRain.SetActive (true);
                spawnObjManager.Level3ChangePriorList();
                ChangeSeason(Season.Autumn);
                break;
            case 4:
                spawnObjManager.AddElementToDictionary(new KeyValuePair<TypePlatform, int>(TypePlatform.Snake, 2));
                break;
		case 5:
				particleSnow.SetActive (true);
				spawnObjManager.Level5ChangePriorList ();
                spawnObjManager.AddElementToDictionary(new KeyValuePair<TypePlatform, int>(TypePlatform.Hippopotamus, 2));
                ChangeSeason(Season.Winter);
                break;
        }
    }

    public void ChangeStateGame(StateGame stateGame)
    {
        this.stateGame = stateGame;

        switch (stateGame)
        {
            case StateGame.GamePlay:
                
                spawnObjManager.InitalSpawnObj();

                player.Reset();
                player.transform.position = Vector2.zero;
                player.gameObject.SetActive(true);
                break;
            case StateGame.Home:
                break;
		case StateGame.GameOver:
			soundController.SoundGameOverOn ();
			soundController.SoundBackgroundOff ();

			spawnObjManager.StopSpawn ();
			ai.StopMove ();
			SaveScoreData ();
			uiManager.CallGameOVer ();

             //   StartCoroutine(CallRestartGame());
                break;
        }
    }
    //IEnumerator CallRestartGame()
    //{
    //    yield return new WaitForSeconds(2.0f);
    //    RestartGame();
    //}
    public void RestartGame()
    {
		// Reset particle
		particleRain.SetActive (false);
		particleSnow.SetActive (false);

        currentScore = 0;
        uiManager.ChangeScoreText(currentScore);
        spawnObjManager.ResetSpawn();
        player.transform.position = Vector2.zero;

        player.gameObject.SetActive(true);
        player.Reset();

		soundController.SoundGameOverOff ();
		soundController.SoundBackgroundOn ();

        // Reset level game
        currentLevel = 1;
        scoreNextLevel = UnityEngine.Random.Range(10, 15) + currentScore;

        if(currentSeason != Season.Spring)
            ChangeSeason(Season.Spring);

        Camera.main.GetComponent<CameraFollowPlayer>().Reset();
        stateGame = StateGame.GamePlay;
    }

    void SaveScoreData()
    {
        if (!PlayerPrefs.HasKey("best_score"))
            highestScore = currentScore;
        else
            highestScore = PlayerPrefs.GetInt("best_score");

        if (highestScore < currentScore)
            highestScore = currentScore;

        PlayerPrefs.SetInt("best_score", highestScore);
    }

    // Method check first play to show tutorial
    bool CheckFirstPlay()
    {
        bool firstPlay = (PlayerPrefs.HasKey("first_play")) ? true : false;
        PlayerPrefs.SetInt("first_play", 1);
        return firstPlay;
    }

    // Method  to change season
    public void ChangeSeason(Season season)
    {
        currentSeason = season;
        foreach (GradientColorChangeObject obj in listCreatures)
            obj.ChangeColor((int)season);
    }
}
}