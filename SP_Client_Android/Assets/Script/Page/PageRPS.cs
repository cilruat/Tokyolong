﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageRPS : SingletonMonobehaviour<PageRPS>  {

    //싱글톤이어도 상관X

    //Basic info

    public GameObject objBoard;
    public Text[] txtTableNum;
    public Text txtReqGameCnt;
    public Text txtGameName;
    public Text txtMyTableNum;
    byte tableNum = 0;
    int GameCnt = 0;
    string GameName = "";

    int winscore = 0;
    int losescore = 0;


    public int myRsp = -1; //0 바위, 1가위, 2보

    public int yourRsp = -1; //0 바위, 1가위, 2보


    //UpdateCheck
    public int Check = 0;



    //Round text
    int Round;
    public Text mainRoundCnt;
    public Text subRoundCnt;
    public GameObject mainRoundPanel;
    public GameObject subRoundPanel;

    // Choice R / P / S Panel
    public GameObject ChoicePanel;

    public GameObject objRock;
    public GameObject objPaper;
    public GameObject objScissor;

    public GameObject BlindPanel;

    // CoutDown Panel

    public int countdownTime;
    public Text countdownDisplay;
    public GameObject objDisplay;

    // after Choice

    public GameObject waitOherPanel;
    public GameObject DownPanel;

    // animation 0 = 바위, 1 가위 , 2 보

    public GameObject AnimationPanel;

    public GameObject objMyAnimation;
    public GameObject objYourAnimation;

    public Animator animMy;
    public Animator animOpponent;

    // win lose  win1,2 lose 1,2 두개씩 해야겟네 ㅋㅋ

    public GameObject WinPanel;
    public GameObject LosePanel;
    public GameObject DrawPanel;

    public GameObject objwinscore_1;
    public GameObject objlosescore_1;

    public GameObject objwinscore_2;
    public GameObject objlosescore_2;

    IEnumerator countdown;



    private void Start()
    {
        myRsp = -1;
        yourRsp = -1;
        Round = 1;
        Check = 0;
        winscore = 0;
        losescore = 0;

        //기본 SetActive 설정
        mainRoundPanel.SetActive(true);
        subRoundPanel.SetActive(false);
        ChoicePanel.SetActive(false);
        waitOherPanel.SetActive(false);
        DownPanel.SetActive(true);
        AnimationPanel.SetActive(false);
        BlindPanel.SetActive(true);
        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
        DrawPanel.SetActive(false);

        objwinscore_1.SetActive(false);
        objwinscore_2.SetActive(false);
        objlosescore_1.SetActive(false);
        objlosescore_2.SetActive(false);

        //Round Coroutine
        StartCoroutine(RoundStart());


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

    }

    private void Update()
    {
        if(Check == 2)
        {

            waitOherPanel.SetActive(false);
            DownPanel.SetActive(false);

            AnimationPanel.SetActive(true);

            StartCoroutine(myRspAnimation());
            StartCoroutine(ShowOppoRspAnim());

        }
    }


        // myRsp, your Rsp 정보 가져와서 StartCorutine 에서 보여주어야겟지

    IEnumerator myRspAnimation()
    {
        Debug.Log(myRsp);

        if (myRsp == 0)
        {
            animMy.Play("Rock");
        }

        if (myRsp == 1)
        {
            animMy.Play("Scissor");
        }

        if (myRsp == 2)
        {
            animMy.Play("Paper");
        }

        // 애니메이션 3초뒤에 함수 실행
        yield return new WaitForSeconds(3f); 

        ShowResult();

    }


    IEnumerator ShowOppoRspAnim()
    {
        if (yourRsp == 0)
        {
            animOpponent.Play("Rock");
        }

        else if (yourRsp == 1)
        {
            animOpponent.Play("Scissor");
        }

        else if (yourRsp == 2)
        {
            animOpponent.Play("Paper");
        }


        yield return new WaitForSeconds(3f); // 애니메이션 몇초 뒤 꺼지니


    }

    public void ShowResult()
    {
        StopAllCoroutines();

        //Draw
        if (myRsp == yourRsp)
        {
            StartCoroutine(Draw());
        }

        //Lose
        else if ((myRsp == 0 && yourRsp == 2) || (myRsp == 1 && yourRsp == 0) || (myRsp == 2 && yourRsp == 1))
        {
            StartCoroutine(Lose());
        }

        else // 짐
        {
            StartCoroutine(Win());
        }
    }




    IEnumerator Win()
    {
        CheckReset();
        winscore = 1;
        WinPanel.SetActive(true);
        yield return new WaitForSeconds(3f);

        NetworkManager.Instance.Versus_Win_REQ(tableNum);


        // Round 판정
        if(winscore == 1)
        {
            objwinscore_1.SetActive(true);
            Base();
        }

        // else if win ==2 빅토리 화면


    }

    IEnumerator Lose()
    {
        CheckReset();

        LosePanel.SetActive(true);
        yield return new WaitForSeconds(3f);


        Base();

    }


    IEnumerator Draw()
    {
        CheckReset();

        DrawPanel.SetActive(true);
        yield return new WaitForSeconds(3f);


        Base();
    }

    void Base()
    {
        UITweenScale.Start(objRock.gameObject, 1f, 1f, TWParam.New(.3f).Curve(TWCurve.Bounce));
        UITweenScale.Start(objScissor.gameObject, 1f, 1f, TWParam.New(.3f).Curve(TWCurve.Bounce));
        UITweenScale.Start(objPaper.gameObject, 1f, 1f, TWParam.New(.3f).Curve(TWCurve.Bounce));

        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
        DrawPanel.SetActive(false);

        DownPanel.SetActive(true);
        AnimationPanel.SetActive(false);

        RoundCheck();
        StartCoroutine(RoundStart());
    }


    void RoundCheck()
    {
        int sumscore = winscore + losescore;

        if(sumscore == 0)
        {
            Round = 1;
        }
        else if(sumscore == 1)
        {
            Round = 2;
        }

        else if(sumscore == 2)
        {
            Round = 3;
        }
        else
        {
            Debug.Log("라운드 안들어오나 else 로 빠짐" + Round);
        }
    }




    IEnumerator RoundStart()
    {
        // Round 동기화
        mainRoundCnt.text = Round.ToString();
        subRoundCnt.text = Round.ToString();
        //

        mainRoundPanel.SetActive(true);

        yield return new WaitForSeconds(3f);
        mainRoundPanel.SetActive(false);

        subRoundPanel.SetActive(true);
        ChoicePanel.SetActive(true);
        countdownDisplay.gameObject.SetActive(true);
        BlindPanel.SetActive(false);

        StartCountdown();
    }


    void StartCountdown()
    {
        countdown = CountdownToStart();
        StartCoroutine(countdown);
    }

    void StopCountdown()
    {
        if (countdown != null)
        {
            StopCoroutine(countdown);
        }
    }


    IEnumerator CountdownToStart()
    {
        yield return null;

        countdownTime = 20;
        //countdownDisplay.gameObject.SetActive(true);


        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);
            UITweenAlpha.Start(objDisplay, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
            UITweenScale.Start(objDisplay.gameObject, 1f, 1.3f, TWParam.New(.3f).Curve(TWCurve.Bounce));


            countdownTime--;
        }

        countdownDisplay.text = "시간만료";


        yield return new WaitForSeconds(1f);

        // 자동패 올라가게 REQ 보낼것
        //UI 작업
        Debug.Log("카운트 다운 종료 UI 작업");

        //countdownDisplay.gameObject.SetActive(false);
    }


    
    // 작업환경 확실하게 구분할것 왜 싱글톤으로 받는가? 아니면 바로 해도 되는가? 이런것들 ACK는 바로하고, NOT 는 싱글톤으로 작업하고 OK?

    // Win or Lose 뒤에 ROUND 바꾸어야하는 NOT 작업 넣기


   

    public void OnClickRock()
    {
        UITweenScale.Start(objRock.gameObject, 1f, 1.5f, TWParam.New(.3f).Curve(TWCurve.Bounce));
        BlindPanel.SetActive(true);
        ChoicePanel.SetActive(false);
        waitOherPanel.SetActive(true);


        StopCountdown();

        Debug.Log(countdownTime + "카운트다운 남은거 체크 디버그");
        NetworkManager.Instance.Versus_Rock_REQ(tableNum);



    }

    public void OnClickPaper()
    {
        UITweenScale.Start(objPaper.gameObject, 1f, 1.5f, TWParam.New(.3f).Curve(TWCurve.Bounce));
        BlindPanel.SetActive(true);
        ChoicePanel.SetActive(false);
        waitOherPanel.SetActive(true);

        StopCountdown();
        NetworkManager.Instance.Versus_Paper_REQ(tableNum);

    }

    public void OnClickScissor()
    {
        UITweenScale.Start(objScissor.gameObject, 1f, 1.5f, TWParam.New(.3f).Curve(TWCurve.Bounce));
        BlindPanel.SetActive(true);
        ChoicePanel.SetActive(false);
        waitOherPanel.SetActive(true);

        StopCountdown();
        NetworkManager.Instance.Versus_Scissor_REQ(tableNum);

    }


    public void CheckReset()
    {
        Check = 0;
    }

    public void ImRock(int tableNo)
    {
        if (tableNo == tableNum)
        {
            yourRsp = 0;
            Check++;
        }
    }

    public void ImScissor(int tableNo)
    {
        if (tableNo == tableNum)
        {
            yourRsp = 1;
            Check++;
        }
    }

    public void ImPaper(int tableNo)
    {
        if (tableNo == tableNum)
        {
            yourRsp = 2;
            Check++;
        }
    }











    public void ReturnHome()
    {
        SceneChanger.LoadScene("Mail", PageBase.Instance.curBoardObj());


    }






}
