using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatSystem : MonoBehaviour {


    public Queue<string> sentences;
    public TextMeshPro text;
    public string currentSentence;


    public void Ondialogue(string[] lines)
    {
        sentences = new Queue<string>();
        sentences.Clear();
        foreach(var line in lines)
        {
            sentences.Enqueue(line);
        }
        StartCoroutine(DialogueFlow());
    }


    IEnumerator DialogueFlow()
    {
        yield return null;
        while(sentences.Count > 0)
        {
            currentSentence = sentences.Dequeue();
            text.text = currentSentence;
            yield return new WaitForSeconds(3f);
        }
        Destroy(gameObject);
    }

}
