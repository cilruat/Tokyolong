using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GameManager gameManager = (GameManager)target;

        if (GUILayout.Button("Increase Player Cash")){
            gameManager.IncreasePlayerCash();
        }
        if (GUILayout.Button("Decrease Player Cash"))
        {
            gameManager.DecreasePlayerCash();
        }
        if (GUILayout.Button("Clear Player Data"))
        {
            gameManager.ClearPlayerData();
        }
    }
}
