using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMovement : MonoBehaviour {


    private Camera camera;
    public Animator animator;
    public float moveSpeed;


    private bool isMove;
    private Vector3 destination;

    private void Awake()
    {
        camera = Camera.main;
        animator = GetComponent<Animator>();
    }




    void Start () {
		
	}
	
	void Update () {
		
        if(Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                SetDestination(hit.point);
            }
        }
        Move();
	}



    private void SetDestination(Vector3 dest)
    {
        destination = dest;
        isMove = true;
        animator.SetBool("isMove", true);
    }

    private void Move()
    {
        if(isMove)
        {
            var dir = destination - transform.position;
            transform.position += dir.normalized * Time.deltaTime * moveSpeed;
        }

        if(Vector3.Distance(transform.position, destination) <= 0.1f)
        {
            isMove = false;
            animator.SetBool("isMove", false);
        }
    }

}
