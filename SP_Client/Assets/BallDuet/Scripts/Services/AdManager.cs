using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ADS 
using UnityEngine.Advertisements;
#endif
#if OG_ADMOB
using GoogleMobileAds.Api;
#endif

namespace OnefallGames
{
	enum AdType
	{
		UnityAd,
		Admob,
	}

	[System.Serializable]
	class ShowAdConfig
	{
		public GameState GamestateForShowAd = GameState.GameOver;
		public AdType AdType = AdType.Admob;
		public int GameStateCountForShowAd = 2;
		public float ShowAdDelay = 1;
	}

    public class AdManager : MonoBehaviour
    {
        public static AdManager Instance { get; set; }
#if UNITY_ADS
        [Header("Unity Ads Config")]
        [SerializeField] private string unityAdsId = "1611450";
        [SerializeField] private string videoAdPlacementID = "video";
        [SerializeField] private string rewardedVideoAdPlacementID = "rewardedVideo";
#endif


#if OG_ADMOB

        [Header("Admob Ads Config")]
        [Header("Admob Id")]
#if UNITY_ANDROID
        [SerializeField]
        private string androidAdmobAppId = "ca-app-pub-1064078647772222~8462516402";
#elif UNITY_IOS
        [SerializeField]
        private string iOSAdmobAppId = "ca-app-pub-1064078647772222~8462516402";
#endif
        [Header("Banner Id")]
#if UNITY_ANDROID
        [SerializeField]
        private string androidBannerId = "ca-app-pub-1064078647772222/9329609006";
#elif UNITY_IOS
        [SerializeField]
        private string iOSBannerId = "ca-app-pub-1064078647772222/9329609006";
#endif
        [SerializeField]
        private AdPosition bannerPosition = AdPosition.Bottom;

        [Header("Interstitial Ad Id")]
#if UNITY_ANDROID
        [SerializeField]
        private string androidInterstitialId = "ca-app-pub-1064078647772222/2139808686";
#elif UNITY_IOS
        [SerializeField]
        private string iOSInterstitialId = "ca-app-pub-1064078647772222/2139808686";
#endif

        [Header("Rewarded Base Video Id")]
#if UNITY_ANDROID
        [SerializeField]
        private string androidRewardedBaseVideoId = "ca-app-pub-1064078647772222/9919321234";
#elif UNITY_IOS
        [SerializeField]
        private string iOSRewardedBaseVideoId = "ca-app-pub-1064078647772222/9919321234";
#endif
#endif

        [Header("Rewarded Video Ad Type Config")]
        [SerializeField]
        private AdType rewardedVideoAdType = AdType.UnityAd;
        [SerializeField]
        private float showRewardedVideoAdDelay = 0.5f;

        [Header("Show Interstitial Ad Config")]
        [SerializeField]
        private List<ShowAdConfig> listShowInterstitialAdConfig = new List<ShowAdConfig>();

        private List<int> listShowAdCount = new List<int>();

#if OG_ADMOB
        private BannerView bannerView;
        private InterstitialAd interstitial;
        private RewardBasedVideoAd rewardBasedVideo;
#endif
        private bool isAdmobRewardedVideoFinished = false;
        private void OnEnable()
        {
            GameManager.GameStateChanged += GameManager_GameStateChanged;
        }

        private void OnDisable()
        {
            GameManager.GameStateChanged -= GameManager_GameStateChanged;
        }

        private void Awake()
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


