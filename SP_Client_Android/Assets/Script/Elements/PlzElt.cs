using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlzElt : MonoBehaviour {

    public Text Table;
    public Text Count;
    public Text Desc;

    UserPlzInfo info = null;


    public void SetInfo(UserPlzInfo info)
    {
        this.info = info;
        Table.text = string.Format("{0:D2}", info.tableNo);
        Count.text = string.Format("{0:D1}", info.plzCount);
    }

    public void OnDelete()
    {
        Destroy(gameObject);
    }
}
