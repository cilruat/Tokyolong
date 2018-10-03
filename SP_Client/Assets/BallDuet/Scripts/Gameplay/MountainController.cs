using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnefallGames
{
	public class MountainController : MonoBehaviour {


	    private SpriteRenderer spRender = null;

	    private void OnEnable()
	    {
	        StartCoroutine(WaitAndSetParameters());
	    }

	    private void Update()
	    {
	        if (GameManager.Instance.GameState == GameState.Playing)
	        {
	            //float x = Camera.main.WorldToViewportPoint(transform.position).x;
				float cameraX = Camera.main.transform.position.x;
				float dist = cameraX - transform.position.x;
				if (dist > 50f)
	                gameObject.SetActive(false);
	        }
	    }


	    private IEnumerator WaitAndSetParameters()
	    {
	        yield return null;
	        if (spRender == null)
	            spRender = GetComponent<SpriteRenderer>();
	        spRender.color = GameManager.Instance.CurrentMountainColor;
	    }




	    /// <summary>
	    /// Changing color form the current color to the new color
	    /// </summary>
	    /// <param name="newColor"></param>
	    /// <param name="lerpTime"></param>
	    public void ChangeColor(Color newColor, float lerpTime)
	    {
	        StartCoroutine(ChangingColor(newColor, lerpTime));
	    }

	    private IEnumerator ChangingColor(Color newColor, float lerpTime)
	    {
	        Color startColor = spRender.color;
	        float t = 0;
	        while (t < lerpTime)
	        {
	            t += Time.deltaTime;
	            float factor = t / lerpTime;
	            spRender.color = Color.Lerp(startColor, newColor, factor);
	            yield return null;
	        }
	    }
	}
}