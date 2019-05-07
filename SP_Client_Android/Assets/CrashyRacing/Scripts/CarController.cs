using UnityEngine;
using System.Collections;
using System;

namespace CrashRacing
{
	public class CarController : MonoBehaviour
	{
	  
	    [HideInInspector]
	    public Rigidbody rigid;
	    [HideInInspector]
	    public float speed;
	    [HideInInspector]
	    public float turnTime;
	    [HideInInspector]
	    public bool stopMoving;
	    [HideInInspector]
	    public bool stopTurn;
	    [HideInInspector]
	    public bool isReverseCar;

	    private Vector3 movingDirection;
	    private bool isTurning;
	    // Use this for initialization
	    void Start()
	    {
	        rigid = GetComponent<Rigidbody>();
	        //Identify moving direcion and the rotation
	        if (isReverseCar)
	        {
	            transform.rotation = Quaternion.Euler(0, 180, 0);
	            movingDirection = Vector3.back;
	        }
	        else
	        {
	            transform.rotation = Quaternion.Euler(0, 0, 0);
	            movingDirection = Vector3.forward;
	        }
	    }
		
	    // Update is called once per frame
	    void FixedUpdate()
	    {

	        if (!stopMoving)
	        {
	            rigid.MovePosition(transform.position + movingDirection * speed * Time.fixedDeltaTime);
	//            rigid.velocity = movingDirection * speed;
	        }

	        if (!isReverseCar)
	        {
	            if (!stopTurn)
	            {
	                if (!isTurning)
	                {
	                    Ray rayFowward = new Ray(transform.position + new Vector3(0, 0.5f, 0), movingDirection);
	                    if (Physics.Raycast(rayFowward, 15f))
	                    {
	                        CheckAndTurn();
	                    }
	                }
	            }
	           
	        }
	    }

	    void CheckAndTurn()
	    {
	        if (Mathf.Round(transform.position.x) == -8) //Turn right
	        {
	            Ray rayRight = new Ray(transform.position + new Vector3(0, 0.5f, 0), Vector3.right);
	            if (!Physics.Raycast(rayRight, 4f))
	            {
	                Vector3 rayPoint = transform.position + new Vector3(0, 0.5f, 0) + Vector3.right * 4f;
	                Ray rayForwad = new Ray(rayPoint, Vector3.forward);
	                Ray rayBack = new Ray(rayPoint, Vector3.back);
	                if (!Physics.Raycast(rayForwad, 10f) && !Physics.Raycast(rayBack, 10f))
	                {
	                    StartCoroutine(Turn(4f));
	                    StartCoroutine(Rotate(-15f));
	                }
	            }
	        }
	        else if (Mathf.Round(transform.position.x) == 8) //Turn left
	        {
	            Ray rayLeft = new Ray(transform.position + new Vector3(0, 0.5f, 0), Vector3.left);
	            if (Physics.Raycast(rayLeft, 4f))
	            {
	                Vector3 rayPoint = transform.position + new Vector3(0, 0.5f, 0) + Vector3.left * 4f;
	                Ray rayFoward = new Ray(rayPoint, Vector3.forward);
	                Ray rayBack = new Ray(rayPoint, Vector3.back);
	                if (!Physics.Raycast(rayFoward, 10f) && !Physics.Raycast(rayBack, 10f))
	                {
	                    StartCoroutine(Turn(-4f));
	                    StartCoroutine(Rotate(15f));
	                }
	            }          
	        }
	        else
	        {
	            Ray rayRight = new Ray(transform.position + new Vector3(0, 0.5f, 0), Vector3.right);
	            Ray rayLeft = new Ray(transform.position + new Vector3(0, 0.5f, 0), Vector3.left);

	            if (Physics.Raycast(rayRight, 4f) && !Physics.Raycast(rayLeft, 4f))
	            {
	                Vector3 rayPoint = transform.position + new Vector3(0, 0.5f, 0) + Vector3.left * 4f;
	                Ray rayFoward = new Ray(rayPoint, Vector3.forward);
	                Ray rayBack = new Ray(rayPoint, Vector3.back);
	                if (!Physics.Raycast(rayFoward, 10f) && !Physics.Raycast(rayBack, 10f))
	                {
	                    StartCoroutine(Turn(-4f));
	                    StartCoroutine(Rotate(15f));
	                }
	            }
	            else if (Physics.Raycast(rayLeft, 4f) && !Physics.Raycast(rayRight, 4f))
	            {
	                Vector3 rayPoint = transform.position + new Vector3(0, 0.5f, 0) + Vector3.right * 4f;
	                Ray rayForwad = new Ray(rayPoint, Vector3.forward);
	                Ray rayBack = new Ray(rayPoint, Vector3.back);
	                if (!Physics.Raycast(rayForwad, 10f) && !Physics.Raycast(rayBack, 10f))
	                {
	                    StartCoroutine(Turn(4f));
	                    StartCoroutine(Rotate(-15f));
	                }
	            }
	            else if (!Physics.Raycast(rayRight, 4f) || !Physics.Raycast(rayLeft, 4f))
	            {
	                if (UnityEngine.Random.value <= 0.5f)
	                {
	                    Vector3 rayPoint = transform.position + new Vector3(0, 0.5f, 0) + Vector3.right * 4f;
	                    Ray rayForwad = new Ray(rayPoint, Vector3.forward);
	                    Ray rayBack = new Ray(rayPoint, Vector3.back);
	                    if (!Physics.Raycast(rayForwad, 10f) && !Physics.Raycast(rayBack, 10f))
	                    {
	                        StartCoroutine(Turn(4f));
	                        StartCoroutine(Rotate(-15f));
	                    }
	                }
	                else
	                {
	                    Vector3 rayPoint = transform.position + new Vector3(0, 0.5f, 0) + Vector3.left * 4f;
	                    Ray rayFoward = new Ray(rayPoint, Vector3.forward);
	                    Ray rayBack = new Ray(rayPoint, Vector3.back);
	                    if (!Physics.Raycast(rayFoward, 10f) && !Physics.Raycast(rayBack, 10f))
	                    {
	                        StartCoroutine(Turn(-4f));
	                        StartCoroutine(Rotate(15f));
	                    }
	                }
	            }            
	        }
	    }

