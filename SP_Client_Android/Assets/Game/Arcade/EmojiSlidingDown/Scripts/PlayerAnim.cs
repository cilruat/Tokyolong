using UnityEngine;
using System.Collections;

public class PlayerAnim : MonoBehaviour
{

    [Header("Squeeze")]
    public float squeezeDuration;
    public AnimationCurve xCurve, yCurve;

    protected Vector3 initScale;

    public void Start()
    {
        initScale = transform.localScale;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Squeeze();
    }

    public void Squeeze()
    {
        StartCoroutine(CRSqueeze());
    }

    public IEnumerator CRSqueeze()
    {
        float time = 0;
        float scaleX, scaleY;
        while (time < squeezeDuration)
        {
            scaleX = xCurve.Evaluate(time / squeezeDuration);
            scaleY = yCurve.Evaluate(time / squeezeDuration);
            transform.localScale = new Vector3(scaleX * initScale.x, scaleY * initScale.y, 1);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = initScale;
    }
}
