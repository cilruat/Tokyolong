using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Bridges
{
	public class PathController : MonoBehaviour
	{
	    [HideInInspector]
	    public bool isTurnPath;
	    private GameManager gameManager;
	    private List<Vector3> listRotation = new List<Vector3>();
	    private int turn;

	    public void GetFirstRotation()
	    {
	        listRotation.Clear();
	        turn = 1;
	        //Get rotation 
	        for (int i = 0; i < transform.childCount; i++)
	        {
	            if (transform.GetChild(i).CompareTag("Bridge"))
	                listRotation.Add(transform.GetChild(i).transform.eulerAngles);
	        }
	    }

		
	    //Reset rotation and turn variable
	    public void ResetRotation()
	    {
	        int listRotationIndex = -1;
	        for (int i = 0; i < transform.childCount; i++)
	        {
	            if (transform.GetChild(i).CompareTag("Bridge"))
	            {
	                listRotationIndex += 1;
	                transform.GetChild(i).eulerAngles = listRotation[listRotationIndex];
	            }
	                
	        }
	        turn = 1;
	    }

	    //Rotate all bridges
	    public void RotateBridges()
	    {
	        turn = turn * (-1);

	        for (int i = 0; i < transform.childCount; i++)
	        {
	            if (transform.GetChild(i).CompareTag("Bridge"))
	            {
	                StartCoroutine(DoRotate(transform.GetChild(i).gameObject, turn));
	            }
	        }
	    }

	    //Rotate bridge
	    IEnumerator DoRotate(GameObject bridge, int turn)
	    {
	        if (gameManager == null)
	            gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

	        gameManager.playerController.disableCheckGameOver = true;
	        Vector3 startRot = bridge.transform.eulerAngles;
	        Vector3 endRot = (turn < 0) ?
	                        startRot + new Vector3(0, 90, 0) :
	                        startRot + new Vector3(0, -90, 0);

	        float t = 0;
	        while (t < gameManager.rotateBridgeTime)
	        {
	            t += Time.deltaTime;
	            float fraction = t / gameManager.rotateBridgeTime;
	            bridge.transform.eulerAngles = Vector3.Lerp(startRot, endRot, fraction);
	            yield return null;
	        }

	        gameManager.playerController.touchDisable = false;
	        gameManager.playerController.disableCheckGameOver = false;
	    }
	}
}