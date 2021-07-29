using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NPCControll : MonoBehaviour {



    public void Talk()
    {
        Debug.Log("g");
        StartCoroutine(Wait());


    }


    IEnumerator Wait()
    {

        yield return new WaitForSeconds(3f);
        Debug.Log("hhhhhhhhhhh");
        DOTween.Rewind(1, true);
        
    }
}
