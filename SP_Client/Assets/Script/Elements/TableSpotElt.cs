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

	public byte tableNo;
    public RawImage iconSpot;
    public Text textNum;

	public Texture[] imgSpots = new Texture[System.Enum.GetValues(typeof(ESpotType)).Length];
    public void SetTableSpot(byte customer)
    {
		iconSpot.texture = imgSpots [customer];
		textNum.text = tableNo.ToString() + "번";
    }
}