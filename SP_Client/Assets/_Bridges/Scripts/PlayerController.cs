using UnityEngine;
using System.Collections;
using SgLib;

namespace Bridges
{
	public class PlayerController : MonoBehaviour
	{
	    public static event System.Action PlayerFall = delegate {};

	    [Header("Gameplay References")]
	    public GameManager gameManager;
	    public CameraController cameraController;
	    public GameObject playerMesh;
	    [HideInInspector]
	    public Vector3 movingDirection;
	    [HideInInspector]
	    public bool isRunning;
	    [HideInInspector]
	    public bool touchDisable;
	    [HideInInspector]
	    public bool disableCheckGameOver;

	    [Header("Gameplay Config")]
	    public float speed;

	    private GameObject previousPlane;
	    private GameObject previousBridge;
	    private Animator anim;
	    private Ray rayCheckObject;
	    private RaycastHit hit;
	    private bool stopMoving;
	    private bool isFixingPosition;
	    private Coroutine fallingCoroutine;

	    void OnEnable()
	    {
	        GameManager.NewGameEvent += GameManager_NewGameEvent;
	    }

	    void OnDisable()
	    {
	        GameManager.NewGameEvent -= GameManager_NewGameEvent;
	    }

	    void GameManager_NewGameEvent(GameEvent e)
	    {
	        if (e == GameEvent.Start)
	        {
	            if (!isRunning)
	                isRunning = true;

	            // Play anim
	            if (!anim.enabled)
	                anim.enabled = true;
	        }
	    }

	    // Use this for initialization
	    void Start()
	    {

	        //Change playerMesh to the selected one
	        GameObject main = playerMesh.transform.Find("Main").gameObject;
	        GameObject leftFoot = playerMesh.transform.Find("LeftFoot").gameObject;
	        GameObject leftHand = playerMesh.transform.Find("LeftHand").gameObject;
	        GameObject rightFoot = playerMesh.transform.Find("RightFoot").gameObject;
	        GameObject rightHand = playerMesh.transform.Find("RightHand").gameObject;

	        GameObject currentChar = CharacterManager.Instance.characters[CharacterManager.Instance.CurrentCharacterIndex];
	        Mesh mainMesh = currentChar.transform.Find("Main").GetComponent<MeshFilter>().sharedMesh;
	        Mesh leftFootMesh = currentChar.transform.Find("LeftFoot").GetComponent<MeshFilter>().sharedMesh;
	        Mesh rightFootMesh = currentChar.transform.Find("RightFoot").GetComponent<MeshFilter>().sharedMesh;
	        Mesh leftHandMesh = currentChar.transform.Find("LeftHand").GetComponent<MeshFilter>().sharedMesh;
	        Mesh rightHandMesh = currentChar.transform.Find("RightHand").GetComponent<MeshFilter>().sharedMesh;
	        Material currentCharMaterial = currentChar.transform.Find("Main").GetComponent<Renderer>().sharedMaterial;

	        main.GetComponent<MeshFilter>().mesh = mainMesh;
	        main.GetComponent<MeshRenderer>().material = currentCharMaterial;
	        leftFoot.GetComponent<MeshRenderer>().material = currentCharMaterial;
	        leftFoot.GetComponent<MeshFilter>().mesh = leftFootMesh;
	        leftHand.GetComponent<MeshRenderer>().material = currentCharMaterial;
	        leftHand.GetComponent<MeshFilter>().mesh = leftHandMesh;
	        rightFoot.GetComponent<MeshRenderer>().material = currentCharMaterial;
	        rightFoot.GetComponent<MeshFilter>().mesh = rightFootMesh;
	        rightHand.GetComponent<MeshRenderer>().material = currentCharMaterial;
	        rightHand.GetComponent<MeshFilter>().mesh = rightHandMesh;

	        anim = playerMesh.GetComponent<Animator>();
	        anim.enabled = false;
	        movingDirection = Vector3.right;
	        previousPlane = null;
	        previousBridge = null;
	    }
		
