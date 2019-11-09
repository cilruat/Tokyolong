using UnityEngine;
using System.Collections;


public class point : MonoBehaviour {

	public int pointnum;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnDrawGizmos ()
	{
		#if UNITY_EDITOR
		Gizmos.color = Color.red;
		Gizmos.DrawIcon(transform.position, "mark",true);
		//Gizmos.DrawWireCube (transform.position, this.transform.localScale);

		#endif
	}
}