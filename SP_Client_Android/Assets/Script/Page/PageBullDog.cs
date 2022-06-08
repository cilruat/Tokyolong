using System.Collections;
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

    // choice object

    public GameObject objFirstTurnArrow, objPostTurnArrow;
    public GameObject objFirstTurnScale, objPostTurnScale;

    IEnumerator countdownfirstval;
    public int countdownfirstTime;
    public Text txtcountdownFirst;
    public GameObject objcntFirstDisplay;

    //
    public GameObject GameOverPanel;
    public GameObject VictoryPanel;

    // Teeth Panel

    public List<GameObject> normalTeeth = new List<GameObject>();
    public List<GameObject> openTeeth = new List<GameObject>();

    //

    public GameObject objTrunPanel;
    public Text TurnText;
    public Animator MoveToPanel;

    public bool ImTurn;

    // Reuslt

    public Animator DogBark;
    public bool Elect;

    IEnumerator onturncountdown;
    public int turncount;
    public Text turncountText;
    public GameObject objTurncount;

    public GameObject objWaiting;

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
        objFirstTurnArrow.SetActive(false);
        objPostTurnArrow.SetActive(false);
        objTrunPanel.SetActive(false);
        objWaiting.SetActive(false);

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

        if(needBullDogStartNum == 2 && !First)
        {
            StartCoroutine(CalculateFirstVal());
            First = true;

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
            needBullDogStartNum = 0;

            StartCoroutine(firstAttack());
        }

        if (FirstPostValue < OpFirstPostValue)
        {
            objImSecondPanel.SetActive(true);
            needBullDogStartNum = 0;

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

    //처음 한번만 수행하는 코루틴입니다.

    IEnumerator firstAttack()
    {
        yield return new WaitForSeconds(3f);
        objImFirstPanel.SetActive(false);
        objFirstMainPanel.SetActive(false);
        objGamePanel.SetActive(true);

        objWaiting.SetActive(false);

        Result = Random.Range(0, 9);
        Debug.Log(Result);
        NetworkManager.Instance.VERSUS_Random_REQ(tableNum, Result);
        //애니메이션도 삽입해야되네 시발!

        objFirstTurnArrow.SetActive(true);
        UITweenScale.Start(objFirstTurnScale.gameObject, 1f, 1.3f, TWParam.New(.3f).Curve(TWCurve.Bounce));
        objPostTurnArrow.SetActive(false);

        ImTurn = true;
        OnCountDown();

    }

    IEnumerator postDefend()
    {
        yield return new WaitForSeconds(3f);
        objImFirstPanel.SetActive(false);
        objFirstMainPanel.SetActive(false);
        objGamePanel.SetActive(true);
        // 작업추가 후턴은 블라인드를 켜놓는다.
        GameBlinder.SetActive(true);
        objWaiting.SetActive(true);

        objFirstTurnArrow.SetActive(false);
        objPostTurnArrow.SetActive(true);
        UITweenScale.Start(objPostTurnScale.gameObject, 1f, 1.3f, TWParam.New(.3f).Curve(TWCurve.Bounce));


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



    public void OnclickTeeth(int teeth)
    {
        NetworkManager.Instance.VERSUS_Choice_REQ(tableNum, teeth);
        Debug.Log(teeth);

    }

    public void teethState(int teeth)
    {
        normalTeeth[teeth].SetActive(false);
        openTeeth[teeth].SetActive(true);


        if(teeth == Result)
        {
            DogBark.Play("Dog_Bark_Versus");
            Elect = true;


            if(ImTurn == true)
            {
                StartCoroutine(GameOver());
            }

            if(ImTurn == false)
            {
                StartCoroutine(Victory());
            }
        }

    }


    public void FirstTurn()
    {
        // 선턴이고 이제 턴을 넘긴다
        StartCoroutine(AfterfirstTurn());

        objTrunPanel.SetActive(true);
        MoveToPanel.Play("MoveTurnPanel");
        TurnText.text = "상대";

        OffCountDown();

        ImTurn = false;
    }


    public void SecondTurn()
    {
        // 후턴에서 턴을 받는다

        StartCoroutine(AfterpostTurn());

        objTrunPanel.SetActive(true);
        MoveToPanel.Play("MoveTurnPanel");
        TurnText.text = "나의";

        OnCountDown();

        ImTurn = true;
    }


    IEnumerator AfterfirstTurn()
    {
        yield return new WaitForSeconds(.01f);
        objImFirstPanel.SetActive(false);
        objFirstMainPanel.SetActive(false);
        objGamePanel.SetActive(true);

        GameBlinder.SetActive(true);
        objWaiting.SetActive(true);

        objFirstTurnArrow.SetActive(false);
        objPostTurnArrow.SetActive(true);
        UITweenScale.Start(objPostTurnScale.gameObject, 1f, 1.3f, TWParam.New(.3f).Curve(TWCurve.Bounce));

        yield return new WaitForSeconds(1.5f);
        objTrunPanel.SetActive(false);

    }

    IEnumerator AfterpostTurn()
    {
        yield return new WaitForSeconds(.01f);
        objImFirstPanel.SetActive(false);
        objFirstMainPanel.SetActive(false);
        objGamePanel.SetActive(true);

        GameBlinder.SetActive(false);
        objWaiting.SetActive(false);

        objFirstTurnArrow.SetActive(true);
        objPostTurnArrow.SetActive(false);
        UITweenScale.Start(objFirstTurnScale.gameObject, 1f, 1.3f, TWParam.New(.3f).Curve(TWCurve.Bounce));


        yield return new WaitForSeconds(1.5f);
        objTrunPanel.SetActive(false);


    }

    // 턴이 종료될때 마다 카운트다운을 초기화하고 카운트다운이 만료가 되면 자동으로 게임오버
    // 이건 내가 턴이 종료되도 마찬가지 아니야? 상대 턴 종료랑 내 턴 종료 둘다 해야지..ㅋㅋ 

    void OnCountDown()
    {
        onturncountdown = OnTurnCountDown();
        StartCoroutine(onturncountdown);
    }

    void OffCountDown()
    {
        if(onturncountdown != null)
        {
            StopCoroutine(onturncountdown);
        }
    }


    IEnumerator OnTurnCountDown()
    {
        yield return null;

        turncount = 15;

        while(turncount > 0)
        {
            turncountText.text = turncount.ToString();

            yield return new WaitForSeconds(1f);
            UITweenAlpha.Start(objTurncount, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
            UITweenScale.Start(objTurncount.gameObject, 1f, 1.3f, TWParam.New(.3f).Curve(TWCurve.Bounce));


            turncount--;
        }
        turncountText.text = "시간만료";

        yield return new WaitForSeconds(2f);

        SystemMessage.Instance.Add("시간안에 선택을 못해서 패배합니다");
        NetworkManager.Instance.Versus_GameOver_REQ(tableNum, GameCnt);

        UIManager.Instance.isGameRoom = false;


    }

    public void ExitGame()
    {
        SystemMessage.Instance.Add("패배를 선언하고 로비로 나갑니다");
        StartCoroutine(GameOver());
    }


    IEnumerator Victory()
    {
        objTrunPanel.SetActive(false);
        objWaiting.SetActive(false);

        yield return new WaitForSeconds(2f);
        VictoryPanel.SetActive(true);

        yield return new WaitForSeconds(3f);

    }



    IEnumerator GameOver()
    {
        objTrunPanel.SetActive(false);
        objWaiting.SetActive(false);

        yield return new WaitForSeconds(2f);
        GameOverPanel.SetActive(true);

        yield return new WaitForSeconds(3f);
        NetworkManager.Instance.Versus_GameOver_REQ(tableNum, GameCnt);
    }


}
