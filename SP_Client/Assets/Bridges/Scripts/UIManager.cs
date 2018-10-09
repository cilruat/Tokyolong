using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;
using SgLib;

#if EASY_MOBILE
using EasyMobile;
#endif

namespace Bridges
{
	public class UIManager : MonoBehaviour
	{
	    public static bool firstLoad = true;
		public static UIManager Instance { get; private set; }

	    [Header("Object References")]
	    public GameManager gameManager;
	    public CameraController camController;
	    public DailyRewardController dailyRewardController;

	    public GameObject mainCanvas;
	    public GameObject settingsCanvas;
	    public GameObject storeCanvas;

	    public GameObject header;
	    public Text score;
	    public Text bestScore;
	    public Text gold;
	    public Text title;
	    public GameObject tapToStart;
	    public GameObject tapToContinue;
	    public GameObject characterSelectBtn;
	    public GameObject menuButtons;
	    public GameObject continueLostGame;
	    public GameObject continueByCoinsBtn;
	    public Text dailyRewardBtnText;
	    public GameObject dailyRewardBtn;
	    public GameObject rewardUI;
	    public GameObject soundOffBtn;
	    public GameObject soundOnBtn;
	    public GameObject musicOnBtn;
	    public GameObject musicOffBtn;

	    [Header("Premium Features Only")]
	    public GameObject continueByAdsBtn;
	    public GameObject watchForCoinsBtn;
	    public GameObject leaderboardBtn;
	    public GameObject iapPurchaseBtn;
	    public GameObject removeAdsBtn;
	    public GameObject restorePurchaseBtn;

	    [Header("Sharing-Specific")]
	    public GameObject shareUI;
	    public ShareUIController shareUIController;

		public GameObject tapToStart2;
		public RawImage imgVictory;
		public GameObject objVictory;
		public GameObject objSendServer;
		public GameObject objGameOver;
		public GameObject objReady;
		public GameObject objGo;

		int finishPoint = 0;

	    Animator scoreAnimator;
	    Animator dailyRewardAnimator;
	    bool isWatchAdsForCoinBtnActive;

	    void OnEnable()
	    {
	        GameManager.NewGameEvent += GameManager_NewGameEvent;
	        ScoreManager.ScoreUpdated += OnScoreUpdated;
	    }

	    void OnDisable()
	    {
	        GameManager.NewGameEvent -= GameManager_NewGameEvent;
	        ScoreManager.ScoreUpdated -= OnScoreUpdated;
	    }

