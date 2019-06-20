using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace BlockJumpRabbit
{
public class GradientColorChangeObject : MonoBehaviour
{
    public List<SpriteRenderer> listSprite;
    public int orderLayer;
    public int gradient = 255;
    public bool water;

    private SpriteRenderer currentSpriteActive;
    public void Awake()
    {
        // Change order layer each element
        foreach (SpriteRenderer spr in listSprite)
        {
            spr.sortingOrder = orderLayer;
            spr.gameObject.SetActive(false);
        }

        // Chose default element active
        listSprite[0].gameObject.SetActive(true);
        currentSpriteActive = listSprite[0];

    }

    // Method change color from current target color
    public void ChangeColor(int indexColor)
    {
        listSprite[indexColor].gameObject.SetActive(true);
        listSprite[indexColor].sortingOrder = orderLayer - 1;

        if(!water)
            listSprite[indexColor].color = ClearColor(listSprite[indexColor]);
        else
            listSprite[indexColor].color = ResetColor(listSprite[indexColor]);

        StartCoroutine(SmoothChangeColor(indexColor));
    }

    // Method corountine run make smooth change color
    IEnumerator SmoothChangeColor(int indexColor)
    {
        while(currentSpriteActive.color.a > 0.1f)
        {
            currentSpriteActive.color = Color32.Lerp(currentSpriteActive.color, new Color32(((Color32)currentSpriteActive.color).r, ((Color32)currentSpriteActive.color).b, ((Color32)currentSpriteActive.color).g, 0), Time.deltaTime*2);
            listSprite[indexColor].color = Color32.Lerp(listSprite[indexColor].color, ResetColor(listSprite[indexColor]), Time.deltaTime * 3);
            yield return null;
        }

        listSprite[indexColor].color = ResetColor(listSprite[indexColor]);
        listSprite[indexColor].sortingOrder = orderLayer;
        currentSpriteActive.gameObject.SetActive(false);
        currentSpriteActive = listSprite[indexColor];
    }

    // Method reset aplpha color
    Color32 ResetColor( SpriteRenderer spr)
    {
        return new Color32(((Color32)spr.color).r, ((Color32)spr.color).b, ((Color32)spr.color).g, (byte) gradient);
    }

    // Method set zero alpha color = zero
    Color32 ClearColor(SpriteRenderer spr)
    {
        return new Color32(((Color32)spr.color).r, ((Color32)spr.color).b, ((Color32)spr.color).g, 0);
    }

    //// Test
    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        ChangeColor(1);
    //    }
    //}
	}
}

