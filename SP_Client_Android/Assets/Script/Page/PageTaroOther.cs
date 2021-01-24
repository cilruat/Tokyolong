using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PageTaroOther : PageBase {

    public CanvasGroup[] cgBoard;
    public CanvasGroup cgFirst;
    public GameObject objReturnHome;
    public GameObject objReturnPrev;
    public GameObject objReturnFirst;

    public GameObject objStartBtn;
    public GameObject objResetBtn;


    public GameObject BlindPanel;
    public GameObject CardSelectPanel;
    public GameObject TextPanel;

    public Text tx;
    public string m_text = "";


    CanvasGroup _prevCG;
    CanvasGroup _curCG;

    private void Start()
    {
        objStartBtn.SetActive(true);
        objResetBtn.SetActive(false);
        CardSelectPanel.SetActive(false);
        BlindPanel.SetActive(false);
        TextPanel.SetActive(false);


    }

    protected override void Awake()
    {
        base.boards = cgBoard;
        base.Awake();

        _curCG = cgFirst;
        _ChangeReturn(true);
    }

    void _ChangeShow(CanvasGroup nextCG)
    {
        _prevCG = _curCG;
        Info.AnimateChangeObj(_curCG, nextCG);
        _curCG = nextCG;

    }

    void _ChangeReturn(bool isHome)
    {
        objReturnHome.SetActive(isHome);
        objReturnPrev.SetActive(!isHome);
        objReturnFirst.SetActive(!isHome);
    }

    public void OnPrev()
    {
        if (_prevCG == cgFirst)
            OnGoFirst();
        else
            _ChangeShow(_prevCG);
    }

    public void OnClick(CanvasGroup showCG)
    {
        _ChangeShow(showCG);
        _ChangeReturn(false);
    }

    public void OnGoFirst()
    {
        _ChangeShow(cgFirst);
        _ChangeReturn(true);
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------

    public void StartBtn()
    {
        objStartBtn.SetActive(false);
        TextPanel.SetActive(true);
        DOTween.PlayAll();
        StartCoroutine(ShowCardPanel());
        StartCoroutine(_taroTyping());

        //코루틴으로 N초뒤에 SetAcitve True 할것 패널
    }


    public void ResetBtn()
    {
        objStartBtn.SetActive(true);
        //DOTween.PlayAll();

    }

    IEnumerator _taroTyping()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            for(int i = 0; i < m_text.Length; i++)
            {
                tx.text = m_text.Substring(0, i);
                yield return new WaitForSeconds(0.3f);
            }
        }
    }


    IEnumerator ShowCardPanel()
    {
        //연출이 중요
        yield return new WaitForSeconds(10f);
        //UITweenAlpha.Start(CardSelectPanel, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
        TextPanel.SetActive(false);
        BlindPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        CardSelectPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
    }





}


