using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameElt : MonoBehaviour {

    public Text Table;
    public Text Count;
    public Text GameName;
    public Text Desc;

    UserGameInfo info = null;


    public void SetInfo(UserGameInfo info)
    {
        this.info = info;
        Table.text = string.Format("{0:D2}", info.tableNo);
        Count.text = string.Format("{0:D1}", info.reqGameCnt);
        Count.text = string.Format("{0:D1}", info.gameName);

    }

    public void OnDelete()
    {
        if (!Info.isCheckScene("Mail"))
            return;

        PageMail.Instance.DeleteGameElt(this);
    }
    public byte GetTableNo() { return info.tableNo; }
}
