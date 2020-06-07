/********************************************************************
 * Game : Freaking Game
 * Scene : Common
 * Description : GoogleAds
 * History:
 *	2016/09/25	TungNguyen	First Edition
********************************************************************/
using UnityEngine;
using GoogleMobileAds.Api;


namespace NumberTest
{
// Example script showing how to invoke the Google Mobile Ads Unity plugin.
public class GoogleMobileAdsScript : MonoBehaviour
{
	static public BannerView bannerView; 
	static public InterstitialAd inter;

	void Start()
	{
		RequestAdsBanner();
	}

	public void RequestAdsBanner()
	{
		if (Util.isConnect() == true) {
			if(cMgrCommon.isAds == false)
			{
				bannerView = new BannerView("ca-app-pub-8802938959898313/9010249781", AdSize.SmartBanner, AdPosition.Top);
				inter = new InterstitialAd("ca-app-pub-8802938959898313/1486982985");

				// Create an empty ad request.
				AdRequest request = new AdRequest.Builder().Build();

				// Load the banner with the request.
				bannerView.LoadAd(request);

				// Load the interstitial with the request.
				inter.LoadAd(request);

				cMgrCommon.isAds = true;

				bannerView.Hide();
			}
		}
	}

	static public void RequestAdsInter()
	{
		if (cMgrCommon.isAds == true)
		{
			if (Random.Range(1, 4) == 2)
			{
				inter.Show();
			}
		}
	}
}
}