        // Use this for initialization
        void Start()
        {
            //
            foreach (ShowAdConfig o in listShowInterstitialAdConfig)
            {
                listShowAdCount.Add(o.GameStateCountForShowAd);
            }


            //Init unity ads id
#if UNITY_ADS
            Advertisement.Initialize(unityAdsId);
#endif

#if OG_ADMOB

            // Initialize the Google Mobile Ads SDK.
#if UNITY_ANDROID
            MobileAds.Initialize(androidAdmobAppId);
#elif UNITY_IOS
            MobileAds.Initialize(iOSAdmobAppId);
#endif
            // Get singleton reward based video ad reference.
            rewardBasedVideo = RewardBasedVideoAd.Instance;

            // RewardBasedVideoAd is a singleton, so handlers should only be registered once.
            rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
            rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
#endif
            //Request banner, interstitial and rewarded base video ads
            RequestAdmobBanner();
            RequestAdmobInterstitial();
            RequestAdmobRewardBasedVideo();
        }


        private void GameManager_GameStateChanged(GameState obj)
        {
            for (int i = 0; i < listShowAdCount.Count; i++)
            {
                if (listShowInterstitialAdConfig[i].GamestateForShowAd == obj)
                {
                    listShowAdCount[i]--;
                    if (listShowAdCount[i] <= 0)
                    {
                        //Reset gameCount 
                        listShowAdCount[i] = listShowInterstitialAdConfig[i].GameStateCountForShowAd;

                        if (listShowInterstitialAdConfig[i].AdType == AdType.UnityAd)
                            ShowUnityVideoAd(listShowInterstitialAdConfig[i].ShowAdDelay);
                        else
                            ShowAdmobInterstitialAd(listShowInterstitialAdConfig[i].ShowAdDelay);
                    }
                }
            }
        }


        /// <summary>
        /// Determines whether rewarded video ad is ready.
        /// </summary>
        /// <returns></returns>
        public bool IsRewardedVideoAdReady()
        {
            if (rewardedVideoAdType == AdType.UnityAd)
            {
#if UNITY_ADS
                return Advertisement.IsReady(rewardedVideoAdPlacementID);
#else
                return false;
#endif
            }
            else
            {
#if OG_ADMOB
                if (rewardBasedVideo.IsLoaded())
                    return true;
                else
                {
                    RequestAdmobRewardBasedVideo();
                    return false;
                }
#else
                return false;
#endif
            }
        }


        /// <summary>
        /// Show the rewarded video ad with delay time
        /// </summary>
        /// <param name="delay"></param>
        public void ShowRewardedVideoAd()
        {
            if (rewardedVideoAdType == AdType.UnityAd)
            {
                ShowUnityRewardedVideo(showRewardedVideoAdDelay);
            }
            else
            {
                ShowAdmobRewardBasedVideoAd(showRewardedVideoAdDelay);
            }
        }





        /// <summary>
        /// Show the video ad with delay time
        /// </summary>
        /// <param name="delay"></param>
        public void ShowUnityVideoAd(float delay)
        {
#if UNITY_ADS
            if (Advertisement.IsReady(videoAdPlacementID))
                StartCoroutine(UnityVideoAd(delay));
#endif
        }

#if UNITY_ADS
        IEnumerator UnityVideoAd(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            ShowOptions option = new ShowOptions();
            Advertisement.Show(videoAdPlacementID, option);
        }
#endif

        /// <summary>
        /// Show unity rewarded video with delay time
        /// </summary>
        /// <param name="delay"></param>
        public void ShowUnityRewardedVideo(float delay)
        {
#if UNITY_ADS
            if (Advertisement.IsReady(rewardedVideoAdPlacementID))
                StartCoroutine(UnityRewardedVideoAd(delay));
#endif
        }

#if UNITY_ADS
        IEnumerator UnityRewardedVideoAd(float delay)
        {
            yield return new WaitForSeconds(delay);
            ShowOptions option = new ShowOptions();
            option.resultCallback = OnUnityRewardedAdShowResult;
            Advertisement.Show(rewardedVideoAdPlacementID, option);
        }
#endif

#if UNITY_ADS
        private void OnUnityRewardedAdShowResult(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Finished:
                    {
                        HandleOnRewardedVideoClosed();
                        break;
                    }
            }
        }
#endif

