using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageRPS : SingletonMonobehaviour<PageRPS>  {


    public GameObject objBoard;



    //public Text txtPlayCnt;
    public Text txtTableNo;
    public Text txtOtherTableNo;

    public GameObject objReadyPlayer_1, objReadyPlayer_2;
    public GameObject objReadyBtn, objReadyFinBtn;

    public int RPSGameCnt;

}
