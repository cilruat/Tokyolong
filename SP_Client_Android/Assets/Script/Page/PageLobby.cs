using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PageLobby : PageBase
{

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
        if (Info.isCheckScene("Lobby") == false)
            return;

        txtPlayCnt.text = Info.GamePlayCnt.ToString();

    }


    public void ReturnHome()
    {
        SceneChanger.LoadScene("Main", objBoard);
    }

    public void ReturnFirst()
    {
        SceneChanger.LoadScene("SelectGame", objBoard);
    }




}
