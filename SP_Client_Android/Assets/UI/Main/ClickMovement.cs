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
            //방향
            var dir = destination - transform.position;


            transform.position += dir.normalized * Time.deltaTime * moveSpeed;

            // 각도 계산해서 그 각도가 180 / 360 나눠서 그 이하면 회전하고 아니면 회전하지마라 이렇게만 정의하고 카메라는 위치는 옮기긴 해야겟는디..
            // 마우스 클릭한 위치가 플레이어위 위치값보다 +면 아니면 y축을 반전해라
            if( dir.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        if (Vector3.Distance(transform.position, destination) <= 0.1f)
        {
            isMove = false;
            animator.SetBool("isMove", false);
        }
    }

}
