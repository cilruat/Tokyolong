using UnityEngine;
using System.Collections;

/// <summary>
// Pachinko Result Window
/// </summary>
public class PinBall_ResultWIndow : MonoBehaviour {
    public TextMesh title;
    public TextMesh total;
    public TextMesh box1;
    public TextMesh box2;
    public TextMesh box3;
    public TextMesh box4;
    public TextMesh box5;
    
	// Use this for initialization
	void Awake() {
		ButtonEvent closeBtn = transform.Find("Close").gameObject.GetComponent<ButtonEvent>();
        total = transform.Find("Total").gameObject.GetComponent<TextMesh>();
        box1 = transform.Find("box/box1").gameObject.GetComponent<TextMesh>();
        box2 = transform.Find("box/box2").gameObject.GetComponent<TextMesh>();
        box3 = transform.Find("box/box3").gameObject.GetComponent<TextMesh>();
        box4 = transform.Find("box/box4").gameObject.GetComponent<TextMesh>();
        box5 = transform.Find("box/box5").gameObject.GetComponent<TextMesh>();

		ButtonEvent[] btns = FindObjectsOfType(typeof(ButtonEvent)) as ButtonEvent[];
        for (int i=0;i<btns.Length;i++) {
        	if(closeBtn == btns[i]){
        		btns[i].enable = true;
                Debug.Log("button enable");
        	}else{
            	btns[i].enable = false;
                Debug.Log("button disable");
            }
        }
	}

    //update Result Window Information!
    public int result(int[] r){
        box1.text = "x"+r[0];
        box2.text = "x"+r[1];
        box3.text = "x"+r[2];
        box4.text = "x"+r[3];
        box5.text = "x"+r[4];

        int getCoin = (int)(r[4] * 10 + r[3] * 3 + r[2] * 1 + r[1] * 0.5 + r[0] * 0);
        total.text = "+"+getCoin;

        return getCoin;
    }

    // close animation finished.
    public void OnFinish(){
        ButtonEvent[] btns = FindObjectsOfType(typeof(ButtonEvent)) as ButtonEvent[];
        for (int i=0;i<btns.Length;i++) {
            btns[i].enable = true;
        }

        Destroy(gameObject);
    }
}