	    IEnumerator Turn(float x)
	    {
	        isTurning = true;

	        float startX = transform.position.x;
	        float endX = startX + x;
	        float t = 0;
	        while (t < turnTime && !stopTurn)
	        {
	            t += Time.deltaTime;
	            float fraction = t / turnTime;
	            float newX = Mathf.Lerp(startX, endX, fraction);
	            Vector3 newPos = transform.position;
	            newPos.x = newX;
	            transform.position = newPos;
	            yield return null;
	        }

	        isTurning = false;
	    }

	    IEnumerator Rotate(float angle)
	    {
	        isTurning = true;
	        Quaternion startRot = transform.rotation;
	        Quaternion endRot = Quaternion.Euler(0, -angle, 0);
	        if (!isReverseCar)
	        {
	            endRot = Quaternion.Euler(0, -angle, 0);
	        }
	        float t = 0;
	        while (t < turnTime / 2f && !stopTurn)
	        {
	            t += Time.deltaTime;
	            float fraction = t / (turnTime / 2f);
	            transform.rotation = Quaternion.Lerp(startRot, endRot, fraction);
	            yield return null;

	        }

	        float r = 0;
	        while (r < turnTime / 2f && !stopTurn)
	        {
	            r += Time.deltaTime;
	            float fraction = r / (turnTime / 2f);
	            transform.rotation = Quaternion.Lerp(endRot, startRot, fraction);
	            yield return null;
	        }

	        isTurning = false;
	    }

	    void OnCollisionEnter(Collision col)
	    {
	        if (!stopMoving)
	        {
	            if (col.gameObject.CompareTag("Car"))
	            {
	                StartCoroutine(AddForce(transform.position - col.transform.position));
	                stopTurn = true;
	                stopMoving = true;
	            }
	            else if (col.gameObject.CompareTag("Player"))
	            {
	                if (!GameManager.Instance.GameState.Equals(GameState.Playing))
	                {
	                    StartCoroutine(AddForce(transform.position - col.transform.position));
	                    stopTurn = true;
	                    stopMoving = true;
	                }
	            }
	        }
	    }

	    IEnumerator AddForce(Vector3 dirCollision)
	    {
	        for (int i = 0; i < 2; i++)
	        {
	            yield return new WaitForFixedUpdate();
	            Vector3 dirForce = (Vector3.up * 6 + dirCollision).normalized;
	            rigid.AddForce(dirForce * 100f);
	            rigid.AddTorque(dirCollision * 40f);
	        }
	    }
	}
}