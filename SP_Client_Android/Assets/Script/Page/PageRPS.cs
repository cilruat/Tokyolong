using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageRPS : MonoBehaviour  {

    //Basic info

    public GameObject objBoard;
    public Text[] txtTableNum;
    public Text txtReqGameCnt;
    public Text txtGameName;
    public Text txtMyTableNum;
    byte tableNum = 0;
    int GameCnt = 0;
    string GameName = "";

    //Round text
    int Round;
    public Text mainRoundCnt;
    public Text subRoundCnt;
    public GameObject mainRoundPanel;
    public GameObject subRoundPanel;

    // Choice R / P / S Panel
    public GameObject ChoicePanel;




    private void Start()
    {
        Round = 1;
        //기본 SetActive 설정
        mainRoundPanel.SetActive(true);
        subRoundPanel.SetActive(false);
        ChoicePanel.SetActive(false);

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
    }


    public void ReturnHome()
    {
        SceneChanger.LoadScene("Mail", PageBase.Instance.curBoardObj());


    }






}
