using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToClick : MonoBehaviour {


    [SerializeField] float speed = 10f;
    Vector3 mousePos, transPos, targetPos;






	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetMouseButtonDown(0))
        {
            CalTargetPos();
            Debug.Log("키다운");
            MoveToTarget();
        }
    }


    void CalTargetPos()
    {
        mousePos = Input.mousePosition;
        transPos = Camera.main.WorldToScreenPoint(mousePos);
        targetPos = new Vector3(transPos.x, transPos.y, 0);
    }


    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
    }
}
