using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// YOU BETTER RUN By BITBOYS STUDIO.
public class BurstRotateUI : MonoBehaviour { // This simple script manages the rotation of the UI burst that appears below the new best badge when the player gets a new best score.

	[Header("UI New Best Rotation Burst Rays")]

	private float spinSpeed = 50f; // The rotation speed

	void Update () {

		this.transform.Rotate (0, 0, - spinSpeed * Time.deltaTime);
	}
}
