using UnityEngine;
using System.Collections;
// Sets how long to wait before destroying an object.
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class DestroyAfterSeconds : MonoBehaviour
{
	public float lifetime; // Time to wait before destroy the object.
	
	void Start ()
	{
		Destroy (gameObject, lifetime);
	}
}
