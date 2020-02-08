using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace Bowling
{
public class Fbar : MonoBehaviour {
	public Image forcebar;
	public float force = 0;
	public float speedamount;
	void Start () {
	
	}
	
	void Update () {
			force += Input.GetAxis("Mouse Y")*speedamount*Time.deltaTime;
		forcebar.fillAmount = (force)/100;
		if (force > 100) { force = 100;}
		if (force < 0) { force = 0;}
	}
}
}