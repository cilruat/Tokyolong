using UnityEngine;
using System.Collections;
namespace Stealth
{
public class FastParticleDestroyer : MonoBehaviour {
	
	void Start (){
		Destroy(gameObject, 0.2f);
	}
	}
}