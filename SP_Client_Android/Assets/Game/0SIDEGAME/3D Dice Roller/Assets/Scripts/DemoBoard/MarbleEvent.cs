using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class MarbleEvent : MonoBehaviour {


    public List<GameObject> cgTile = new List<GameObject>();
    public List<GameObject> TileImageList = new List<GameObject>();
    public GameObject ShowPanel;
    public DiceManager diceManager;




    public void OnTileOn()
    {
        TileClass tilenum = new TileClass(); //클래스를 가져왓고

        int nType = tilenum.TileNum; // 매개변수를 지정해서 뭔지 알게햇고

        for (int i = 0; i < cgTile.Count; i++)
        {
            /* 맞는 무언가가 있어야해
            if (cgTile[nType] = )
            {
                Debug.Log(i + "Check");

            }
            */
            //cgTile[nType].gameObject.SetActive(true);
        }
    }





    public void CloseShowPanel(int idx)
    {
        ShowPanel.SetActive(false);
        TileImageList[idx].SetActive(false);
    }
    
}



