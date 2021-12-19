using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersusElt : MonoBehaviour {

    public Text Table;
    public Text Count;
    public Text GameName;
    public Text Desc;

    UserGameAcceptInfo info = null;


    public void SetInfo(UserGameAcceptInfo info)
    {
        this.info = info;
        Table.text = string.Format("{0:D2}", info.tableNo);
        Count.text = string.Format("{0:D1}", info.reqGameCnt);
        Count.text = string.Format("{0:D1}", info.gameName);

    }



    public byte GetTableNo() { return info.tableNo; }


}
