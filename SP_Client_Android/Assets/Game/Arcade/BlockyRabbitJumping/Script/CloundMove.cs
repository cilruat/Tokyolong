using UnityEngine;
using System.Collections;

namespace BlockJumpRabbit
{

public class CloundMove : MonoBehaviour {

	public float speed = 2.0f;

	void Update(){
		for(int i=0;i<transform.childCount;i++){
			transform.GetChild (i).Translate (-Vector2.right * speed * Time.deltaTime);
		}
	}
}
}