using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShiningGraphic : MonoBehaviour {

	const float FADE_OUT_SPEED = 3.5f;
	const float STEP_GAP = .045f;

	float alpha = 1f;
	int step = 0;
	float elapsedStep = 0;

	MaskableGraphic graphic;

	static public void Start (MaskableGraphic owner) {

		GameObject obj = new GameObject ();
		obj.name = owner.name + " (Shining)";

		RectTransform rt = obj.AddComponent<RectTransform> ();
		RectTransform rtOwner = owner.rectTransform;

		rt.SetParent (owner.transform, false);
		rt.SetAsFirstSibling ();

		rt.anchorMin = Vector2.zero;
		rt.anchorMax = Vector2.one;
		rt.offsetMin = Vector2.zero;
		rt.offsetMax = Vector2.zero;
		ShiningGraphic _sg = obj.AddComponent<ShiningGraphic> ();

		if (owner.GetType () == typeof(Image)) {
			Image _src = (Image)owner;
			Image _dst = obj.AddComponent<Image> ();
			_dst.sprite = _src.sprite;
			_dst.type = _src.type;
			_sg.graphic = _dst;
		}
		else if (owner.GetType () == typeof(RawImage)) {
			RawImage _src = (RawImage)owner;
			RawImage _dst = obj.AddComponent<RawImage> ();
			_dst.texture = _src.texture;
			_dst.uvRect = _src.uvRect;
			_sg.graphic = _dst;
		}

		_sg.graphic.color = Color.white;
		_sg.graphic.material = new Material (Shader.Find ("UI/Shining"));
	}

	void Update () {
		if (step < 2) {
			elapsedStep += Time.unscaledDeltaTime;
			if (elapsedStep > STEP_GAP) {
				step++;
				elapsedStep = 0;
				if (step == 1)
					graphic.color = new Color (1f, 1f, 1f, 0);
				else
					graphic.color = new Color (1f, 1f, 1f, 1f);
			}				
			return;
		}

		alpha = Mathf.Max (0, alpha - FADE_OUT_SPEED * Time.unscaledDeltaTime);
		graphic.color = new Color (1f, 1f, 1f, alpha);

		if (alpha == 0)
			Destroy (gameObject);
	}
}
