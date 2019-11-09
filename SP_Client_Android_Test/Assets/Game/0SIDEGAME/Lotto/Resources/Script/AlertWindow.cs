using UnityEngine;
using System.Collections;

/// <summary>
// Alert Window.
// instancing with Instantiate function. and modify text. ex) alert.text.text = "";
/// </summary>
public class AlertWindow : MonoBehaviour {
    public TextMesh title;
    public TextMesh text;
	// Use this for initialization
	void Awake() {
		ButtonEvent closeBtn = transform.Find("Close").gameObject.GetComponent<ButtonEvent>();
        title = transform.Find("Title").gameObject.GetComponent<TextMesh>();
        text = transform.Find("Text").gameObject.GetComponent<TextMesh>();

		ButtonEvent[] btns = FindObjectsOfType(typeof(ButtonEvent)) as ButtonEvent[];
        for (int i=0;i<btns.Length;i++) {
        	if(closeBtn == btns[i]){
        		btns[i].enable = true;
        	}else{
            	btns[i].enable = false;
            }
        }
	}

    public void OnFinish(){
        ButtonEvent[] btns = FindObjectsOfType(typeof(ButtonEvent)) as ButtonEvent[];
        for (int i=0;i<btns.Length;i++) {
            btns[i].enable = true;
        }

        Destroy(gameObject);
    }
}
