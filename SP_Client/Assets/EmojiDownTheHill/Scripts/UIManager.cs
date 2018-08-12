using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;
using SgLib;

#if EASY_MOBILE
using EasyMobile;
#endif

namespace Emoji2
{
    public class UIManager : MonoBehaviour
    {
        [Header("Object References")]
        public GameManager gameManager;
        public SelectCharacter characterSprite;
        public GameObject header;
        public Text title;
        public Text score;
        public Text bestScore;
        public GameObject nextBtn;
        public GameObject prevBtn;
        public GameObject tapToStart;
        public GameObject restartBtn;
        public GameObject menuButtons;
        public GameObject settingsUI;
        public GameObject soundOnBtn;
        public GameObject soundOffBtn;

        [Header("Premium Features Buttons")]
        public GameObject leaderboardBtn;
        public GameObject shareBtn;
        public GameObject removeAdsBtn;
        public GameObject restorePurchaseBtn;

        Animator scoreAnimator;

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

        // Use this for initialization
        void Start()
        {
            scoreAnimator = score.GetComponent<Animator>();

            Reset();
            ShowStartUI();
        }

        // Update is called once per frame
        void Update()
        {
            score.text = ScoreManager.Instance.Score.ToString();
            bestScore.text = ScoreManager.Instance.HighScore.ToString();

            if (settingsUI.activeSelf)
            {
                UpdateMuteButtons();
            }
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
                Invoke("ShowGameOverUI", 1f);
            }
        }

        void OnScoreUpdated(int newScore)
        {
            scoreAnimator.Play("NewScore");
        }

        void Reset()
        {
            settingsUI.SetActive(false);

            header.SetActive(false);
            title.gameObject.SetActive(false);
            score.gameObject.SetActive(false);
            tapToStart.SetActive(false);
            restartBtn.SetActive(false);
            menuButtons.SetActive(false);
            settingsUI.SetActive(false);

            // Enable or disable premium stuff
            bool enablePremium = PremiumFeaturesManager.Instance.enablePremiumFeatures;
            leaderboardBtn.SetActive(enablePremium);
            shareBtn.SetActive(enablePremium);
            removeAdsBtn.SetActive(enablePremium);
            restorePurchaseBtn.SetActive(enablePremium);

            // Hidden by default
            settingsUI.SetActive(false);
        }

        public void NextCharacter()
        {
            characterSprite.index++;
        }

        public void PrevCharacter()
        {
            characterSprite.index--;
        }

        public void StartGame()
        {
            gameManager.StartGame();
        }

        public void EndGame()
        {
            gameManager.GameOver();
        }

        public void RestartGame()
        {
            gameManager.RestartGame(0.2f);
        }

        public void ShowStartUI()
        {
            settingsUI.SetActive(false);

            header.SetActive(true);
            title.gameObject.SetActive(true);
            tapToStart.SetActive(true);
            nextBtn.SetActive(true);
            prevBtn.SetActive(true);
        }

        public void ShowGameUI()
        {
            header.SetActive(true);
            title.gameObject.SetActive(false);
            score.gameObject.SetActive(true);
            tapToStart.SetActive(false);
            nextBtn.SetActive(false);
            prevBtn.SetActive(false);
        }

        public void ShowGameOverUI()
        {
            header.SetActive(true);
            title.gameObject.SetActive(false);
            score.gameObject.SetActive(true);
            tapToStart.SetActive(false);
            restartBtn.SetActive(true);
            menuButtons.SetActive(true);
            settingsUI.SetActive(false);
        }

        public void ShowSettingsUI()
        {
            settingsUI.SetActive(true);
        }

        public void HideSettingsUI()
        {
            settingsUI.SetActive(false);
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

        public void ShareScreenshot()
        {
            #if EASY_MOBILE
            ScreenshotSharer.Instance.ShareScreenshot();
            #endif
        }

        public void ToggleSound()
        {
            SoundManager.Instance.ToggleMute();
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

        bool IsPremiumFeaturesEnabled()
        {
            return PremiumFeaturesManager.Instance != null && PremiumFeaturesManager.Instance.enablePremiumFeatures;
        }
    }
}