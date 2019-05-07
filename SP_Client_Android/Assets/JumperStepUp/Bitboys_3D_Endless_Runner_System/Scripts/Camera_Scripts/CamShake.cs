using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JumperStepUp
{
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class CamShake : MonoBehaviour {

	[Header("Camera Shake")]
	// Use "shake.ShakeCamera (0.35f, 0.3f); " To call the shake function from other scripts

	private float shakeTimer; // The camera shake effect time
	private float shakeAmount; // The camera shake effect amount

	public void ShakeCamera (float shakePwr, float shakeDur)
	{
		shakeAmount = shakePwr;
		shakeTimer = shakeDur;

	}

	void Update(){

		if (shakeTimer >= 0) //If the camera shake movement has stopped.
		{
			Vector3 ShakePos = Random.insideUnitCircle * shakeAmount;
			transform.position = new Vector3 (transform.position.x + ShakePos.x, transform.position.y + ShakePos.y, transform.position.z + ShakePos.z);
			shakeTimer -= Time.deltaTime;
		}
	}
}
}