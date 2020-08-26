using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageCashShop : PageBase {


    public GameObject objBoard;

    public Text txtPlayCnt;
    public GameObject objDiscountChance;
    public GameObject objFileCraker;


    //접근이 필요한 게임오브젝트들을 여기 다 정의해주기
    /*
    public GameObject prefabMenu;
    public Transform MenuScrollView;
    public GameObject MenuShopHolderPanel;
    */

    protected override void Awake()
    {
        base.Awake();

        txtPlayCnt.text = Info.GamePlayCnt.ToString();


    }

    public void RefreshGamePlay()
    {
        txtPlayCnt.text = Info.GamePlayCnt.ToString();
    }



    public void ReturnHome()
    {
        SceneChanger.LoadScene("Main", objBoard);

    }


}
