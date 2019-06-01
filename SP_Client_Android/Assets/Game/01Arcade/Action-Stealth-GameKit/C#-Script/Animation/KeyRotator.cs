using UnityEngine;
using System.Collections;
namespace Stealth
{
public class KeyRotator : MonoBehaviour {

	///*************************************************************************///
	/// A very simple rotation for objects that need player's attention.
	///*************************************************************************///

	private float rotationSpeed = 90;

	void Update (){
		transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
	}
}
}