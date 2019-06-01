using UnityEngine;
using System.Collections;
namespace Turnbase
{
public class InputFollow : MonoBehaviour {

	/// <summary>
	/// This object will always follows the player input (Mouse/Touch) position 
	/// and helps other controllers to calculate the distance and shoot power.
	/// </summary>

	void Start() {
		//starting position and important Y offset
		transform.position = new Vector3(transform.position.x,
		                                 1,
		                                 transform.position.z);
	}
	
	
	void Update() {
		Vector3 a = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
		                                                       Input.mousePosition.y, 
		                                                       10));
		transform.position = new Vector3(a.x, 1, a.z);
	}
	}
}