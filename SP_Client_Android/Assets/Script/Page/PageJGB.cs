using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PageJGB : PageBase
{

    public GameObject objBoard;
    public Text txtPlayCnt;


    protected override void Awake()
    {
        base.Awake();
        txtPlayCnt.text = Info.GamePlayCnt.ToString();
    }

    public void RefreshGamePlayChance()
    {
        if (Info.isCheckScene("JJangGameBbo") == false)
            return;

        txtPlayCnt.text = Info.GamePlayCnt.ToString();

    }

    public void ReturnPrev()
    {
        SceneChanger.LoadScene("LuckGame", objBoard);
    }

    public void ReturnHome()
    {
        SceneChanger.LoadScene("Main", objBoard);
    }

}
