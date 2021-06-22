using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class MarbleEvent : MonoBehaviour {


    public CanvasGroup[] cgTile;
    public GameObject Token;
    public List<GameObject> TileImageList = new List<GameObject>();
    public bool IsGoing = false;
    PlayerToken token = new PlayerToken();

    public GameObject ShowPanel;
    public List<Tile> listTile;


    public void OnTileOn()
    {
        TileClass tilenum = new TileClass();

        int nType = tilenum.TileNum;

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



