using UnityEngine;
using System.Collections;

public class Scroller : MonoBehaviour {

	public GameObject	m_movingCam = null;
	public float	 	m_scrollingSpeed = 1.0f;

	Vector3 m_starPosition;

	// Use this for initialization
	void Start () {
		m_starPosition = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if(m_movingCam != null)
		{
			
			gameObject.transform.position = new Vector3(m_starPosition.x+m_movingCam.transform.position.x*m_scrollingSpeed,m_starPosition.y+m_movingCam.transform.position.y*m_scrollingSpeed,gameObject.transform.position.z);
		}
	
	}
}
