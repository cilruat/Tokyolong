using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarController : MonoBehaviour {

    [SerializeField] private string colorType = string.Empty;
    [SerializeField] private Sprite greyPillar;

    private SpriteRenderer spRender = null;
    private Sprite originalSprite = null;
    private void OnEnable()
    {
        if (spRender == null)
            spRender = GetComponent<SpriteRenderer>();
        if (originalSprite == null)
            originalSprite = spRender.sprite;
        spRender.sprite = originalSprite;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.Playing)
        {
            float x = Camera.main.WorldToViewportPoint(transform.position).x;
            if (x <= -0.2f)
            {
                GameManager.Instance.CreatePillar();
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(Bouncing());
        bool matched = collision.tag.ToLower().Equals(colorType.ToLower());
        PlayerController.Instance.SetMatchedColor(matched);
    }

    private IEnumerator Bouncing()
    {
        //change sprite
        spRender.sprite = greyPillar;

        //bouncing down
        float t = 0;
        float bouncingTime = 0.1f;
        float startY = transform.position.y;
        float endY = startY - 0.5f;
        while (t < bouncingTime)
        {
            t += Time.deltaTime;
            float factor = t / bouncingTime;
            Vector2 pos = transform.position;
            pos.y = Mathf.Lerp(startY, endY, factor);
            transform.position = pos;
            yield return null;
        }

        //bouncing up
        t = 0;
        while (t < bouncingTime)
        {
            t += Time.deltaTime;
            float factor = t / bouncingTime;
            Vector2 pos = transform.position;
            pos.y = Mathf.Lerp(endY, startY, factor);
            transform.position = pos;
            yield return null;
        }
    }

    
}
