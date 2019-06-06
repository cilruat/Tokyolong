using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace BSK
{
//This scripts works with ball throwing system. 
public class Shooter : MonoBehaviour {
	
    public Cloth net;											//The reference to net. Need to set in Cloth component balls colliders so cloth can interact with them.
    public GameObject TrajectoryPointPrefab;					//Aim point prefab
    public GameObject BallPrefab;
	public bool inverseAim;										//Boolean that defines if we have inversed aim or not
	private List<GameObject> Balls;								//Balls list for polling
	private Vector3 mouseStartPos;
    [HideInInspector] public GameObject currentBall;			//Current new spawned ball
	[HideInInspector] public Rigidbody ballRigidbody;			//Reference to current new spawned ball rigidbody component
	[HideInInspector] public bool isPressed, isBallThrown;
	[HideInInspector] public Vector3 newBallPosition;			//The position for new spawned ball
    public float power = 2.5f;									//Throw power coefficient
    public static int aimDotsNum;								//Amount of points in trajectory
    private List<GameObject> trajectoryPoints;					//The list to keep trajectory points
	private ClothSphereColliderPair[] colPair;					//Array to keep balls sphere colliders
	private GameObject SpawnedObjects;							//A container object to keep there all spawned object. Keeps hierarchy clean.
	private Vector3 ThrowForce;									//A vector that defines direction and power of the throw
	private bool needBall;										//A booalen to know if we need to spawn new ball
	private bool outOfscreen;
	
    void Awake () {
		SpawnedObjects = new GameObject("SpawnedObjects");
		GameObject dots = new GameObject("Dots");
		dots.transform.parent = SpawnedObjects.transform;
        trajectoryPoints = new List<GameObject>();
        isPressed = isBallThrown =false;
        for(int i=0;i<aimDotsNum;i++) {
            GameObject dot= (GameObject) Instantiate(TrajectoryPointPrefab);
			dot.transform.parent = dots.transform;
            dot.GetComponent<Renderer>().enabled = false;
            trajectoryPoints.Insert(i,dot);
        }
		Balls = new List<GameObject>();
		colPair = new ClothSphereColliderPair[5];
    }
 
    void Update () {
		if(!GameController.data.isPlaying)
			return;
		if(needBall && !AdaptiveCamera.extraMode) {
			PoolBall();
		}
		
        if(Input.GetMouseButtonDown(0) && !isPressed) {
			if(EventSystem.current.IsPointerOverGameObject())
				return;
            isPressed = true;
			mouseStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        } else if(Input.GetMouseButtonUp(0) && isPressed) {
            isPressed = false;
            if(!isBallThrown && !outOfscreen) {
				throwBall();
				ClearDots();
            } else {
				ClearDots();
			}
        }
    }
	
	void LateUpdate(){
		if(!GameController.data.isPlaying)
			return;
		if(isPressed && !isBallThrown) {
			if(inverseAim)
				ThrowForce = -GetForceFrom(mouseStartPos,Camera.main.ScreenToWorldPoint(Input.mousePosition));
			else
				ThrowForce = GetForceFrom(mouseStartPos,Camera.main.ScreenToWorldPoint(Input.mousePosition));
			float angle = Mathf.Atan2(ThrowForce.y,ThrowForce.x)* Mathf.Rad2Deg;
			transform.eulerAngles = new Vector3(0,0,angle);
			UpdateTrajectory(currentBall.transform.position, ThrowForce/ballRigidbody.mass);
			float xPos = Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
			float yPos = Camera.main.ScreenToViewportPoint(Input.mousePosition).y;
			if(yPos < 0.03f || yPos > 0.97f || xPos < 0.03f || xPos > 0.97f) {
				outOfscreen = true;
				isPressed = false;
				ClearDots();
			} else {
				outOfscreen = false;
			}
		}
	}
 
    public void spawnBall() {
		needBall = true;
	}
	
	void PoolBall(){
		needBall = false;
		isBallThrown = false;
		for (int i=0; i < Balls.Count; i++){
			if(!Balls[i].activeInHierarchy) {
				addBallCollider2Net(Balls[i].GetComponent<SphereCollider>());
				Balls[i].transform.position = newBallPosition;
				Balls[i].transform.rotation = GetRandomRot();
				Balls[i].SetActive(true);
				Balls[i].GetComponent<Rigidbody>().isKinematic = true;
				currentBall = Balls[i];
				ballRigidbody = currentBall.GetComponent<Rigidbody>();
				AdaptiveCamera.ResizeCam(currentBall.transform.position);
				return;
			}
		}
		addBall();
    }
	
	private void addBall(){
		GameObject obj = (GameObject)Instantiate(BallPrefab);
		obj.SetActive(false);
		obj.transform.parent = SpawnedObjects.transform;
		Balls.Add(obj);
		PoolBall();
	}
	
	void addBallCollider2Net(SphereCollider collider){
		for (int i=0; i < 5; i++){
			if((colPair[i].first == null || !colPair[i].first.gameObject.activeInHierarchy)) {
				colPair[i].first = collider;
				net.sphereColliders = colPair;
				return;
			}
		}
	}
	
	public Quaternion GetRandomRot(){
		Quaternion randRot = new Quaternion();
		randRot.eulerAngles = new Vector3(Random.Range(0,360),Random.Range(0,360),Random.Range(0,360));
		return randRot;
	}
	
    private void throwBall() { 
        ballRigidbody.isKinematic = false;
        ballRigidbody.AddForce(ThrowForce,ForceMode.Impulse);
        ballRigidbody.AddTorque(0,0,-30);
        ballRigidbody.constraints = RigidbodyConstraints.None;
        isBallThrown = true;
		currentBall.GetComponent<Ball>().SetThrown();
		currentBall.GetComponent<Ball>().audioSource.PlayOneShot(SoundController.data.ballWoofs[Random.Range(0,SoundController.data.ballWoofs.Length)],1);
    }
 
    private Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos) {
        return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y))*power;
    }
 
    void UpdateTrajectory(Vector3 startPos , Vector3 forceVector ) {
        float velocity = Mathf.Sqrt((forceVector.x * forceVector.x) + (forceVector.y * forceVector.y));
        float angle = Mathf.Rad2Deg*(Mathf.Atan2(forceVector.y , forceVector.x));
        float timeStep = 0;
        
        timeStep += 0.05f;
        for (int i = 0 ; i < aimDotsNum ; i++) {
            float dx = velocity * timeStep * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = velocity * timeStep * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * timeStep * timeStep / 2.0f);
            Vector3 pos = new Vector3(startPos.x + dx , startPos.y + dy ,0);
            trajectoryPoints[i].transform.position = pos;
            trajectoryPoints[i].GetComponent<Renderer>().sortingOrder = i;
            trajectoryPoints[i].GetComponent<Renderer>().enabled = true;
            trajectoryPoints[i].transform.eulerAngles = new Vector3(0,0,Mathf.Atan2(forceVector.y - (Physics.gravity.magnitude)*timeStep,forceVector.x)*Mathf.Rad2Deg);
            timeStep += 0.05f;
        }
    }
	
	void ClearDots(){
		for (int i = 0 ; i < aimDotsNum ; i++){
            trajectoryPoints[i].GetComponent<Renderer>().enabled = false;
        }
	}
}
}