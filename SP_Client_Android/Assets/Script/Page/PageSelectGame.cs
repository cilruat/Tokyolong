using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PageSelectGame : PageBase  {


    public GameObject objBoard;
    public Text txtPlayCnt;
    public Text txtTableNo;


    protected override void Awake()
    {
        base.Awake();

        txtPlayCnt.text = Info.GamePlayCnt.ToString();
        txtTableNo.text = Info.TableNum.ToString();
        //Info.practiceGame = false;
    }

    public void RefreshGamePlayChance()
    {
        if (Info.isCheckScene("SelectGame") == false)
            return;

        txtPlayCnt.text = Info.GamePlayCnt.ToString();

    }


    public void OnGoSingle()
    {
        SceneChanger.LoadScene("Game", objBoard);
    }

    public void OnGoLobby()
    {
        SceneChanger.LoadScene("Lobby", objBoard);
    }

    public void ReturnHome()
    {
        SceneChanger.LoadScene("Main", objBoard);
    }


}
