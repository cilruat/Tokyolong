using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatTableElt : MonoBehaviour 
{
    public Button btn;
    public Image imgSelect;
    public Text textTableNo;
    public GameObject objNew;

    public void AddNewChat()
    {
        if (objNew.activeSelf == false)
            objNew.gameObject.SetActive(true);



    }

    public void OnSelect()
    {
        if (objNew.activeSelf)
            objNew.gameObject.SetActive(false);
    }
}
