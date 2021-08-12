using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MailPresent : SingletonMonobehaviour<MailPresent>
{
    public Text table;
    public Text count;
    byte tableNo = 0;
    int inputCount = 0;

    void _init()
    {
        inputCount = 0;
        count.text = "0";
    }

    public void SetInfo(byte tableNo)
    {
        this.tableNo = tableNo;
        table.text = tableNo.ToString() + "번 테이블";
        _init();
    }


    public void OnPressedNum(int num)
    {
        if (inputCount == 0)
        {
            inputCount = useMinus ? num * -1 : num;
            useMinus = false;
        }
        else
        {
            if (inputCount >= 0)
                inputCount = (inputCount * 10) + num;
            else
                inputCount = (inputCount * 10) - num;
        }
        if (inputCount > Info.GAMEPLAY_MAX_COUNT || inputCount < Info.GAMEPLAY_MAX_COUNT * -1)
        {
            inputCount = inputCount > 0 ? Info.GAMEPLAY_MAX_COUNT : Info.GAMEPLAY_MAX_COUNT * -1;
            SystemMessage.Instance.Add("한번에 " + Info.GAMEPLAY_MAX_COUNT.ToString() + "개 이상 선물 할 수 없어요!");
        }

        count.text = inputCount.ToString();
    }

    bool useMinus = false;

    public void OnPressedMinus()
    {
        useMinus = true;

        if (inputCount != 0)
        {
            inputCount *= -1;

            if (inputCount > Info.GAMEPLAY_MAX_COUNT || inputCount < Info.GAMEPLAY_MAX_COUNT * -1)
            {
                inputCount = inputCount > 0 ? Info.GAMEPLAY_MAX_COUNT : Info.GAMEPLAY_MAX_COUNT * -1;
                SystemMessage.Instance.Add("한번에 " + Info.GAMEPLAY_MAX_COUNT.ToString() + "개 이상 선물 할 수 없어용!");
            }

            count.text = inputCount.ToString();
        }
    }

    public void OnInit()
    {
        _init();
    }

    public void OnConfirm()
    {
        //GamePlayCnt == 보유값, inputCount == 입력값
        if (Info.GamePlayCnt >= inputCount)
        { 
            SystemMessage.Instance.Add(tableNo.ToString() + "번 테이블에 쿠폰을 쏴! 쏴! (❁´▽`❁) 드렸어요~!");
            NetworkManager.Instance.Prensent_Send_REQ(this.tableNo, this.inputCount);

            Info.AddOrderCount(-inputCount);

            _init();
        }
        else
        {
            SystemMessage.Instance.Add("가진거보단 많아야 보내드리죵? ƪ(•ε•)∫ 못보내 못보내~");
        }
        OnClose();
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }
}
