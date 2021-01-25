using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PageTaroOther : PageBase {

	public enum ETaroType
	{
		eCharacter,
	}

    public CanvasGroup[] cgBoard;

	public CanvasGroup[] cgTypeSecond;
	public CanvasGroup[] cgTypeResult;

    public GameObject objReturnHome;
    public GameObject objReturnPrev;

    public GameObject objStartBtn;

    public GameObject BlindPanel;
    public GameObject CardSelectPanel;
    public GameObject TextPanel;

    public Text tx;
    public string m_text = "";

	ETaroType curType = ETaroType.eCharacter;

    private void Start()
    {
		_Init ();
    }

    protected override void Awake()
    {
        base.boards = cgBoard;
        base.Awake();

		_ChangeBtnsActive(true);
    }

    void _ChangeBtnsActive(bool isFirst)
    {
		objReturnHome.SetActive(isFirst);
		objReturnPrev.SetActive(!isFirst);
    }

	void _Init()
	{
		StopAllCoroutines ();

		for (int i = 0; i < cgTypeSecond.Length; i++)
			cgTypeSecond [i].alpha = i == curBoardIdx ? 1f : 0f;

		for (int i = 0; i < cgTypeResult.Length; i++)
			cgTypeResult [i].alpha = i == curBoardIdx ? 1f : 0f;

		DOTween.RewindAll ();

		objStartBtn.SetActive(true);
		CardSelectPanel.SetActive(false);
		BlindPanel.SetActive(false);
		TextPanel.SetActive(false);

		_ChangeBtnsActive (true);
	}

	public void OnClick(int nType)
    {
		curType = (ETaroType)nType;

		base.OnNext ();

		for (int i = 0; i < cgTypeSecond.Length; i++)
			cgTypeSecond [i].alpha = i == nType ? 1f : 0f;

		_ChangeBtnsActive(false);
    }

    public void OnGoFirst()
    {
		base.OnFirst ();
		StartCoroutine (_delayInit());
    }

	IEnumerator _delayInit()
	{
		yield return new WaitForSeconds (.8f);
		_Init ();
	}

	public void OnPrevBoard()
	{
		base.OnPrev ();	

		if (curBoardIdx == 0)
			StartCoroutine (_delayInit ());
	}

    public void StartBtn()
    {
        objStartBtn.SetActive(false);
        TextPanel.SetActive(true);

        DOTween.PlayAll();

		StartCoroutine(ShowCardPanel());
		StartCoroutine (_taroTyping ());
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