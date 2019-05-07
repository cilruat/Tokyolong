using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGTScoreEffect : MonoBehaviour {

    public Color[] effectColor;
    public Text text;
    private Color textColor;

    public int ScoreValue { get; set; }

    //private Animator animator;

    void Awake()
    {
        //animator = GetComponent<Animator>();
    }

    void Start ()
    {
        
        textColor = effectColor[Random.Range(0, effectColor.Length)];
        text.color = textColor;
        Destroy(gameObject, 1.2f);
	}
	
    public void SetScoreValue(int scoreValue)
    {
        text.text = "+" + scoreValue;
    }
}
