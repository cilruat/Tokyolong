using UnityEngine;
using System.Collections;
using SgLib;

namespace CrashRacing
{
	public class PlayerController : MonoBehaviour
	{
	    public static event System.Action PlayerDied;

	    [Header("Gameplay References")]
	    public CameraController cameraController;
	    public ParticleSystem particle;
	    public Shader curvedWorld;
	    //    [HideInInspector]
	    public float currentSpeed;
	    public float maxAngularVelocity = 25;
	    [Header("Gameplay Config")]
	    public float initialHealth = 5;

	    private Rigidbody rigid;
	    private Vector3 mouseDownPosition;
	    private Vector3 mouseUpPosition;
	    private bool finishTurn;
	    
	    private float xDistance;
	    private float yDistance;
	    private float timeAtMouseUp;
	    private float currentTime;

	    void OnEnable()
	    {
	        GameManager.GameStateChanged += OnGameStateChanged;
	    }

	    void OnDisable()
	    {
	        GameManager.GameStateChanged -= OnGameStateChanged;
	    }

	    void OnGameStateChanged(GameState newState, GameState oldState)
	    {
	        if (newState == GameState.Playing)
	        {
	            // Do whatever necessary when a new game starts
	        }
	    }

	    // Calls this when the player dies and game over
	    public void Die()
	    {
	        // Fire event
	        if (PlayerDied != null)
	            PlayerDied();
	    }

	    void Start()
	    {

	        //Change the character to the selected one
	        GameObject currentCharacter = CharacterManager.Instance.characters[CharacterManager.Instance.CurrentCharacterIndex];
	        Mesh charMesh = currentCharacter.GetComponent<MeshFilter>().sharedMesh;
	        Material charMaterial = currentCharacter.GetComponent<Renderer>().sharedMaterial;
	        GetComponent<MeshFilter>().mesh = charMesh;
	        GetComponent<MeshRenderer>().material = charMaterial;
	        GetComponent<Renderer>().material.shader = curvedWorld;

	        finishTurn = true;
	        currentSpeed = GameManager.Instance.initialSpeed;
	        rigid = GetComponent<Rigidbody>();
	        particle.gameObject.SetActive(false);
	        StartCoroutine(IncreaseCurrentSpeed());

	        rigid.maxAngularVelocity = maxAngularVelocity;
	        rigid.ResetCenterOfMass();
	    }

	    void FixedUpdate()
	    {
			if (UIManager.Instance.isStop)
				return;
			
	        //Move player and particle
	        if (GameManager.Instance.GameState == GameState.Playing)
	        {
	            rigid.velocity = Vector3.forward * currentSpeed;
	            particle.gameObject.transform.position = transform.position + new Vector3(0,0,30);
	        }
	    }

		public void Click(int arrow)
		{
			if (GameManager.Instance.GameState.Equals (GameState.Playing) && UIManager.Instance.isStop == false) {
				if (finishTurn) {
					if (GameManager.Instance.GameState.Equals (GameState.Playing) == false)
						return;
					
					switch (arrow) {
					case 0:		// Left
						StartCoroutine(TurnLeft());
						StartCoroutine(Rotate(-GameManager.Instance.rotateAngle));
						cameraController.MoveLeft();
						break;
					case 1:		// Right
						StartCoroutine(TurnRight());
						StartCoroutine(Rotate(GameManager.Instance.rotateAngle));
						cameraController.MoveRight();
						break;
					case 2:		// Up
						timeAtMouseUp = Time.time;

						if (!particle.gameObject.activeInHierarchy)
							particle.gameObject.SetActive (true);

						currentSpeed += GameManager.Instance.increaseSpeedFactor * Time.deltaTime;
						break;
					}
				}
			}
		}

