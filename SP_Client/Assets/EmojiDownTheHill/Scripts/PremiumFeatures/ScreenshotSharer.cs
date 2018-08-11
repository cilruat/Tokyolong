using UnityEngine;
using System.Collections;

#if EASY_MOBILE
using EasyMobile;
#endif

namespace SgLib
{
    public class ScreenshotSharer : MonoBehaviour
    {
        [Header("Sharing Config")]
        [Tooltip("Any instances of [score] will be replaced by the actual score achieved in the last game, [AppName] will be replaced by the app name declared in AppInfo")]
        [TextArea(3, 3)]
        public string shareMessage = "Awesome! I've just scored [score] in [AppName]! [#AppName]";
        public string screenshotFilename = "screenshot.png";

        #if EASY_MOBILE
        public static ScreenshotSharer Instance { get; private set; }

        void OnEnable()
        {
            PlayerController.PlayerDied += PlayerController_PlayerDied;
        }

        void OnDisable()
        {
            PlayerController.PlayerDied -= PlayerController_PlayerDied;
        }

        void Awake()
        {
            Instance = this;
        }

        void PlayerController_PlayerDied()
        {
            if (PremiumFeaturesManager.Instance.enablePremiumFeatures)
            {
                Application.CaptureScreenshot(screenshotFilename);
            }
        }

        public void ShareScreenshot()
        {
            string path = System.IO.Path.Combine(Application.persistentDataPath, screenshotFilename);
            string msg = shareMessage;
            msg = msg.Replace("[score]", ScoreManager.Instance.Score.ToString());
            msg = msg.Replace("[AppName]", AppInfo.Instance.APP_NAME);
            msg = msg.Replace("[#AppName]", "#" + AppInfo.Instance.APP_NAME.Replace(" ", ""));
            Sharing.ShareImage(path, screenshotFilename, msg);
        }

        #endif
    }
}
