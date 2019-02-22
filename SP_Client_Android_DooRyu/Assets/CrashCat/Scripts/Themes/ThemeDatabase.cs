﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssetBundles;

namespace CrashCat
{
    // Handles loading data from the Asset Bundle to handle different themes for the game
    public class ThemeDatabase
    {
        static protected Dictionary<string, ThemeData> themeDataList;
        static public Dictionary<string, ThemeData> dictionnary { get { return themeDataList; } }

        static protected bool m_Loaded = false;
        static public bool loaded { get { return m_Loaded; } }

        static public ThemeData GetThemeData(string type)
        {
            bool night = Random.Range(0, 2) == 1;
            if (night)
                type = "NightTime";

            ThemeData list;
            if (themeDataList == null || !themeDataList.TryGetValue(type, out list))
                return null;

            return list;
        }

        static public void SetThemeData(List<ThemeData> list)
        {
            if (themeDataList != null)
                return;

            themeDataList = new Dictionary<string, ThemeData>();
            for (int i = 0; i < list.Count; i++)
            {
                ThemeData t = list[i];
                themeDataList.Add(t.themeName, t);
            }
        }

        static public IEnumerator LoadDatabase(List<string> packages)
        {
            // If not null the dictionary was already loaded.
            if (themeDataList == null)
            {
                themeDataList = new Dictionary<string, ThemeData>();

                foreach (string s in packages)
                {
                    AssetBundleLoadAssetOperation op = AssetBundleManager.LoadAssetAsync(s, "themeData", typeof(ThemeData));
                    yield return CoroutineHandler.StartStaticCoroutine(op);

                    ThemeData list = op.GetAsset<ThemeData>();
                    if (list != null)
                    {
                        themeDataList.Add(list.themeName, list);
                    }
                }

                m_Loaded = true;
            }

        }
    }
}