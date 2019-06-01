using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BlockJumpRabbit
{
public class BackgroundParallax : MonoBehaviour {

	public Transform[] layer01;
	public float ParallaxReductionFactor_layer01 = 50;

	public Transform[] layer02;
	public float ParallaxReductionFactor_layer02 = 40;

	public Transform[] layer03;
	public float ParallaxReductionFactor_layer03 = 30;

	public Transform[] layer04;
	public float ParallaxReductionFactor_layer04 = 20;

	public Transform[] layer05;
	public float ParallaxReductionFactor_layer05 = 10;

	public float ParallaxScale = 1;
	public float Smoothing = 0.5f;

	private Vector3 _lastPosition;
	private float parallax;

	private List<Vector3> backupLayer01;
	private List<Vector3> backupLayer02;
	private List<Vector3> backupLayer03;
	private List<Vector3> backupLayer04;
	private List<Vector3> backupLayer05;

	public void Start()
	{
		_lastPosition = transform.position;

		backupLayer01 = new List<Vector3> ();
		backupLayer02 = new List<Vector3> ();
		backupLayer03 = new List<Vector3> ();
		backupLayer04 = new List<Vector3> ();
		backupLayer05 = new List<Vector3> ();

		// Backup layer
		SaveArrayToList(backupLayer01,layer01);
		SaveArrayToList(backupLayer02,layer02);
		SaveArrayToList(backupLayer03,layer03);
		SaveArrayToList(backupLayer04,layer04);
		SaveArrayToList(backupLayer05,layer05);
	}

	//Fuction save postion to list
	void SaveArrayToList(List<Vector3> listPosSave,Transform[] layer){
		if(layer.Length> 0)
			foreach (Transform trans in layer)
				listPosSave.Add (trans.position);
	}

	void LoadListToArray(List<Vector3> listPosSave,Transform[] layer){
		if (listPosSave.Count > 0)
			for (int i = 0; i < listPosSave.Count; i++) {
				layer [i].position = listPosSave [i];
			}
	}

	// Reset when restart game
	public void Reset(){
		LoadListToArray (backupLayer01, layer01);
		LoadListToArray (backupLayer02, layer02);
		LoadListToArray (backupLayer03, layer03);
		LoadListToArray (backupLayer04, layer04);
		LoadListToArray (backupLayer05, layer05);
		_lastPosition = transform.position;
	}

	public void LateUpdate()
	{
		parallax = (_lastPosition.x - transform.position.x) * ParallaxScale;

		ParallaxLayerMove(layer01, ParallaxReductionFactor_layer01);
		ParallaxLayerMove(layer02, ParallaxReductionFactor_layer02);
		ParallaxLayerMove(layer03, ParallaxReductionFactor_layer03);
		ParallaxLayerMove(layer04, ParallaxReductionFactor_layer04);
		ParallaxLayerMove(layer05, ParallaxReductionFactor_layer05);

		_lastPosition = transform.position; 
	}

	void ParallaxLayerMove(Transform[] Layer,float ParallaxReductionFactor)
	{
		if (Layer.Length == 0)
			return;

		foreach (Transform layer in Layer)
		{
			var backgroundTargetPosition = layer.position.x + parallax * (ParallaxReductionFactor + 1);
			layer.position = Vector3.Lerp(
				layer.position, new Vector3(backgroundTargetPosition, layer.position.y, layer.position.z),
				Smoothing * Time.deltaTime);
		}
	}
	}
}