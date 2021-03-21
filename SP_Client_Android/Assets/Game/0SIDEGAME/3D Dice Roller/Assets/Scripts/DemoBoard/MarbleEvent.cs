using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class MarbleEvent : MonoBehaviour {

    public List<string> BadCard;
    public List<string> GoodCard;

    public int StackSoju = 0;


    public List<GameObject> ShowTileList = new List<GameObject>();

    //OntriggerStay 써서 계속 있으면 그 값을 셋엑티브 하면 될거같다 일단
    //case문에서 for문을 돌면서 i값을 상속받고 그리스트 값을 활성화한다 까지정도

    public void EndRollEvent()
    {

    }

}
