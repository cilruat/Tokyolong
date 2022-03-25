using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Npc_9_Dog : MonoBehaviour {

    Tween myPathTween;
    SpriteRenderer sprite;


    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();


        // Get tween from DOTweenPath and attach callback
        myPathTween = this.GetComponent<DOTweenPath>().GetTween();
        myPathTween.OnWaypointChange(WPCallback);

    }

    void WPCallback(int waypointIndex)
    {
        if (waypointIndex == 10)
        {
            myPathTween.Pause();
            StartCoroutine(Wait());
        }

        if (waypointIndex == 16)
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
        sprite.flipX = true;
        yield return new WaitForSeconds(3f);
        myPathTween.Play();

    }

    IEnumerator Flip()
    {
        yield return new WaitForSeconds(0.1f);
        sprite.flipX = false;
    }


}
