using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InSsaGoHome : MonoBehaviour {

    public GameObject objBoard;


    public void GoHome()
	{
        SceneChanger.LoadScene("InSsa", objBoard);
    }

    public void GoHomeArcadeGame()
    {
        SceneChanger.LoadScene("ArcadeGame", objBoard);
    }


}
