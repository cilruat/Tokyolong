using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTypeGroup : MonoBehaviour 
{
    public RectTransform rt;
    public float start;
    public float end;
    public float duration;
    public float startDelay;
    public float endDelay;
    public AnimationCurve showCurve;
    public AnimationCurve hideCurve;

    [System.NonSerialized]public bool show = false;

    Coroutine moveRoutine = null;

    public void Show()
    {
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, start);
        moveRoutine = StartCoroutine(_Move(true, start, end, duration, startDelay));
    }
        
    public void Hide()
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);
        
        float endPosY = rt.anchoredPosition.y > end ? rt.anchoredPosition.y : end;

        moveRoutine = StartCoroutine(_Move(false, endPosY, start, duration, endDelay));
    }

    IEnumerator _Move(bool show, float startPosY, float endPosY, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);

        float rate = 0f;
        float elapsed = 0f;
        while (rate < 1f)
        {
            elapsed += Time.deltaTime;
            rate = Mathf.Min(1f, elapsed / duration);

            float rateIncDec = show ? showCurve.Evaluate(rate) : hideCurve.Evaluate(rate);

            float posY = startPosY + ((endPosY-startPosY) * rateIncDec);

            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, posY);
            yield return null;
        }

        moveRoutine = null;
        this.show = show;
    }

    float RATE_INC( float rate ) { return 1f - Mathf.Cos (rate * Mathf.PI * .5f); }
    float RATE_DEC( float rate ) { return Mathf.Sin (rate * Mathf.PI * .5f); }
}
