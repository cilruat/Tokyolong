using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class MarbleEvent : MonoBehaviour {


    public CanvasGroup[] cgTile;

    public List<GameObject> cgTile = new List<GameObject>();

    public List<GameObject> TileImageList = new List<GameObject>();
    public GameObject ShowPanel;


    //이 스크립트는 단순히 어떤 행동을 종료하면 그 호출뒤에 하는 행동을 이벤트로 불러오는 기능이므로 순서는 Invoke 한 곳에서 바로 실행되는것. Invoke를 어디서 하느냐가 중요하다

    public void OnTileOn()
    {
        TileClass tilenum = new TileClass(); //클래스를 가져왓고

        int nType = tilenum.TileNum; // 매개변수를 지정해서 뭔지 알게햇고

        for (int i = 0; i < cgTile.Length; i++)
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



