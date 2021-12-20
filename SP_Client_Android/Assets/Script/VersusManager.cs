using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class VersusManager : SingletonMonobehaviour<VersusManager> {

    public GameObject objBoard;

    public void LoadPage()
    {

        UserGameAcceptInfo gameinfo = new UserGameAcceptInfo();
        SceneChanger.LoadScene("VersusLobby", PageBase.Instance.curBoardObj());


        if(gameinfo.gameName == "가위바위보")
        {
            PageVersusLobby Lobby = GetComponent<PageVersusLobby>();
            Lobby.SetInfo();
        }


    }





}
