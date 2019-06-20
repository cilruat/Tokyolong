using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace BlockJumpRabbit
{
// Test background change solor

public class TestChangeColor : MonoBehaviour
{
    public List<GradientColorChangeObject> listCreatures;

    void Awake()
    {
        StartCoroutine(ChangeColor());

        listCreatures = new List<GradientColorChangeObject>();
        GradientColorChangeObject[] array = FindObjectsOfType<GradientColorChangeObject>();
        foreach (GradientColorChangeObject obj in array)
            listCreatures.Add(obj);
    }


    void ChangeSeason(Season season)
    {
        foreach (GradientColorChangeObject obj in listCreatures)
            obj.ChangeColor((int)season);
    }
    IEnumerator ChangeColor()
    {
        ChangeSeason(Season.Spring);

        yield return new WaitForSeconds(3.0f);
        ChangeSeason(Season.Summer);

        yield return new WaitForSeconds(3.0f);
        ChangeSeason(Season.Autumn);

        yield return new WaitForSeconds(3.0f);
        ChangeSeason(Season.Winter);

        yield return new WaitForSeconds(3.0f);
        ChangeSeason(Season.Spring);
    }
}
}