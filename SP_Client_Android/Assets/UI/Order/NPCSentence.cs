using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSentence : MonoBehaviour {


    public string[] sentences;
    public Transform chatTr;
    public GameObject chatBoxPrefab;

    private void Start()
    {
        Invoke("TalkNPC", 10f);
    }

    public void TalkNPC()
    {
        GameObject go = Instantiate(chatBoxPrefab);
        go.transform.SetParent(transform, false);
        go.GetComponent<ChatSystem>().Ondialogue(sentences, chatTr);
        Invoke("TalkNPC", 10f);
    }

    private void OnMouseDown()
    {
        TalkNPC();
    }



}
