using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageCashShop : PageBase {


    public GameObject objBoard;
    public Text txtPlayCnt;


    protected override void Awake()
    {
        base.Awake();
        txtPlayCnt.text = Info.GamePlayCnt.ToString();
    }



    public void ReturnHome()
    {
        SceneChanger.LoadScene("Main", objBoard);

    }


}
