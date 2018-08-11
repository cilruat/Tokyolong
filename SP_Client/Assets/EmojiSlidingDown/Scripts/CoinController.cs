using UnityEngine;
using System.Collections;
using SgLib;
using Emoji;

public class CoinController : MonoBehaviour
{
    public bool bounce = true;
    public bool rotate = true;
    public Vector3 rotateAxis = Vector3.forward;
    public float rotateSpeed = 2;
    public GameObject coinEffect;

    private bool stop;
    // Use this for initialization
    void Start()
    {
        if (bounce)
            StartCoroutine(Bounce());

        if (rotate)
            StartCoroutine(Rotate());
    }

    void OnEnable()
    {
        GameManager.GameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState, GameState oldState)
    {
        if (newState == GameState.GameOver)
            gameObject.SetActive(false);
    }

    void OnDisable()
    {
        GameManager.GameStateChanged -= OnGameStateChanged;
    }

    public void GoUp()
    {
        stop = true;
        StartCoroutine(Up());
    }

    IEnumerator Rotate()
    {
        while (true)
        {
            transform.Rotate(rotateAxis * rotateSpeed);
            yield return null;
        }
    }

    IEnumerator Bounce()
    {
        while (true)
        {
            float bounceTime = 1f;

            float startY = transform.localPosition.y;
            float endY = startY + 0.5f;

            float t = 0;
            while (t < bounceTime / 2f)
            {
                if (stop)
                    yield break;
                t += Time.deltaTime;
                float fraction = t / (bounceTime / 2f);
                float newY = Mathf.Lerp(startY, endY, fraction);
                Vector3 newPos = transform.localPosition;
                newPos.y = newY;
                transform.localPosition = newPos;
                yield return null;
            }

            float r = 0;
            while (r < bounceTime / 2f)
            {
                if (stop)
                    yield break;
                r += Time.deltaTime;
                float fraction = r / (bounceTime / 2f);
                float newY = Mathf.Lerp(endY, startY, fraction);
                Vector3 newPos = transform.localPosition;
                newPos.y = newY;
                transform.localPosition = newPos;
                yield return null;
            }
        }
    }

    //Move up
    IEnumerator Up()
    {
        float time = 1f;

        float startY = transform.position.y;
        float endY = startY + 10f;

        float t = 0;
        while (t < time / 2f)
        {
            t += Time.deltaTime;
            float fraction = t / (time / 2f);
            float newY = Mathf.Lerp(startY, endY, fraction);
            Vector3 newPos = transform.position;
            newPos.y = newY;
            transform.position = newPos;
            yield return null;
        }

        gameObject.SetActive(false);
        GetComponent<MeshCollider>().enabled = true;
        transform.position = Vector3.zero;
        transform.parent = CoinManager.Instance.transform;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            gameObject.SetActive(false);
            CoinManager.Instance.AddCoins(1);
            SoundManager.Instance.PlaySound(SoundManager.Instance.coin);
            if (coinEffect != null)
            {
                coinEffect.transform.position = transform.position;
                coinEffect.SetActive(true);
            }
        }
    }
}
