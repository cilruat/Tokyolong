using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiceManager : MonoBehaviour 
{
    public List<Dice> diceList;
    public int totalValue;
    public UnityEvent EndRollEvent;
    public GameObject objBoard;


    public GameObject ShowPanel;

    public List<GameObject> TileImageList = new List<GameObject>();


    public void RollAllDie()
    {
        totalValue = 0;
        //reset total value

        for (int i = 0; i < diceList.Count; i++)
        {
            diceList[i].AddForceToDice();
        }
    }
    public void CountAllDieValues()
    {
        for (int i = 0; i < diceList.Count; i++)
        {
            if(diceList[i].isRolling == true)
            {
                totalValue = 0;
                return;
            }
            else
            {
                totalValue += diceList[i].value;
            }
        }
        EndRollEvent.Invoke();
    }

    // 복불복 게임에서 쓸것

    public void RollAllDiceMain()
    {
        totalValue = 0;
        //reset total value

        for (int i = 0; i < diceList.Count; i++)
        {
            diceList[i].AddForceDiceMain();
        }
    }

    //이건 그냥 PlayerToken이 거기에 올라갔을대를 상정하는것, idx와 타일에 올라갔을때를 연결해야겟네 for문 돌리고.. 이게 맞는지 확인하는 작업을 해야겟네 그럼 PalyerToken에서 말이지..즉 타일에서 확인하고 그게 이거면 셋엑티브한다는 것을 만들어야해
    public void OnTile(int idx)
    {
        ShowPanel.SetActive(true);
        TileImageList[idx].SetActive(true);
        // 효과주려면 코루틴 쓰고 그냥 셋엑티브만..일단먼저
    }



    // 패널 Showpanel에 버튼달것
    public void CloseShowPanel(int idx)
    {
        ShowPanel.SetActive(false);
        TileImageList[idx].SetActive(false);
    }



    public void OnGoHome(string SceneName)
    {
        SceneChanger.LoadScene(SceneName, objBoard);
    }

}