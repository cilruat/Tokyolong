using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAnim2 : MonoBehaviour {
    public Texture2D[] Textures;
    public Texture2D[] NormalTextures;

    public bool NormalMapOn;
    public int fps;
    int counter = 0;

    void Start () {

        float timer = 1.0f / fps;
        InvokeRepeating("Increment", 0, timer);
    }
    void Increment()
    {
        counter += 1;

        if (counter == Textures.Length)
        {
            counter = 0;
        }
    }
    void Update () {
        gameObject.GetComponent<Renderer>().material.mainTexture = Textures[counter];
        if (NormalMapOn != true) return;
        GetComponent<Renderer>().material.SetTexture("_BumpMap", NormalTextures[counter]);
    }
}
