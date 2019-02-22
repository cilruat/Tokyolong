using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickStar : MonoBehaviour 
{
    public RectTransform rt;
    public List<ParticleSystem> listParticle;

    public void ShowClickStar(Vector3 mousePos)
    {
        float anchorX = mousePos.x / Screen.width;
        float anchorY = mousePos.y / Screen.height;
        rt.anchorMin = new Vector2(anchorX, anchorY);
        rt.anchorMax = new Vector2(anchorX, anchorY);
        rt.anchoredPosition = Vector2.zero;

        this.gameObject.SetActive(true);

        StartCoroutine(CheckParticle());
    }

    float destroyTime = 5f;
    float elapsedTime = 0f;
    IEnumerator CheckParticle()
    {
        bool isEnd = false;
        while(isEnd == false)
        {
            bool isAlive = false;
            for (int i = 0; i < listParticle.Count; i++)
            {
                if (listParticle[i].IsAlive())
                {
                    isAlive = true;
                    break;
                }
            }

            elapsedTime += Time.deltaTime;

            if (isAlive == false || elapsedTime >= destroyTime)
                isEnd = true;

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
