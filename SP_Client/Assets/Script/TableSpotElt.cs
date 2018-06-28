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
}