using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColorPalette))]
public class ColorPaletteEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ColorPalette colorPalette = (ColorPalette)target;

        if (GUILayout.Button("Fix Colors Alpha"))
        {
            colorPalette.FixColorAlpha();
        }
      
    }
}
