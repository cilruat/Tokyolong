using UnityEngine;
using PnCCasualGameKit;

namespace PnCCasualGameKit
{
/// <summary>
///IMPORTANT : Refer documentation for a detailed explanation of Gameplay logic
/// Responsible for the complete Gamplay which mainly includes:
///  - Blocks incoming and cut logic on user input
///  - calculation of when is perfect score, Gameover etc
///  - Applying color to the blocks
/// </summary>
public class GameplayController : LazySingleton<GameplayController>
{

    [Header("Block")]
    [Tooltip("The Block from which the Game starts")]
    [SerializeField]
    private Transform startBaseBlock;
    private Transform baseBlock;
    private GameObject currentBlock;

    private Material blockMat;
    private string shaderType = "Standard";
    private Texture selectedSkinTexture;

    [Header("Gameplay")]
    [Tooltip("Threshold for perfect score. Bigger the value, Easier to score a perfect")]
    [SerializeField]
    private float perfectDist;
    private bool xOrZ = false; // from x --> true; from z --> false
    private float yOffset;

    //Others
    [Tooltip("Color Controller Reference")]
    public ColorController colorController;

    [Tooltip("Camera Controller Reference")]
    [SerializeField]
    private CameraController cameraController;
    private ObjectPooler objectPooler;
    private bool isInputActivated;
    private bool isGameRunning;

    [Header("Cheat")]
    [Tooltip("Enable this to let the game play on its own.")]
    [SerializeField]
    private bool cheat;
    [Tooltip("Degree of Perfection in cheat mode. smaller value --> more perfects")]
    [SerializeField]
    private float cheatRandomisation;



    [Header("Block Speed")]
    [Tooltip("Default Block Speed when game starts")]
    [SerializeField]
    private float defaultBlockSpeed;
    [Tooltip("The Maximum Block Speed")]
    [SerializeField]
    private float maxBlockSpeed;
    [Tooltip("Block speed increases by this value")]
    [SerializeField]
    private float blockSpeedDelta;
    [Tooltip("Block speed increases every this score")]
    [SerializeField]
    private int blockSpeedIncreaseScore;
    [Tooltip("Block speed resets to default value every this score")]
    [SerializeField]
    private int blockSpeedResetScore;
    private float blockSpeed;

    /// <summary>
    /// Registering for events and doing few initialisations in Awake
    /// </summary>
    void Awake()
    {
        objectPooler = GetComponent<ObjectPooler>();
        yOffset = startBaseBlock.lossyScale.y;
        GameManager.Instance.GameInitialized += InitGame;
        GameManager.Instance.GameStarted += StartGame;
    }

    /// <summary>
    /// This is when the game has initialised but has not started.
    /// </summary>
    void InitGame()
    {
        objectPooler.disableAllPooled();
        baseBlock = startBaseBlock;

        //Reset the Color conttoller and assign the first colors
        colorController.Reset();
        baseBlock.GetComponent<MeshRenderer>().material.SetColor("_Color", colorController.BlockColor);

        MeshUVAdjuster.AdjustUVs(baseBlock);

        //So that we start with default speed for the first block
        blockSpeed = defaultBlockSpeed - blockSpeedDelta;
    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    public void StartGame()
    {
        //So that the method is not called again if tapped twice
        if (isGameRunning)
        {
            return;
        }
        isGameRunning = true;

        //Since input might conflict with the start game input, we start it with a little delay
        Invoke("ActivateInputWithDelay", 0.2f);
        BringNewBlock();
    }

    /// <summary>
    /// Updates the skin texture on the block
    /// </summary>
    public void UpdateSkin(Texture texture)
    {
        selectedSkinTexture = texture;
        startBaseBlock.GetComponent<MeshRenderer>().material.mainTexture = selectedSkinTexture;
    }


    void ActivateInputWithDelay()
    {
        isInputActivated = true;
    }

    /// <summary>
    /// Checking for tap in Update
    /// </summary>
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // Additional spacebar Input for making editor testing easy
        if (isInputActivated && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            Tap();
        }

        //This is a cheat. Can be used for testing etc. 
        if (currentBlock != null && isInputActivated && cheat)
        {
            if (Vector2.Distance(new Vector2(currentBlock.transform.position.x, currentBlock.transform.position.z), 
                                 new Vector2(baseBlock.transform.position.x, baseBlock.transform.position.z)) < perfectDist * cheatRandomisation)
            {
                Tap();
            }
        }

#elif UNITY_ANDROID || UNITY_IOS
         if (isInputActivated && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Tap();
        }
#endif

    }

