using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PageSelectGame : PageBase  {


    public GameObject objBoard;
    public Text txtPlayCnt;
    public Text txtTableNo;


    public GameObject objSlotWin;
    public GameObject objSlotLose;


    public GameObject objYabawiWin;
    public GameObject objYabawiLose;

    public GameObject objQuizWin;
    public GameObject objQuizLose;


    protected override void Awake()
    {
        base.Awake();

        txtPlayCnt.text = Info.GamePlayCnt.ToString();
        txtTableNo.text = Info.TableNum.ToString();

        if (Info.SlotWin == true)
        {
            objSlotWin.SetActive(true);
        }
        else if (Info.SlotLose == true)
        {
            objSlotLose.SetActive(true);
        }


        if (Info.YabawiWin ==true)
        {
            objYabawiWin.SetActive(true);
        }
        else if(Info.YabawiLose == true)
        {
            objYabawiLose.SetActive(true);
        }


        if (Info.QuizWin == true)
        {
            objQuizWin.SetActive(true);
        }
        else if (Info.QuizLose == true)
        {
            objQuizLose.SetActive(true);
        }

    }

    public void RefreshGamePlayChance()
    {
        if (Info.isCheckScene("SelectGame") == false)
            return;

        txtPlayCnt.text = Info.GamePlayCnt.ToString();

    }


    public void OnGoSingle()
    {
        if (Info.GamePlayCnt < 1)
        {
            SystemMessage.Instance.Add("최소한 코인 1개는 있어야지 참여할 수 있습니다");
        }

        else if (Info.isSlot == false)
        {
            SceneChanger.LoadScene("Game", objBoard);
            Info.isSlot = true;

        }
        else
            SystemMessage.Instance.Add("슬롯머신 게임을 이미 참여하셨습니다");

    }

    public void OnGoLobby()
    {
        if (Info.GamePlayCnt < 1)
        {
            SystemMessage.Instance.Add("최소한 코인 1개는 있어야지 참여할 수 있습니다");
        }

        else if (Info.isYabawi == false)
        {
            SceneChanger.LoadScene("Trickery", objBoard);
            Info.isYabawi = true;
        }
        else
            SystemMessage.Instance.Add("야바위 게임을 이미 참여하셨습니다");

    }

    public void OnGoLuck()
    {
        if (Info.GamePlayCnt < 1)
        {
            SystemMessage.Instance.Add("최소한 코인 1개는 있어야지 참여할 수 있습니다");
        }

        else if (Info.isQuiz == false)
        {
            SceneChanger.LoadScene("QuizShow", objBoard);
            Info.isQuiz = true;
        }
        else
            SystemMessage.Instance.Add("퀴즈 게임을 이미 참여하셨습니다");
    }

    public void OnGoCash()
    {
        SceneChanger.LoadScene("CashShop", objBoard);
    }

    public void ReturnHome()
    {
        SceneChanger.LoadScene("Main", objBoard);
    }


}
