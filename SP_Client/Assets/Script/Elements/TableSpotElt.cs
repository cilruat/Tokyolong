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
    public Text textPeopleCnt;
    public Text textNum;

    byte customer = (byte)ESpotType.eNone;
    ESpotType type = ESpotType.eNone;
    public bool IsNone { get { return this.type == ESpotType.eNone; } }

    UserInfo info = null;

	public Texture[] imgSpots = new Texture[System.Enum.GetValues(typeof(ESpotType)).Length];
    public void SetTableSpot(UserInfo info)
    {
        textNum.text = tableNo.ToString();

        this.info = info;
        if (info == null)
        {
            if (iconSpot.gameObject.activeSelf)
                iconSpot.gameObject.SetActive(false);

            return;
        }

        this.type = (ESpotType)this.info.customerType;
        iconSpot.gameObject.SetActive(type != ESpotType.eNone);
        if (this.type != ESpotType.eNone)
        {
            if (tableNo == Info.TableNum)
                this.type = ESpotType.eMe;

            textPeopleCnt.gameObject.SetActive(this.type != ESpotType.eMe);
            textPeopleCnt.text = info.peopleCnt.ToString();
            iconSpot.texture = imgSpots[(byte)this.type];
            UITweenPosY.Start(iconSpot.gameObject, 20f, 30f, TWParam.New(1f).Curve(TWCurve.Back).Speed(TWSpeed.Slower));
            UITweenAlpha.Start(iconSpot.gameObject, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.Back).Speed(TWSpeed.Slower));
        }
    }
}