using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class coTest : MonoBehaviour {


    public int countdownTime;
    public Text countdownDisplay;
    public GameObject objDisplay;


    // GoldFish
    public GameObject objFish;

    public float xMinBoundary;
    public float yMinBoundary;
    public float xMaxBoundary;
    public float yMaxBoundary;

    RectTransform fish;


    IEnumerator coroutine;
    public void MoveSpaceShip()
    {

        float randX = Random.Range(xMinBoundary, xMaxBoundary);

        float randY = Random.Range(yMinBoundary, yMaxBoundary);

        Vector2 target = new Vector2(randX, randY);

        target = this.gameObject.transform.position;

        //Option 1

        //small_star.transform.Translate(target * 2);

        //Option 2


        fish = objFish.GetComponent<RectTransform>();



        
        }

​

    public void StartCountdown()
    {
        coroutine = myCoroutine();
        StartCoroutine(coroutine);
    }

    public void StopCountdown()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    IEnumerator myCoroutine()
    {
        yield return null;

        countdownTime = 20;

        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            UITweenAlpha.Start(objDisplay, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
            UITweenScale.Start(objDisplay.gameObject, 1f, 1.3f, TWParam.New(.3f).Curve(TWCurve.Bounce));

            countdownTime--;
        }

        countdownDisplay.text = "시간만료";


        yield return new WaitForSeconds(1f);

        // 자동패 올라가게 REQ 보낼것
        //UI 작업
        Debug.Log("카운트 다운 종료 UI 작업");

        countdownDisplay.gameObject.SetActive(false);
    }





}
