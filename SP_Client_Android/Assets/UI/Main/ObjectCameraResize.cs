using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class ObjectCameraResize : MonoBehaviour {


    public SpriteRenderer sprite;

    public Color day;
    public Color night;

    public float oneDay;
    public float currentTime;

    [Range(0.01f, 0.2f)]
    public float transitionTime;

    bool isSwap = false;


    private void Awake()
    {
       float spritex = sprite.sprite.bounds.size.x;
       float spritey = sprite.sprite.bounds.size.y;


        float screenY = Camera.main.orthographicSize * 2;
        float screenX = screenY / Screen.height * Screen.width;

        transform.localScale = new Vector2(Mathf.Ceil(screenX / spritex), Mathf.Ceil(screenY / spritey));



        sprite.color = day;
    }




    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        currentTime += Time.deltaTime;

        if(currentTime >= oneDay)
        {
            currentTime = 0;
        }


        if(!isSwap)
        {
            if (Mathf.FloorToInt(oneDay * 0.4f) == Mathf.FloorToInt(currentTime))
            {
                // day -> night
                isSwap = true;
                StartCoroutine(SwapColor(sprite.color, night));
            }
            else if (Mathf.FloorToInt(oneDay * 0.9f) == Mathf.FloorToInt(currentTime))
            {
                // night -> day
                isSwap = true;
                StartCoroutine(SwapColor(sprite.color, day));
            }
        }




        IEnumerator SwapColor(Color start, Color end)
        {
            float t = 0;

            while(t<1)
            {
                t += Time.deltaTime * (1 / (transitionTime * oneDay));
                sprite.color = Color.Lerp(start, end, t);
                yield return null;
            }
            isSwap = false;
        }
    }
}
*/
