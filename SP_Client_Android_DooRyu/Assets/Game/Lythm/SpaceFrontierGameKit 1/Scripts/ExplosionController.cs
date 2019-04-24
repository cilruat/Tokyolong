using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceF
{
public class ExplosionController : MonoBehaviour {

	void Start () {
		Destroy (gameObject, 0.9f);
	}

}
}