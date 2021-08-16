using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public float m_cameraSpeed = 2.0f;
	public float m_zoomSpeed = 5.0f;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
	}
	
	// Update is called once per frame
	void Update () {
		float fDelta = Time.deltaTime;
		Vector3 oldPosition = gameObject.transform.position;
		float yPos = oldPosition.y;
		if (Input.GetKey(KeyCode.W))
		{
			yPos = Mathf.Min(yPos + (fDelta*m_cameraSpeed), 4.0f);
			gameObject.transform.position = new Vector3(oldPosition.x,yPos,oldPosition.z);
		}
		else if(Input.GetKey(KeyCode.S))
		{
			yPos = Mathf.Max(yPos - (fDelta*m_cameraSpeed),-1.0f);
			gameObject.transform.position = new Vector3(oldPosition.x,yPos,oldPosition.z);
		}

		if (Input.GetKey(KeyCode.A))
		{
			float xPos = Mathf.Max(oldPosition.x - (fDelta*m_cameraSpeed),-15.0f);
			gameObject.transform.position = new Vector3(xPos, yPos,oldPosition.z);
		}
		else if(Input.GetKey(KeyCode.D))
		{
			float xPos = Mathf.Min(oldPosition.x + (fDelta*m_cameraSpeed),15.0f);
			gameObject.transform.position = new Vector3(xPos, yPos,oldPosition.z);
		}

		if(Input.GetKey(KeyCode.I))
		{
			GetComponent<Camera>().orthographicSize -= Time.deltaTime * m_zoomSpeed;
		}
		else if(Input.GetKey(KeyCode.O))
		{
			GetComponent<Camera>().orthographicSize += Time.deltaTime * m_zoomSpeed;
		}
	}
}
