using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object for games color.
/// </summary>
[CreateAssetMenu(fileName = "Color Palette", menuName = "Stack the Blocks/Color Palette", order = 0)]
public class ColorPalette : ScriptableObject
{
    /// <summary> List of colors </summary>
    public List<ColorBP> colors;

    /// <summary>
    ///  For some reason, By default the color's alpha is 0 when assigned in the inspector
    /// This is to fix all the colors alpha to the max(through the inspector). Use only if required.
    ///  </summary>
    public void FixColorAlpha()
    {
        for (int i = 0; i < colors.Count; i++)
        {
            colors[i].blockColor.a = 255;
            colors[i].TopColor.a = 255;
            colors[i].BottomColor.a = 255;
        }
    }
}

[System.Serializable]
public class ColorBP
{
    public Color blockColor;
    [Header("Background")]
    public Color TopColor;
    public Color BottomColor;
}


