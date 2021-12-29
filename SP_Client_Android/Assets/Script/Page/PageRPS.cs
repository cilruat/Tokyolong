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




    private void Start()
    {
        myRsp = -1;
        yourRsp = -1;
        Round = 1;
        Check = 0;
        //기본 SetActive 설정
        mainRoundPanel.SetActive(true);
        subRoundPanel.SetActive(false);
        ChoicePanel.SetActive(false);
        waitOherPanel.SetActive(false);
        DownPanel.SetActive(true);
        AnimationPanel.SetActive(false);
        BlindPanel.SetActive(true);
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

            //StartCoroutine(myRPS());


            AnimationPanel.SetActive(true);

            StartCoroutine(myRspAnimation());

            StartCoroutine(ShowOppoRspAnim());

            //1. 먼저 각각의 애니메이션을 스타트 한다.

            //각각의 애니 다음에 If 하위로 넣어야함

            //2. 결과를 보여준다
            //3. 승리 패배 패널 넣기
            //4. 빅토리 게임오버 패널넣기
            //5. 게임카운트 및 퇴장


            ///여기서 어케해야되노 이제 애니메이션 집어넣어야하거든
            /// 애니메이션 넣을때 내꺼 상대꺼 따로할거고
            /// 스타트 코루틴 먼저해야겟지?
            /// 
            /// 애니메이션이 끝나거든 Win Lose 를 넣어야하네

            ///손가락 이 있으니깐 그게 나오면서..! Set Active 하면 애니는 시작되니깐
            ///Play 를 하면 되겟구만
            ///
            /// 여기서 경우의 수를 조져야되나?



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


        yield return new WaitForSeconds(3f); // 애니메이션 몇초 뒤 꺼지니

        ShowResult();

    }


    IEnumerator ShowOppoRspAnim()
    {
        if (yourRsp == 0)
        {
            animOpponent.Play("Rock");
        }


        yield return new WaitForSeconds(3f); // 애니메이션 몇초 뒤 꺼지니


    }

    public void ShowResult()
    {
        //승리 패배 계산해서 넣고

        //승리면 REQ

        //패배면 REQ

        // REQ에서 승,패 애니메이션을 활성화

        Debug.Log("결과창 일단 되나 테스트");

    }




    void RSPWin()
    {


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

        StartCoroutine(CountdownToStart());
    }


    IEnumerator CountdownToStart()
    {
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
        Debug.Log("패작업");

        countdownDisplay.gameObject.SetActive(false);
    }


    
    // 작업환경 확실하게 구분할것 왜 싱글톤으로 받는가? 아니면 바로 해도 되는가? 이런것들 ACK는 바로하고, NOT 는 싱글톤으로 작업하고 OK?

    // Win or Lose 뒤에 ROUND 바꾸어야하는 NOT 작업 넣기


   

    public void OnClickRock()
    {
        UITweenScale.Start(objRock.gameObject, 1f, 1.5f, TWParam.New(.3f).Curve(TWCurve.Bounce));
        BlindPanel.SetActive(true);
        ChoicePanel.SetActive(false);
        waitOherPanel.SetActive(true);



        StopAllCoroutines(); // ROund 동기화 되는지 볼것


        NetworkManager.Instance.Versus_Rock_REQ(tableNum);



    }

    public void OnClickPaper()
    {
        UITweenScale.Start(objPaper.gameObject, 1f, 1.5f, TWParam.New(.3f).Curve(TWCurve.Bounce));
        BlindPanel.SetActive(true);


    }

    public void OnClickScissor()
    {
        UITweenScale.Start(objScissor.gameObject, 1f, 1.5f, TWParam.New(.3f).Curve(TWCurve.Bounce));
        BlindPanel.SetActive(true);


    }




    public void ImRock(int tableNo)
    {
        if (tableNo == tableNum)
        {
            yourRsp = 0;
            Check++;
            Debug.Log(Check);
        }
    }












    public void ReturnHome()
    {
        SceneChanger.LoadScene("Mail", PageBase.Instance.curBoardObj());


    }






}
