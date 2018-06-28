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

    public RawImage iconSpot;
    public Text textNum;

    byte customer = 3;
    int tableNo = -1;
    public int TableNo { get { return tableNo; } }

    public void SetTableSpot(byte customer, int tableNo)
    {
        this.customer = customer;
        this.tableNo = tableNo;

        textNum.text = tableNo.ToString() + "번";
    }
}