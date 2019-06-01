using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGTObjectMove : MonoBehaviour {

    public Space rotationSpace = Space.Self;
    public Space moveSpace = Space.World;

    public float moveSpeedMin;
    public float moveSpeedMax;
    private float moveSpeedRandom;
    public Vector3 moveSpeed;
    
    public float rotationSpeedMin;
    public float rotationSpeedMax;
    private float rotationSpeedRandom;
    public Vector3 rotationSpeed;
    
    void Start ()
    {
        moveSpeedRandom = Random.Range(moveSpeedMin, moveSpeedMax);
        rotationSpeedRandom = Random.Range(rotationSpeedMin, rotationSpeedMax);

    }
	
	void Update ()
    {
        transform.Translate(moveSpeed * moveSpeedRandom * Time.deltaTime, moveSpace);
        transform.Rotate(rotationSpeed * rotationSpeedRandom * Time.deltaTime, rotationSpace);
    }
}
