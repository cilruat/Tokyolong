using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {

	public GameObject FakeBasketBall;

	public Camera cameraForShooter;
	public GameObject ballPrefab;
	public GameObject fireballPrefab;
	public Transform shotPoint;

	private float targetZ = 12.0f;			//screen point z to world point
	private float shotPowerMin = 3.0f;		//minimum shot power
	private float shotPowerMax = 12.0f;		//maximum shot power
	private float offsetY = 100.0f;			//offset Y for trajectory
	private float shotTimeMin = 0.2f;		//minimum time till to release finger
	private float shotTimeMax = 0.55f;		//maximum time till to release finger
	private float torque = 30.0f;			//torque (backspin)




	public float shotPower {get; private set;}		//shot power (initial velocity)
	public Vector3 direction {get; private set;}	//shot direction (normalized)


	GameObject objBall;
	Rigidbody ballRigidbody;
	float startTime;

	Vector2 touchPos;

	private GameMgr gamemanagerscript;

	public GameObject root;
	private CamFollow rootscript;


	
	enum ShotState {
		Prepare,					//prepare
		Ready,						//ready
		DirectionAndPower			//on swiping
	}
	
	ShotState state = ShotState.Prepare;



	// Use this for initialization
	void Start () {
		touchPos.x = -1.0f;

		gamemanagerscript = GameObject.Find ("GameManager").GetComponent<GameMgr> ();

		rootscript = root.GetComponent<CamFollow> ();

	
	}



	// Update is called once per frame
	void Update () {				
		if (state == ShotState.Prepare) {
			ChargeBall();
			CheckTrigger();

		} else if (state == ShotState.Ready) {
			CheckTrigger();

		} else if (state == ShotState.DirectionAndPower) {
			CheckShot();
		}
	}



	
	void ChargeBall () {

        if (!gamemanagerscript.hidefakeball) {
			FakeBasketBall.SetActive (true);
			state = ShotState.Ready;
		}
	}

	public void hideFakeBasketBall()
	{
	
		FakeBasketBall.SetActive (false);
	}

	public void showFakeBasketBall()
	{
		
		FakeBasketBall.SetActive (true);
		state = ShotState.Ready;
	}

	void CheckTrigger () {
		if (touchPos.x < 0) {
			if (Input.GetMouseButtonDown (0)) {
				Ray ray = cameraForShooter.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100)) {

					if(hit.collider.gameObject.tag=="BasketBallFake")
					{
						touchPos = Input.mousePosition;
						shotPower = 0.0f;
					}
				

				}
			}
		} else {
			if (touchPos.x != Input.mousePosition.x || touchPos.y != Input.mousePosition.y) {
				touchPos.x = -1.0f;
				startTime = Time.time;
				state = ShotState.DirectionAndPower;
			}
		}
	}

	

	void CheckShot () {
		float elapseTime = Time.time - startTime;

		if (Input.GetMouseButtonUp (0)) {

				ShootBall(elapseTime);

			Invoke ("setStatetoPrepare", 1);

			objBall = null;
		}
	
	}




	private void setStatetoPrepare()
	{
		if (!gamemanagerscript.islastshot)
			state = ShotState.Prepare;
	}

	

	void ShootBall (float elapseTime) {

		FakeBasketBall.SetActive (false);

		if (objBall == null) {
			
			if (!gamemanagerscript.islastshot)
				objBall= Instantiate (ballPrefab, FakeBasketBall.transform.position,Quaternion.identity) as GameObject;

			else
				objBall= Instantiate (fireballPrefab, FakeBasketBall.transform.position,Quaternion.identity) as GameObject;


		
			ballRigidbody = objBall.GetComponent<Rigidbody> ();
		}


		if (elapseTime < shotTimeMin) {
			shotPower = shotPowerMax;
		} else if (shotTimeMax < elapseTime) {
			shotPower = shotPowerMin;
		} else {
			float tmin100 = shotTimeMin * 10000.0f;
			float tmax100 = shotTimeMax * 10000.0f;
			float ep100 = elapseTime * 10000.0f;
			float rate = (ep100 - tmin100) / (tmax100 - tmin100);
			shotPower = shotPowerMax - ((shotPowerMax - shotPowerMin) * rate);
		}




		Vector3 screenPoint = Input.mousePosition;
		screenPoint.z = targetZ;
		Vector3 worldPoint = cameraForShooter.ScreenToWorldPoint (screenPoint);

		worldPoint.y += (offsetY / shotPower);

		direction = (worldPoint - shotPoint.transform.position).normalized;
		
		ballRigidbody.velocity = direction * shotPower;
		ballRigidbody.AddTorque (-shotPoint.transform.right * torque);


		if (gamemanagerscript.islastshot) {

			rootscript.setTarget (objBall.transform);

			gamemanagerscript.LastShotDone();
			gamemanagerscript.hidefakeball=true;
			state = ShotState.Prepare;
			Time.timeScale = 0.5f;
		}
	}
	



}
