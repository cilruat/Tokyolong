using UnityEngine;
using System.Collections;

namespace TwoCars
{
	public class SpawnManager : MonoBehaviour {

	    public GameObject[] blue;
	    public GameObject[] red;
	    public Transform obstacleParent;

	    Transform parent;

	    void Awake()
	    {
	        parent = GameObject.Find("Obstacles").transform;
	    }

	    public void StartSpawning()
	    {
	        StartCoroutine(SpawnRed());
	        StartCoroutine(SpawnBlue());
	    }     
	    
	    public void StopSpawning()
	    {
	        StopAllCoroutines();
	    }

	    IEnumerator SpawnRed()
	    {
	        yield return new WaitForSeconds(Random.Range(0f, 1f));

	        while (Managers.Game.isGameActive)
	        {
	            int type = Random.Range(0, 2);
	            int side = Random.Range(0, 2);
	            float posX;

	            if (side == 0) posX = Constants.LL;
	            else posX = Constants.LR;

	            GameObject go = Instantiate(red[type], new Vector3(posX,10.5f,0f),Quaternion.identity) as GameObject;
	            go.transform.SetParent(parent);

	            yield return new WaitForSeconds(Managers.Difficulty.spawnInterval);
	        }
	        yield break;
	    }

	    IEnumerator SpawnBlue()
	    {
	        yield return new WaitForSeconds(Random.Range(0f, 1f));

	        while (Managers.Game.isGameActive)
	        {
	            int type = Random.Range(0, 2);
	            int side = Random.Range(0, 2);
	            float posX;

	            if (side == 0) posX = Constants.RL;
	            else posX = Constants.RR;

	            GameObject go = Instantiate(blue[type], new Vector3(posX, 10.5f, 0f), Quaternion.identity) as GameObject;
	            go.transform.SetParent(parent);

	            yield return new WaitForSeconds(Managers.Difficulty.spawnInterval);
	        }

	        yield break;
	    }

	    public void ClearObstacles()
	    {
	        foreach (Transform t in obstacleParent)
	            Destroy(t.gameObject);
	    }
	}
}