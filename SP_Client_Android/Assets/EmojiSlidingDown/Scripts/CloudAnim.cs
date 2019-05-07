using UnityEngine;
using System.Collections;

namespace Emoji
{
	public class CloudAnim : MonoBehaviour
	{
	    [Header("Bounce")]
	    public float bounciness;

	    [Header("Zoom")]
	    public float zoomDuration;
	    public AnimationCurve xCurve, yCurve;
	    public bool zoomInOnAwake;

	    [Header("References")]
	    public ParticleSystem particle;

	    protected Vector3 initScale;

	    public void Start()
	    {
	        initScale = transform.localScale;
	        if (zoomInOnAwake)
	            ZoomIn();
	    }

	    void OnEnable()
	    {
	        GameManager.GameStateChanged += OnGameStateChanged;
	    }

	    void OnDisable()
	    {
	        GameManager.GameStateChanged -= OnGameStateChanged;
	    }

	    private void OnGameStateChanged(GameState newState, GameState oldState)
	    {
	        if (newState == GameState.GameOver)
	        {
	            ZoomOut();
	        }
	    }

	    public void ZoomIn()
	    {
	        StartCoroutine(CRZoomIn());
	    }

	    public IEnumerator CRZoomIn()
	    {
	        float time = 0;
	        float scaleX, scaleY;
	        while (time < zoomDuration)
	        {
	            scaleX = xCurve.Evaluate(time / zoomDuration);
	            scaleY = yCurve.Evaluate(time / zoomDuration);
	            transform.localScale = new Vector3(scaleX * initScale.x, scaleY * initScale.y, 1);
	            time += Time.deltaTime;
	            yield return null;
	        }

	        scaleX = xCurve.Evaluate(1);
	        scaleY = yCurve.Evaluate(1);
	        transform.localScale = new Vector3(scaleX * initScale.x, scaleY * initScale.y, 1);

	        particle.Play();
	    }

	    public void ZoomOut()
	    {
	        StartCoroutine(CRZoomOut());
	    }

	    public IEnumerator CRZoomOut()
	    {
	        float time = 0;
	        float scaleX, scaleY;
	        while (time < zoomDuration)
	        {
	            scaleX = xCurve.Evaluate(1 - time / zoomDuration);
	            scaleY = yCurve.Evaluate(1 - time / zoomDuration);
	            transform.localScale = new Vector3(scaleX * initScale.x, scaleY * initScale.y, 1);
	            time += Time.deltaTime;
	            yield return null;
	        }

	        scaleX = xCurve.Evaluate(0);
	        scaleY = yCurve.Evaluate(0);
	        transform.localScale = new Vector3(scaleX * initScale.x, scaleY * initScale.y, 1);

	        particle.Stop();
	    }

	    public void Bounce()
	    {
	        StartCoroutine(CRBounce());
	    }

	    public IEnumerator CRBounce()
	    {
	        Vector3 beginPos, endPos;
	        beginPos = transform.localPosition;
	        endPos = beginPos + Vector3.down * bounciness;

	        while (transform.localPosition != endPos)
	        {
	            transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPos, bounciness / 3);
	            yield return null;
	        }

	        while (transform.localPosition != beginPos)
	        {
	            transform.localPosition = Vector3.MoveTowards(transform.localPosition, beginPos, bounciness / 3);
	            yield return null;
	        }
	    }
	}
}