	    // Update is called once per frame
	    void Update()
	    {			
			if (GameManager.Instance.GameState.Equals(GameState.Playing) && UIManager.Instance.isStop == false)
	        {
	            if (finishTurn)
	            {
					if (GameManager.Instance.GameState.Equals (GameState.Playing) == false)
						return;

					if (Input.GetKeyDown (KeyCode.UpArrow)) {
						timeAtMouseUp = Time.time;

						if (!particle.gameObject.activeInHierarchy)
							particle.gameObject.SetActive (true);
						
						currentSpeed += GameManager.Instance.increaseSpeedFactor * Time.deltaTime;
					} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
						StartCoroutine(TurnLeft());
						StartCoroutine(Rotate(-GameManager.Instance.rotateAngle));
						cameraController.MoveLeft();
					} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
						StartCoroutine(TurnRight());
						StartCoroutine(Rotate(GameManager.Instance.rotateAngle));
						cameraController.MoveRight();
					}

	                /*if (Input.GetMouseButtonDown(0) && GameManager.Instance.GameState.Equals(GameState.Playing))
	                {
	                    mouseDownPosition = Input.mousePosition; //Get mouse down position
	                }
	                
	                if (Input.GetMouseButtonUp(0))
	                {

	                    mouseUpPosition = Input.mousePosition; //Get mouse position

	                    xDistance = (mouseDownPosition.x - mouseUpPosition.x)/Screen.width; //Caculate the distance between them

	                    //If swipping up -> increase speed
	                    timeAtMouseUp = Time.time;
	                    yDistance = (mouseDownPosition.y - mouseUpPosition.y)/Screen.height;

	                    // Up swiping
	                    if (Mathf.Abs(yDistance) >= GameManager.Instance.verticalThresholdSwipe && mouseDownPosition.x != 0)
	                    {
	                        if (!particle.gameObject.activeInHierarchy)
	                        {
	                            particle.gameObject.SetActive(true);
	                        }
	                        currentSpeed += GameManager.Instance.increaseSpeedFactor * Time.deltaTime;
	                    }

	                    // Side swiping
	                    if (Mathf.Abs(xDistance) > GameManager.Instance.horizontalThresholdSwipe && mouseDownPosition.x != 0)
	                    {
	                        if (xDistance < 0) // Right
	                        {
	                            StartCoroutine(TurnRight());
	                            StartCoroutine(Rotate(GameManager.Instance.rotateAngle));
	                            cameraController.MoveRight();
	                        }
	                        else // Left
	                        {
	                            StartCoroutine(TurnLeft());
	                            StartCoroutine(Rotate(-GameManager.Instance.rotateAngle));
	                            cameraController.MoveLeft();
	                        }
	                    }
	                }*/
	            }

