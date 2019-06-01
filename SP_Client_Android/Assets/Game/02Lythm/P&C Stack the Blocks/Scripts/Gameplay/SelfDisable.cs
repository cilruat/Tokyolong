using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For self disabling instantiated and pooled items
/// </summary>
public class SelfDisable : MonoBehaviour {

    [Tooltip("If required to be only disabled. left over blocks are required to be made kinematic after sometime.")]
    [SerializeField]
    private bool onlyDisable;

    [Tooltip("Will make the Gameobject's Rigidbody kinematic after this times")]
    [SerializeField]
    private int makeKinematicTime;

    [Tooltip("Will disable the Gameobject afer this time")]
    [SerializeField]
    private int disableTime;

    /// <summary>
    /// Invokes the makekinematic/ disable or both when gameobjects enable
    /// </summary>
    private void OnEnable()
    {
        if (!onlyDisable)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            //Making the leftover objects kinemtic because some fall on top of other blocks and directly disabling them doesn't look good
            Invoke("MakeKinematic", makeKinematicTime);
            Invoke("Disable", disableTime);

        }
        else
        {
            //Disabling them after these seconds. block must have moved out of frame by now.
            Invoke("Disable", disableTime);
        }
    }

    /// <summary>
    /// Makes the gameobject's rigidbody kinematic.
    /// </summary>
    void MakeKinematic () {
        GetComponent<Rigidbody>().isKinematic = true;
	}

    /// <summary>
    /// Disable this gameobject.
    /// </summary>
    void Disable(){
        gameObject.SetActive(false);
    }

    /// <summary>
    /// If case of force disable. Cancel invokes and make kinematic
    /// </summary>
    private void OnDisable()
    {
        //Cancel invokes as the object might be disabled by Object pooler before invoke executes
        CancelInvoke();
        if (!onlyDisable)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
