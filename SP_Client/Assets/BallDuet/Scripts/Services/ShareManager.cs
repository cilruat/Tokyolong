using System.IO;
using UnityEngine;

namespace OnefallGames
{
    public class ShareManager : MonoBehaviour
    {

        public static ShareManager Instance { get; private set; }

        [Header("Native Sharing Config")]
        [SerializeField] private string screenshotName = "screenshot.png";
        [SerializeField] private string shareText = "Can you beat my score!!!";
        [SerializeField] private string shareSubject = "Share Via";
        [SerializeField] private string appUrl = "https://play.google.com/store/apps/details?id=com.onefall.HeavenStairs";

        [Header("Twitter Sharing Config")]
        [SerializeField] private string titterAddress = "http://twitter.com/intent/tweet";
        [SerializeField] private string textToDisplay = "Hey Guys! Check out my score: ";
        [SerializeField] private string tweetLanguage = "en";

        [Header("Facebook Sharing Config")]
        [SerializeField] private string fbAppID = "1013093142200006";
        [SerializeField] private string caption = "Check out My New Score: ";
        [Tooltip("The URL of a picture attached to this post.The Size must be atleat 200px by 200px.If you dont want to share picture, leave this field empty.")]
        [SerializeField] private string pictureUrl = "http://i-cdn.phonearena.com/images/article/85835-thumb/Google-Pixel-3-codenamed-Bison-to-be-powered-by-Andromeda-OS.jpg";
        [SerializeField] private string description = "Enjoy Fun, free games! Challenge yourself or share with friends. Fun and easy to use games.";

        public string ScreenshotPath { private set; get; }
        public string AppUrl { private set; get; }

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

            AppUrl = appUrl;
        }

        /// <summary>
        /// Create the screenshot
        /// </summary>
        public void CreateScreenshot()
        {
            ScreenshotPath = Path.Combine(Application.persistentDataPath, screenshotName);
#if UNITY_EDITOR
            ScreenCapture.CaptureScreenshot(ScreenshotPath);
#else
            ScreenCapture.CaptureScreenshot(screenshotName);
#endif
        }


        /// <summary>
        /// Share screenshot with text
        /// </summary>
        public void NativeShare()
        {
            Share(shareText, ScreenshotPath, AppUrl, shareSubject);
        }


        /// <summary>
        /// Share on titter page
        /// </summary>
        public void TwitterShare()
        {
            Application.OpenURL(titterAddress + "?text=" + WWW.EscapeURL(textToDisplay) + "&amp;lang=" + WWW.EscapeURL(tweetLanguage));
        }


        /// <summary>
        /// Share on facbook page
        /// </summary>
        public void FacebookShare()
        {
            if (!string.IsNullOrEmpty(pictureUrl))
            {
                Application.OpenURL("https://www.facebook.com/dialog/feed?" + "app_id=" + fbAppID + "&link=" + appUrl + "&picture=" + pictureUrl
                             + "&caption=" + caption + "&description=" + description);
            }
            else
            {
                Application.OpenURL("https://www.facebook.com/dialog/feed?" + "app_id=" + fbAppID + "&link=" + appUrl + "&caption=" + caption + "&description=" + description);
            }
        }


        private void Share(string shareText, string imagePath, string url, string subject)
        {
#if UNITY_ANDROID
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + imagePath);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("setType", "image/png");

            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareText + "  " + url);

            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, subject);
            currentActivity.Call("startActivity", jChooser);
#elif UNITY_IOS

            CallSocialShareAdvanced(shareText, subject, url, imagePath);
#else
			Debug.Log("No sharing set up for this platform.");
#endif
        }


#if UNITY_IOS
    public struct ConfigStruct
    {
        public string title;
        public string message;
    }

    [DllImport("__Internal")] private static extern void showAlertMessage(ref ConfigStruct conf);

    public struct SocialSharingStruct
    {
        public string text;
        public string url;
        public string image;
        public string subject;
    }

    [DllImport("__Internal")] private static extern void showSocialSharing(ref SocialSharingStruct conf);

    public static void CallSocialShare(string title, string message)
    {
        ConfigStruct conf = new ConfigStruct();
        conf.title = title;
        conf.message = message;
        showAlertMessage(ref conf);
    }


    public static void CallSocialShareAdvanced(string defaultTxt, string subject, string url, string img)
    {
        SocialSharingStruct conf = new SocialSharingStruct();
        conf.text = defaultTxt;
        conf.url = url;
        conf.image = img;
        conf.subject = subject;

        showSocialSharing(ref conf);
    }
#endif
    }

}