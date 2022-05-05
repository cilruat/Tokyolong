using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;
using SgLib;

#if EASY_MOBILE
using EasyMobile;
#endif

namespace CrashRacing
{
	public class UIManager : MonoBehaviour
	{
		public static UIManager Instance { get; private set; }

	    [Header("Object References")]
	    public PlayerController playerController;
	    public GameObject mainCanvas;
	    public GameObject characterSelectionUI;
	    public GameObject header;
	    public GameObject title;
	    public Text score;
	    public Text bestScore;
	    public Text coinText;
	    public GameObject newBestScore;
	    public GameObject playBtn;
	    public GameObject restartBtn;
	    public GameObject menuButtons;
	    public GameObject dailyRewardBtn;
	    public Text dailyRewardBtnText;
	    public GameObject rewardUI;
	    public GameObject settingsUI;
	    public GameObject soundOnBtn;
	    public GameObject soundOffBtn;
	    public GameObject musicOnBtn;
	    public GameObject musicOffBtn;
	    public GameObject tutorial;
	    public GameObject swipeLR;
	    public GameObject swipeUp;
	    public GameObject velocityBoard;
	    public Text velocityText;
	    public GameObject velocityNote;

	    [Header("Premium Features Buttons")]
	    public GameObject watchRewardedAdBtn;
	    public GameObject leaderboardBtn;
	    public GameObject achievementBtn;
	    public GameObject iapPurchaseBtn;
	    public GameObject removeAdsBtn;
	    public GameObject restorePurchaseBtn;

	    [Header("In-App Purchase Store")]
	    public GameObject storeUI;

	    [Header("Sharing-Specific")]
	    public GameObject shareUI;
	    public ShareUIController shareUIController;

		public GameObject tapToStart;
		public RawImage imgVictory;
		public GameObject objVictory;
		public GameObject objSendServer;
		public GameObject objGameOver;
		public GameObject objReady;
		public GameObject objGo;
		public Text txtTime;
		public Image imgTime;
		public CountDown limitTime;

		public bool isStop = false;
		int finishLimitTime = 0;

	    Animator scoreAnimator;
	    //Animator dailyRewardAnimator;
	    bool isWatchAdsForCoinBtnActive;
	    float timeCount;
	    float maxSpeed;

	    void OnEnable()
	    {
	        GameManager.GameStateChanged += GameManager_GameStateChanged;
	        ScoreManager.ScoreUpdated += OnScoreUpdated;
	    }

	    void OnDisable()
	    {
	        GameManager.GameStateChanged -= GameManager_GameStateChanged;
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
	        //dailyRewardAnimator = dailyRewardBtn.GetComponent<Animator>();

	        Reset();
	        ShowStartUI();

			finishLimitTime = Info.CRASH_RACING_LIMIT_TIME;
			txtTime.text = Info.practiceGame ? "∞" : finishLimitTime.ToString();
	    }

	    // Update is called once per frame
	    void Update()
	    {
			if (Info.practiceGame)
				return;

			float elapsed = limitTime.GetElapsed();
			float fill = (finishLimitTime - elapsed) / (float)finishLimitTime;
			imgTime.fillAmount = fill;

			return;

	        /*if (GameManager.Instance.GameState == GameState.Playing)
	        {
	            timeCount += Time.deltaTime;
	            if (timeCount > 0.25f)
	            {
	                velocityText.text = ((int)playerController.currentSpeed).ToString();
	                if (maxSpeed < playerController.currentSpeed)
	                    maxSpeed = playerController.currentSpeed;
	                timeCount = 0;
	            }
	        }

	        score.text = ScoreManager.Instance.Score.ToString();
	        bestScore.text = ScoreManager.Instance.HighScore.ToString();
	        coinText.text = CoinManager.Instance.Coins.ToString();

	        if (!DailyRewardController.Instance.disable && dailyRewardBtn.gameObject.activeInHierarchy)
	        {
	            if (DailyRewardController.Instance.CanRewardNow())
	            {
	                dailyRewardBtnText.text = "GRAB YOUR REWARD!";
	                dailyRewardAnimator.SetTrigger("activate");
	            }
	            else
	            {
	                TimeSpan timeToReward = DailyRewardController.Instance.TimeUntilReward;
	                dailyRewardBtnText.text = string.Format("REWARD IN {0:00}:{1:00}:{2:00}", timeToReward.Hours, timeToReward.Minutes, timeToReward.Seconds);
	                dailyRewardAnimator.SetTrigger("deactivate");
	            }
	        }

	        if (settingsUI.activeSelf)
	        {
	            UpdateSoundButtons();
	            UpdateMusicButtons();
	        }*/
	    }

