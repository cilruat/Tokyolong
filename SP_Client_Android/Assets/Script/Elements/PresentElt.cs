using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PresentElt : MonoBehaviour {

    public Text Table;
    public Text Count;
    public Text Desc;

    UserPresentInfo info = null;

    public void SetInfo(UserPresentInfo info)
    {
        this.info = info;
        Table.text = string.Format("{0:D2}", info.tableNo);
        Count.text = string.Format("{0:D1}", info.presentCount);
    }

    public void OnDelete()
    {
        Destroy(gameObject);
    }
}
