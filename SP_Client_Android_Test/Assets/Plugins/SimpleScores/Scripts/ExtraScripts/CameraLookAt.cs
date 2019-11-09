using UnityEngine;
using System.Collections;

public class CameraLookAt : MonoBehaviour 
{
	public GameObject lookAtObject;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		Camera.main.transform.LookAt(lookAtObject.transform);
	}
}
