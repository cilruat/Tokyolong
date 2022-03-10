using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Npc_8 : MonoBehaviour {

    Tween myPathTween;
    SpriteRenderer sprite;
    public GameObject objTalkBox;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        objTalkBox.SetActive(false);


        // Get tween from DOTweenPath and attach callback
        myPathTween = this.GetComponent<DOTweenPath>().GetTween();
        myPathTween.OnWaypointChange(WPCallback);

    }

    void WPCallback(int waypointIndex)
    {
        if (waypointIndex == 4)
        {
            myPathTween.Pause();
            StartCoroutine(Wait());
        }

        if (waypointIndex == 5)
        {
            StartCoroutine(Flip());
        }

        if (waypointIndex == 8)
        {
            StartCoroutine(Flip());
        }

    }

    // Call this method when you're ready to resume the tween
    void ResumeTween()
    {
        myPathTween.Play();
    }


    IEnumerator Wait()
    {
        objTalkBox.SetActive(true);
        sprite.flipX = true;
        yield return new WaitForSeconds(3f);
        myPathTween.Play();
        objTalkBox.SetActive(false);

    }

    IEnumerator Flip()
    {
        yield return new WaitForSeconds(0.1f);
        sprite.flipX = false;
    }

}
