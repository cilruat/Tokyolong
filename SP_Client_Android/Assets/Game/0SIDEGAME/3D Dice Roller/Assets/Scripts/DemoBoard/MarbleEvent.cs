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
        if (token.IsGoing == true)
        {
            Debug.Log("가지않는중");
        }
        else
        {
            for (int i = 0; i < cgTile.Length; i++)
            {
                cgTile[i].alpha = i == tilenum.TileNum ? 1f : 0f;
                Debug.Log(i + "Check");
            }
        }
    }


    public void CloseShowPanel(int idx)
    {
        ShowPanel.SetActive(false);
        TileImageList[idx].SetActive(false);
    }
}



