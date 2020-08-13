using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailPlz : SingletonMonobehaviour<MailPlz>
{
    //Tostring  배워보자
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
        if (inputCount > Info.PLZ_MAX_COUNT || inputCount < Info.PLZ_MAX_COUNT * -1)
        {
            inputCount = inputCount > 0 ? Info.PLZ_MAX_COUNT : Info.PLZ_MAX_COUNT * -1;
            SystemMessage.Instance.Add("한번에 " + Info.PLZ_MAX_COUNT.ToString() + "개 이상 조르기 할 수는 없어용!");
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

            if (inputCount > Info.PLZ_MAX_COUNT || inputCount < Info.PLZ_MAX_COUNT * -1)
            {
                inputCount = inputCount > 0 ? Info.PLZ_MAX_COUNT : Info.PLZ_MAX_COUNT * -1;
                SystemMessage.Instance.Add("한번에 " + Info.PLZ_MAX_COUNT.ToString() + "개 이상 조르기 할 수는 없어용!");
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
        NetworkManager.Instance.Please_Send_REQ(this.tableNo, this.inputCount);
        _init();
        OnClose();
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    public void InputComplete()
    {
        SystemMessage.Instance.Add(this.tableNo.ToString() + "번 테이블에 조르기를 부탁했습니다 이모티콘나오니?");
        this.tableNo = 0;
        OnClose();
    }

}
