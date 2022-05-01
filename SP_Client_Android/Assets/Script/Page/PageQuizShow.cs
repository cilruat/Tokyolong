using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageQuizShow : PageBase {


    public CanvasGroup[] cgBoard;
    public CanvasGroup cgFirst;
    public CanvasGroup cgSecond;

    public GameObject objTokyoQuiz;

    CanvasGroup _prevCG;
    CanvasGroup _curCG;



    void Start () {

        StartCoroutine(GoSecond());
	}

    protected override void Awake()
    {
        base.boards = cgBoard;
        base.Awake();

        _curCG = cgFirst;
    }


    void _ChangeShow(CanvasGroup nextCG)
    {
        _prevCG = _curCG;
        Info.AnimateChangeObj(_curCG, nextCG);
        _curCG = nextCG;
    }



    IEnumerator GoSecond()
    {
        yield return new WaitForSeconds(8f);
        OnGoSecond();
    }

    public void OnGoSecond()
    {
        _ChangeShow(cgSecond);

        objTokyoQuiz.GetComponent<TokyoQuiz>().OnStart();
    }



}
