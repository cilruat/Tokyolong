using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PageVersusLobby : SingletonMonobehaviour<PageVersusLobby>
{

    public GameObject objBoard;

    public Text txtMyTableNum;

    public Text[] txtTableNum;
    public Text txtReqGameCnt;
    public Text txtGameName;

    public GameObject objReadyBtn;

    public GameObject objReady1P;
    public GameObject objReady2P;

    public GameObject objStartPanel;


    public int needStartNum = 0;

    byte tableNum = 0;
    int GameCnt = 0;
    string GameName = "";


    private void Start()
    {
        needStartNum = 0;

        objReady1P.SetActive(false);
        objReady2P.SetActive(false);
        objStartPanel.SetActive(false);

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

        Debug.Log(UIManager.Instance.isGameRoom);
    }

    private void Update()
    {
        Debug.Log(needStartNum);

        if (needStartNum == 2)
        {
            StartCoroutine(StartGame());
        }
    }


    public void CancelMatch()
    {
        NetworkManager.Instance.Game_Cancel_REQ(tableNum);
    }


    public void OnReady_1Player()
    {
        objReadyBtn.SetActive(false);
        objReady1P.SetActive(true);
        NetworkManager.Instance.Game_Ready_REQ(tableNum);
    }


    public void OnReady_2Player()
    {
        byte tableNum = 0;


        objReady2P.SetActive(true);
        needStartNum++;
    }




    IEnumerator StartGame()
    {
        objStartPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        if(GameName == "가위바위보")
        {
            SceneChanger.LoadScene("LoadingRPS", objBoard);
        }

        else if(GameName == "악어룰렛")
        {
            //SceneChanger.LoadScene("LoadingRPS", objBoard);
            Debug.Log("아직안되용");
        }
    }

    //Start 누르면 판정에서 마지막으로 갯수판정하고, --Count 할것.
    // 자동으로 튕기니깐.. 숫자없으면. 괜히 넣을 필요없겠다요!
}