		void Awake()
		{
			if (Instance)
			{
				DestroyImmediate(gameObject);
			}
			else
			{
				Instance = this;
				//DontDestroyOnLoad(gameObject);
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
	    void Start()
	    {
	        scoreAnimator = score.GetComponent<Animator>();
	        dailyRewardAnimator = dailyRewardBtn.GetComponent<Animator>();

	        Reset();
	        ShowStartUI();

			finishPoint = Info.BRIDGES_FINISH_POINT;
			score.text = Info.practiceGame ? "0" : "0 / " + finishPoint.ToString ();
	    }

	    // Update is called once per frame
	    void Update()
	    {
			return;

	        if (mainCanvas.activeSelf)
	        {
	            score.text = ScoreManager.Instance.Score.ToString();
	            bestScore.text = ScoreManager.Instance.HighScore.ToString();
	            gold.text = CoinManager.Instance.Coins.ToString();

	            if (!DailyRewardController.Instance.disable && dailyRewardBtn.gameObject.activeSelf)
	            {
	                TimeSpan timeToReward = DailyRewardController.Instance.TimeUntilReward;

	                if (timeToReward <= TimeSpan.Zero)
	                {
	                    dailyRewardBtnText.text = "GRAB YOUR REWARD!";
	                    dailyRewardAnimator.SetTrigger("activate");
	                }
	                else
	                {
	                    dailyRewardBtnText.text = string.Format("REWARD IN {0:00}:{1:00}:{2:00}", timeToReward.Hours, timeToReward.Minutes, timeToReward.Seconds);
	                    dailyRewardAnimator.SetTrigger("deactivate");
	                }
	            }
	        }

	        if (settingsCanvas.activeSelf)
	        {
	            UpdateMuteButtons();
	            UpdateMusicButtons();
	        }
	    }

	    void GameManager_NewGameEvent(GameEvent e)
	    {
	        if (e == GameEvent.Start)
	        {              
	            ShowGameUI();
	        }
	        else if (e == GameEvent.PreGameOver)
	        {
	            // Before game over, i.e. game potentially will be recovered
	        }
	        else if (e == GameEvent.GameOver)
	        {
	            Invoke("ShowGameOverUI", 0.5f);
	        }
	    }

		IEnumerator _StartGame()
		{
			UITweenAlpha.Start (tapToStart2, 1f, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2).DisableOnFinish ());
			yield return new WaitForSeconds (.5f);

			UITweenAlpha.Start(objReady, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

			yield return new WaitForSeconds (1f);
			UITweenAlpha.Start(objReady, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

			yield return new WaitForSeconds (.25f);
			UITweenAlpha.Start(objGo, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

			yield return new WaitForSeconds (1f);
			UITweenAlpha.Start(objGo, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

			yield return new WaitForSeconds (.3f);

			gameManager.StartGame();
		}

	    void OnScoreUpdated(int newScore)
	    {
			scoreAnimator.Play("NewScore");

			if (Info.practiceGame)
				score.text = newScore.ToString ();
			else {
				score.text = newScore.ToString () + " / " + finishPoint.ToString ();

				if (Info.practiceGame == false && newScore >= finishPoint)
					StartCoroutine (_SuccessEndGame ());
			}
	    }

		IEnumerator _SuccessEndGame()
		{
			gameManager.gameOver = true;

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
			SceneChanger.LoadScene ("Main", mainCanvas);
		}

	    void Reset()
	    {
	        mainCanvas.SetActive(true);
	        settingsCanvas.SetActive(false);

	        header.SetActive(false);
	        title.gameObject.SetActive(false);
	        score.gameObject.SetActive(false);
	        tapToStart.SetActive(false);
	        tapToContinue.SetActive(false);
	        characterSelectBtn.SetActive(false);
	        menuButtons.SetActive(false);
	        dailyRewardBtn.SetActive(false);
	        continueLostGame.SetActive(false);
	        continueByCoinsBtn.SetActive(false);
	        settingsCanvas.SetActive(false);

	        // Enable or disable premium stuff
	        /*bool enablePremium = PremiumFeaturesManager.Instance.enablePremiumFeatures;
	        leaderboardBtn.SetActive(enablePremium);
	        iapPurchaseBtn.SetActive(enablePremium);
	        removeAdsBtn.SetActive(enablePremium);
	        restorePurchaseBtn.SetActive(enablePremium);*/

	        // Hide Share screnenshot by default
	        //shareUI.SetActive(false);

	        // These premium feature buttons are hidden by default
	        // and shown when certain criteria are met (e.g. rewarded ad is loaded)
	        /*continueByAdsBtn.SetActive(false);
	        watchForCoinsBtn.gameObject.SetActive(false);*/
	    }

	    public void ShowStartUI()
	    {
	        mainCanvas.SetActive(true);
	        settingsCanvas.SetActive(false);
			score.gameObject.SetActive(true);

	        /*header.SetActive(true);
	        title.gameObject.SetActive(true);
	        tapToStart.SetActive(true);
	        characterSelectBtn.SetActive(true);*/
	    }

	    public void ShowGameUI()
	    {			
			return;

	        header.SetActive(true);
	        title.gameObject.SetActive(false);
	        score.gameObject.SetActive(true);
	        tapToStart.SetActive(false);
	        tapToContinue.SetActive(false);
	        characterSelectBtn.SetActive(false);
	    }

	    public void ShowContinueLostGameUI(bool canUseCoins, bool canWatchAd)
	    {
	        continueByCoinsBtn.SetActive(canUseCoins);
	        continueByAdsBtn.SetActive(canWatchAd);

	        continueLostGame.SetActive(true);
	    }

	    public void ShowResumeUI()
	    {
			return;

	        tapToContinue.SetActive(true);
	        score.gameObject.SetActive(true);
	    }

	    public void ShowGameOverUI()
	    {
			objGameOver.SetActive (true);
			return;

	        header.SetActive(true);
	        title.gameObject.SetActive(false);
	        score.gameObject.SetActive(true);
	        tapToStart.SetActive(false);
	        menuButtons.SetActive(true);

	        continueLostGame.SetActive(false);
	        watchForCoinsBtn.gameObject.SetActive(false);
	        settingsCanvas.SetActive(false);

	        // Only show "watch for coins button" if a rewarded ad is loaded and premium features are enabled
	#if EASY_MOBILE
	        if (IsPremiumFeaturesEnabled() && AdDisplayer.Instance.CanShowRewardedAd())
	        {
	            watchForCoinsBtn.SetActive(true);
	            watchForCoinsBtn.GetComponent<Animator>().SetTrigger("activate");
	        }
	        else
	        {
	            watchForCoinsBtn.SetActive(false);
	        }
	#endif

	        // Not showing the daily reward button if the feature is disabled
	        if (!DailyRewardController.Instance.disable)
	        {
	            dailyRewardBtn.SetActive(true);
	        }

	        if (IsPremiumFeaturesEnabled())
	        {
	            ShowShareUI();
	        }
	    }

	    public void ShowSettingsUI()
	    {
	        mainCanvas.SetActive(false);
	        settingsCanvas.SetActive(true);
	    }

	    public void HideSettingsUI()
	    {
	        mainCanvas.SetActive(true);
	        settingsCanvas.SetActive(false);
	    }

	    public void ShowStoreUI()
	    {
	        mainCanvas.SetActive(false);
	        storeCanvas.SetActive(true);
	    }

	    public void HideStoreUI()
	    {
	        mainCanvas.SetActive(true);
	        storeCanvas.SetActive(false);
	    }

	    public void StartGame()
	    {
	        //gameManager.StartGame();
			StartCoroutine(_StartGame());
	    }

	    public void ResumeLostGameByCoins()
	    {
	        ResumeLostGame(true);
	    }

	    public void ResumeLostGameByAds()
	    {
	        #if EASY_MOBILE
	        AdDisplayer.CompleteRewardedAdToRecoverLostGame += OnCompleteRewardedAdToRecoverLostGame;
	        AdDisplayer.Instance.ShowRewardedAdToRecoverLostGame();
	        #endif
	    }

	    void OnCompleteRewardedAdToRecoverLostGame()
	    {
	        #if EASY_MOBILE
	        // Unsubscribe
	        AdDisplayer.CompleteRewardedAdToRecoverLostGame -= OnCompleteRewardedAdToRecoverLostGame;

	        // Resume game
	        ResumeLostGame(false);
	        #endif
	    }

	    public void ResumeLostGame(bool useCoins)
	    {
	        continueLostGame.SetActive(false);
	        ShowResumeUI();
	        gameManager.RecoverLostGame(useCoins);
	    }

	    public void EndGame()
	    {
	        gameManager.GameOver();
	    }

	    public void RestartGame()
	    {
	        gameManager.RestartGame(0.2f);
	    }

	    public void WatchRewardedAdForCoins()
	    {
	        #if EASY_MOBILE
	        // Hide the button
	        watchForCoinsBtn.SetActive(false);

	        AdDisplayer.CompleteRewardedAdToEarnCoins += OnCompleteRewardedAdToEarnCoins;
	        AdDisplayer.Instance.ShowRewardedAdToEarnCoins();
	        #endif
	    }

	    void OnCompleteRewardedAdToEarnCoins()
	    {
	        #if EASY_MOBILE
	        // Unsubscribe
	        AdDisplayer.CompleteRewardedAdToEarnCoins -= OnCompleteRewardedAdToEarnCoins;

	        // Give the coins!
	        ShowRewardUI(AdDisplayer.Instance.rewardedCoins);
	        #endif
	    }

	    public void GrabDailyReward()
	    {
	        if (DailyRewardController.Instance.TimeUntilReward <= TimeSpan.Zero)
	        {
	            float reward = UnityEngine.Random.Range(dailyRewardController.minRewardValue, dailyRewardController.maxRewardValue);

	            // Round the number and make it mutiplies of 5 only.
	            int roundedReward = (Mathf.RoundToInt(reward) / 5) * 5;

	            // Show the reward UI
	            ShowRewardUI(roundedReward);

	            // Update next time for the reward
	            DailyRewardController.Instance.SetNextRewardTime(dailyRewardController.rewardIntervalHours, dailyRewardController.rewardIntervalMinutes, dailyRewardController.rewardIntervalSeconds);
	        }
	    }

	    public void ShowRewardUI(int reward)
	    {
	        rewardUI.SetActive(true);
	        rewardUI.GetComponent<RewardUIController>().Reward(reward);
	    }

	    public void HideRewardUI()
	    {
	        rewardUI.SetActive(false);
	    }

	    public void ShowLeaderboardUI()
	    {
	        #if EASY_MOBILE
	        if (GameServices.IsInitialized())
	        {
	            GameServices.ShowLeaderboardUI();
	        }
	        else
	        {
	            #if UNITY_IOS
	            NativeUI.Alert("Service Unavailable", "The user is not logged in to Game Center.");
	            #elif UNITY_ANDROID
	            GameServices.Init();
	            #endif
	        }
	        #endif
	    }

	    public void PurchaseRemoveAds()
	    {
	        #if EASY_MOBILE
	        InAppPurchaser.Instance.Purchase(InAppPurchaser.Instance.removeAds);
	        #endif
	    }

	    public void RestorePurchase()
	    {
	        #if EASY_MOBILE
	        InAppPurchaser.Instance.RestorePurchase();
	        #endif
	    }

	    public void ShowShareUI()
	    {
	        //StartCoroutine(CRShowShareUI());
	    }

	    IEnumerator CRShowShareUI()
	    {
	        yield return new WaitForSeconds(0.6F);
	        if (!ScreenshotSharer.Instance.disableSharing)
	        {
	            Texture2D texture = ScreenshotSharer.Instance.CapturedScreenshot;
	            shareUIController.ImgTex = texture;

	#if EASY_MOBILE_PRO
	            AnimatedClip clip = ScreenshotSharer.Instance.RecordedClip;
	            shareUIController.AnimClip = clip;
	#endif

	            shareUI.SetActive(true);
	        }
	    }

	    public void HideShareUI()
	    {
	        shareUI.SetActive(false);
	    }

	    public void ToggleSound()
	    {
	        SoundManager.Instance.ToggleMute();
	    }

	    public void ToggleMusic()
	    {
	        SoundManager.Instance.ToggleMusic();
	    }

	    public void SelectCharacter()
	    {
	        SceneManager.LoadScene("CharacterSelection");
	    }

	    void UpdateMuteButtons()
	    {
	        if (SoundManager.Instance.IsMuted())
	        {
	            soundOnBtn.gameObject.SetActive(false);
	            soundOffBtn.gameObject.SetActive(true);
	        }
	        else
	        {
	            soundOnBtn.gameObject.SetActive(true);
	            soundOffBtn.gameObject.SetActive(false);
	        }
	    }

	    void UpdateMusicButtons()
	    {
	        if (SoundManager.Instance.IsMusicOff())
	        {
	            musicOffBtn.gameObject.SetActive(true);
	            musicOnBtn.gameObject.SetActive(false);
	        }
	        else
	        {
	            musicOffBtn.gameObject.SetActive(false);
	            musicOnBtn.gameObject.SetActive(true);
	        }
	    }

	    bool IsPremiumFeaturesEnabled()
	    {
	        return PremiumFeaturesManager.Instance != null && PremiumFeaturesManager.Instance.enablePremiumFeatures;
	    }
	}
}