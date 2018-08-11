using UnityEngine;
using System.Collections;

public class SunburstAnim : MonoBehaviour
{

    public float duration;

    public AnimationCurve rotationDeltaOverTime;
    public AnimationCurve scaleOverTime;
    public Gradient colorOverTime;

    private SpriteRenderer spriteRenderer;
    private Vector3 initScale;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initScale = transform.localScale;
    }

    public void Start()
    {

    }



    public void PlayAnim()
    {
        gameObject.SetActive(true);
        StartCoroutine(CRPlayAnim());
    }

    public IEnumerator CRPlayAnim()
    {
        float time = 0;
        float scaleX, scaleY;
        float rotationDelta;
        Color color;
        while (time < duration)
        {
            scaleX = scaleOverTime.Evaluate(time / duration);
            scaleY = scaleX;
            transform.localScale = new Vector3(scaleX * initScale.x, scaleY * initScale.y, 1);

            rotationDelta = rotationDeltaOverTime.Evaluate(time / duration);
            transform.Rotate(new Vector3(0, 0, rotationDelta));

            color = colorOverTime.Evaluate(time / duration);
            spriteRenderer.color = color;
            time += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
