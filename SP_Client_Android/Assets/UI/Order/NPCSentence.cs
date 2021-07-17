using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSentence : MonoBehaviour {


    public string[] sentences;
    public Transform chatTr;
    public GameObject chatBoxPrefab;

    private void Start()
    {
        
    }

    public void TalkNPC()
    {
        GameObject go = Instantiate(chatBoxPrefab);
        go.GetComponent<ChatSystem>().Ondialogue(sentences);
    }

    private void OnMouseDown()
    {
        TalkNPC();
    }



}
