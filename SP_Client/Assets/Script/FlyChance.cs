using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        if (Info.isCheckScene("Main") && SceneChanger.nextName == "Main")
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
            xWeight = Random.Range(-1f, 0f);

        if (yWeight == 0f)
            yWeight = Random.Range(-1.5f, 1.5f);

        elapsedTime += Time.deltaTime*.5f;
        rate = elapsedTime;		
        Vector2 pos = rt.anchoredPosition;
        Vector2 bestDir = (target.anchoredPosition -pos).normalized;

		float moveSpeed = Mathf.LerpUnclamped(0f, 20f, RATE_INC(rate));

        if (rate < 1f)
        {
			pos -= (bestDir * Mathf.LerpUnclamped(0f, 2f, RATE_DEC(rate)));
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
