using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class VersusManager : SingletonMonobehaviour<VersusManager> {

    public void LoadPage()
    {
        SceneChanger.LoadScene("VersusLobby", PageBase.Instance.curBoardObj());
    }
}
