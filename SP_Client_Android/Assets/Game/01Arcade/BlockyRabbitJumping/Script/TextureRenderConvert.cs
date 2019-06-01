using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace BlockJumpRabbit
{
public class TextureRenderConvert : MonoBehaviour
{
	public Camera realCamera;
	public Camera thisCamera;

	// Animator will active when effecr finish
	public Animator animMainUI;
	public Animator anim;

	public Image image;
	public GameObject UILoading;

	CanvasScaler canvasScaler;
	RenderTexture thisRenderTexture;
	float percentage;

	void Awake(){

		canvasScaler = FindObjectOfType<CanvasScaler> ();
		percentage = (float)Screen.height / (float)Screen.width;

		thisRenderTexture =  new RenderTexture((int) canvasScaler.referenceResolution.x,(int) (canvasScaler.referenceResolution.x * percentage), 16, RenderTextureFormat.ARGB32);
		thisCamera.targetTexture = thisRenderTexture;
	}

//	void Update(){
//		if(Input.GetMouseButtonDown(0)) {
//			//realCamera.gameObject.SetActive (false);	
//			SaveTexture ();
//		}
//	}

	public void SaveTexture(){
		RenderTexture.active = thisRenderTexture;

		int width = (int) canvasScaler.referenceResolution.x;
		int height = (int)(canvasScaler.referenceResolution.x * percentage);

		Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		tex.Apply();
		RenderTexture.active = null;

		image.enabled = true;
		image.sprite = Sprite.Create (tex, new Rect (0, 0,tex.width,tex.height), new Vector2(0.0f,0.0f));
		image.SetNativeSize ();

		//StartCoroutine(SaveTextureToFile(tex));
		anim.SetTrigger ("run");
	}

	public void Deactive(){		
		thisCamera.gameObject.SetActive (false);
		animMainUI.enabled = true;
		UILoading.SetActive (false);
	}

//    IEnumerator SaveTextureToFile(Texture2D savedTexture)
//    {
//        string fullPath = Application.dataPath;
//        System.DateTime date = System.DateTime.Now;
//        string fileName = "/CanvasTexture.png";
//        if (!System.IO.Directory.Exists(fullPath))
//            System.IO.Directory.CreateDirectory(fullPath);
//        var bytes = savedTexture.EncodeToPNG();
//        System.IO.File.WriteAllBytes(fullPath + fileName, bytes);
//        yield return null;
//    }
}
}
