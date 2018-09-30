using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hammer
{
	public class UIManager : MonoBehaviour
	{
	    public AudioClip clickClip;
	    public GameObject gameOverScreen, mainMenu, gameUI;
	    public Toggle soundTgl;
	    public AudioSource bgMusic, effectSource;
	    public Text scoreGameOver, higScoreGameOver;

	    private void Awake()
	    {
	        PlayGameManager.Instance.Authenticate();
	    }
	    private void Start()
	    {
	        _highScore = PlayerPrefs.GetInt(HIGH_SCORE, 0);
	        _totemsBroken = PlayerPrefs.GetInt(TOTEMS_BROKEN, 0);
	        Volume = PlayerPrefs.GetInt(VOLUME, 1) == 1;
	        soundTgl.onValueChanged.AddListener(soundToggleClicked);
	    }
	    public void PlayClick()
	    {
	        if (Volume)
	        {
	            effectSource.clip = clickClip;
	            effectSource.Play();
	        }
	    }
	    public void StartGame()
	    {
	        PlayClick();
	        mainMenu.SetActive(false);
	        gameOverScreen.SetActive(false);
	        gameUI.SetActive(true);
	        GameManager.Instance.InitGame();

	    }
	    public void GameOver()
	    {
	        GameManager.Instance.GameOver();
	        ComputeScore();
	        gameOverScreen.SetActive(true);
	    }
	    static int _totemsBroken = 0;
	    public static int TotemsBroken
	    {
	        get
	        {
	            return _totemsBroken;
	        }
	        set
	        {
	            _totemsBroken = value;
	            PlayerPrefs.SetInt(TOTEMS_BROKEN, _totemsBroken);
	        }
	    }
	    bool _vol;
	    public bool Volume
	    {
	        get
	        {
	            return _vol;
	        }
	        set
	        {
	            _vol = value;
	            soundTgl.isOn = value;
	            bgMusic.volume = _vol ? 1 : 0;
	            foreach (var item in GameManager.Instance.effectAudio)
	            {
	                item.volume = _vol ? 1 : 0;
	            }
	            PlayerPrefs.SetInt(VOLUME, _vol ? 1 : 0);
	            PlayerPrefs.Save();
	        }
	    }
	    public const string HIGH_SCORE = "HIGH_SCORE", TOTEMS_BROKEN = "BROKEN_TOTEMS", VOLUME = "VOLUME";
	    static int _highScore;
	    public static int HighScore
	    {
	        get
	        {
	            return _highScore;
	        }
	        set
	        {
	            _highScore = value;
	            PlayerPrefs.SetInt(HIGH_SCORE, _highScore);
	            PlayerPrefs.Save();
	        }
	    }

	    public void ShowAchievementUI()
	    {
	        PlayClick();
	        PlayGameManager.Instance.ShowAchievementsUI();
	    }
	    public void ShowLeaderboard()
	    {
	        PlayClick();
	        PlayGameManager.Instance.ShowLeaderboardUI();
	    }

	    public void ComputeScore()
	    {
	        if (GameManager.Instance.Score > HighScore)
	        {
	            HighScore = GameManager.Instance.Score;
	        }
	        PlayGameManager.Instance.PostToLeaderboard(HighScore);
	        scoreGameOver.text = "SCORE " + GameManager.Instance.Score;
	        higScoreGameOver.text = "BEST " + HighScore;
	        ComputeAchievements();
	    }
	    public void BackGameOverScreen()
	    {
	        gameOverScreen.SetActive(false);
	        mainMenu.SetActive(true);
	    }
	    void soundToggleClicked(bool state)
	    {
	        Volume = !Volume;
	        PlayClick();
	    }

	    int[] achScore = { 25, 50, 75, 100, 200, 300, 400, 500 },
	        achTotem = { 500, 1000, 2000, 3000, 5000, 9999, 50000, 99999 };


	    string[] ACH_SCORE_IDS = { GPGSIds.achievement_25_score, GPGSIds.achievement_50_score,
	         GPGSIds.achievement_75_score, GPGSIds.achievement_100_score,
	         GPGSIds.achievement_200_score, GPGSIds.achievement_300_score,
	         GPGSIds.achievement_400_score, GPGSIds.achievement_500_score
	    },
	        ACH_TOTEM_IDS = { GPGSIds.achievement_500_totems, GPGSIds.achievement_1000_totems,
	        GPGSIds.achievement_2000_totems,GPGSIds.achievement_3000_totems,
	        GPGSIds.achievement_5000_totems,GPGSIds.achievement_9999_totems,GPGSIds.achievement_50000_totems,
	        GPGSIds.achievement_99999_totems,
	    };

	    public void ComputeAchievements()
	    {
	        for (int i = 0; i < achScore.Length; i++)
	        {
	            if (HighScore >= achScore[i])
	            {
	                PlayGameManager.Instance.UnlockAchievement(ACH_SCORE_IDS[i]);
	            }
	        }
	        for (int i = 0; i < achTotem.Length; i++)
	        {
	            if (TotemsBroken >= achTotem[i])
	            {
	                PlayGameManager.Instance.UnlockAchievement(ACH_TOTEM_IDS[i]);
	            }
	        }
	    }
	    public void OnClickRate()
	    {

	        string rateURL = "http://gameslyce.com/";
	        string androidPackageName = "com.newgengames.dartsmathstrainerpro",
	            iOSStoreAppID = "145789999";

	#if UNITY_ANDROID
	        rateURL = "market://details?id=" + androidPackageName;
	#elif UNITY_IOS
	            rateURL = "itms-apps://itunes.apple.com/app/" + iOSStoreAppID;
	#endif
	        Application.OpenURL(rateURL);
	    }

	    private static UIManager _instance;
	    public static UIManager Instance
	    {
	        get
	        {
	            if (_instance == null) _instance = GameObject.FindObjectOfType<UIManager>();
	            return _instance;
	        }
	    }
	}
}