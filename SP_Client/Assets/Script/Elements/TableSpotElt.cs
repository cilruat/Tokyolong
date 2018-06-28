using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableSpotElt : MonoBehaviour 
{
    public enum ESpotType
    {
        eMan = 0,
        eWoman = 1,
        eCouple = 2,
        eMy = 3,
    }

    public int tableNo;
    public RawImage iconSpot;
    public Text textNum;

    byte customer = 3;

    public void SetTableSpot(byte customer)
    {
        this.customer = customer;

        textNum.text = tableNo.ToString() + "번";
    }
}