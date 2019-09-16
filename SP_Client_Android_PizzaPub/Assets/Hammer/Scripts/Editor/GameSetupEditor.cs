using UnityEditor;
using UnityEngine;

namespace GSEditor
{
    using UnityEngine;
    using UnityEditor;
#pragma warning disable CS0162 // Unreachable code detected
    [CustomEditor(typeof(GameSetup))]
    public class GameSetupEditor : Editor
    {
        private GameSetup instance;
        void OnEnable()
        {
            instance = (GameSetup)target;
        }
        public override void OnInspectorGUI()
        {
            EditorUtils.CenterTitle("Totem Hunter Settings");
            FBSocialAdsUI();
            FinishButtonGUI();
            if (GUI.changed)
            {
                GameSetup.DirtyEditor();
            }
        }
        public const string REMOVE_CONFIG = "Remove Configuration", ADD_CONFIG = "Configure";

        public bool EnableAds
        {
            get
            {
                return instance.useAds;
            }

            set
            {
                bool prev_val = instance.useAds;
                if (prev_val == value)
                    return;
                instance.useAds = value;
                EditorUtils.SetScriptingDefinedSymbols(GameSetup.ENABLE_ADMOB_ADS, value);
            }
        }

        public bool EnableSocial
        {
            get
            {
                return instance.enableSocial;
            }

            set
            {
                bool prev_val = instance.enableSocial;
                if (prev_val == value)
                    return;
                instance.enableSocial = value;
                EditorUtils.SetScriptingDefinedSymbols(GameSetup.ENABLE_SOCIAL, value);
            }
        }
        void FBSocialAdsUI()
        {
#if !UNITY_IOS && !UNITY_ANDROID
            EditorGUILayout.Space();
            //Facebook SDK, Social Feature(Google Play Games and Game Center), Admob and Unity Ads are Supported on iOS and Android Only!
           EditorUtils.CenterTitle("");
            EditorGUILayout.Space();
            return;
#endif
            EnableAds = EditorGUILayout.BeginToggleGroup("Enable Ads", EnableAds);
            EditorGUILayout.EndToggleGroup();
#if ENABLE_ADMOB_ADS
            serializedObject.Update();
            if (instance.useAds)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("bannerAd"), new GUIContent("Admob Banner"), true);

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("interstitialAd"), new GUIContent("Admob Interstitial"), true);
                EditorGUI.indentLevel--;
            }
            serializedObject.ApplyModifiedProperties();
#endif
            EnableSocial = EditorGUILayout.BeginToggleGroup("Enable Google Play Games", EnableSocial);
            EditorGUILayout.EndToggleGroup();
#if ENABLE_SOCIAL
            EditorGUILayout.LabelField("Go to \"Window>Google Play Games>Setup>Android\" from MenuBar for Configurations");
#endif
        }

        void FinishButtonGUI()
        {
            EditorUtils.DrawLine();
            EditorUtils.CenterTitle("Totem Hunter Version is 1.0");
            EditorUtils.AddSpace(2);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Contact Us"))
            {
                Application.OpenURL("mailto:info.curiologix@gmail.com");
            }
            if (GUILayout.Button("Online Docs"))
            {
                Application.OpenURL("");
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
public class EditorUtils : Editor
{
    public static void CenterTitle(string text)
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(text, EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
    public static void DrawLine()
    {
        EditorGUI.indentLevel--;
        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
        EditorGUI.indentLevel++;
    }
    public static bool FoldOut(string prefName, bool isBold = true)
    {
        bool defaultState = true;
        bool state = EditorPrefs.GetBool(prefName, defaultState);

        GUIStyle style = EditorStyles.foldout;
        FontStyle previousStyle = style.fontStyle;
        style.fontStyle = isBold ? FontStyle.Bold : FontStyle.Normal;
        bool newState = EditorGUILayout.Foldout(state, prefName, style);
        style.fontStyle = previousStyle;
        if (newState != state)
        {
            EditorPrefs.SetBool(prefName, newState);
        }
        return newState;
    }
    public static void AddSpace(int count)
    {
        for (int i = 0; i < count; i++)
        {
            EditorGUILayout.Space();
        }
    }

    public static void SetScriptingDefinedSymbols(string symbol, bool state)
    {
        SetScriptingDefinedSymbolsInternal(symbol, BuildTargetGroup.Android, state);
        SetScriptingDefinedSymbolsInternal(symbol, BuildTargetGroup.iOS, state);
    }
    static void SetScriptingDefinedSymbolsInternal(string symbol, BuildTargetGroup target, bool state)
    {
        var sNow = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
        sNow = sNow.Replace(symbol + ";", ""); sNow = sNow.Replace(symbol, "");
        if (state) sNow = symbol + ";" + sNow;
        PlayerSettings.SetScriptingDefineSymbolsForGroup(target, sNow);
    }
}