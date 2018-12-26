using UnityEngine;
using System.Collections;
using UnityStandardAssets_ImageEffects;
using System;

namespace CrashRacing
{
	public class CameraController : MonoBehaviour
	{
	    public Transform playerTransform;
	    private Vector3 velocity = Vector3.zero;
	    private Vector3 originalDistance;

	    [Header("Camera Follow Smooth-Time")]
	    public float smoothTime = 0.1f;

	    [Header("Shaking Effect")]
	    // How long the camera shaking.
	    public float shakeDuration = 0.1f;
	    // Amplitude of the shake. A larger value shakes the camera harder.
	    public float shakeAmount = 0.2f;
	    public float decreaseFactor = 0.3f;
	    [HideInInspector]
	    public Vector3 originalPos;

	    private float currentShakeDuration;
	    private float currentDistance;

	    public Camera mainCam;
	    private Vector3 defaultPlayerInViewport;
	    private Vector3 defaultEulerAngles;
	    [HideInInspector]
	    public bool stop;

	    float defaultFOV;
	    public float zoomInFov = 40;
	    void Start()
	    {
	        mainCam = GetComponent<Camera>();
	        defaultFOV = mainCam.fieldOfView;
	        defaultPlayerInViewport = mainCam.WorldToViewportPoint(playerTransform.position);
	        defaultEulerAngles = transform.localEulerAngles;
	        originalDistance = transform.position - playerTransform.transform.position;
	        PlayerController.PlayerDied += OnPlayerDie;
	        GameManager.GameStateChanged += OnGameStateChanged;
	    }
	    void OnDestroy()
	    {
	        PlayerController.PlayerDied -= OnPlayerDie;
	        GameManager.GameStateChanged -= OnGameStateChanged;
	    }

	    private void OnGameStateChanged(GameState gameState, GameState oldGameState)
	    {
	        if (gameState.Equals(GameState.Playing))
	        {
	            mainCam.fieldOfView = defaultFOV;
	            transform.localEulerAngles = defaultEulerAngles;
	        }
	    }

	    private void OnPlayerDie()
	    {
	        StartCoroutine(ZoomIn());
	    }

	    private IEnumerator ZoomIn()
	    {
	        float cameraFOV = mainCam.fieldOfView;
	        while (cameraFOV > zoomInFov)
	        {
	            yield return null;
	            cameraFOV -= 2;
	            mainCam.fieldOfView = cameraFOV;
	        }
	        Vector3 playerInViewport = mainCam.WorldToViewportPoint(playerTransform.position);
	        Vector3 eulerAngles = transform.localEulerAngles;
	        while (!GameManager.Instance.GameState.Equals(GameState.Playing))
	        {
	            yield return null;
	            playerInViewport = mainCam.WorldToViewportPoint(playerTransform.position);
	            eulerAngles = transform.localEulerAngles;
	            eulerAngles.x += (defaultPlayerInViewport.y - playerInViewport.y)*0.8f;
	            transform.localEulerAngles = eulerAngles;
	        }
	    }

	    public void MoveLeft()
	    {
	        StartCoroutine(TurnLeft());
	    }

	    public void MoveRight()
	    {
	        StartCoroutine(TurnRight());
	    }

	    IEnumerator TurnLeft()
	    {
	        float startX = Mathf.Round(transform.position.x);
	        float endX = startX - 4f;
	        if (endX >= -8)
	        {
	            float t = 0;
	            while (t < GameManager.Instance.turnTime && GameManager.Instance.GameState == GameState.Playing)
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
	    }


	    IEnumerator TurnRight()
	    {
	        float startX = Mathf.Round(transform.position.x);
	        float endX = startX + 4f;     
	        if (endX <= 8)
	        {
	            float t = 0;
	            while (t < GameManager.Instance.turnTime && GameManager.Instance.GameState == GameState.Playing)
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
	    }

	    void LateUpdate()
	    {
	        if (GameManager.Instance.GameState == GameState.Playing||GameManager.Instance.cameraAlwaysfollowPlayer)
	        {
	            float currentSmoothTime = smoothTime;
	            if (GameManager.Instance.GameState != GameState.Playing)
	                currentSmoothTime = 0.5f;
	            Vector3 pos = playerTransform.position + originalDistance;
	            transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, currentSmoothTime);
	        }
	    }

	    public void FixPosition()
	    {
	        transform.position = playerTransform.position + originalDistance;
	    }

	    public void ShakeCamera()
	    {
	        StartCoroutine(Shake());
	    }

	    IEnumerator Shake()
	    {
	        originalPos = transform.position;
	        currentShakeDuration = shakeDuration;
	        while (currentShakeDuration > 0)
	        {
	            originalPos = transform.position;
	            transform.position = originalPos + UnityEngine.Random.insideUnitSphere * shakeAmount;
	            currentShakeDuration -= Time.deltaTime * decreaseFactor;
	            yield return null;
	        }
	        transform.position = originalPos;
	    }

	}
}