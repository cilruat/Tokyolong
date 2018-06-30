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
        eMe = 3,
        eNone = 255,
    }

	public byte tableNo;
    public RawImage iconSpot;
    public Text textNum;

    ESpotType type = ESpotType.eNone;
    public bool IsNone { get { return this.type == ESpotType.eNone; } }

	public Texture[] imgSpots = new Texture[System.Enum.GetValues(typeof(ESpotType)).Length];
    public void SetTableSpot(byte customer)
    {
        this.type = (ESpotType)customer;

		textNum.text = tableNo.ToString() + "번";
        iconSpot.gameObject.SetActive(type != ESpotType.eNone);
        if (this.type != ESpotType.eNone)
        {
            if (tableNo == Info.TableNum)
                customer = (byte)ESpotType.eMe;

            iconSpot.texture = imgSpots[customer];
            UITweenPosY.Start(iconSpot.gameObject, 20f, 30f, TWParam.New(1f).Curve(TWCurve.Back).Speed(TWSpeed.Slower));
            UITweenAlpha.Start(iconSpot.gameObject, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.Back).Speed(TWSpeed.Slower));
        }
    }
}