    /// <summary>
    /// Responsible for instantiating and setting up next blocks on scoring. 
    /// </summary>
    void BringNewBlock()
    {
        //Inverting the boolean because the blocks come from alternate directions everytime you score
        // If xOrZ is true the block is coming from x direction, otherwise from z.
        xOrZ = !xOrZ;

        //Get a new block from Object pooler
        GameObject block = objectPooler.GetPooledObject(ObjectPoolItems.Block, true);

        //Set the block's scale to the scale of the base block
        block.transform.localScale = baseBlock.transform.localScale;

        float x = baseBlock.transform.position.x;
        float y = baseBlock.transform.position.y + yOffset;
        float z = baseBlock.transform.position.z;
        float offset = 0;

        BlockScript blockScript = block.GetComponent<BlockScript>();

        //Calculating the ranges inside which the blocks will oscillate. Since the sizes of the blocks change as we progress in the game, the position also changes.
        // offsetFromCentre_x / offsetFromCentre_z are offsets from the centre. it is taken as 85 % of the start base block's scale towards the respective incoming direction
        // This offset is then added to half of incoming block's scale. This total offset is calculated so that start and end points of incoming block never overlap with the start base block.
        if (xOrZ)
        {
            offset = startBaseBlock.transform.lossyScale.x * 0.85f + block.transform.localScale.x * 0.5f;
            blockScript.setPositions(
                new Vector3(startBaseBlock.transform.position.x + offset, y, z),
                new Vector3(startBaseBlock.transform.position.x - offset, y, z));
        }
        else
        {
            offset = startBaseBlock.transform.lossyScale.z * 0.85f + block.transform.localScale.z * 0.5f;
            blockScript.setPositions(
                new Vector3(x, y, startBaseBlock.transform.position.z + offset),
                new Vector3(x, y, startBaseBlock.transform.position.z - offset));
        }

        //Start position is offset by twice so that the block starts from outside the screen
        Vector3 offsetVector = xOrZ ? offset * Vector3.right : offset * Vector3.forward;
        Vector3 yoffset = Vector3.up * yOffset;
        block.transform.position = baseBlock.transform.position - offsetVector * 2 + yoffset;
        block.transform.rotation = baseBlock.transform.rotation;


        //Create a new material for the block and set its colot and texture
        blockMat = new Material(Shader.Find(shaderType));
        blockMat.SetColor("_Color", colorController.BlockColor);
        blockMat.mainTexture = selectedSkinTexture;
        block.GetComponent<MeshRenderer>().material = blockMat;


        //Block speed
        if (ScoreAndCashManager.Instance.currentScore % blockSpeedResetScore == 0)
        {
            blockSpeed = defaultBlockSpeed;
        }
        else if (ScoreAndCashManager.Instance.currentScore % blockSpeedIncreaseScore == 0)
        {
            float newSpeed = blockSpeed + blockSpeedDelta;
            blockSpeed = newSpeed <= maxBlockSpeed ? newSpeed : maxBlockSpeed;
        }
        blockScript.speed = blockSpeed;


        //Set currentBlock and enable blockScript since it might be disabled if it is a pooled object. 
        currentBlock = block;
        blockScript.enabled = true;

        //Copy the uvs from baseBlock to this newBlock because default UVs will not work as the UVs are adjusted to match a cut.
        MeshUVAdjuster.CopyUVs(baseBlock, currentBlock.transform);
    }

    /// <summary>
    /// Responsible for the block cutting , perfect and game over logic when user taps.
    /// </summary>
    void Tap()
    {
        //Storing positions with other component zeroed out so that the distance can be along one for score/gameover. (can also be checked on XZ plane)
        //using some util methods for doing so. Makes the code smaller
        Vector3 currentBlockPosVector = xOrZ ? VectorHelper.GetXVector(currentBlock.transform.position) : VectorHelper.GetZVector(currentBlock.transform.position);
        Vector3 baseBlockPosVector = xOrZ ? VectorHelper.GetXVector(baseBlock.transform.position) : VectorHelper.GetZVector(baseBlock.transform.position);

        float gameOverDist = xOrZ ? currentBlock.transform.lossyScale.x * 0.5f + baseBlock.lossyScale.x * 0.5f :
                                   currentBlock.transform.lossyScale.z * 0.5f + baseBlock.lossyScale.z * 0.5f;

        if (Vector3.Distance(currentBlockPosVector, baseBlockPosVector) > gameOverDist)
        {
            GameOver();
            return;
        }

        //If the distance between the blocks is less than the perfetDist, It is a perfect score. YAY!!
        if (Vector3.Distance(currentBlockPosVector, baseBlockPosVector) < perfectDist)
        {
            PerfectScore();
        }
        else
        {
            ChopBlock();
        }

        BringNewBlock();
        cameraController.UpdatePos();
        ScoreAndCashManager.Instance.UpdateScore();
    }


