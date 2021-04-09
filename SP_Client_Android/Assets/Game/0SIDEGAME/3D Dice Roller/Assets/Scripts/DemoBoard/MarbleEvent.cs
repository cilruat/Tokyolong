using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class MarbleEvent : MonoBehaviour {

    //public List<string> BadCard;
    //public List<string> GoodCard;


    public CanvasGroup[] cgTile;
    public GameObject Token;
    public List<GameObject> TileImageList = new List<GameObject>();
    public bool IsGoing = false;
    TileClass tilenum = new TileClass();
    PlayerToken token = new PlayerToken();

    public GameObject ShowPanel;
    public List<Tile> listTile;


    public void OnTileOn()
    {
        int nType = tilenum.TileNum;

        //Tile에 붙은 TileNum의 값이 내가 위에 있는 타일의idx값과 같다면



        for (int i = 0; i < cgTile.Length; i++)
        {
            //cgTile[i].alpha = i == nType ? 1f : 0f;
            //if(cgTile[nType] == )
            cgTile[nType].gameObject.SetActive(true);
            Debug.Log(i + "Check");
        }
    }


    public void CloseShowPanel(int idx)
    {
        ShowPanel.SetActive(false);
        TileImageList[idx].SetActive(false);
    }
}



