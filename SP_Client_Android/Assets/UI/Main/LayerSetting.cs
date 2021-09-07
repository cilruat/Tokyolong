using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSetting : MonoBehaviour {

    SpriteRenderer sr;


    void Start()
    {

        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = Mathf.RoundToInt(transform.position.y) * -1;
    }



}
