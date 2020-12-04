using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
// LottoResult Close Button.
// Repeat Lotto!
/// </summary>
public class LottoResultWindow_Close : MonoBehaviour {
	void OnMouseClicked(){
        SceneManager.LoadScene("LuckGame");
	}

	void OnMouseRelease(){
		transform.localScale = new Vector3(1.5f,1.5f,1);
	}

	void OnMouseOver(){
		transform.localScale = new Vector3(1.6f,1.6f,1);
	}

	void OnMouseOut(){
		transform.localScale = new Vector3(1.5f,1.5f,1);
	}
}
