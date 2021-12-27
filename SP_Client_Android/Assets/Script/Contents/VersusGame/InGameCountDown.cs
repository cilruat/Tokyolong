using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InGameCountDown : MonoBehaviour {

    public int countdownTime;
    public Text countdownDisplay;
    public GameObject objDisplay;

    private void Start()
    {
        countdownDisplay.gameObject.SetActive(true);
        StartCoroutine(CountdownToStart());
    }


    IEnumerator CountdownToStart()
    {
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);
            UITweenAlpha.Start(objDisplay, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
            UITweenScale.Start(objDisplay.gameObject, 1f, 1.3f, TWParam.New(.3f).Curve(TWCurve.Bounce));


            countdownTime--;
        }

        countdownDisplay.text = "랜덤으로 선택합니다";



        yield return new WaitForSeconds(1f);

        UIManager.Instance.isGameRoom = false;

        countdownDisplay.gameObject.SetActive(false);
    }
}
