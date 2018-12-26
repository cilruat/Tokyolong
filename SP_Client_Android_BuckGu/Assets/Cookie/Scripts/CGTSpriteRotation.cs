using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGTSpriteRotation : MonoBehaviour {

    public Space rotationSpace = Space.Self;
    public Vector3 rotationSpeed;

	void Update ()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, rotationSpace);	
	}
}
