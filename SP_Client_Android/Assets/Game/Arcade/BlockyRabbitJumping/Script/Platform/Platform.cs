using UnityEngine;
using System.Collections;

namespace BlockJumpRabbit
{
// Value as difficult level
public enum TypePlatform
{
    Empty = 0,
    Stable = 1,
    Turtle = 2,
    Crocodile = 3,
    Snake = 4,
    Hippopotamus = 5
}

[RequireComponent(typeof(BoxCollider2D))]
public class Platform : MonoBehaviour {

    public Animator animPlatform;
	
    public TypePlatform typePlatform;
	public ParticleSystem animEffectWater;

	private BoxCollider2D boxPlatform;

    void Awake(){
		boxPlatform = GetComponent<BoxCollider2D> ();

        SettupPlatform();
	}

    /// <summary>
    /// Hold all platform in same size
    /// </summary>
	void SettupPlatform(){
        boxPlatform.size = new Vector2(GameController.DISTANCE_OBJ, boxPlatform.size.y);
	}

    // Platform effect
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
            PlatformAction(other.gameObject);

		if (other.tag == "Wolf" && animEffectWater)
			animEffectWater.Play ();
    }

    /// <summary>
    /// Need overrrite this method
    /// </summary>
    public virtual void PlatformAction(GameObject player)
    {
		// Run effect hit water when jump hit platform
		if (animEffectWater)
			animEffectWater.Play ();

    }

	public virtual void Deactive(){
		gameObject.SetActive (false);
	}

    #region Editor
#if UNITY_EDITOR
    [ContextMenu("GetNamePlatform")]
    void SetNamePlatform()
    {
        switch (typePlatform)
        {
            case TypePlatform.Empty:
                gameObject.name = "PLATFORM_EMPTY";
                break;
            case TypePlatform.Crocodile:
                gameObject.name = "PLATFORM_SNAKE01";
                break;
            case TypePlatform.Snake:
                gameObject.name = "PLATFORM_SNAKE02";
                break;
            case TypePlatform.Hippopotamus:
                gameObject.name = "PLATFORM_HIPPOPOTAMUS";
                break;
            case TypePlatform.Turtle:
                gameObject.name = "PLATFORM_TURTLE";
                break;
            case TypePlatform.Stable:
                gameObject.name = "PLATFORM_STABLE";
                break;
        }
    }
#endif
#endregion

}
}