using System.Collections;
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

    public GameObject VictoryPanel;
    public GameObject GameOverPanel;


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

        VictoryPanel.SetActive(false);
        GameOverPanel.SetActive(false);

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

        else 
        {
            StartCoroutine(Win());
        }
    }




    IEnumerator Win()
    {
        CheckReset();
        winscore++;
        WinPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        NetworkManager.Instance.Versus_Win_REQ(tableNum);
    }

    public void WinResult()
    {

        // Round 판정
        if (winscore == 1)
        {
            objwinscore_1.SetActive(true);
            Base();
        }

        else if (winscore == 2)
        {
            objwinscore_2.SetActive(true);
            StartCoroutine(Victory());
        }
        // else if win ==2 빅토리 화면

        else
        {
            Debug.Log(winscore + "윈스코어 0이거나 그 이상일때 else뜹니다");
        }

    }


    // 내가 이기면 상대에게 보내줘야하는 함수
    public void WinResult_Oppo(int tableNo)
    {
        if(tableNo == tableNum)
        {
            //내가 이겼으므로 나에게 올라가야하겠지 상대는?
            // Round 판정
            if (losescore == 1)
            {
                objlosescore_1.SetActive(true);
                Base();
            }

            else if (losescore == 2)
            {
                objlosescore_2.SetActive(true);
                //StartCoroutine(GameOver());
            }
            // else if win ==2 빅토리 화면

            else
            {
                Debug.Log(losescore + "스코어 0이거나 그 이상일때 else뜹니다");
            }
        }
    }





    IEnumerator Lose()
    {
        CheckReset();
        losescore++;
        LosePanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        NetworkManager.Instance.Versus_Lose_REQ(tableNum);
    }

    public void LoseResult()
    {

        // Round 판정
        if (losescore == 1)
        {
            objlosescore_1.SetActive(true);
            Base();
        }

        else if (losescore == 2)
        {
            objlosescore_2.SetActive(true);
            StartCoroutine(GameOver());
        }
        // else if win ==2 빅토리 화면

        else
        {
            Debug.Log(losescore + "로즈스코어 0이거나 그 이상일때 else뜹니다");
        }
    }

    public void LoseResult_Oppo(int tableNo)
    {
        if (tableNo == tableNum)
        {
            //내가 이겼으므로 나에게 올라가야하겠지 상대는?
            // Round 판정
            if (winscore == 1)
            {
                objwinscore_1.SetActive(true);
                Base();
            }

            else if (winscore == 2)
            {
                objwinscore_2.SetActive(true);
                //StartCoroutine(Victory());
            }
            // else if win ==2 빅토리 화면

            else
            {
                Debug.Log(winscore + "스코어 0이거나 그 이상일때 else뜹니다");
            }
        }
    }






    IEnumerator Draw()
    {
        CheckReset();

        DrawPanel.SetActive(true);
        yield return new WaitForSeconds(3f);


        Base();
    }

    

    IEnumerator Victory()
    {

        yield return new WaitForSeconds(1f);
        WinPanel.SetActive(false);

        VictoryPanel.SetActive(true);

        yield return new WaitForSeconds(3f);

        //NetworkManager.Instance.Versus_Victory_REQ(tableNum, GameCnt);
    }



    IEnumerator GameOver()
    {

        yield return new WaitForSeconds(1f);
        LosePanel.SetActive(false);

        GameOverPanel.SetActive(true);

        yield return new WaitForSeconds(3f);

        NetworkManager.Instance.Versus_GameOver_REQ(tableNum, GameCnt);
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


        GameOverPanel.SetActive(true);

        yield return new WaitForSeconds(3f);

        NetworkManager.Instance.Versus_GameOver_REQ(tableNum, GameCnt);
        UIManager.Instance.isGameRoom = false; // ACK 에 넣는것과 같은역할

        // 버그 = 두명 다 시간만료일때 먼저 시간만료된애가 패배임 ㅋㅋ 빠른애가 진다..미안

    }


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

    // 강제 종료된건 isCheckScene 이면? 아니면 어떻게 하라는거에서 힌트를 얻기





}
