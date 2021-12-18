using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class VersusManager : SingletonMonobehaviour<VersusManager> {


    public Text txtTableNo;
    public Text txtPlayCnt;


    public Text txtTargetTableNo;
    public Text txtReqGameCnt;
    public Text txtGameName;



    public void Start()
    {
        txtPlayCnt.text = Info.GamePlayCnt.ToString();
    }

    private void Awake()
    {
        txtTableNo.text = Info.TableNum.ToString();
    }

    public void SetInfo(byte targetTableNo, int reqGameCnt, string gameName)
    {
        txtTargetTableNo.text = targetTableNo.ToString();
        txtReqGameCnt.text = reqGameCnt.ToString();
        txtGameName.text = gameName.ToString();
    }

}
