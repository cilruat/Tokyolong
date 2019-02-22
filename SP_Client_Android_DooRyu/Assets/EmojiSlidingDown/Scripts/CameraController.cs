using UnityEngine;
using System.Collections;

namespace Emoji
{
	public class CameraController : MonoBehaviour
	{
	    public static CameraController Instance;

	    public Transform playerTransform;
	    private Vector3 originalDistance;

	    [Header("Camera Follow")]
	    public float smoothness = 0.97f;
	    public float verticalOffset;
	    public bool followVertically;

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

	    public void OnEnable()
	    {
	        GameManager.GameStateChanged += OnGameStateChanged;
	    }

	    public void OnDisable()
	    {
	        GameManager.GameStateChanged -= OnGameStateChanged;
	    }

	    private void OnGameStateChanged(GameState newState, GameState oldState)
	    {
	        if (newState != GameState.Playing)
	            enabled = false;
	    }

	    void Start()
	    {
	        originalDistance = transform.position - playerTransform.transform.position;
	    }

	    void Update()
	    {
	        Vector3 nextPos = Vector3.Lerp(transform.position, playerTransform.position + Vector3.up * verticalOffset, 1 - smoothness);
	        Vector2 translation = nextPos - transform.position;
	        if (followVertically)
	            translation.x = 0;
	        transform.Translate(translation);
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
	            transform.position = originalPos + Random.insideUnitSphere * shakeAmount;
	            currentShakeDuration -= Time.deltaTime * decreaseFactor;
	            yield return null;
	        }
	        transform.position = originalPos;
	    }
	}
}