using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceF
{
public class QuickDestroy : MonoBehaviour {

	void Start () {
		Destroy (gameObject, 1.25f);
	}

}
}