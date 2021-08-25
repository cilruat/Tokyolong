using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameCountDown : MonoBehaviour {

    public int countdownTime;
    public Text countdownDisplay;

    private void Start()
    {
        countdownDisplay.gameObject.SetActive(true);
        StartCoroutine(CountdownToStart());
    }


    IEnumerator CountdownToStart()
    {
        while(countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }

        countdownDisplay.text = "GO!";



        yield return new WaitForSeconds(1f);

        //UI 작업
        //if~~
        Debug.Log("UI작업");

        countdownDisplay.gameObject.SetActive(false);
    }

}
