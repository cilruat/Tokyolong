using UnityEngine;
using System.Collections;

#if EASY_MOBILE
using EasyMobile;
#endif

namespace SgLib
{
    public class AdDisplayer : MonoBehaviour
    {
        #if EASY_MOBILE
        public static AdDisplayer Instance { get; private set; }

        [Header("Banner Ad Display Config")]
        [Tooltip("Whether or not to show banner ad")]
        public bool showBannerAd = true;
        public BannerAdPosition bannerAdPosition = BannerAdPosition.Bottom;

        [Header("Interstitial Ad Display Config")]
        [Tooltip("Whether or not to show interstitial ad")]
        public bool showInterstitialAd = true;
        [Tooltip("Show interstitial ad every [how many] games")]
        public int gamesPerInterstitial = 3;
        [Tooltip("How many seconds after game over that interstitial ad is shown")]
        public float showInterstitialDelay = 2f;

        private static int gameCount = 0;

        void OnEnable()
        {
            GameManager.GameStateChanged += OnGameStateChanged;
        }

        void OnDisable()
        {
            GameManager.GameStateChanged -= OnGameStateChanged;
        }

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            // Show banner ad
            if (showBannerAd && !Advertising.IsAdRemoved())
            {
                Advertising.ShowBannerAd(bannerAdPosition);
            }
        }

        void OnGameStateChanged(GameState newState, GameState oldState)
        {       
            if (newState == GameState.GameOver)
            {
                // Show interstitial ad
                if (showInterstitialAd && !Advertising.IsAdRemoved())
                {
                    gameCount++;

                    if (gameCount >= gamesPerInterstitial)
                    {
                        if (Advertising.IsInterstitialAdReady())
                        {
                            // Show default ad after some optional delay
                            StartCoroutine(ShowInterstitial(showInterstitialDelay));

                            // Reset game count
                            gameCount = 0;
                        }
                    }
                }
            }
        }

        IEnumerator ShowInterstitial(float delay = 0f)
        {        
            if (delay > 0)
                yield return new WaitForSeconds(delay);

            Advertising.ShowInterstitialAd();
        }

        #endif
    }
}
