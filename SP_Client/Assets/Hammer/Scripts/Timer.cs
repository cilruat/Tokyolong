using System;
using UnityEngine;
using UnityEngine.UI;

namespace Hammer
{
	public class Timer : MonoBehaviour
	{
	    public bool run = false, showTimeLeft = false, pause = true;
	    public float startTime, endTime, curTime, timeAvailable = 20;
	    public Image image;
	    public int gameID = 0;
	    public void RunTimer(float t = 10.0f)
	    {
	        displayed = false;
	        run = true;
	        pause = false;
	        timeAvailable = t;
	        image.fillAmount = 0.5f;
	        startTime = Time.time;
	    }
	    public void EndTimer()
	    {
	        pause = true;
	    }

	    public void ResetTimer()
	    {
	        image.fillAmount = 0.5f;
	    }
	    [HideInInspector]
	    public bool displayed;
	    private void Update()
	    {
	        if (pause)
	        {
	            return;
	        }

	        if (run)
	        {
	            curTime = Time.time - startTime;
	        }
	        else
	        {
	            curTime = endTime - startTime;
	        }

	        float showTime = curTime;

	        if (showTimeLeft)
	        {
	            showTime = timeAvailable - curTime;
	            if (showTime <= 0) // TimeEnd
	            {
	                showTime = 0;
	                if (!displayed)
	                {
	                    displayed = true;
	                    pause = true;
	                    UIManager.Instance.GameOver();
	                }
	            }
	        }
	        image.fillAmount = ((showTime % 60) / 20f);
	    }

	    public void IncrementTimer()
	    {
	        timeAvailable += 0.4f;
	    }
	}
}