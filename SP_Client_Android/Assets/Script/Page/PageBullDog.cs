using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PageBullDog : SingletonMonobehaviour<PageBullDog>
{

    public GameObject objBoard;
    public Text[] txtTableNum;
    public Text txtReqGameCnt;
    public Text txtGameName;
    public Text txtMyTableNum;
    byte tableNum = 0;
    int GameCnt = 0;
    string GameName = "";

    IEnumerator countdown;

    int FirstPostValue = 0;
    int OpFirstPostValue = 0;








    private void Start ()
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




    }

    //업데이트에서 FirstPost 를 체크해서 값의 수에 따라 둘이 맞으면 선픽 다르면 후픽으로 결정
    // 숫자가 같으면 숫자가 다르면!

    void Update () {


        if (FirstPostValue == 1)
        {

        }

    }

    //상대편이 무조건 해주니깐!
    public void OpFirstPost()
    {
        OpFirstPostValue = Random.Range(1, 3);

        NetworkManager.Instance.Versus_First_REQ(tableNum, OpFirstPostValue);
        Debug.Log(OpFirstPostValue);

    }


}
