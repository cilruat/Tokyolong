using UnityEngine;
using System.Collections;

namespace Emoji2
{
	public class CharacterManager : MonoBehaviour
	{
	    public static CharacterManager Instance;

	    public Sprite character;
	    [HideInInspector]	
	    public int index;

	    void Awake()
	    {
	        if (Instance)
	        {
	            DestroyImmediate(gameObject);
	        }
	        else
	        {
	            Instance = this;
	        }
	    }

	}
}