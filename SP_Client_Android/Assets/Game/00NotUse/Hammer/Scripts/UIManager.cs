using System.Collections;
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

		public GameObject tapToStart;
		public RawImage imgVictory;
		public GameObject objVictory;
		public GameObject objSendServer;
		public GameObject objGameOver;
		public GameObject objReady;
		public GameObject objGo;
		public GameObject objBoard;
		public Text scoreTxt;

		public bool isStart = false;
		int finishPoint = 0;

	    private void Awake()
	    {
	        PlayGameManager.Instance.Authenticate();
	    }
	    private void Start()
	    {
			finishPoint = Info.HAMMER_FINISH_POINT;
			scoreTxt.text = Info.practiceGame ? "0" : "0 / " + finishPoint.ToString ();

	        /*_highScore = PlayerPrefs.GetInt(HIGH_SCORE, 0);
	        _totemsBroken = PlayerPrefs.GetInt(TOTEMS_BROKEN, 0);
	        Volume = PlayerPrefs.GetInt(VOLUME, 1) == 1;
	        soundTgl.onValueChanged.AddListener(soundToggleClicked);*/
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
	        /*mainMenu.SetActive(false);
	        gameOverScreen.SetActive(false);
	        gameUI.SetActive(true);*/
	        GameManager.Instance.InitGame();
			StartCoroutine (_StartGame ());
	    }

		IEnumerator _StartGame()
		{
			UITweenAlpha.Start (tapToStart, 1f, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2).DisableOnFinish ());
			yield return new WaitForSeconds (.5f);

			UITweenAlpha.Start(objReady, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

			yield return new WaitForSeconds (1f);
			UITweenAlpha.Start(objReady, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

			yield return new WaitForSeconds (.25f);
			UITweenAlpha.Start(objGo, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

			yield return new WaitForSeconds (1f);
			UITweenAlpha.Start(objGo, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

			yield return new WaitForSeconds (.3f);

			isStart = true;
		}

	    public void GameOver()
	    {
	        /*GameManager.Instance.GameOver();
	        ComputeScore();
	        gameOverScreen.SetActive(true);*/
			objGameOver.SetActive (true);
	    }

		public void SetScore(int score)
		{
			if (Info.practiceGame)
				scoreTxt.text = score.ToString ();
			else {
				scoreTxt.text = score.ToString () + " / " + finishPoint.ToString ();

				if (Info.practiceGame == false && score >= finishPoint)
					StartCoroutine (_SuccessEndGame ());
			}
		}

		IEnumerator _SuccessEndGame()
		{
			isStart = false;

			ShiningGraphic.Start (imgVictory);
			objVictory.SetActive (true);
			yield return new WaitForSeconds (4f);

			UITweenAlpha.Start (objVictory, 1f, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
			UITweenAlpha.Start (objSendServer, 0f, 1f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));

			yield return new WaitForSeconds (1f);

			if (Info.TableNum == 0)
				ReturnHome ();
			else
				NetworkManager.Instance.Game_Discount_REQ (Info.GameDiscountWon);
		}

		public void ReturnHome()
		{
			SceneChanger.LoadScene ("Main", objBoard);
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

	#if UNITY_ANDROID
			string androidPackageName = "com.newgengames.dartsmathstrainerpro",
						iOSStoreAppID = "145789999";
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