    /// <summary>
    /// When game over
    /// </summary>
    void GameOver()
    {
        GameManager.Instance.GameOver();

        //This is game over and the desired behaviour is that the block simply falls.
        //Instead of attaching a rb to this block, replacing the current block with a leftover block and disabling current block, since leftover Block already has a rigidbody attached.
        currentBlock.SetActive(false);

        GameObject leftoverBlockPiece = objectPooler.GetPooledObject(ObjectPoolItems.leftoverBlock, true);
        VectorHelper.CopyTransformProperties(leftoverBlockPiece.transform, currentBlock.transform);

        leftoverBlockPiece.GetComponent<MeshRenderer>().material = blockMat;
        MeshUVAdjuster.CopyUVs(currentBlock.transform, leftoverBlockPiece.transform);

        SoundManager.Instance.playSound(AudioClips.split);
        isGameRunning = false;
        isInputActivated = false;
    }

    /// <summary>
    /// When it is a perfect score
    /// </summary>
    void PerfectScore()
    {
        //Disable BlockScript so it stops moving
        currentBlock.GetComponent<BlockScript>().enabled = false;

        //Position exactly on to of the base block
        currentBlock.transform.position = new Vector3(baseBlock.position.x, currentBlock.transform.position.y, baseBlock.position.z);

        //Set current cube as baseCube
        baseBlock = currentBlock.transform;

        //Perfect fx
        objectPooler.GetPooledObject(ObjectPoolItems.Perfectfx, currentBlock.transform.position, true);
        SoundManager.Instance.playSound(AudioClips.perfect);
    }



    /// <summary>
    /// Block chopping logic
    /// </summary>
    void ChopBlock()
    {
        VectorHelper.Vector3Coord axis = xOrZ ? VectorHelper.Vector3Coord.x : VectorHelper.Vector3Coord.z;

        SoundManager.Instance.playSound(AudioClips.split);

        currentBlock.GetComponent<MeshRenderer>().material = blockMat;

        //Storing  base and current block's position because this are being used in a lot of places
        float baseBlockPos = xOrZ ? baseBlock.position.x : baseBlock.position.z;
        float baseBlockScale = xOrZ ? baseBlock.lossyScale.x : baseBlock.lossyScale.z;
        float currentBlockPos = xOrZ ? currentBlock.transform.position.x : currentBlock.transform.position.z;

        //Distane between the blocks
        float relDist = Mathf.Abs(baseBlockPos - currentBlockPos);

        //Scale of the remaining block is equal to the above distance subracted from base block's scale . (refer documentation for a detailed explaination)
        float remainingBlockScale = baseBlockScale - relDist;

        //This is the piece left after the cut
        GameObject leftoverBlockPiece = objectPooler.GetPooledObject(ObjectPoolItems.leftoverBlock, true);

        int sign = currentBlockPos < baseBlockPos ? -1 : 1;

        float remainingBlockPos = (baseBlockPos + sign * baseBlockScale * 0.5f) - sign * remainingBlockScale * 0.5f;
        float leftoverBlockScale = relDist;// also equal to baseBlockScale - newBlockScale;
        float leftoverBlockPos = remainingBlockPos + sign * remainingBlockScale * 0.5f + sign * leftoverBlockScale * 0.5f;

        leftoverBlockPiece.transform.localScale = VectorHelper.GetVectorWith(axis, currentBlock.transform.lossyScale, leftoverBlockScale);
        leftoverBlockPiece.transform.position = VectorHelper.GetVectorWith(axis, currentBlock.transform.position, leftoverBlockPos);

        leftoverBlockPiece.GetComponent<MeshRenderer>().material = blockMat;

        //current block is the new Block
        currentBlock.transform.position = VectorHelper.GetVectorWith(axis, currentBlock.transform.position, remainingBlockPos);
        currentBlock.transform.localScale = VectorHelper.GetVectorWith(axis, currentBlock.transform.lossyScale, remainingBlockScale);
        leftoverBlockPiece.transform.rotation = currentBlock.transform.rotation;


        currentBlock.GetComponent<BlockScript>().enabled = false;

        //Adjust UVs for the pieces . xOrZ and currentBlockPos<baseBlockPos determine the order for UV adjustment. 
        //For UV adjsutment logic check MeshUVAdjuster.
        if (xOrZ)
        {
            MeshUVAdjuster.AjustUVsforpiecesFromX(baseBlock, leftoverBlockPiece.transform, currentBlock.transform, currentBlockPos < baseBlockPos);
        }
        else
        {
            MeshUVAdjuster.AdjustUVsforpiecesFromZ(baseBlock, currentBlock.transform, leftoverBlockPiece.transform, currentBlockPos < baseBlockPos);
        }
        baseBlock = currentBlock.transform;
    }

    /// <summary>
    /// Deregistering from all events in OnDestroy
    /// </summary>
    private void OnDestroy()
    {
        GameManager.Instance.GameInitialized -= InitGame;
        GameManager.Instance.GameStarted -= StartGame;
    }
}
}
