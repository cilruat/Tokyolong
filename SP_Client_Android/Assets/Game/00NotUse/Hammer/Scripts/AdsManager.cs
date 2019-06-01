#if ENABLE_ADMOB_ADS
using GoogleMobileAds.Api;
#endif
using System;
using UnityEngine;

namespace Hammer
{
	public class AdsManager : MonoBehaviour
	{
	#if ENABLE_ADMOB_ADS
	    void Awake()
	    {
	        RequestBanner();
	        RequestInterstitial();
	    }
	    public void ShowBanner()
	    {
	        if (bannerView != null)
	            bannerView.Show();
	        else
	            RequestBanner();
	    }
	    public void HideBanner()
	    {
	        if (bannerView != null)
	            bannerView.Hide();
	    }
	    private BannerView bannerView;
	    private void RequestBanner()
	    {

	#if UNITY_EDITOR
	        string adUnitId = "unused";
	#elif UNITY_ANDROID
	            string adUnitId =  GameSetup.Instance.bannerAd.ANDROID_ID;
	#elif UNITY_IOS
	            string adUnitId = GameSetup.Instance.bannerAd.IOS_ID;
	#else
	            string adUnitId = "unexpected_platform";
	#endif

	        // Create a 320x50 banner at the top of the screen.
	        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
	        // Register for ad events.
	        //bannerView.OnAdFailedToLoad += HandleAdFailedToLoad;
	        bannerView.OnAdLoaded += HandleAdLoaded;
	        // Load a banner ad.
	        bannerView.LoadAd(createAdRequest());
	    }
	    public void HandleAdLoaded(object sender, EventArgs args)
	    {
	        //bannerView.Show();
	        ShowBanner();
	    }
	    //public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	    //{ 
	    //    print("HandleFailedToReceiveAd event received with message: " + args.Message);
	    //    RequestBanner();
	    //}
	    // Returns an ad request with custom ad targeting.
	    private AdRequest createAdRequest()
	    {
	        return new AdRequest.Builder().Build();
	    }
	    private InterstitialAd interstitial;
	    private void RequestInterstitial()
	    {
	#if UNITY_EDITOR
	        string adUnitId = "unused";
	#elif UNITY_ANDROID
	            string adUnitId = GameSetup.Instance.interstitialAd.ANDROID_ID;
	#elif UNITY_IOS
	            string adUnitId = GameSetup.Instance.interstitialAd.IOS_ID;
	#else
	            string adUnitId = "unexpected_platform";
	#endif
	        // Create an interstitial.
	        interstitial = new InterstitialAd(adUnitId);
	        // Register for ad events.
	        //interstitial.OnAdLoaded += HandleInterstitialLoaded;
	        //interstitial.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
	        //this.interstitial.OnAdOpening += this.HandleInterstitialOpened;
	        //this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;
	        this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
	        // Load an interstitial ad.
	        interstitial.LoadAd(createAdRequest());
	    }
	    //public void HandleInterstitialLoaded(object sender, EventArgs args)
	    //{
	    //print("HandleInterstitialLoaded event received.");
	    //if (interstitial != null && interstitial.IsLoaded())
	    //{
	    //    interstitial.Show();
	    //}
	    //}
	    //public void HandleInterstitialOpened(object sender, EventArgs args)
	    //{
	    //    //MonoBehaviour.print("HandleInterstitialOpened event received");
	    //    if (interstitial != null)
	    //        interstitial.Destroy();
	    //    RequestInterstitial();
	    //}


	    //public void HandleInterstitialLeftApplication(object sender, EventArgs args)
	    //{
	    //    //MonoBehaviour.print("HandleInterstitialLeftApplication event received");
	    //    if (interstitial != null)
	    //        interstitial.Destroy();
	    //    RequestInterstitial();
	    //}
	    public void HandleInterstitialClosed(object sender, EventArgs args)
	    {
	        //MonoBehaviour.print("HandleInterstitialClosed event received");
	        //if (interstitial != null)
	        print("Destroying and Loading New Ad.");
	        interstitial.Destroy();
	        RequestInterstitial();
	    }
	#endif
	    public void ShowInterstitial()
	    {
	#if ENABLE_ADMOB_ADS
	        if (interstitial != null && interstitial.IsLoaded())
	        {
	            interstitial.Show();
	        }
	        else if (interstitial == null)
	            RequestInterstitial();
	#endif
	    }

	    private static AdsManager _instance;
	    public static AdsManager Instance
	    {
	        get { if (_instance == null) _instance = GameObject.FindObjectOfType<AdsManager>(); return _instance; }
	    }
	}
}