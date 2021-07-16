using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OrderNPCMove : MonoBehaviour {

    public GameObject chrJessi;



    private void Update()
    {
        Vector3 target = new Vector3(311, 78, 0);

        chrJessi.transform.position = Vector3.MoveTowards(chrJessi.transform.position, target, 1f);

    }

    public void MoveToSideMenu()
    {
    }


    public void MessageOnSideMenu()
    {

    }


}
