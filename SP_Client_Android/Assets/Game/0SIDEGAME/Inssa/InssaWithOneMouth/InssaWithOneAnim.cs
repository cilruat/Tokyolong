using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InssaWithOneAnim : MonoBehaviour {

    public Animator CountdownAnimation;
    public bool CountdownPlayAnimation = false;

    public GameObject CountdownPanel;

    public GameObject StartBtn;

    public void OnClick()
    {
        StartBtn.SetActive(false);
        if (CountdownPlayAnimation == false)
        {
            CountdownPanel.SetActive(true);
            CountdownAnimation.Play("WithOneMouth");
        }
    }

    public void OnCountdownAnimComplete()
    {
        CountdownPlayAnimation = false;
        StartBtn.SetActive(true);
        StartCoroutine(RestartAnim());
    }


    IEnumerator RestartAnim()
    {
        CountdownPanel.SetActive(false);
        yield return new WaitForSeconds(2.0f);
    }

}
