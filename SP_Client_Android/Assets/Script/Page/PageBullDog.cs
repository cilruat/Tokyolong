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

    // First Check
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
        objFirstMainPanel.SetActive(true);
        objBlindPanel.SetActive(true);
        objImFirstPanel.SetActive(false);
        objImSecondPanel.SetActive(false);

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

        StartCountdown();

    }


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
            StartCoroutine(FirstValueDraw());
        }
    }

    public void FirstValue_1Player()
    {
        FirstPostValue = Random.Range(1, 3); // 1~3까지 수정하고 나서 비기는거 확인하면 될것!
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

    IEnumerator FirstValueDraw()
    {
        Debug.Log(needBullDogStartNum);
        yield return new WaitForSeconds(1.5f);
        txt1PlayerFirstVal.text = "";
        txt2PlayerFirstVal.text = "";

        yield return new WaitForSeconds(0.5f);
        btnFirstValue.SetActive(true);

        SystemMessage.Instance.Add("숫자가 같아 다시한번 할께요!");


    }



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
