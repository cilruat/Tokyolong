using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClass : MonoBehaviour {

    public int Tileidx = 0;

    public int SetTile()
    {
        return Tileidx;
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("ㅎㅎㅎ");
    }
}
