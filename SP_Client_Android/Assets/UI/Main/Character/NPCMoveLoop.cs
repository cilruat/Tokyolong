using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NPCMoveLoop : MonoBehaviour {


    //왕복운동을 하는 NPC 따로 관리하는 스크립트
    public float loopTime;

    SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(RoundTrip());
    }

    IEnumerator RoundTrip()
    {
        while(true)
        {
            yield return new WaitForSeconds(loopTime);
            sprite.flipX = false;
            yield return new WaitForSeconds(loopTime);
            sprite.flipX = true;

        }
    }
}
