using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class InssaFingerAnim : MonoBehaviour {

    public Animator CountdownAnimation;
    public bool CountdownPlayAnimation = false;

    public Animator FingerAnimation;
    public bool FingerPlayAnimation = false;



    public GameObject CountdownPanel;


    public void OnClickFinger()
    {
        if (CountdownPlayAnimation == false)
        {
            CountdownPanel.SetActive(true);
            CountdownAnimation.Play("321");
        }
    }

    //다시 false로 돌아올수 없으니 안되겟네




    public void OnCountdownAnimComplete()
    {
        CountdownPlayAnimation = false;
        StartCoroutine(RestartAnim());
    }


    IEnumerator RestartAnim()
    {
        CountdownPanel.SetActive(false);
        yield return new WaitForSeconds(2.0f);
    }


}

