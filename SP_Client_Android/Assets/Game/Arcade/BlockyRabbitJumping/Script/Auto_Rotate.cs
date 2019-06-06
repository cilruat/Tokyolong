using UnityEngine;
using System.Collections;

namespace BlockJumpRabbit
{
public class Auto_Rotate : MonoBehaviour {

    public float speed = 100.0f;

    void Update()
    {
        transform.RotateAround(transform.position, Vector3.forward, -speed * Time.deltaTime);
    }
	}
}