	    void GameManager_GameStateChanged(GameState newState, GameState oldState)
	    {
	        if (newState == GameState.Playing)
	        {
	            ShowGameUI();
	        }
	        else if (newState == GameState.PreGameOver)
	        {
	            // Before game over, i.e. game potentially will be recovered
	        }
	        else if (newState == GameState.GameOver)
	        {
	            Invoke("ShowGameOverUI", 1.2f);
	        }
	    }

	    void OnScoreUpdated(int newScore)
	    {
	        scoreAnimator.Play("NewScore");
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

			GameManager.Instance.StartGame();

			if (Info.practiceGame == false)
				limitTime.Set (finishLimitTime, () => StartCoroutine (_SuccessEndGame ()));
		}

		IEnumerator _SuccessEndGame()
		{
			isStop = true;
			limitTime.Stop ();

			GameObject cars = GameManager.Instance.activeCarManager.transform.gameObject;
			for (int i = 0; i < cars.transform.childCount; i++) {
				GameObject theCar = cars.transform.GetChild(i).gameObject;
				theCar.GetComponent<CarController>().stopMoving = true;    
				theCar.GetComponent<CarController> ().stopTurn = true;
			}

			ShiningGraphic.Start (imgVictory);
			objVictory.SetActive (true);
			yield return new WaitForSeconds (4f);

			UITweenAlpha.Start (objVictory, 1f, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
			UITweenAlpha.Start (objSendServer, 0f, 1f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));

			yield return new WaitForSeconds (1f);

			if (Info.TableNum == 0)
				ReturnHome ();
			else
                NetworkManager.Instance.GameCountInput_REQ(Info.TableNum, Info.GameCoinNum + 2);
                Info.AfterDiscountBehavior();
                Info.SlotWin = true;
        }

        public void ReturnHome()
		{
			SceneChanger.LoadScene ("SelectGame", mainCanvas);
		}


	    void Reset()
	    {
	        mainCanvas.SetActive(true);
	        characterSelectionUI.SetActive(false);
	        header.SetActive(false);
	        title.SetActive(false);
	        score.gameObject.SetActive(false);
	        newBestScore.SetActive(false);
	        playBtn.SetActive(false);
	        menuButtons.SetActive(false);
	        dailyRewardBtn.SetActive(false);
	        velocityBoard.SetActive(false);

	        // Enable or disable premium stuff
	        /*bool enablePremium = IsPremiumFeaturesEnabled();
	        leaderboardBtn.SetActive(enablePremium);
	        iapPurchaseBtn.SetActive(enablePremium);
	        removeAdsBtn.SetActive(enablePremium);
	        restorePurchaseBtn.SetActive(enablePremium);

	        // Hidden by default
	        storeUI.SetActive(false);
	        settingsUI.SetActive(false);
	        shareUI.SetActive(false);

	        // These premium feature buttons are hidden by default
	        // and shown when certain criteria are met (e.g. rewarded ad is loaded)
	        watchRewardedAdBtn.gameObject.SetActive(false);*/
	    }

	    public void StartGame()
	    {
	        //GameManager.Instance.StartGame();
			StartCoroutine(_StartGame());
	    }

	    public void EndGame()
	    {
	        GameManager.Instance.GameOver();
            Info.SlotLose = true;

        }

        public void RestartGame()
	    {
	        GameManager.Instance.RestartGame(0.2f);
	    }

	    public void ShowStartUI()
	    {	
			return;

	        /*settingsUI.SetActive(false);      
	        header.SetActive(true);
	        title.SetActive(true);
	        playBtn.SetActive(true);
	        restartBtn.SetActive(false);
	        menuButtons.SetActive(true);
	        shareUI.SetActive(false);
	        velocityBoard.SetActive(false);*/

	        // If first launch: show "WatchForCoins" and "DailyReward" buttons if the conditions are met
	        /*if (GameManager.GameCount == 0)
	        {
	            ShowWatchForCoinsBtn();
	            ShowDailyRewardBtn();
	        }*/
	    }

	    public void ShowGameUI()
	    {
			return;

	        /*header.SetActive(true);
	        title.SetActive(false);
	        score.gameObject.SetActive(true);
	        playBtn.SetActive(false);
	        menuButtons.SetActive(false);
	        dailyRewardBtn.SetActive(false);
	        watchRewardedAdBtn.SetActive(false);
	        shareUI.SetActive(false);
	        velocityNote.SetActive(false);
	        velocityBoard.SetActive(true);*/
	    }