	            currentTime = Time.time;
	            if (Mathf.Abs(currentTime - timeAtMouseUp) >= 1f)
	            {
	                if (currentSpeed > GameManager.Instance.playerSpeed)
	                {
	                    currentSpeed -= GameManager.Instance.increaseSpeedFactor * Time.deltaTime/10;
	                }
	                else
	                {
	                    if (particle.gameObject.activeInHierarchy)
	                    {
	                        particle.gameObject.SetActive(false);
	                    }
	                }
	            }

	        }
	    }

	    IEnumerator TurnRight()
	    {
	        finishTurn = false;

	        yield return new WaitForFixedUpdate();

	        float startX = Mathf.Round(transform.position.x);
	        float endX = startX + 4f;

	        if (endX <= 8)
	        {
	            float t = 0;
	            while (t < GameManager.Instance.turnTime)
	            {
	                t += Time.deltaTime;
	                float fraction = t / GameManager.Instance.turnTime;
	                float newX = Mathf.Lerp(startX, endX, fraction);
	                Vector3 newPos = transform.position;
	                newPos.x = newX;
	                transform.position = newPos;
	                yield return null;
	            }
	        }      
	        finishTurn = true;
	    }

	    IEnumerator TurnLeft()
	    {
	        finishTurn = false;

	        yield return new WaitForFixedUpdate();

	        float startX = Mathf.Round(transform.position.x);
	        float endX = startX - 4f;

	        if (endX >= -8)
	        {
	            float t = 0;
	            while (t < GameManager.Instance.turnTime)
	            {
	                t += Time.deltaTime;
	                float fraction = t / GameManager.Instance.turnTime;
	                float newX = Mathf.Lerp(startX, endX, fraction);
	                Vector3 newPos = transform.position;
	                newPos.x = newX;
	                transform.position = newPos;
	                yield return null;
	            }
	        }

	        finishTurn = true;
	    }

	    IEnumerator Rotate(float angle)
	    {
	        finishTurn = false;

	        yield return new WaitForFixedUpdate();

	        if (transform.position.x < 8 && transform.position.x > -8)
	        {
	            Quaternion startRot = transform.rotation;
	            Quaternion endRot = Quaternion.Euler(0, angle, 0);
	            float t = 0;
	            while (t < GameManager.Instance.turnTime / 2f)
	            {
	                t += Time.deltaTime;
	                float fraction = t / (GameManager.Instance.turnTime / 2f);
	                transform.rotation = Quaternion.Lerp(startRot, endRot, fraction);
	                yield return null;
	               
	            }

	            float r = 0;
	            while (r < GameManager.Instance.turnTime / 2f)
	            {
	                r += Time.deltaTime;
	                float fraction = r / (GameManager.Instance.turnTime / 2f);
	                transform.rotation = Quaternion.Lerp(endRot, startRot, fraction);
	                yield return null;
	            }

	        }
	        finishTurn = true;
	    }

	    IEnumerator IncreaseCurrentSpeed()
	    {
	        while (currentSpeed < GameManager.Instance.playerSpeed && GameManager.Instance.GameState.Equals(GameState.Playing))
	        {
	            currentSpeed += GameManager.Instance.increaseSpeedFactor * Time.deltaTime;
	            yield return new WaitForSeconds(0.5f);
	        }

	        currentSpeed = GameManager.Instance.playerSpeed;
	    }

	    void OnCollisionEnter(Collision col)
	    {
			if (UIManager.Instance.isStop)
				return;
			
	        if (GameManager.Instance.GameState.Equals(GameState.Playing))
	        {
	            if (col.gameObject.CompareTag("Car")) //Hit another car
	            {
	                initialHealth -= col.impulse.magnitude; //Turn down health
	                CarController carController = col.gameObject.GetComponent<CarController>();
	                Vector3 dirCollision = (col.transform.position - transform.position).normalized;
	                carController.stopMoving = true;
	                carController.stopTurn = true;

	                Rigidbody carRigid = col.gameObject.GetComponent<Rigidbody>();
	                carRigid.constraints = RigidbodyConstraints.None;

	                if (initialHealth <= 0 || currentSpeed >= GameManager.Instance.playerSpeed) //Game over
	                {
	                    SoundManager.Instance.PlaySound(SoundManager.Instance.gameOver);
	                    Die();
	                    PlayerDied();
						UIManager.Instance.limitTime.Stop ();
	                    cameraController.ShakeCamera();
	                    if (particle.gameObject.activeInHierarchy)
	                        particle.gameObject.SetActive(false);
	                    rigid.constraints = RigidbodyConstraints.None;             
	                    StartCoroutine(AddForce(rigid, true, 500f, 550f, dirCollision, col.rigidbody));
	                }

	                int dir;
	                if (col.transform.position.x - rigid.position.x < 0) { dir = 1; }
	                else
	                {
	                    dir = -1;
	                }
	                rigid.AddForce(new Vector3(40 * dir, 0, (200 - currentSpeed)*2), ForceMode.Impulse);
	            }
	        }
	    }

	    void OnTriggerEnter(Collider other)
	    {
	        if (other.CompareTag("Gold") && GameManager.Instance.GameState.Equals(GameState.Playing))
	        {
	            CoinManager.Instance.AddCoins(1);
	            SoundManager.Instance.PlaySound(SoundManager.Instance.hitGold);
	            other.gameObject.SetActive(false);
	            other.transform.parent = CoinManager.Instance.transform;
	        }
	    }

	    IEnumerator AddForce(Rigidbody rigid, bool isForPlayer, float minForce, float maxForce, Vector3 dirCollision,Rigidbody other)
	    {
	        for (int i = 0; i < 2; i++)
	        {
	            yield return new WaitForFixedUpdate();
	            Vector3 torqueDir = (isForPlayer) ? (-dirCollision * 500f) : (dirCollision * 40f);
	            rigid.AddTorque(torqueDir);
	        }
	        yield return new WaitForEndOfFrame();
	        Vector3 angularV = rigid.angularVelocity;
	        angularV.x /= Mathf.Abs(angularV.x);
	        angularV.y /= Mathf.Abs(angularV.y);
	        angularV.z /= Mathf.Abs(angularV.z);

	        rigid.angularVelocity = angularV * maxAngularVelocity;
	    }
	}
}