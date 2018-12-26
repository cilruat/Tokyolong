using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class PlatformFragment : MonoBehaviour {


	public float lifetime = 5f;



	void FixedUpdate(){



		GetComponent<Rigidbody>().AddForce(new Vector3 (0, -50 * GetComponent<Rigidbody>().mass, 0));


		Destroy (this.gameObject, lifetime);

	}
}