using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FlowGlowSlow : MonoBehaviour {

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

		flow = Mathf.Repeat (flow + Time.unscaledDeltaTime * speed, 5.0f);
		material.SetVector ("_Flow",
			new Vector4 (.5f - flow, -.5f + flow, bright, 0.8f)); 
	}
}