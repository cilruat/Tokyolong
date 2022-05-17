using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PageBullDog : SingletonMonobehaviour<PageBullDog>
{

    public GameObject objBoard;
    public Text[] txtTableNum;
    public Text txtReqGameCnt;
    public Text txtGameName;
    public Text txtMyTableNum;
    byte tableNum = 0;
    int GameCnt = 0;
    string GameName = "";

    IEnumerator countdown;

    int FirstPostValue = -1;
    int OpFirstPostValue = -1;

    bool First = false;

    public int needBullDogStartNum = 0;


    public GameObject objGamePanel;
    public GameObject objFirstMainPanel;
    public GameObject objBlindPanel;
    public GameObject objImFirstPanel;
    public GameObject objImSecondPanel;

    public Text txt1PlayerFirstVal, txt2PlayerFirstVal;

    public GameObject btnFirstValue;

    private void Start ()
    {

        needBullDogStartNum = 0;
        objGamePanel.SetActive(false);
        objFirstMainPanel.SetActive(true);
        objBlindPanel.SetActive(true);
        objImFirstPanel.SetActive(false);
        objImSecondPanel.SetActive(false);

        txt1PlayerFirstVal.text = "";
        txt2PlayerFirstVal.text = "";

        First = false;


        if (Info.myInfo.listGameAcceptInfo.Count > 0)
        {
            UserGameAcceptInfo info = Info.myInfo.listGameAcceptInfo[Info.myInfo.listGameAcceptInfo.Count - 1];
            tableNum = info.tableNo;
            GameCnt = info.reqGameCnt;
            GameName = info.gameName;

        }

        // Null 일걸? 스타트 했는데 ..?

        /*
        if(Info.myInfo.listBullDogFirstInfo.Count > 0)
        {
            UserBullDogFirstInfo info = Info.myInfo.listBullDogFirstInfo[Info.myInfo.listBullDogFirstInfo.Count - 1];
            OpFirstPostValue = info.firstValue;

            Debug.Log(OpFirstPostValue);
        }
        */

        txtMyTableNum.text = Info.TableNum.ToString();

        txtTableNum[0].text = tableNum.ToString();
        txtReqGameCnt.text = GameCnt.ToString();
        txtGameName.text = GameName.ToString();

    }

        // 첫시작하면 선턴이 누구부터인지 확인시켜주는 작업부터 하자
     


    void Update () {

        if(needBullDogStartNum == 2)
        {
            StartCoroutine(CalculateFirstVal());
        }

    }

    IEnumerator CalculateFirstVal()
    {
        yield return new WaitForSeconds(1f);

        if(FirstPostValue > OpFirstPostValue)
        {
            objImFirstPanel.SetActive(true);
            First = true;
            // 코루틴 실행해서 턴에 맞게 블라인드 시킬것

        }

        if (FirstPostValue < OpFirstPostValue)
        {
            objImSecondPanel.SetActive(true);
            // 코루틴 실행해서 턴에 맞게 블라인드 시킬것
        }

        if (FirstPostValue == OpFirstPostValue)
        {
            needBullDogStartNum = 0;
            btnFirstValue.SetActive(true);
        }
    }

    //카운트 다운 넣어서 번호만들기 할까? 그게 제일 합리적이다..20초안에 번호를 말해주라!

    public void FirstValue_1Player()
    {
        FirstPostValue = Random.Range(1, 100);

        txt1PlayerFirstVal.text = FirstPostValue.ToString();

        SendMyFirstValue();
        btnFirstValue.SetActive(false);
    }

    public void FirstValue_2Player(int tableNo, int firstcnt)
    {

        firstcnt = OpFirstPostValue;

        if (tableNo == tableNum)
        {
            txt2PlayerFirstVal.text = OpFirstPostValue.ToString();
            needBullDogStartNum++;
        }

    }


    public void SendMyFirstValue()
    {

        NetworkManager.Instance.Versus_First_REQ(tableNum, FirstPostValue);
        Debug.Log(FirstPostValue);

    }



}
