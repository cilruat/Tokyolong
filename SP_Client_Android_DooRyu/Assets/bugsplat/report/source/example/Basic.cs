using UnityEngine;
using System.Collections;
using BugSplat;


/// <summary>
/// Simple example of sending reports.
/// </summary>
public class Basic : MonoBehaviour 
{
    /// <summary>
    /// Reference to the reporter class.
    /// </summary>
	public Reporter reporter;

    /// <summary>
    /// Dummy cube.
    /// </summary>
    public Transform cube;
    
    /// <summary>
    /// CTOR.
    /// </summary>
    void Start() {

        reporter.Initialize(gameObject);
        reporter.SetCallback((success, message) =>
        {
            Debug.Log("BugSplat Report Posted: " + success);
            Debug.Log("BugSplat API Response: " + message);
        });
    
		reporter.prompt = false;
    }

    /// <summary>
    /// Animates the cube.
    /// </summary>
    void Update() {

        cube.transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * 90f,Vector3.one);        

		if (Input.GetKeyDown (KeyCode.A)) {			
			int[] i = new int[1];
			i [1] = 2;
		}
        
		if (Input.GetKeyDown (KeyCode.S)) {
			GameObject g = null;
			g.name = "Name";
		}
    }

	/// <summary>
    /// GUI callback.
    /// </summary>
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0,0,350f,500f));

        GUILayout.Label("BugSplat Report Exception Count ["+reporter.count+"] Ignored ["+reporter.ignored+"]");
        
        if(GUILayout.Button("Prompted Exception")) {
            System.Exception err = new System.Exception("Prompted Exception");
            reporter.prompt = true;
            throw err;            
        }

        if(GUILayout.Button("Custom Exception")) {                    
            System.Exception err = new System.Exception("Custom Exception");
            reporter.prompt = false;
            throw err;
        }

        if(GUILayout.Button("Null Reference Exception")) {            
            reporter.prompt = false;
            GameObject g = null;
            g.name = "Name";
        }
        
        GUILayout.EndArea();
    }
    
	
}
