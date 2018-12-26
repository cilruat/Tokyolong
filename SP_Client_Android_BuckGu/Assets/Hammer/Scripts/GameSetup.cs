using UnityEngine;
using System.IO;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif
public class GameSetup : ScriptableObject
{
    public bool useAds = true, enableSocial = false;
    public AdsData interstitialAd, bannerAd;
    public const string ENABLE_ADMOB_ADS = "ENABLE_ADMOB_ADS", ENABLE_SOCIAL = "ENABLE_SOCIAL";

    const string assetDataPath = "Assets/_TotemHunter/Resources/";
    const string assetName = "GameSetup";
    const string assetExt = ".asset";
    private static GameSetup instance;
    public static GameSetup Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load(assetName) as GameSetup;
                if (instance == null)
                {
                    instance = CreateInstance<GameSetup>();
#if UNITY_EDITOR
                    if (!Directory.Exists(assetDataPath))
                    {
                        Directory.CreateDirectory(assetDataPath);
                    }
                    string fullPath = assetDataPath + assetName + assetExt;
                    AssetDatabase.CreateAsset(instance, fullPath);
                    AssetDatabase.SaveAssets();
#endif
                }
            }
            return instance;
        }
    }
    public static void DirtyEditor()
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(Instance);
#endif
    }
#if UNITY_EDITOR
    [MenuItem("Window/TotemHunter")]
    public static void Edit()
    {
        Selection.activeObject = Instance;
    }
#endif

}
[System.Serializable]
public struct AdsData
{
    public string IOS_ID, ANDROID_ID;
}