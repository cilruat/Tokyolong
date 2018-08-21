using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : SingletonMonobehaviour<SceneChanger> {

    public static string nextName = "";
	public static GameObject objHide;

	public static void LoadScene(string name, GameObject obj)
    {
        if (Info.RunInGameScene == false)
            UIManager.Instance.isMouseClickEff = CheckMouseClickEff(name);

        nextName = name;
		objHide = obj;
		SceneChanger.Instance._LoadScene();
    }

    static bool CheckMouseClickEff(string sceneName)
    {
        if (sceneName == "PicturePuzzle"     ||
            sceneName == "PairCards"         ||
            sceneName == "CrashCatStart"     ||
            sceneName == "FlappyBirdMain"    ||
            sceneName == "EmojiMain"         ||
            sceneName == "Emoji2Main")
            return false;
        else
            return true;
    }

	void _LoadScene()
	{
		UITweenAlpha.Start (objHide, 1f, 0f, TWParam.New (1f).Curve (TWCurve.CurveLevel2)).AddCallback (_StartCallBack);
	}

	void _StartCallBack()
	{
		SceneManager.LoadScene (nextName);
	}
}
