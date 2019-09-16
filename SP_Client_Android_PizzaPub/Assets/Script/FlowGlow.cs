using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowGlow : MonoBehaviour {

	public float speed = 1f;
	public float bright = 1f;
	public float delay = 0f;

	Material material;

	float flow = 0;
	float timeToStart = 0f;

	void Start () {
		MaskableGraphic img = GetComponent<MaskableGraphic> ();
		material = Instantiate (img.material) as Material;
		img.material = material;

		timeToStart = Time.timeSinceLevelLoad;
	}

	void Update () {
		if (Time.timeSinceLevelLoad - timeToStart <= delay)
			return;

		flow = Mathf.Repeat (flow + Time.unscaledDeltaTime * speed, 1.6f);
		material.SetVector ("_Flow",
			new Vector4 (.8f - flow, -.8f + flow, bright, 1f)); 
	}
}
