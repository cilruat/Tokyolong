using UnityEngine;
using System.Collections;

namespace BlockJumpRabbit
{
public class PlayerController : MonoBehaviour {

    public enum TypeJump
    {
        NormalJump ,
        DoubleJump 
    }

    public enum TypeController
    {
        Player,
        AI
    }
		
    public Animator animPlayer;
    public float speedJump = 500.0f;
    public TypeController typeController = TypeController.Player;
    public GameObject spritePlayer;
    public bool IsJump { get { return isJumping; } }
	public ParticleSystem particleJumpHitGround;
    public ParticleSystem particleFallWater;
    public ParticleSystem particleBlood;

	public AudioSource eatedAudio;
	public AudioSource fallWaterAudio;

    private GameController gameController;
	private BoxCollider2D boxPlayer;
	private AudioSource sound_FX;
	private Rigidbody2D rid;

    private Vector2 posHeighest;
    private Vector2 posReach;
	private bool isJumping;											// This value to check when player in state jump or not
    private bool isDead;

    private const float distanceJumpOneStep = GameController.DISTANCE_OBJ;
    private const float distanceJumpTwoStep = GameController.DISTANCE_OBJ * 2;
    private const float jumpHeight = 1.0f;

    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
		boxPlayer = GetComponent<BoxCollider2D> ();
		sound_FX = GetComponent<AudioSource> ();
        isDead = true;

    }

	private bool isTaping;
	private float timeTap = 0.2f;
	private float lastTap = 0.0f;

    void Update()
    {
#if UNITY_EDITOR
        if(typeController == TypeController.Player && !isDead)
            EditorJump();
#endif

		if(typeController == TypeController.Player && !isDead) {
			if(Input.GetKeyDown(KeyCode.K)) {
				if(!isTaping)  {
					isTaping = true;
					StartCoroutine (SingleTap ());
				}

				if(Time.time - lastTap < timeTap){
					Debug.Log ("Double tap");
					isTaping = false;
				}

				lastTap = Time.time;
			}
		}



    }

	IEnumerator SingleTap(){
		yield return new WaitForSeconds (timeTap);
		if(isTaping) {
			Debug.Log ("Single tap");
			isTaping = false;
		}
	}

    #region ControlEditorOrAdroid
#if UNITY_EDITOR
    void EditorJump()
    {
        if (Input.GetMouseButtonDown(0) & !isJumping )
        {
            gameController.AddScore(1);
            Jump(TypeJump.NormalJump);
        }
        else if (Input.GetMouseButtonDown(1) & !isJumping)
        {
            gameController.AddScore(2);
            Jump(TypeJump.DoubleJump);
        }
    }
#endif

#if !UNITY_EDITOR
    public void NormalJumpButton(){
		if(!isJumping && !isDead)
		{
			gameController.AddScore(1);
			Jump(TypeJump.NormalJump);
		}
	}

	public void DoubleJumpButton(){
		if(!isJumping && !isDead)
		{
			gameController.AddScore(2);
			Jump(TypeJump.DoubleJump);
		}
	}

	#endif
    #endregion

    #region JumpProcess
    public void Jump(TypeJump typeJump)
    {
//        animPlayer.SetBool("jump",true);

		// Play sound jump
		if (sound_FX)
			sound_FX.Play ();

		isJumping = true;
        switch (typeJump)
        {
            case TypeJump.NormalJump:             
                StartCoroutine( JumpParabol(distanceJumpOneStep));
                break;
            case TypeJump.DoubleJump:                
                StartCoroutine( JumpParabol(distanceJumpTwoStep));
                break;
        }
    }

    IEnumerator JumpParabol(float distanceJump)
    {
		spritePlayer.transform.localScale = new Vector3 (1, 1, 1);

		posHeighest = new Vector2(transform.position.x + distanceJump / 2, 0 + jumpHeight);
		posReach = new Vector2(transform.position.x + distanceJump, 0);

        // Jump
		while (transform.position.y < posHeighest.y - 0.1f)
        {
			transform.position = Vector2.Lerp(transform.position, posHeighest, Time.deltaTime * speedJump );
            yield return null;
        }

        // Fall
		while(transform.position.y > 0.1f)
        {
			transform.position = Vector2.Lerp(transform.position, posReach, Time.deltaTime * speedJump );
            yield return null;
        }
			
		isJumping = false;



		transform.position = posReach;
//		animPlayer.SetBool("jump", false);

		// Animation scale when hit platform when finish jump
		spritePlayer.transform.localScale = new Vector3 (1, 0.7f, 1);

		if(particleJumpHitGround) {
			particleJumpHitGround.Clear ();
			particleJumpHitGround.Stop ();
			particleJumpHitGround.Play ();
		}

		while(spritePlayer.transform.localScale.y <0.99)
		{
			spritePlayer.transform.localScale = Vector3.Lerp (spritePlayer.transform.localScale, new Vector3 (1, 1, 1), Time.deltaTime*10.0f);
			yield return null;
		}
		spritePlayer.transform.localScale = new Vector3 (1, 1, 1);
    }
    #endregion

    public void PlayerStopMove()
    {
        StopAllCoroutines();
    }

	// Call dead when fall water
    public void FallWater()
    {
		fallWaterAudio.Play ();
		boxPlayer.enabled = false;
        isDead = true;
        animPlayer.SetTrigger("dead_fall_water");

		// Set particle
		particleJumpHitGround.Pause ();
        particleFallWater.Play();

        StartCoroutine(CallGameOver());
    }

	// Call dead when enemy kill
	public void EatPlayer(){
		eatedAudio.Play ();
		boxPlayer.enabled = false;
		isDead = true;
		animPlayer.SetTrigger ("dead_eat_by_enemy");
        StartCoroutine (CallGameOver ());
	}

    // Call from animation particle blood play when play fall 
    public void BloodParticlePlay()
    {
        particleBlood.Play();
    }

    // Call dead when enemy kill
    public void WolfEatPlayer()
    {
		eatedAudio.Play ();
        boxPlayer.enabled = false;
        isDead = true;
        animPlayer.SetTrigger("dead_eat_by_wolf");

        particleBlood.Play();

        StartCoroutine(CallGameOver());
    }

    IEnumerator CallGameOver()
    {
        yield return new WaitForSeconds(2.0f);
        gameController.ChangeStateGame(StateGame.GameOver);

        //animPlayer.SetTrigger("idle");
        spritePlayer.SetActive(true);
        gameObject.SetActive(false);
    }

	// Reset player
    public void Reset()
    {
		boxPlayer.enabled = true;
        isDead = false;
       spritePlayer.SetActive(true);

		print ("run");
    }

    // Method to reset sprite layer
    public void ResetSprite()
    {
        var sprites = transform.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer spr in sprites)
            spr.sortingLayerName = "Default";
    }

	}



}
