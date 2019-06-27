using UnityEngine;
using System.Collections;

public class AutoMoveAndRotate : MonoBehaviour {

	public Vector3andSpace moveUnitsPerSecond;
	public Vector3andSpace rotateDegreesPerSecond;
	public bool ignoreTimescale;
	float lastRealTime;

	public float timePadding=0;
	public float timerule=4;

	public bool starting=false;

	private  int type=0;

	private Vector3 goalholderinitposition;

	private float timerulelevel=4;

	void Start()
	{
		lastRealTime = Time.realtimeSinceStartup;
		goalholderinitposition = transform.position;
	}

	public void StartMove(int intype)
	{
		transform.position=goalholderinitposition;
		transform.rotation=new Quaternion(0,0,0,0);

		timePadding = 0;
		lastRealTime = Time.realtimeSinceStartup;
		type = intype;
		starting = true;
		switch (type) {
		
		case 0: //don't move
			starting = false;
			moveUnitsPerSecond.value=new Vector3(0f,0,0);
			rotateDegreesPerSecond.value=new Vector3(0,0,0);
			break;

		case 1: //left ritgh
			timerulelevel=4;
			timerule = timerulelevel;
			moveUnitsPerSecond.value=new Vector3(0.2f,0,0);
			rotateDegreesPerSecond.value=new Vector3(0,0,0);
			break;

		case 2: //up down
			timerulelevel=4;
			timerule = timerulelevel;
			moveUnitsPerSecond.value=new Vector3(0f,0.2f,0);
			rotateDegreesPerSecond.value=new Vector3(0,0,0);
			break;

		case 3: //rotation
			timerulelevel=4;
			timerule = timerulelevel;
			moveUnitsPerSecond.value=new Vector3(0f,0f,0);
			rotateDegreesPerSecond.value=new Vector3(0,0,5);
			break;
		
		case 4: //left ritgh  fast
			timerulelevel=2;
			timerule = timerulelevel;
			moveUnitsPerSecond.value=new Vector3(0.4f,0,0);
			rotateDegreesPerSecond.value=new Vector3(0,0,0);
			break;
			
		case 5: //up down fast
			timerulelevel=2;
			timerule = timerulelevel;
			moveUnitsPerSecond.value=new Vector3(0f,0.4f,0);
			rotateDegreesPerSecond.value=new Vector3(0,0,0);
			break;
			
		case 6: //rotation
			timerulelevel=2;
			timerule = timerulelevel;
			moveUnitsPerSecond.value=new Vector3(0f,0f,0);
			rotateDegreesPerSecond.value=new Vector3(0,0,10);
			break;

		case 7: //left ritgh very fast
			timerulelevel=1;
			timerule = timerulelevel;
			moveUnitsPerSecond.value=new Vector3(0.8f,0,0);
			rotateDegreesPerSecond.value=new Vector3(0,0,0);
			break;
			
		case 8: //up down fast
			timerulelevel=1;
			timerule = timerulelevel;
			moveUnitsPerSecond.value=new Vector3(0f,0.8f,0);
			rotateDegreesPerSecond.value=new Vector3(0,0,0);
			break;
			
		case 9: //rotation
			timerulelevel=1;
			timerule = timerulelevel;
			moveUnitsPerSecond.value=new Vector3(0f,0f,0);
			rotateDegreesPerSecond.value=new Vector3(0,0,20);
			break;

		case 10: //rotation
			timerulelevel=1;
			timerule = timerulelevel;
			moveUnitsPerSecond.value=new Vector3(0.8f,0f,0);
			rotateDegreesPerSecond.value=new Vector3(0,0,20);
			break;

		case 11: //rotation
			timerulelevel=1;
			timerule = timerulelevel;
			moveUnitsPerSecond.value=new Vector3(0.8f,0.8f,0);
			rotateDegreesPerSecond.value=new Vector3(0,0,20);
			break;
		}
	
	
	}


	// Update is called once per frame
	void Update () {

		if (starting) {

						float deltaTime = Time.deltaTime;
						if (ignoreTimescale) {
								deltaTime = (Time.realtimeSinceStartup - lastRealTime);
								lastRealTime = Time.realtimeSinceStartup;
						}

			transform.Translate (moveUnitsPerSecond.value * deltaTime, moveUnitsPerSecond.space);
			transform.Rotate (rotateDegreesPerSecond.value * deltaTime, moveUnitsPerSecond.space);
			
			timePadding += Time.deltaTime;
			
			if (timePadding >= timerule) {
				moveUnitsPerSecond.value = moveUnitsPerSecond.value* -1;
				rotateDegreesPerSecond.value.z = rotateDegreesPerSecond.value.z * -1;
				timePadding = 0;
				timerule = timerulelevel*2;
			}


			/*
			           switch(type)
			{
			case 0:
			case 1:
			case 2:
				transform.Translate (moveUnitsPerSecond.value * deltaTime, moveUnitsPerSecond.space);

				
				timePadding += Time.deltaTime;
				
				if (timePadding >= timerule) {
					moveUnitsPerSecond.value = moveUnitsPerSecond.value* -1;
					timePadding = 0;
					timerule = timerulelevel*2;
				}

				break;


			case 3:


				transform.Rotate (rotateDegreesPerSecond.value * deltaTime, moveUnitsPerSecond.space);
				
				timePadding += Time.deltaTime;
				
				if (timePadding >= timerule) {
					rotateDegreesPerSecond.value.z = rotateDegreesPerSecond.value.z * -1;
					timePadding = 0;
					timerule = timerulelevel*2;
				}

				break;



			}

     */

					


				}
						
	}

	[System.Serializable]
	public class Vector3andSpace
	{
		public Vector3 value;
		public Space space = Space.Self;
	}

}
