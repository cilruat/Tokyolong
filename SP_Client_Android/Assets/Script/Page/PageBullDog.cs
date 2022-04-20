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

    int FirstPostValue = -1;
    int OpFirstPostValue = -1;








    private void Start ()
    {

        FirstPostValue = Random.Range(1, 100);
        OpFirstPost();



        if (Info.myInfo.listGameAcceptInfo.Count > 0)
        {
            UserGameAcceptInfo info = Info.myInfo.listGameAcceptInfo[Info.myInfo.listGameAcceptInfo.Count - 1];
            tableNum = info.tableNo;
            GameCnt = info.reqGameCnt;
            GameName = info.gameName;

        }

        if(Info.myInfo.listBullDogFirstInfo.Count > 0)
        {
            UserBullDogFirstInfo info = Info.myInfo.listBullDogFirstInfo[Info.myInfo.listBullDogFirstInfo.Count - 1];
            OpFirstPostValue = info.firstValue;

            Debug.Log(OpFirstPostValue);
        }

        txtMyTableNum.text = Info.TableNum.ToString();

        txtTableNum[0].text = tableNum.ToString();
        txtReqGameCnt.text = GameCnt.ToString();
        txtGameName.text = GameName.ToString();

        if (OpFirstPostValue >= 0)
        {
            CheckFirst();
        }



    }


    //상대방의 숫자를 받아오는 걸 만들어야겠네

    //업데이트에서 FirstPost 를 체크해서 값의 수에 따라 둘이 맞으면 선픽 다르면 후픽으로 결정
    // 숫자가 같으면 숫자가 다르면!

    void Update () {



    }

    //상대편이 무조건 해주니깐!
    // 리스트 만들어야겟다..ㅎㅎ
    public void OpFirstPost()
    {

        NetworkManager.Instance.Versus_First_REQ(tableNum, FirstPostValue);
        Debug.Log(FirstPostValue);

    }


    public void CheckFirst()
    {
        
    }

}
