using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class MarbleEvent : MonoBehaviour {

    //public List<string> BadCard;
    //public List<string> GoodCard;

    public bool IsGoing = false;

    public int TileNum = 0;
    public GameObject ShowPanel;
    public List<GameObject> TileImageList = new List<GameObject>();

    public GameObject Token;


    public List<Tile> listTile;





    //OnTileCheck 뒤에 하는거 있어야겟네


    IEnumerator _StayCheck()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("코루틴실행되고 정지되있는거 체크된다는 뜻");
        OnTileCheck();
    }


    //내가 서있는 타일의 값만 체크,...어떻게할까

    public void OnTileCheck()
    {
        int findidx = -1;
        TileClass tile = GetComponent<TileClass>();

        for(int i = 0; i < TileImageList.Count; i++)
        {

            if (TileImageList[i].tag == "Tile")
            {

                //다 같으니깐 다 찍어주겟지 임마
                //현재 내가 서 있는 Tile값만 체크해주어야한다..
                findidx = i;
                Debug.Log(i + "번입니다 ");
            }





            //listTile[i].gameObject.SetActive(true);
        }
    }




    public void CloseShowPanel(int idx)
    {
        ShowPanel.SetActive(true);
        TileImageList[idx].SetActive(true);
        // 효과주려면 코루틴 쓰고 그냥 셋엑티브만..일단먼저



        ShowPanel.SetActive(false);
        TileImageList[idx].SetActive(false);
    }


    public void OnEvent()
    {

        Debug.Log("트리거이벤트");
    }
}



