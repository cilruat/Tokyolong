using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NPCControll : MonoBehaviour {


    Tween myPathTween;

    public Tween[] Tweens;
    public GameObject[] objNpc;

    //각각의 스크립트를 짜야하나....슬프네..스크립트 하나에 각각의 리스트, 트윈들의 배열을 넣고 인덱스 ㅏㄴ큼 넣으면 되는것 아닌가요

    void Start()
    {

        for (int i = 0; i < objNpc.Length; ++i)
        {
            Tweens[i] = objNpc[i].GetComponent<DOTweenPath>().GetTween();

        }

        // Get tween from DOTweenPath and attach callback
        myPathTween = this.GetComponent<DOTweenPath>().GetTween();
        myPathTween.OnWaypointChange(WPCallback);
    }

    void WPCallback(int waypointIndex)
    {
        if (waypointIndex == 3)
        {
            myPathTween.Pause();
            Debug.Log("3에도착했습니다");
            StartCoroutine(Wait());
        }
    }

    // Call this method when you're ready to resume the tween
    void ResumeTween()
    {
        myPathTween.Play();
    }



    public void Talk()
    {
        Debug.Log("g");
        StartCoroutine(Wait());


    }


    IEnumerator Wait()
    {
        Debug.Log("3초 시자악!");

        yield return new WaitForSeconds(3f);
        Debug.Log("3초끝");
        myPathTween.Play();

    }
}
