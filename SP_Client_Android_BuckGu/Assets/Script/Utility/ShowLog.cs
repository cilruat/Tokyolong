using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLog : MonoBehaviour
{
    public RectTransform rtBoard;
    public ScrollRect srBoard;
    public ShowLogElt eltLog; 

    public RectTransform rtDetail;
    public ScrollRect srDetail;
    public Text textDetailLog;

    public bool show = false;

    int lastIndex = -1;
    SPLog[] logs;

    int selectIdx = -1;

    Dictionary<int, ShowLogElt> dictLog = new Dictionary<int, ShowLogElt>();
    GridLayoutGroup glLogBoard;

    void Awake()
    {
        if (eltLog.gameObject.activeSelf)
            eltLog.gameObject.SetActive(false);

        glLogBoard = srBoard.content.GetComponent<GridLayoutGroup>();
    }

    public void Show()
    {
        show = true;
        this.gameObject.SetActive(true);

        this.lastIndex = VLogSave.lastIndex;
        this.logs = VLogSave.logs;

        int idx = this.lastIndex;
        idx = (idx + 1 < this.logs.Length && this.logs[idx + 1] != null) ? idx + 1 : 0;

        for (int i = 0; i < this.logs.Length; i++)
        {
            if (logs[idx] == null)
                break;

            _AddLog(idx, logs[idx]);

            ++idx;
            if (idx >= this.logs.Length)
                idx = 0;
        }

        textDetailLog.text = "";

        LogBoardResize(true);
        UITweenScale.Start(this.gameObject, .5f, 1f, TWParam.New(.2f).Curve(TWCurve.CurveLevel4).Speed(TWSpeed.Slower));

        VLogSave.onSendLog = _AddLog;
    }

    public void Hide()
    {
        show = false;
        VLogSave.onSendLog = null;
        UITweenScale.Start(this.gameObject, 1f, 0f, TWParam.New(.2f).Curve(TWCurve.CurveLevel4).Speed(TWSpeed.Faster).DisableOnFinish());
    }

    ShowLogElt CreateShowLogElt()
    {
        GameObject newObj = Instantiate(eltLog.gameObject) as GameObject;
        newObj.gameObject.SetActive(true);

        ShowLogElt newElt = newObj.GetComponent<ShowLogElt>();
        if (newElt == null)
        {
            Debug.Log("ShowLogElt Componet Error");
            return null;
        }

        newElt.transform.SetParent(srBoard.content);
        newElt.transform.InitTransform();

        return newElt;
    }

    void _AddLog(int idx, SPLog log)
    {
        ShowLogElt elt = null;
        if (dictLog.ContainsKey(idx) == false)
            dictLog.Add(idx, CreateShowLogElt());

        elt = dictLog[idx];
        if (elt == null)
            return;

        elt.SetLog(idx, log);
        elt.transform.SetAsLastSibling();
        LogBoardResize(true);
    }

    public void OnSelectElt(int selectIdx)
    {
        if (this.selectIdx == selectIdx)
            return;

        if (dictLog.ContainsKey(this.selectIdx))
            dictLog[this.selectIdx].RefreshBackColor();

        this.selectIdx = selectIdx;
        SPLog log = logs[selectIdx];
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

        textDetailLog.text = strLog;

        float textHeight = textDetailLog.preferredHeight;
        srDetail.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textHeight);
    }

    void LogBoardResize(bool focus)
    {
        float height = glLogBoard.cellSize.y * (float)dictLog.Count;
        height += glLogBoard.spacing.y * (float)Mathf.Max((dictLog.Count - 1), 0);

        if (height != srBoard.content.rect.height)
            srBoard.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        if(focus)
            srBoard.content.anchoredPosition = new Vector2(0f, Mathf.Max( (height - rtBoard.rect.height) + 8f, 0f));
    }
}
