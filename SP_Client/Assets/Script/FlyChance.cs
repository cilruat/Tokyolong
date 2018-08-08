using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyChance : MonoBehaviour
{
    public RectTransform rt;
    public RectTransform target;

    float rate = 0f;
    float elapsedTime = 0f;

    float xWeight = 0f;
    float yWeight = 0f;

    void OnDestroy()
    {
        Info.GamePlayCnt++;

        if (Info.isCheckScene("Main"))
            ((PageMain)PageBase.Instance).RefreshGamePlayChance();
    }

    void Update()
    {
        Flying();
    }

    void Flying()
    {
        if (target == false)
            return;

        if (xWeight == 0f)
            xWeight = Random.Range(-.25f, 0f);

        if (yWeight == 0f)
            yWeight = Random.Range(-1f, 1f);

        elapsedTime += Time.deltaTime *.25f;
        rate = Mathf.Clamp01(elapsedTime);
        Vector2 pos = rt.anchoredPosition;
        Vector2 bestDir = (target.anchoredPosition -pos).normalized;

        float moveSpeed = Mathf.Lerp(0f, 20f, RATE_INC(rate));

        if (rate < .75f)
        {
            pos -= (bestDir * Mathf.Lerp(.5f, 1f, RATE_DEC(rate)));
            pos.x += RATE_DEC(RATE_DEC(xWeight));
            pos.y -= RATE_DEC(RATE_DEC(yWeight));
        }

        Vector2 v2 = pos + (bestDir * moveSpeed);
        rt.anchoredPosition = new Vector2(v2.x, v2.y);

        float sqrDis = (target.anchoredPosition - rt.anchoredPosition).sqrMagnitude;
        if (sqrDis <= target.rect.width *.5f)
            Destroy(this.gameObject);
    }

    float RATE_INC( float rate ) { return 1f - Mathf.Cos (rate * Mathf.PI * .5f); }
    float RATE_DEC( float rate ) { return Mathf.Sin (rate * Mathf.PI * .5f); }
}
