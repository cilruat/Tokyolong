using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PageLuckGame : PageBase    {

    public GameObject objBoard;
    public Text txtPlayCnt;
    public Text txtTableNo;


    protected override void Awake()
    {
        base.Awake();

        txtPlayCnt.text = Info.GamePlayCnt.ToString();
        txtTableNo.text = Info.TableNum.ToString();
    }

    public void RefreshGamePlayChance()
    {
        if (Info.isCheckScene("LuckGame") == false)
            return;

        txtPlayCnt.text = Info.GamePlayCnt.ToString();

    }

    public void OnGoJGB()
    {
        SceneChanger.LoadScene("JJangGameBbo", objBoard);
    }


    public void OnGoDoll()
    {
        SceneChanger.LoadScene("ClawMachine - Red & Blue", objBoard);
    }


    public void OnGoCash()
    {
        SceneChanger.LoadScene("CashShop", objBoard);
    }

    public void ReturnPrev()
    {
        SceneChanger.LoadScene("SelectGame", objBoard);
    }

    public void ReturnHome()
    {
        SceneChanger.LoadScene("Main", objBoard);
    }


}
