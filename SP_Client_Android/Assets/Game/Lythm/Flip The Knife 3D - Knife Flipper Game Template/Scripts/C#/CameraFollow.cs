using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnifeFlip
{
public class CameraFollow : MonoBehaviour {

	//This script needs to be attached to a Main Camera and you need to set a target for the camera to follow. You also need to set up the cord correctly from the inspector.

	public Vector3 myPos;
	public Transform myPlay;

	void  Update (){
		transform.position = myPlay.position + myPos;
	}
	}
}