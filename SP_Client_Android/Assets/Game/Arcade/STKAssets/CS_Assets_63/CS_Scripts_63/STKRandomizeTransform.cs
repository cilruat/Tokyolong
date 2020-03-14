using UnityEngine;
using System;

namespace StackGameTemplate
{
	/// <summary>
	/// This class randomizes the rotation of an object
	/// </summary>
	public class STKRandomizeTransform : MonoBehaviour
	{
        void Start()
        {
            transform.eulerAngles = Vector3.up * UnityEngine.Random.Range(0, 360);
        }
	}
}