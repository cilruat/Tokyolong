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

    public bool IsGoing = false;


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



    IEnumerator _StayCheck()
    {
        IsGoing = true;
        yield return new WaitForSeconds(1f);
        Debug.Log("코루틴실행되고 정지되있는거 체크된다는 뜻");

        // bool 값을, 1초 이상 정지해있다면 true, 아니라면 false
        // bool 값에따라서 bool이 true면 가고있는중이니껜 안되고, bool이 false이면 멈춘상태이니깐 false값 확인해서 false면 그 타일의 셋엑티브 true, 태그를 다 달고...? 하면 되나
    }


    //코루틴 만들어서 0.5초 이상 콘텍하고 있으면?


    // 패널 Showpanel에 버튼달것
    public void CloseShowPanel(int idx)
    {

        if (IsGoing == true)
        {

        }


        ShowPanel.SetActive(false);
        TileImageList[idx].SetActive(false);
    }



    public void OnGoHome(string SceneName)
    {
        SceneChanger.LoadScene(SceneName, objBoard);
    }

    //Collider에 멈추어 있으면 이게 발동 -> 코루틴 실행(IsGoing 체크) -> 1초의 간격을 두고 결국에는 멈추게 될것이니까 1초 뒤에 false가 되는 값들이 있을꺼야, 그 값에 해당하는걸 for문 돌려서 하면 안될까?
    private void OnCollisionStay(Collision collision)
    {
        StartCoroutine(_StayCheck());
    }

    


}