using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace BlockJumpRabbit
{
public class TutorialController:MonoBehaviour
{
    private bool canClick;

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && canClick)
        {
            canClick = false;
            GetComponent<Animator>().SetTrigger("finish");       
        }
    }

    void CanClick()
    {
        canClick = true;
    }

    void Deactive()
    {
        StartCoroutine(DeactiveByTime());
    }

    IEnumerator DeactiveByTime()
    {
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }
}
}
