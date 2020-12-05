using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PageLobby : PageBase
{

    public GameObject objBoard;


    public Text txtPlayCnt;
    public Text txtTableNo;

    public Button[] CellBtn;
    public Button PreviousBtn, NextBtn;
    public List<string> myList = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18" };

    int currentPage = 1, maxPage, multiple;



    protected override void Awake()
    {
        base.Awake();

        txtPlayCnt.text = Info.GamePlayCnt.ToString();
        txtTableNo.text = Info.TableNum.ToString();
        //Info.practiceGame = false;
    }


    private void Start()
    {
        // 최대페이지
        maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        // 이전, 다음버튼
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellBtn[i].GetComponentInChildren<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i] : "";
        }
    }


    // ◀버튼 -2 , ▶버튼 -1 , 셀 숫자
    public void BtnClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else print(myList[multiple + num]);

        Start();
    }


    [ContextMenu("리스트추가")]
    void ListAdd() { myList.Add("뚝배기"); Start(); }


    [ContextMenu("리스트제거")]
    void ListRemove() { myList.RemoveAt(0); Start(); }



    public void RefreshGamePlayChance()
    {
        if (Info.isCheckScene("Lobby") == false)
            return;

        txtPlayCnt.text = Info.GamePlayCnt.ToString();

    }


    public void ReturnHome()
    {
        SceneChanger.LoadScene("Main", objBoard);
    }

    public void ReturnFirst()
    {
        SceneChanger.LoadScene("SelectGame", objBoard);
    }




}
