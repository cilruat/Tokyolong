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
            UIManager.Instance.isMouseClickEff = !Info.CheckGameScene(name);

        if (name == "Login")
            UIManager.Instance.Hide_All();

        nextName = name;
		objHide = obj;
		SceneChanger.Instance._LoadScene();
    }		   

	void _LoadScene()
	{
		if (objHide != null)
			UITweenAlpha.Start (objHide, 1f, 0f, TWParam.New (1f).Curve (TWCurve.CurveLevel2)).AddCallback (_StartCallBack);
		else
			_StartCallBack ();
	}

	void _StartCallBack()
	{
		SceneManager.LoadScene (nextName);
	}
}
