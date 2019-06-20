using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    [HideInInspector]
    public int simultaneousPlayCount = 0;
}