	    public void ShowGameOverUI()
	    {			
			objGameOver.SetActive (true);

            return;
			
	        /*header.SetActive(true);
	        title.SetActive(false);
	        score.gameObject.SetActive(true);
	        newBestScore.SetActive(ScoreManager.Instance.HasNewHighScore);

	        playBtn.SetActive(false);
	        restartBtn.SetActive(true);
	        menuButtons.SetActive(true);
	        settingsUI.SetActive(false);
	        velocityText.text = ((int)maxSpeed).ToString();
	        velocityNote.SetActive(true);*/

	        // Show 'daily reward' button
	        //ShowDailyRewardBtn();

	        // Show these if premium features are enabled (and relevant conditions are met)
	        /*if (IsPremiumFeaturesEnabled())
	        {
	            ShowShareUI();
	            ShowWatchForCoinsBtn();
	        }*/
	    }

	    public void ShowTutorial()
	    {
	        tutorial.SetActive(true);
	        swipeLR.SetActive(true);
	        swipeUp.SetActive(false);
	    }

	    public void ShowSwipeup()
	    {
	        swipeLR.SetActive(false);
	        swipeUp.SetActive(true);
	    }

	    public void HideTutorial()
	    {
	        tutorial.SetActive(false);
	    }
	    void ShowWatchForCoinsBtn()
	    {
	        // Only show "watch for coins button" if a rewarded ad is loaded and premium features are enabled
	#if EASY_MOBILE
	        if (IsPremiumFeaturesEnabled() && AdDisplayer.Instance.CanShowRewardedAd() && AdDisplayer.Instance.watchAdToEarnCoins)
	        {
	            watchRewardedAdBtn.SetActive(true);
	            watchRewardedAdBtn.GetComponent<Animator>().SetTrigger("activate");
	        }
	        else
	        {
	            watchRewardedAdBtn.SetActive(false);
	        }
	#endif
	    }

	    void ShowDailyRewardBtn()
	    {
	        // Not showing the daily reward button if the feature is disabled
	        if (!DailyRewardController.Instance.disable)
	        {
	            dailyRewardBtn.SetActive(true);
	        }
	    }

	    public void ShowSettingsUI()
	    {
	        settingsUI.SetActive(true);
	    }

	    public void HideSettingsUI()
	    {
	        settingsUI.SetActive(false);
	    }

	    public void ShowStoreUI()
	    {
	        storeUI.SetActive(true);
	    }

	    public void HideStoreUI()
	    {
	        storeUI.SetActive(false);
	    }

	    public void ShowCharacterSelectionScene()
	    {
	        mainCanvas.SetActive(false);
	        characterSelectionUI.SetActive(true);
	    }

	    public void CloseCharacterSelectionScene()
	    {
	        mainCanvas.SetActive(true);
	        characterSelectionUI.SetActive(false);
	    }

	    public void WatchRewardedAd()
	    {
	#if EASY_MOBILE
	        // Hide the button
	        watchRewardedAdBtn.SetActive(false);

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
	        if (DailyRewardController.Instance.CanRewardNow())
	        {
	            int reward = DailyRewardController.Instance.GetRandomReward();

	            // Round the number and make it mutiplies of 5 only.
	            int roundedReward = (reward / 5) * 5;

	            // Show the reward UI
	            ShowRewardUI(roundedReward);

	            // Update next time for the reward
	            DailyRewardController.Instance.ResetNextRewardTime();
	        }
	    }

	    public void ShowRewardUI(int reward)
	    {
	        rewardUI.SetActive(true);
	        rewardUI.GetComponent<RewardUIController>().Reward(reward);
	    }

	    public void HideRewardUI()
	    {
	        rewardUI.GetComponent<RewardUIController>().Close();
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

	    public void ShowAchievementsUI()
	    {
	#if EASY_MOBILE
	        if (GameServices.IsInitialized())
	        {
	            GameServices.ShowAchievementsUI();
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
	        SoundManager.Instance.ToggleSound();
	    }

	    public void ToggleMusic()
	    {
	        SoundManager.Instance.ToggleMusic();
	    }

	    public void RateApp()
	    {
	        Utilities.RateApp();
	    }

	    public void OpenTwitterPage()
	    {
	        Utilities.OpenTwitterPage();
	    }

	    public void OpenFacebookPage()
	    {
	        Utilities.OpenFacebookPage();
	    }

	    public void ButtonClickSound()
	    {
	        Utilities.ButtonClickSound();
	    }

	    void UpdateSoundButtons()
	    {
	        if (SoundManager.Instance.IsSoundOff())
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