using UnityEngine;
using System.Collections;

public class Float : MonoBehaviour
{
    //This script gives a floating effect to any object it's attached to. You can set how fast and how high
    //the object floats from the component inspector
	public GameObject oriobject;

	internal float OriginalPosition; //holds the original position of the object
    internal float Phase; //Holds the phase of the floating animation cycle. When giving it a random number we make different objects in the scene float a little differently.
    
    public float    Height  = 0.2f; //How high the object floats
    public int      Speed   = 5; //How fast the object floats

    //We set the original position of the object once at the start of the game
    void Start()
    {
        Phase = Random.value; //Set a random value for the floating phase
        OriginalPosition = transform.position.y; //The object's original position
    }

    //We animate the floating effect using a Sine function offset by Phase and multiplied by Speed
    void Update()
    {
        //Move the object in a Sine arc to give it a floating effect
		transform.position = new Vector3(transform.position.x, oriobject.transform.position.y + (Mathf.Sin((Time.time + Phase) * Speed)) * Height / 2, transform.position.z);
    }
}
