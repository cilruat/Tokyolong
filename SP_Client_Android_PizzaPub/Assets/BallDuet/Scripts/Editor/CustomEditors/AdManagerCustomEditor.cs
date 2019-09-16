using System.Collections.Generic;
using UnityEditor;

namespace OnefallGames
{
    [CustomEditor(typeof(AdManager))]
    public class AdManagerCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {

#if !UNITY_ADS
            EditorGUILayout.HelpBox("To use Unity Ads, please enable Unity Ads service !!!", MessageType.Warning);
#endif

            if (!ScriptingSymbolsHandler.NamespaceExists(NamespaceData.GoogleMobileAds))
            {
                EditorGUILayout.HelpBox("To use Admob Ads, please import Google Mobile Ads plugin !!!", MessageType.Warning);
            }
            else
            {
                string symbolStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                List<string> currentSymbols = new List<string>(symbolStr.Split(';'));
                if (!currentSymbols.Contains(ScriptingSymbolsData.ADMOB))
                {
                    List<string> sbs = new List<string>();
                    sbs.Add(ScriptingSymbolsData.ADMOB);
                    ScriptingSymbolsHandler.AddDefined_ScriptingSymbol(sbs.ToArray(), EditorUserBuildSettings.selectedBuildTargetGroup);
                }
            }
            base.OnInspectorGUI();
        }
    }
}


