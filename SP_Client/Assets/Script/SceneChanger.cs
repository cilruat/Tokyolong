using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : SingletonMonobehaviour<SceneChanger> {

    public static string nextName = "";
	public static GameObject objHide;

	public static void LoadScene(string name, GameObject obj)
    {
        nextName = name;
		objHide = obj;
		SceneChanger.Instance._LoadScene();
    }        

	void _LoadScene()
	{
		UITweenAlpha.Start (objHide, 0f, TWParam.New (1f).Curve (TWCurve.CurveLevel2)).AddCallback (_StartCallBack);
	}

	void _StartCallBack()
	{
		SceneManager.LoadScene (nextName);
	}
}
