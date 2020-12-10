using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager_ClawMovement : MonoBehaviour {

    [Header("Player Settings")]
    public bool freePlay = false;
    public int playerCoins = 10;
    public int spendCoin = 2;

    [Header("UI Settings")]
    public Text coinsTextLabel;
    public GameObject UI_OutOfCoinsPopup;

    // These are used to move the claw when using UI On Screen Buttons
    private bool UI_ClawButtonUp = false;
    private bool UI_ClawButtonDown = false;
    private bool UI_ClawButtonLeft = false;
    private bool UI_ClawButtonRight = false;

    [Space(5f)]

    [Header("Claw Settings")]

    // Our object that we move which, in turn, moves the claw and rope
    public Transform clawHolder;

    // X and Z movement speed
    public float movementSpeed = 1.0f; 

    // Y drop and raise speed
    public float dropSpeed = 1.0f;

    [Range(0,10)]
    public int failRate = 0;

    [HideInInspector]
    // This is false when we are droping / rasing the claw.
    public bool canMove = true;

    [HideInInspector]
    // This will stop any downward Y movement. Typically called from outside of this class. 
    // This case it is used from WallSensor_ClawMachine to tell the claw to stop moving since it hit a wall.
    public bool stopMovement = false;

    // The vertical limit of when our claw will stop moving down
    public float LimitY = 2.074f;

    // If enabled, the claw will automatically go back to the home / inital start position
    [Tooltip("If enabled, the claw will automatically go back to the home / inital start position")]
    public bool shouldReturnHomeAutomatically = false;

    // Position Variables
    [HideInInspector]
    public Vector3 clawHomePosition;

    [HideInInspector]
    public Vector3 clawDropFromPosition;

    // Animation Variables
    public Animation clawHeadAnimation;

    [Header("Claw Movement Boundary Limits")]

    // Used to add a buffer between the boundary and the center of the claw head (so we don't clip)
    public float clawHeadSizeX = 0.30f;
    public float clawHeadSizeZ = 0.13f;

    // Movement boundaries
    private float boundaryX_Left;
    private float boundaryX_Right;
    private float boundaryZ_Back;
    private float boundaryZ_Forward;
    
    [Space(5f)]

    public Transform clawBoundaryX_Left;
    public Transform clawBoundaryX_Right;
    public Transform clawBoundaryZ_Back;
    public Transform clawBoundaryZ_Forward;

    [Header("Ovverhead Motor Settings")]

    // Motors
    public Transform topMainMotor;
    public Transform overHeadMotorRailSystem;

    [HideInInspector]
    public Vector3 topMainMotorHomePosition;

    [HideInInspector]
    public Vector3 overHeadMotorRailSystemHomePosition;

    [HideInInspector]
    public bool isDroppingBall = false;


    [Header("Prop Joystick Box")]
    public Transform propJoyStick;
    public float propJoystickSpeed = 2.0f;

    [Header("Misc Settings")]
    public PrizeCatcherDetector_ClawMachine prizeCatcherDetector;

	// Use this for initialization
	void Start () {

        // Setup our inital positions
        clawHomePosition = clawHolder.transform.position;
        topMainMotorHomePosition = topMainMotor.position;
        overHeadMotorRailSystemHomePosition = overHeadMotorRailSystem.position;

        // Setup our boundaries
        boundaryX_Left = clawBoundaryX_Left.position.x;
        boundaryX_Right = clawBoundaryX_Right.position.x;
        boundaryZ_Back = clawBoundaryZ_Back.position.z;
        boundaryZ_Forward = clawBoundaryZ_Forward.position.z;

        boundaryX_Left += clawHeadSizeX;
        boundaryX_Right -= clawHeadSizeX;
        boundaryZ_Back -= clawHeadSizeZ;
        boundaryZ_Forward += clawHeadSizeZ;

    }
	
	// Update is called once per frame
	void Update () {

        // Update the UI text label
        coinsTextLabel.text = playerCoins.ToString();
        playerCoins = Info.GamePlayCnt;

    }

    void FixedUpdate()
    {
        // If movement is allowed
        if (canMove)
        {
            // Used to Open Claw Head
            if (Input.GetKey(KeyCode.O))
            {
                openClawButtonInput();
            }

            // Press P key to drop the claw
            if (Input.GetKey(KeyCode.P))
            {
                dropClawButtonInput();
            }

            // Normal inputs below...
            if (Input.GetKey(KeyCode.UpArrow) || UI_ClawButtonUp)
            {
                clawMoveUp();
            }

            if (Input.GetKey(KeyCode.DownArrow) || UI_ClawButtonDown)
            {
                clawMoveDown();
            }

            if (Input.GetKey(KeyCode.LeftArrow) || UI_ClawButtonLeft)
            {
                clawMoveLeft();
            }

            if (Input.GetKey(KeyCode.RightArrow) || UI_ClawButtonRight)
            {
                clawMoveRight();
            }
        }
    }


    private void dropClawButtonInput()
    {
        // Make sure we're NOT above the prize catcher, we need to do a release for that, NOT a drop
        if (prizeCatcherDetector.isClawAbovePrizeCatcher)
        {
            // Open the claw since we're above the prize catcher and we do not want to drop the claw here, just open it.
            openClawButtonInput();
        }
        else
        {
            // Drop like normal...

            // If we're NOT in free play
            if (freePlay)
            {
                // Drop our claw
                StartCoroutine(dropClaw());

                // Disable movement until done performing this action
                canMove = false;
            }
            else if (!freePlay)
            {
                // Make sure the player has coins
                if (playerCoins > 1)
                {
                    //playerCoins--;  // Remove one coin
                    int curCoin = 2;
                    NetworkManager.Instance.GameCountInput_REQ(Info.TableNum, -curCoin);
                    // Drop our claw
                    StartCoroutine(dropClaw());

                    // Disable movement until done performing this action
                    canMove = false;
                }
                else
                {
                    // Alert the player they have no coins left
                    if (!UI_OutOfCoinsPopup.activeInHierarchy)
                        UI_OutOfCoinsPopup.SetActive(true);
                }
            }
        }
    }

    private void openClawButtonInput()
    {
        // If we're not actively dropping the ball - This prevents from trying to drop multiple times
        if (!isDroppingBall)
        {
            // Drop the ball
            StartCoroutine(DropBall());
        }
    }

    private void clawMoveUp()
    {
        // + Z direction
        if (clawHolder.transform.position.z < boundaryZ_Back)
        {
            // Move our claw
            clawHolder.Translate(0f, 0f, movementSpeed * 1 * Time.deltaTime);

            // Also move the motors
            overHeadMotorRailSystem.Translate(0f, 0f, movementSpeed * 1 * Time.deltaTime);
            topMainMotor.Translate(0f, 0f, movementSpeed * 1 * Time.deltaTime);

            // Move the prop joystick
            propJoyStick.Rotate(propJoystickSpeed * 1 * Time.deltaTime, 0f, 0f);
        }
    }

    private void clawMoveDown()
    {
        // - Z direction
        if (clawHolder.transform.position.z > boundaryZ_Forward)
        {
            // Move our claw
            clawHolder.Translate(0f, 0f, movementSpeed * -1 * Time.deltaTime);

            // Also move the motors
            overHeadMotorRailSystem.Translate(0f, 0f, movementSpeed * -1 * Time.deltaTime);
            topMainMotor.Translate(0f, 0f, movementSpeed * -1 * Time.deltaTime);

            // Move the prop joystick
            propJoyStick.Rotate(propJoystickSpeed * -1 * Time.deltaTime, 0f, 0f);
        }
    }

    private void clawMoveLeft()
    {
       
        // + X direction
        if (clawHolder.transform.position.x > boundaryX_Left)
        {
            // Move our claw
            clawHolder.Translate(movementSpeed * -1 * Time.deltaTime, 0f, 0f);

            // Also move the motor on top
            topMainMotor.Translate(movementSpeed * -1 * Time.deltaTime, 0f, 0f);

            // Move the prop joystick
            propJoyStick.Rotate(0f, 0f, propJoystickSpeed * 1 * Time.deltaTime);
        }
    }

    private void clawMoveRight()
    {
        // - X direction
        if (clawHolder.transform.position.x < boundaryX_Right)
        {
            // Move our claww
            clawHolder.Translate(movementSpeed * 1 * Time.deltaTime, 0f, 0f);

            // Also move the motor on top
            topMainMotor.Translate(movementSpeed * 1 * Time.deltaTime, 0f, 0f);

            // Move the prop joystick
            propJoyStick.Rotate(0f, 0f, propJoystickSpeed * -1 * Time.deltaTime);
        }
    }

    /// <summary>
    /// Used to drop the claw from a position. 
    /// </summary>
    /// <returns></returns>
    IEnumerator dropClaw()
    {
        // Save our drop position
        clawDropFromPosition = clawHomePosition;

        // Play opening animation
        OpenClaw();

        // While we're larger than our vertical Y limit
        while (clawHolder.transform.position.y >= LimitY)
        {
            // If something stops our movement, breakout. This is the WallSensor_ClawMachine script right now.
            if(stopMovement)
            {
                break;
            }

            // Drop our claw
            clawHolder.Translate(0f, dropSpeed * -1 * Time.deltaTime, 0f);

            yield return null;
        }

        // Wait a few
        yield return new WaitForSeconds(1.0f);

        // If movement was stopped
        if (stopMovement)
        {
            // Go right back up to where we dropped from
            while (clawHolder.transform.position.y <= clawDropFromPosition.y)
            { 
                // Move
                clawHolder.Translate(0f, dropSpeed * 1 * Time.deltaTime, 0f);

                yield return null;
            }

            yield return new WaitForSeconds(0.15f);

            // Close the claw
            CloseClaw();
        }
        else
        {
            // Implement some level of failure of closing the claw tight enough. Tricky!
            if (Random.Range(1, 10) <= failRate)
            {
                // Fire off the coroutine
                StartCoroutine(WeakClaws());
            }
            else
            {
                // Close claw head
                CloseClaw();
            }

            yield return new WaitForSeconds(1.0f);

            // First go back up
            while (clawHolder.transform.position.y <= clawDropFromPosition.y)
            {
                clawHolder.Translate(0f, dropSpeed * 1 * Time.deltaTime, 0f);

                yield return null;
            }

            if (shouldReturnHomeAutomatically)
            {
                yield return new WaitForSeconds(1.0f);

                // Return home now
                float startTime = Time.time;
                float journeyLength = Vector3.Distance(clawHolder.transform.position, clawHomePosition);

                while (Vector3.Distance(clawHolder.transform.position, clawHomePosition) > 0.05f)
                {
                    // Distance moved = time * speed.
                    float distCovered = (Time.time - startTime) * 0.025f;

                    // Fraction of journey completed = current distance divided by total distance.
                    float fracJourney = distCovered / journeyLength;

                    // Let's lerp the position closer to the home
                    clawHolder.transform.position = Vector3.Lerp(clawHolder.transform.position, clawHomePosition, fracJourney);
                    overHeadMotorRailSystem.position = Vector3.Lerp(overHeadMotorRailSystem.position, overHeadMotorRailSystemHomePosition, fracJourney);
                    topMainMotor.position = Vector3.Lerp(topMainMotor.position, topMainMotorHomePosition, fracJourney);

                    // Move the prop joystick
                    propJoyStick.rotation = Quaternion.Lerp(propJoyStick.rotation, Quaternion.Euler(0f, 0f, 0f), fracJourney);

                    yield return null;
                }

                // Reset to exact position
                clawHolder.transform.position = clawHomePosition;

                // Play opening animation
                OpenClaw();

                yield return new WaitForSeconds(1.55f);

                CloseClaw();
            }
        }

        // We can move now
        canMove = true;

        // Allow movement again if stopped from collding with inside wall
        stopMovement = false;

        yield return null;

    }

    /// <summary>
    /// This is used to return the claw back to the STARTING / HOME position.
    /// The home position is where the claw STARTS when the game begins. Code found in Start() will set this location.
    /// </summary>
    /// <returns></returns>
    IEnumerator returnClawToHomePosition()
    {
        // First go back up
        while(clawHolder.transform.position.y < clawDropFromPosition.y)
        {
            clawHolder.Translate(0f, dropSpeed * 1 * Time.deltaTime, 0f);

            yield return null;
        }

        yield return null;
    }

    /// <summary>
    /// Open the claw, normally, using animations.
    /// </summary>
    private void OpenClaw()
    {
        // Play opening animation
        clawHeadAnimation["Claw_Open_New"].speed = 1f;
        clawHeadAnimation["Claw_Open_New"].time = 0f;
        clawHeadAnimation.CrossFade("Claw_Open_New");
    }

    /// <summary>
    /// Using animations, let's close the claws.
    /// </summary>
    private void CloseClaw()
    {
        clawHeadAnimation["Claw_Open_New"].speed = -1f;
        clawHeadAnimation["Claw_Open_New"].time = clawHeadAnimation["Claw_Open_New"].length;
        clawHeadAnimation.CrossFade("Claw_Open_New");
    }

    /// <summary>
    /// This IEnumerator is used to create a "failed" state, which opens the claws only so far.
    /// </summary>
    /// <returns></returns>
    IEnumerator WeakClaws()
    {
        clawHeadAnimation["Claw_Open_Weak"].speed = -1f;
        clawHeadAnimation["Claw_Open_Weak"].time = clawHeadAnimation["Claw_Open_Weak"].length;
        clawHeadAnimation.CrossFade("Claw_Open_Weak");

        yield return new WaitForSeconds(Random.Range(2.15f, 2.85f));

        clawHeadAnimation["Claw_Close_From_Weak"].speed = -1f;
        clawHeadAnimation["Claw_Close_From_Weak"].time = clawHeadAnimation["Claw_Close_From_Weak"].length;
        clawHeadAnimation.CrossFade("Claw_Close_From_Weak");

    }

    /// <summary>
    /// Used to drop the ball
    /// </summary>
    /// <returns></returns>
    IEnumerator DropBall()
    {
        // Flag we are dropping our balls
        isDroppingBall = true;

        OpenClaw();

        yield return new WaitForSeconds(0.55f);

        CloseClaw();

        // Flag we can drop a ball again
        isDroppingBall = false;
    }


    /// <summary>
    /// This is used to close the UI popup and give the player more coins.
    /// You can have your own IAP code here.
    /// </summary>
    public void UI_OutOfCoinsPopup_PurchaseMore()
    {
        // This is the function where you can wire in the Unity IAP code found here:
        //https://unity3d.com/learn/tutorials/topics/ads-analytics/integrating-unity-iap-your-game
        // And easily add IAP to your own game!!

        // Give the player more coins
        playerCoins = playerCoins + 10;

        // Close the popup
        UI_OutOfCoinsPopup.SetActive(false);
    }

    /// Public UI Functions ///
    /// Used only for on screen buttons
    public void UI_DropClawButton()
    {
        dropClawButtonInput();
    }

    public void UI_OpenClawButton()
    {
        openClawButtonInput();
    }

    // For Event Types when moving Left (Pointer Down / Up)
    public void UI_MoveClawLeft()
    {
        UI_ClawButtonLeft = true;
    }
    public void UI_MoveClawLeft_Off()
    {
        UI_ClawButtonLeft = false;
    }

    // For Event Types when moving Right (Pointer Down / Up)
    public void UI_MoveClawRight()
    {
        UI_ClawButtonRight = true;
    }
    public void UI_MoveClawRight_Off()
    {
        UI_ClawButtonRight = false; ;
    }

    // For Event Types when moving Up (Pointer Down / Up)
    public void UI_MoveClawUp()
    {
        UI_ClawButtonUp = true;
    }
    public void UI_MoveClawUp_Off()
    {
        UI_ClawButtonUp = false;
    }

    // For Event Types when moving Down (Pointer Down / Up)
    public void UI_MoveClawDown()
    {
        UI_ClawButtonDown = true;
    }
    public void UI_MoveClawDown_Off()
    {
        UI_ClawButtonDown = false;
    }


    public void OnGoHome()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnReturn()
    {
        SceneManager.LoadScene("LuckGame");
    }

    public void ClosePopUp()
    {
        UI_OutOfCoinsPopup.SetActive(false);
    }

}
