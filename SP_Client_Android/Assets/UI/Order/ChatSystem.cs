using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatSystem : MonoBehaviour {


    public Queue<string> sentences;
    //public TextMeshPro text;

    public Text text;
    public string currentSentence;


    public void Ondialogue(string[] lines, Transform chatPoint)
    {
        transform.position = chatPoint.position;
        sentences = new Queue<string>();
        sentences.Clear();
        foreach(var line in lines)
        {
            sentences.Enqueue(line);
        }
        StartCoroutine(DialogueFlow(chatPoint));
    }


    IEnumerator DialogueFlow(Transform chatPoint)
    {
        yield return null;
        while(sentences.Count > 0)
        {
            currentSentence = sentences.Dequeue();
            text.text = currentSentence;
            transform.localPosition = new Vector3(chatPoint.position.x, chatPoint.position.y, chatPoint.position.z);
            yield return new WaitForSeconds(3f);
        }
        Destroy(gameObject);
    }

}
