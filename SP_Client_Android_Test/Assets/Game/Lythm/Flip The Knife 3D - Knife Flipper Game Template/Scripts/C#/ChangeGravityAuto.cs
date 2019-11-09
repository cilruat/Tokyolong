using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnifeFlip
{
public class ChangeGravityAuto : MonoBehaviour {

		void Start() {
			Physics.gravity = new Vector3(0, -3.5F, 0);
		}
	}
}