        /// <summary>
        /// Request admob banner ad
        /// </summary>
        public void RequestAdmobBanner()
        {
#if OG_ADMOB
            // Clean up banner ad before creating a new one.
            if (bannerView != null)
            {
                bannerView.Destroy();
            }

            // Create a 320x50 banner at the top of the screen.
#if UNITY_ANDROID
            bannerView = new BannerView(androidBannerId, AdSize.SmartBanner, bannerPosition);
#elif UNITY_IOS
            bannerView = new BannerView(iOSBannerId, AdSize.SmartBanner, bannerPosition);
#endif
            // Load a banner ad.
            bannerView.LoadAd(new AdRequest.Builder().Build());

#endif
        }

        //Request admob interstitial ad
        private void RequestAdmobInterstitial()
        {
#if OG_ADMOB
            // Clean up interstitial ad before creating a new one.
            if (interstitial != null)
            {
                interstitial.Destroy();
            }

            // Create an interstitial.
#if UNITY_ANDROID
            interstitial = new InterstitialAd(androidInterstitialId);
#elif UNITY_IOS
            interstitial = new InterstitialAd(iOSInterstitialId);
#endif
            // Register for ad events.
            interstitial.OnAdClosed += HandleInterstitialClosed;

            // Load an interstitial ad.
            interstitial.LoadAd(new AdRequest.Builder().Build());
#endif
        }

        private void RequestAdmobRewardBasedVideo()
        {
#if OG_ADMOB
#if UNITY_ANDROID
            rewardBasedVideo.LoadAd(new AdRequest.Builder().Build(), androidRewardedBaseVideoId);
#elif UNITY_IOS
            rewardBasedVideo.LoadAd(new AdRequest.Builder().Build(), iOSRewardedBaseVideoId);
#endif
#endif
        }


        /// <summary>
        /// Show admob interstitial ad with delay time 
        /// </summary>
        /// <param name="delay"></param>
        public void ShowAdmobInterstitialAd(float delay)
        {
            StartCoroutine(ShowAdmobInterstitial(delay));
        }

        IEnumerator ShowAdmobInterstitial(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
#if OG_ADMOB
            if (interstitial.IsLoaded())
            {
                interstitial.Show();
            }
            else
            {
                RequestAdmobInterstitial();
            }
#endif
        }



        /// <summary>
        /// Show admob rewarded base video ad with delay time 
        /// </summary>
        /// <param name="delay"></param>
        public void ShowAdmobRewardBasedVideoAd(float delay)
        {
            StartCoroutine(ShowAdmobRewardBasedVideo(delay));
        }
        IEnumerator ShowAdmobRewardBasedVideo(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
#if OG_ADMOB
            if (rewardBasedVideo.IsLoaded())
            {
                rewardBasedVideo.Show();
            }
            else
            {
                RequestAdmobRewardBasedVideo();
            }
#endif
        }


        private void HandleOnRewardedVideoClosed()
        {
            if (rewardedVideoAdType == AdType.Admob) //Admob ads
            {
                if (isAdmobRewardedVideoFinished) //User complet the video
                {
                    GameManager.Instance.SetContinueGame();
                }
                else //User skip the video
                {
                    GameManager.Instance.GameOver();
                }
                isAdmobRewardedVideoFinished = false;
            }
            else //Unity ads
            {
                GameManager.Instance.SetContinueGame();
            }
        }



        //Events callback
        public void HandleInterstitialClosed(object sender, EventArgs args)
        {
            RequestAdmobInterstitial();
        }

#if OG_ADMOB
        private void HandleRewardBasedVideoRewarded(object sender, Reward args)
        {
            isAdmobRewardedVideoFinished = true;
        }
#endif
        private void HandleRewardBasedVideoClosed(object sender, EventArgs args)
        {
            RequestAdmobRewardBasedVideo();
            HandleOnRewardedVideoClosed();
        }
    }
}