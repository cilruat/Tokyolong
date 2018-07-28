using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLogElt : MonoBehaviour
{
    public ShowLog owner;
    public Image imgBack;
    public Text textLog;

    int idx = -1;

    Color colorBasic1 = new Color(.219f, .376f, .415f, .7f);
    Color colorBasic2 = new Color(.121f, .278f, .317f, .7f);
    Color colorSelect = new Color(.545f, .235f, 0f, .7f);

    public void SetLog(int idx, SPLog log)
    {
        this.idx = idx;
        RefreshBackColor();
        string strLog = log.condition + "\n" + log.stackTrace;

        string color = string.Empty;
        switch (log.type)
        {
            case LogType.Error:
            case LogType.Assert:
            case LogType.Exception: color = "<color=#ff0000>";   break;
            case LogType.Warning:   color = "<color=#ffc300>";   break;
            case LogType.Log:                                   break;
        }

        if(string.IsNullOrEmpty(color) == false)
            strLog = color + strLog + "</color>";

        textLog.text = strLog;
    }

    public void OnSelect()
    {
        owner.OnSelectElt(idx);
        imgBack.color = colorSelect;
    }

    public void RefreshBackColor()
    {
        imgBack.color = (idx % 2) == 0 ? colorBasic1 : colorBasic2;
    }
}
