using UnityEngine;
using System.Collections;

public class Point : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnDrawGizmos ()
	{
		#if UNITY_EDITOR
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (transform.position, 0.5f);
		Gizmos.DrawWireCube (transform.position, this.transform.localScale);
	
		#endif
	}
}
