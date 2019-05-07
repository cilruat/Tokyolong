using UnityEngine;
using System.Collections;

namespace BlockJumpRabbit
{
public class DebugPoolObjManager : MonoBehaviour {

	// Test get obj platform same type in list obj pool
	[ContextMenu("Get Random Platform")]
	void GetRandomPlatform(){
		print( GetComponent<PoolObjectManager> ().GetPoolObj (TypePlatform.Empty).name);
	}

}
}