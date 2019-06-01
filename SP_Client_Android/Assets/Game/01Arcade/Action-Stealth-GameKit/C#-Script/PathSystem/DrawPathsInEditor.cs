using UnityEngine;
using System.Collections;
namespace Stealth
{
public class DrawPathsInEditor : MonoBehaviour {
		
	///*************************************************************************///
	/// This class will render paths between waypoints inside editor.
	///*************************************************************************///

	public Transform[] waypoints;

	// draws green line from waypoint to waypoint
	private Vector3 pos;
	private Vector3 prev;

	void OnDrawGizmos (){
	    if(waypoints != null) {
		    Gizmos.color = Color.red;
		    for(int i = 0; i < waypoints.Length; i++) {
		      	pos = waypoints[i].position;
		        if(i > 0) {
		            prev = waypoints[i-1].position;
		            Gizmos.color = Color.green;
		            Gizmos.DrawLine(prev,pos);
		        }
		    }
		}
	}
}
}