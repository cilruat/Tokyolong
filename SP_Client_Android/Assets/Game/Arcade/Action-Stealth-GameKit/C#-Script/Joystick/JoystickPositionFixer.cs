using UnityEngine;
using System.Collections;
namespace Stealth
{
public class JoystickPositionFixer : MonoBehaviour {

	public GameObject joystick;
	public GameObject jPad;

	void Awake (){
		//set joystick handle position on screen.
		joystick.transform.position = new Vector3(0.75f,
		                                          joystick.transform.position.y,
		                                          joystick.transform.position.z);
		jPad.transform.position = new Vector3(0.75f,
		                                      jPad.transform.position.y,
		                                      jPad.transform.position.z);
	}
}
}