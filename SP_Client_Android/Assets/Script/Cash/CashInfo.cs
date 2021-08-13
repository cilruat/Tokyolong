using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CashInfo : MonoBehaviour
{

    public int cashmenu;
    public string title;
    public int cashPrice;

    public Text titleText;
    public Text priceText;


    private void Awake()
    {
        titleText.text = title.ToString();
        priceText.text = cashPrice.ToString();
    }

    public void ConfirmCash()
    {
        if (Info.GamePlayCnt >= cashPrice)
        {
            NetworkManager.Instance.Cash_Send_REQ(title, -cashPrice);
            //RefreshGamePlay();
        }
    }
    /*
    public void RefreshGamePlay()
    {
        txtPlayCnt.text = Info.GamePlayCnt.ToString();
    }
    */



}
