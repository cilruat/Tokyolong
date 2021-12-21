using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VersusCountDown : MonoBehaviour {

    public int countdownTime;
    public Text countdownDisplay;
    public GameObject objDisplay;
    public GameObject objBoard;

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
            UITweenScale.Start(objDisplay.gameObject, 1f, 2f, TWParam.New(.3f).Curve(TWCurve.Bounce));


            countdownTime--;
        }

        countdownDisplay.text = "TIMEOUT!";



        yield return new WaitForSeconds(1f);

        SceneChanger.LoadScene("Main", objBoard);

        //일단 여기서 통제하고 Debug찍기, 항상 true인가, 그래서 초대가 안되는가?
        //UIManager.Instance.isGameRoom = false;

        Debug.Log("UI작업");

        countdownDisplay.gameObject.SetActive(false);
    }
}
