using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
// Back button. go to title.
/// </summary>
public class BackButton : MonoBehaviour {


	void OnMouseClicked()
    {
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
