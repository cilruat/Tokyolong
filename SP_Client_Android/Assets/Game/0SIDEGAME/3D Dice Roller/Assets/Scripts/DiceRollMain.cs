using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DiceRollMain : MonoBehaviour {

    public int PeopleNum;
    public int maxPeopleNum = 8;
    public int minPeopleNum = 1;
    public Text PeopleText;
    public GameObject btnPlus;
    public GameObject btnMinus;


    void _init()
    {
        PeopleNum = 1;
        PeopleText.text = "1";
    }

    private void Start()
    {
        _init();
    }


    public void PlusBtn()
    {
        if(PeopleNum < maxPeopleNum)
        {
            PeopleNum++;
        }


        if( PeopleNum == maxPeopleNum)
        {
            Debug.Log("최대인원수 도달했습니다");
        }
        PeopleText.text = PeopleNum.ToString();

    }

    public void MinusBtn()
    {
        if (PeopleNum > minPeopleNum)
        {
            PeopleNum--;
        }
        if (PeopleNum == minPeopleNum)
        {
            Debug.Log("최소인원수 도달했습니다");
        }
        PeopleText.text = PeopleNum.ToString();

    }


}
