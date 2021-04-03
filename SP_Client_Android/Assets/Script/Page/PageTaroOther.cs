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
    public GameObject objSecondBoard;
    public CanvasGroup[] cgTypeThird;
	public CanvasGroup[] cgTypeResult;

    public GameObject[] objStartBtn;

    public GameObject objReturnHome;
    public GameObject objReturnPrev;

    public GameObject BlindPanel;
    public GameObject CardSelectPanel;
    public GameObject TextPanel;

    public GameObject objTodaySolutionPanel;

    public List<GameObject> ResultList = new List<GameObject>();

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

        for (int i = 0; i < cgTypeThird.Length; i++)
            cgTypeThird[i].alpha = i == curBoardIdx ? 1f : 0f;

        for (int i = 0; i < cgTypeResult.Length; i++)
			cgTypeResult [i].alpha = i == curBoardIdx ? 1f : 0f;

        DOTween.RewindAll ();

        objSecondBoard.SetActive(false);

        _ChangeBtnsActive (true);
	}

    public void OnClickFirst(int nType)
    {
        curType = (ETaroType)nType;
        base.OnNext();
        objSecondBoard.SetActive(true);

        for (int i = 0; i < objStartBtn.Length; i++)
        {
            objStartBtn[nType].SetActive(true);
        }


        _ChangeBtnsActive(false);
    }

    public void OnClick(int nType)
    {
		curType = (ETaroType)nType;
		base.OnNext ();

		for (int i = 0; i < cgTypeThird.Length; i++)
        {
            cgTypeThird[i].alpha = i == nType ? 1f : 0f;
            cgTypeThird[nType].gameObject.SetActive(true);
        }

        _ChangeBtnsActive(false);
    }


    public void OnGoResult(int idx)
    {
        base.OnNext();
        ResultList[idx].SetActive(true);
    }

    public void ActiveTodaySolutionPanel()
    {
        objTodaySolutionPanel.SetActive(true);
    }

    public void DeActiveTodaySolutionPanel()
    {
        StartCoroutine(_DelayClosePanel());
    }


    IEnumerator _DelayClosePanel()
    {
        yield return new WaitForSeconds(2f);
        objTodaySolutionPanel.SetActive(false);

    }


    public void OnGoFirst()
    {
        for (int i = 0; i < ResultList.Count; i++)
            ResultList[i].SetActive(false);
            objTodaySolutionPanel.SetActive(false);

        for (int i = 0; i < objStartBtn.Length; i++)
            objStartBtn[i].SetActive(false);


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
        for (int i = 0; i < ResultList.Count; i++)
            ResultList[i].SetActive(false);
            objTodaySolutionPanel.SetActive(false);

        for (int i = 0; i < objStartBtn.Length; i++)
            objStartBtn[i].SetActive(false);

        base.OnPrev ();

        if (curBoardIdx == 0)
            StartCoroutine(_delayInit());
    }

    public void StartBtn(int nType)
    {
        TextPanel.SetActive(true);
        DOTween.PlayAll();

		StartCoroutine(ShowCardPanel(nType));
		StartCoroutine (_taroTyping ());
    }

    IEnumerator ShowCardPanel(int nType)
    {
        yield return new WaitForSeconds(10f);
        TextPanel.SetActive(false);


        for (int i = 0; i < cgTypeThird.Length; i++)
        {
            cgTypeThird[i].alpha = i == nType ? 1f : 0f;
            cgTypeThird[nType].gameObject.SetActive(true);
        }

        BlindPanel.SetActive(true);

        //여기서 수정해줘야돼 Third를


        yield return new WaitForSeconds(1f);
        CardSelectPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
    }

    IEnumerator _taroTyping()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            for (int i = 0; i < m_text.Length; i++)
            {
                tx.text = m_text.Substring(0, i);
                yield return new WaitForSeconds(0.3f);
            }
        }
    }


}