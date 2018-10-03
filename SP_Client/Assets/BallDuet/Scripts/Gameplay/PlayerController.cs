using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OnefallGames
{
	public enum PlayerState
	{
	    Prepare,
	    Living,
	    Die,
	}

	public class PlayerController : MonoBehaviour {

	    public static PlayerController Instance { private set; get; }
	    public static event System.Action<PlayerState> PlayerStateChanged = delegate { };

	    public PlayerState PlayerState
	    {
	        get
	        {
	            return playerState;
	        }

	        private set
	        {
	            if (value != playerState)
	            {
	                value = playerState;
	                PlayerStateChanged(playerState);
	            }
	        }
	    }


	    private PlayerState playerState = PlayerState.Die;


	    [Header("Player Config")]
	    [SerializeField] private int maxMovingPointsNumber = 80;
	    [SerializeField] private int minMovingPointsNumber = 50;
	    [SerializeField] private int scoreToDecreaseMovingPoints = 10;
	    [SerializeField] private int movingPointsDecreasingAmount = 4;
	    [SerializeField] private float rotatingSpeed = 500f;
	    [SerializeField] private float limitTop = 25;
	    [SerializeField] private float limitBottom = 0;
	    
	    [Header("Player References")]
	    [SerializeField] private SpriteRenderer spRender;
	    [SerializeField] private Material trailMaterial;
	    [SerializeField] private ParticleSystem playerExplode;
	    [SerializeField] private EdgeCollider2D[] childColliders;

	    private int movingPointNumber = 0;
	    private int previousScore = 0;
	    private bool isMatchedColor = false;
	    private bool touchDisable = false;
	    private void OnEnable()
	    {
	        GameManager.GameStateChanged += GameManager_GameStateChanged;
	    }
	    private void OnDisable()
	    {
	        GameManager.GameStateChanged -= GameManager_GameStateChanged;
	    }

	    private void GameManager_GameStateChanged(GameState obj)
	    {
	        if (obj == GameState.Playing)
	        {
	            if (!GameManager.Instance.IsPause)
	                PlayerLiving();
	        }
	        else if (obj == GameState.Prepare)
	        {
	            PlayerPrepare();
	        }
	    }



	    void Awake()
	    {
	        if (Instance == null)
	        {
	            Instance = this;
	        }
	        else
	        {
	            DestroyImmediate(Instance.gameObject);
	            Instance = this;
	        }
	    }

	    void OnDestroy()
	    {
	        if (Instance == this)
	        {
	            Instance = null;
	        }
	    }
		
		// Update is called once per frame
		void Update () {

	        if (GameManager.Instance.GameState == GameState.Playing)
	        {
				if (Input.GetKeyDown (KeyCode.LeftArrow) && !touchDisable) {
					touchDisable = true;
					StartCoroutine (Rotating (false));
				} else if (Input.GetKeyDown (KeyCode.RightArrow) && !touchDisable) {
					touchDisable = true;
					StartCoroutine (Rotating (true));
				}
	        }   
		}

	    void PlayerPrepare()
	    {
	        //Fire event
	        PlayerState = PlayerState.Prepare;
	        playerState = PlayerState.Prepare;

	        //Add another actions here
	        movingPointNumber = maxMovingPointsNumber;
	        SetChildColliders(false);
	        spRender.enabled = false;
	        playerExplode.gameObject.SetActive(false);
	    }

	    void PlayerLiving()
	    {
	        //Fire event
	        PlayerState = PlayerState.Living;
	        playerState = PlayerState.Living;

	        //Add another actions here
	        SetChildColliders(true);
	        spRender.enabled = true;
	        StartCoroutine(FallingDown());
	        StartCoroutine(IncreasingMovingPoint());
	    }

	    public void PlayerDie()
	    {
	        //Fire event
	        PlayerState = PlayerState.Die;
	        playerState = PlayerState.Die;

	        //Add another actions here
	        SoundManager.Instance.PlaySound(SoundManager.Instance.gameOver);
	        SetChildColliders(false);
	        spRender.enabled = false;
	        StartCoroutine(PlayExplosion());
	    }
	    private IEnumerator PlayExplosion()
	    {
	        playerExplode.gameObject.SetActive(true);
	        playerExplode.Play();
	        yield return new WaitForSeconds(playerExplode.main.startLifetimeMultiplier);
	    }

	    private IEnumerator IncreasingMovingPoint()
	    {
	        while (playerState == PlayerState.Living)
	        {
	            int currentScore = ScoreManager.Instance.Score;
	            if (currentScore % scoreToDecreaseMovingPoints == 0)
	            {
	                if (currentScore != previousScore)
	                {
	                    if (movingPointNumber > minMovingPointsNumber)
	                    {
	                        previousScore = currentScore;
	                        movingPointNumber -= movingPointsDecreasingAmount;
	                    }
	                    else
	                    {
	                        yield break;
	                    }      
	                }
	            }
	            yield return null;
	        }
	    }

	    private IEnumerator FallingDown()
	    {
	        if (!GameManager.Instance.IsRevived)
	        {
	            float t = 0;
	            Vector2 startPos = transform.position;
	            Vector2 endPos = new Vector2(startPos.x, limitBottom);
	            float fallingTime = 0.5f;
	            while (t < fallingTime)
	            {
	                t += Time.deltaTime;
	                float factor = t / fallingTime;
	                transform.position = Vector2.Lerp(startPos, endPos, factor);
	                yield return null;
	            }
	        }

	        StartCoroutine(MovingForward());
	    }


	    //Rotating player
	    private IEnumerator Rotating(bool isRotatingRight)
	    {
	        Vector3 startAngles = transform.eulerAngles;
	        Vector3 endangles = new Vector3(0, 0, (isRotatingRight) ? startAngles.z - 90f : startAngles.z + 90f);
	        float t = 0;
	        float rotatingTime = 90f / rotatingSpeed;
	        while (t < rotatingTime)
	        {
	            t += Time.deltaTime;
	            float factor = t / rotatingTime;
	            transform.eulerAngles = Vector3.Lerp(startAngles, endangles, factor);
	            yield return null;
	        }
	        yield return new WaitForSeconds(0.01f);
	        touchDisable = false;
	    }

	    //Move player forward
	    private IEnumerator MovingForward()
	    {
	        List<Vector2> listPositions = new List<Vector2>();
	        while (true)
	        {
				if (playerState == PlayerState.Die)
					yield break;

	            listPositions.Clear();
	            Vector2 startPoint = transform.position;
	            Vector2 endPoint = startPoint + Vector2.right * GameManager.Instance.PillarSpace;
	            Vector2 midPoint = Vector2.Lerp(startPoint, endPoint, 0.5f) + Vector2.up * limitTop;
	            listPositions.Add(transform.position);
	            for(int i = 1; i <= movingPointNumber; i++)
	            {
	                float t = i / (float)movingPointNumber;
	                listPositions.Add(CalculateQuadraticBezierPoint(t, startPoint, midPoint, endPoint));
	            }

	            for(int i = 0; i < listPositions.Count; i++)
	            {
	                transform.position = listPositions[i];
	                yield return null;
	                while (GameManager.Instance.GameState == GameState.Pause)
	                {
	                    yield return null;
	                }
	            }


	            //Determine whether the color of player is match with the pillar's color at the bottom  
	            if (!isMatchedColor) //Didn't match -> player die
	            {           
	                PlayerDie();
	                ShareManager.Instance.CreateScreenshot();
	                GameManager.Instance.HandlePlayerDieState(1f);
	                yield break;
	            }
	            else //Matched -> add score, play sound
	            {
	                ScoreManager.Instance.AddScore(1);
	                SoundManager.Instance.PlaySound(SoundManager.Instance.bounce);
					GameManager.Instance.CreatePillar();
					GameManager.Instance.CreateMountain();
	            }
	        }
	    }


	    //Calculate position for moving player
	    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 from, Vector3 middle, Vector3 to)
	    {
	        return Mathf.Pow((1 - t), 2) * transform.position + 2 * (1 - t) * t * middle + Mathf.Pow(t, 2) * to;
	    }

	    private void SetChildColliders(bool active)
	    {
	        foreach(EdgeCollider2D o in childColliders)
	        {
	            o.enabled = active;
	        }
	    }

	    //////////////////////////////////////////////////public functions


	    /// <summary>
	    /// Set isMatchedColor as given variable
	    /// </summary>
	    /// <param name="isMatched"></param>
	    public void SetMatchedColor(bool isMatched)
	    {
	        isMatchedColor = isMatched;
	    }
	}
}