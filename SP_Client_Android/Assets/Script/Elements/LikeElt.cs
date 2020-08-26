using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LikeElt : MonoBehaviour {

    public Text Table;
    public Text Desc;

    UserLikeInfo info = null;

    //여기는 쪽지보내면 받아오는 정보를 그냥 쭉 나열하기만 하면 되는데
    //SetInfo도 테이블정보만 받아서 나열하는 역할이다 그냥
    //그리고 Del만 잘해주면 되지않냐



    //연결
    public void SetInfo(UserLikeInfo info)
    {
        this.info = info;
        Table.text = string.Format("{0:D2}", info.tableNo);
    }

    public void OnDelete()
    {
        if (!Info.isCheckScene("Mail"))
            return;

        PageMail.Instance.DeleteLikeElt(this);
    }
    public byte GetTableNo() { return info.tableNo; }
}
