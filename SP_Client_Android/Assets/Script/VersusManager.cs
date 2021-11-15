using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class VersusManager : MonoBehaviour {

    public int myCoin;
    public int oppoCoin;

    public GameObject myTableNum;
    public GameObject oppoTableNum;

    public Text txtMyTableNum;
    public Text txtOppoTableNum;

    public GameObject objGameName;
    public Text txtGameName;

    public bool isLobby = false;
    public bool isReady = false;

    public Image imgTimer;

    // Info에서 Dictionary 만들고 거기서 ConstainKey 쓸것
    // 없으면 null반환할것
    // Set만들것
    // 



    public void SetInfo(byte tableNo)
    {
    }

}