	    // Update is called once per frame
	    void Update()
	    {
	        if (isRunning && !gameManager.gameOver)
	        {
	            bool isOnPlane = false;
	            rayCheckObject = new Ray(transform.position + new Vector3(0.5f, 0.5f, 0), Vector3.down);
	            if (Physics.Raycast(rayCheckObject, out hit, 2f))
	            {
	                GameObject theParent = hit.collider.gameObject;
	                if (theParent.CompareTag("Bridge"))
	                {   
	                    //Move player to center of bridge       
	                    if (previousBridge != theParent)
	                    {
	                        ScoreManager.Instance.AddScore(1);
	                        transform.SetParent(theParent.transform, true);

	                        previousBridge = theParent;
	                        StartCoroutine(FixedPosition(Mathf.Abs(transform.localPosition.z) / speed,
	                                transform.localPosition.z, 0, theParent));

	                        FixedRotation();

	                    }

	                    // Bridges may be rotating, so we need to update the moving direction constantly
	                    movingDirection = theParent.transform.TransformDirection(Vector3.right);
	                }
	                else
	                {
	                    isOnPlane = true;
	                    if (previousPlane != theParent)
	                    {
	                        //Move player to center of the plane
	                        previousPlane = theParent;
	                        transform.SetParent(theParent.transform, true);
	                        movingDirection = theParent.transform.TransformDirection(Vector3.right);

	                        StartCoroutine(FixedPosition(Mathf.Abs(transform.localPosition.z) / speed,
	                                transform.localPosition.z, 0, theParent));

	                        FixedRotation();
	                    }
	                }          
	            }

	            //Check game over
	            Ray rayCheckGameOver = new Ray(transform.position + new Vector3(0, 0.5f, 0), Vector3.down);
	            if (!Physics.Raycast(rayCheckGameOver, out hit, 2f) && !disableCheckGameOver)
	            {
	                isRunning = false;
	                anim.enabled = false;

	                PlayerFall();   // fire event
	                transform.parent = null;

	                fallingCoroutine = StartCoroutine(PlayerFalling());
	            }

	            //Mouse down -> rotate all bridge
	            if (Input.GetMouseButtonDown(0) && !touchDisable)
	            {
	                SoundManager.Instance.PlaySound(SoundManager.Instance.rotateBridge);
	                touchDisable = true;
	                gameManager.RotateAllBridge();
	            }

	            // Move player forward
	            float actualSpeed = isOnPlane && isFixingPosition ? speed / 2 : speed;
	            transform.position += movingDirection * actualSpeed * Time.deltaTime;
	        }
	    }

	    public void BackToLastPlane()
	    {
	        if (fallingCoroutine != null)
	        {
	            StopCoroutine(fallingCoroutine);
	        }

	        transform.SetParent(previousPlane.transform, true);
	        Vector3 pos = previousPlane.transform.position;

	        pos.y = 0;
	        transform.position = pos;

	        movingDirection = previousPlane.transform.TransformDirection(Vector3.right);

	        FixedRotation();

	        cameraController.ResetPosition();

	        // Make the next bridge connect with the current plane.
	        Ray rayForward = new Ray(previousPlane.transform.position, movingDirection);
	        if (!Physics.Raycast(rayForward, out hit, 3f))
	        {
	            // The next bridge is not connected. Need to rotate it.
	            gameManager.RotateAllBridge();
	        }

	        previousBridge = null;
	    }

	    IEnumerator PlayerFalling()
	    {     
	        while (true)
	        {
	            transform.position += movingDirection * speed * Time.deltaTime;
	            Ray rayDown = new Ray(transform.position + new Vector3(-0.4f, 0.5f, 0), Vector3.down);
	            if (!Physics.Raycast(rayDown, out hit, 2f)) //Run out of platform
	            {
	                SoundManager.Instance.PlaySound(SoundManager.Instance.gameOver);
	                break;
	            }
	            yield return null;
	        }

	        while (transform.position.y > -100)
	        {
	            transform.position += Vector3.down;
	            yield return null;
	        }
	    }

	    //Move player to given pos
	    IEnumerator FixedPosition(float time, float startZ, float endZ, GameObject theParent)
	    {
	        isFixingPosition = true;
	        float t = 0;
	        while (t < time)
	        {
	            t += Time.deltaTime;
	            float fraction = t / time;
	            float newZ = Mathf.Lerp(startZ, endZ, fraction);
	            Vector3 newLocalPos = transform.localPosition;
	            newLocalPos.z = newZ;
	            transform.localPosition = newLocalPos;
	            yield return null;
	        }
	        isFixingPosition = false;
	    }

	    void FixedRotation()
	    {
	        Vector3 rot = transform.localEulerAngles;
	        rot.y = 0;
	        transform.localEulerAngles = rot;
	    }


	    void OnTriggerEnter(Collider other)
	    {
	        if (other.CompareTag("Gold"))
	        {
	            SoundManager.Instance.PlaySound(SoundManager.Instance.earnCoin);
	            CoinManager.Instance.AddCoins(1);
	            other.GetComponent<MeshCollider>().enabled = false;
	            other.GetComponent<GoldController>().GoUp();
	        }
	    }
	}
}