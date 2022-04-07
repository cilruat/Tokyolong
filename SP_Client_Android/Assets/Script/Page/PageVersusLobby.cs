using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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

    public VersusCountDown versus;



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
        if (needStartNum == 2)
        {
            LoadGame();
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


    public void OnReady_2Player(int tableNo)
    {
        if(tableNo == tableNum) 
        {
            objReady2P.SetActive(true);
            needStartNum++;
            Debug.Log("상대방 준비완료 뜸?");
        }
    }


    void LoadGame()
    {
        objStartPanel.SetActive(true);
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {

        yield return new WaitForSeconds(2f);

        if(GameName == "가위바위보")
        {
            SceneManager.LoadScene("LoadingRPS");

        }

        if (GameName == "불독룰렛")
        {
            SceneManager.LoadScene("LoadingBullDog");

        }



        else if (GameName == "병뚜껑돌리기")
        {
            //SceneChanger.LoadScene("LoadingBullDog", objBoard);
            Debug.Log("아직안돼용");
        }

    }
}
