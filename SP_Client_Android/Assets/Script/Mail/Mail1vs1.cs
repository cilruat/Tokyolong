using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Mail1vs1 : SingletonMonobehaviour<Mail1vs1>
{
    public Text table;
    public Text count;
    public Text game;


    byte tableNo = 0;  //몇번테이블에
    int inputCount = 0; //몇개 코인걸고
    string gameName; // 어떤게임하는데?

    public GameObject[] objGame; // 게임을 이미지로 나타내기

    int maxgameCoin = 5;  // 최대 5코인까지만 걸수있게 제한을둔다
    int mingameCoin = 1;
    public GameObject btnPlus;
    public GameObject btnMinus;


    void _init()
    {
        inputCount = 1;
        count.text = "1";
        gameName = "";
    }

    public void SetInfo(byte tableNo)
    {
        this.tableNo = tableNo;
        table.text = tableNo.ToString() + "번 테이블";
        _init();
    }


    public void PlusBtn()
    {
        if (inputCount < maxgameCoin)
        {
            inputCount++;
        }


        if (inputCount == maxgameCoin)
        {
            Debug.Log("최대");
            SystemMessage.Instance.Add("코인은 최대 " + inputCount.ToString() + "개 까지만 걸수있습니다");

        }
        count.text = inputCount.ToString();

    }

    public void MinusBtn()
    {
        if (inputCount > mingameCoin)
        {
            inputCount--;
        }
        if (inputCount == mingameCoin)
        {
            Debug.Log("최소 도달했습니다");
            SystemMessage.Instance.Add("코인 최소 " + inputCount.ToString() + "개 이상넣어줘");

        }
        count.text = inputCount.ToString();

    }

    //게임의 버튼을 보여주고 누르면 입력이 되는 방식이고 게임을 나열시켜야겠네

    public void SelectGame(int state)
    {

        for (int i = 0; i < objGame.Length; i++)
        {

            if (i != state)
                continue;

            break;
        }

        //여기 코드 이상해 바꿔야행 ㅠ

        // 1 가위바위보 2 불독룰렛
        switch (state)
        {
            case 0:
                // 가위바위보를 신설해서 거기에 정보를 넣는작업
                MailMsgWrite write = objGame[state].GetComponent<MailMsgWrite>();
                if (write)
                    write.SetInfo(tableNo);
                Debug.Log("가위");
                game.text = "가위바위보";
                break;

                // 불복복불복 정보를 만들어서 거기다가 정보를 넣는 작업
            case 1:
                MailLike like = objGame[state].GetComponent<MailLike>();
                if (like)
                    like.SetInfo(tableNo);
                game.text = "복불복룰렛";

                break;
        }
    }



    public void OnInit()
    {
        _init();
    }

    public void OnConfirm()
    {
        //NetworkManager.Instance.Please_Send_REQ(this.tableNo, this.inputCount);
        _init();
        OnClose();
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }


}
