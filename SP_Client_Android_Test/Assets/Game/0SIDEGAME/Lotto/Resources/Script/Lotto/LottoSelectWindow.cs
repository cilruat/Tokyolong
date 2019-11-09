using UnityEngine;
using System.Collections;

/// <summary>
// Lotto Select Window.
/// </summary>
public class LottoSelectWindow : MonoBehaviour {
	// Use this for initialization
	void Awake() {
	}

    public void OnFinish(){
        Destroy(gameObject);
    }
}
