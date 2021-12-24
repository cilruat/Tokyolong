using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PageVersusLobby : MonoBehaviour {

    public GameObject objBoard;

    public Text txtMyTableNum;

    public Text[] txtTableNum;
    public Text txtReqGameCnt;
    public Text txtGameName;

    public GameObject objCancelPanel;


    byte tableNum = 0;
    int GameCnt = 0;
    string GameName = "";


    private void Start()
    {


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



    public void CancelMatch()
    {
        NetworkManager.Instance.Game_Cancel_REQ(tableNum);
    }




    //Start 누르면 판정에서 마지막으로 갯수판정하고, --Count 할것.
    // 자동으로 튕기니깐.. 숫자없으면. 괜히 넣을 필요없겠다요!
}
