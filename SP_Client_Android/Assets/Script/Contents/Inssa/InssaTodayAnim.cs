using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InssaTodayAnim : MonoBehaviour {

    public Animator OpenBookAnim;
    public bool OpenBookAnimPlay = false;

    public Animator CloseBookAnim;
    public bool CloseBookAnimPaly = false;



    public GameObject CountdownPanel;


    public void OnOpenBookAnim()
    {
        if (OpenBookAnimPlay == false)
        {
            CountdownPanel.SetActive(true);
            OpenBookAnim.Play("OpenBook");
        }
    }



    public void OnCloseBookAnim()
    {
        if (CloseBookAnimPaly == false)
        {
            CountdownPanel.SetActive(true);
            CloseBookAnim.Play("CloseBook");
        }
    }




    public void OnCountdownAnimComplete()
    {
        OpenBookAnimPlay = false;
        StartCoroutine(RestartAnim());
    }


    IEnumerator RestartAnim()
    {
        CountdownPanel.SetActive(false);
        yield return new WaitForSeconds(2.0f);
    }

}