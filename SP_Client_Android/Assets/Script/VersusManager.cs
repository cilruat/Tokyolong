using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class VersusManager : SingletonMonobehaviour<VersusManager> {

    public int needStartNum = 0;


    public void LoadPage()
    {
        SceneChanger.LoadScene("VersusLobby", PageBase.Instance.curBoardObj());
    }
}
