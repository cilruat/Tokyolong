using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableSpotElt : MonoBehaviour 
{
    public enum ESpotType
    {
        eMy = 0,
        eMan = 1,
        eWoman = 2,
        eCouple = 3,
    }

    public RawImage iconSpot;
    public Text textNum;
}