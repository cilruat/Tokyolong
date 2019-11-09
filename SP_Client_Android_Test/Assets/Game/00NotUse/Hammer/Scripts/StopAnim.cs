using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hammer
{
	public class StopAnim : MonoBehaviour
	{

	    public void EndAnim()
	    {
	        gameObject.SetActive(false);
	    }


	    public void RemoveBlock()
	    {
	        GameManager.Instance.RemoveBlock();
	    }

	    public void AnimEndGameOver()
	    {
	        AdsManager.Instance.ShowInterstitial();
	    }
	}
}