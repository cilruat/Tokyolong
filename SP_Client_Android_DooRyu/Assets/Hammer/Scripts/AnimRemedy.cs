using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hammer
{
	public class AnimRemedy : MonoBehaviour
	{

	    //public int id = 0;
	    public Animator _anim;
	    private void OnEnable()
	    {
	        _anim.SetTrigger("in");//id == 0 ? "mainMenu" : "gameOver");
	    }
	}
}