﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PageBullDog : SingletonMonobehaviour<PageBullDog>
{

    // Base Info
    public GameObject objBoard;
    public Text[] txtTableNum;
    public Text txtReqGameCnt;
    public Text txtGameName;
    public Text txtMyTableNum;
    byte tableNum = 0;
    int GameCnt = 0;
    string GameName = "";


    public GameObject objGamePanel;
    public GameObject GameBlinder;

    // First Check
    int FirstPostValue = -1;
    int OpFirstPostValue = -1;
    bool First = false;
    public int needBullDogStartNum = 0;

    // result value
    int Result = -1;

    // first post Object
    public GameObject objFirstMainPanel;
    public GameObject objBlindPanel;
    public GameObject objActivePanel;
    public GameObject objImFirstPanel;
    public GameObject objImSecondPanel;
    public Text txt1PlayerFirstVal, txt2PlayerFirstVal;
    public GameObject btnFirstValue;

    IEnumerator countdownfirstval;
    public int countdownfirstTime;
    public Text txtcountdownFirst;
    public GameObject objcntFirstDisplay;

    //
    public GameObject GameOverPanel;
    public GameObject VictoryPanel;


    private void Start ()
    {

        needBullDogStartNum = 0;
        objGamePanel.SetActive(false);
        objFirstMainPanel.SetActive(false);
        objBlindPanel.SetActive(false);
        objActivePanel.SetActive(false);
        objImFirstPanel.SetActive(false);
        objImSecondPanel.SetActive(false);
        GameBlinder.SetActive(false);

        txt1PlayerFirstVal.text = "";
        txt2PlayerFirstVal.text = "";

        First = false;

        GameOverPanel.SetActive(false);
        VictoryPanel.SetActive(false);


        if (Info.myInfo.listGameAcceptInfo.Count > 0)
        {
            UserGameAcceptInfo info = Info.myInfo.listGameAcceptInfo[Info.myInfo.listGameAcceptInfo.Count - 1];
            tableNum = info.tableNo;
            GameCnt = info.reqGameCnt;
            GameName = info.gameName;

        }

        txtMyTableNum.text = Info.TableNum.ToString();

        txtTableNum[0].text = tableNum.ToString();
        txtReqGameCnt.text = GameCnt.ToString();
        txtGameName.text = GameName.ToString();

        StartActive();
    }


    void Update () {

        if(needBullDogStartNum == 2)
        {
            StartCoroutine(CalculateFirstVal());
        }

    }


    public void StartActive()
    {
        StartCoroutine(StartFirstValue());
    }

    IEnumerator StartFirstValue()
    {
        objFirstMainPanel.SetActive(true);
        objBlindPanel.SetActive(true);

        yield return new WaitForSeconds(3f);
        objActivePanel.SetActive(true);
        StartCountdown();

    }



    IEnumerator CalculateFirstVal()
    {
        yield return new WaitForSeconds(1f);

        if(FirstPostValue > OpFirstPostValue)
        {
            objImFirstPanel.SetActive(true);
            First = true;

            StartCoroutine(firstAttack());
        }

        if (FirstPostValue < OpFirstPostValue)
        {
            objImSecondPanel.SetActive(true);
            StartCoroutine(postDefend());
        }

        if (FirstPostValue == OpFirstPostValue)
        {
            needBullDogStartNum = 0;
            StartCoroutine(FirstValueDraw());
        }
    }

    #region first / post 

    public void FirstValue_1Player()
    {
        FirstPostValue = Random.Range(1, 100);
        txt1PlayerFirstVal.text = FirstPostValue.ToString();
        SendMyFirstValue();
        btnFirstValue.SetActive(false);
        StopCountdown();
    }

    public void FirstValue_2Player(int tableNo, int firstcnt)
    {
        if (Info.myInfo.listBullDogFirstInfo.Count > 0)
        {
            UserBullDogFirstInfo info = Info.myInfo.listBullDogFirstInfo[Info.myInfo.listBullDogFirstInfo.Count - 1];
            OpFirstPostValue = info.firstValue;
        }

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
    }


    void StartCountdown()
    {
        countdownfirstval = CountdownToStart();
        StartCoroutine(countdownfirstval);
    }

    void StopCountdown()
    {
        if (countdownfirstval != null)
        {
            StopCoroutine(countdownfirstval);
        }
    }

    IEnumerator CountdownToStart()
    {
        yield return null;

        countdownfirstTime = 20;

        while (countdownfirstTime > 0)
        {
            txtcountdownFirst.text = countdownfirstTime.ToString();

            yield return new WaitForSeconds(1f);
            UITweenAlpha.Start(objcntFirstDisplay, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
            UITweenScale.Start(objcntFirstDisplay.gameObject, 1f, 1.3f, TWParam.New(.3f).Curve(TWCurve.Bounce));


            countdownfirstTime--;
        }

        txtcountdownFirst.text = "시간만료";

        yield return new WaitForSeconds(1f);


        GameOverPanel.SetActive(true);

        yield return new WaitForSeconds(3f);

        NetworkManager.Instance.Versus_GameOver_REQ(tableNum, GameCnt);
        UIManager.Instance.isGameRoom = false;
    }

    //여기 좀있다 수정한번 할것 이게 수행된 만큼 Instance.ADD 되는구나 ㅎㅎ;; 문제가 된다면 
    IEnumerator FirstValueDraw()
    {
        Debug.Log(needBullDogStartNum);
        yield return new WaitForSeconds(1.5f);
        txt1PlayerFirstVal.text = "";
        txt2PlayerFirstVal.text = "";

        yield return new WaitForSeconds(1f);
        btnFirstValue.SetActive(true);

        SystemMessage.Instance.Add("숫자가 같아 다시한번 할께요!");


    }

    #endregion


    // 리스트 저장하고 인트값 받아오기 해야겟다 작업. firstattack 가 보내면 되자나 일단 처음 하는애가..이건무조건 실행이니깐.

    IEnumerator firstAttack()
    {
        yield return new WaitForSeconds(1f);
        objImFirstPanel.SetActive(false);
        objFirstMainPanel.SetActive(false);
        objGamePanel.SetActive(true);

        if(First == true)
        {
            Result = Random.Range(1, 10);
            Debug.Log(Result);
            NetworkManager.Instance.VERSUS_Random_REQ(tableNum, Result);
        }

        //애니메이션도 삽입해야되네 시발!
    }

    IEnumerator postDefend()
    {
        yield return new WaitForSeconds(1f);
        objImFirstPanel.SetActive(false);
        objFirstMainPanel.SetActive(false);
        objGamePanel.SetActive(true);
        // 작업추가 후턴은 블라인드를 켜놓는다.
        GameBlinder.SetActive(true);


    }


    public void ResultValue(int tableNo, int winningnum)
    {
        if(Info.myInfo.listBullDogValueInfo.Count > 0)
        {
            UserBullDogValueInfo info = Info.myInfo.listBullDogValueInfo[Info.myInfo.listBullDogValueInfo.Count - 1];

            Result = info.Randomvalue;
        }

        winningnum = Result;

        Debug.Log(Result);

    }

    // 일단 패널 열리는거까지만 한번 보자







    IEnumerator Victory()
    {

        yield return new WaitForSeconds(1f);
        VictoryPanel.SetActive(true);

        yield return new WaitForSeconds(3f);

    }



    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);
        GameOverPanel.SetActive(true);

        yield return new WaitForSeconds(3f);
        NetworkManager.Instance.Versus_GameOver_REQ(tableNum, GameCnt);
    }


}
