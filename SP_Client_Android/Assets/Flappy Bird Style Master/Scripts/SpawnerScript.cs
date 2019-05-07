﻿using UnityEngine;
using System.Collections;

namespace FlappyBirdStyle
{
	public class SpawnerScript : MonoBehaviour
	{

	    // Use this for initialization
	    void Start()
	    {
	        SpawnObject = SpawnObjects[Random.Range(0, SpawnObjects.Length)];
	        Spawn();
	    }

	    void Spawn()
	    {
	        if (GameStateManager.GameState == GameState.Playing)
	        {
	            //random y position
	            float y = Random.Range(0.3f, 2.1f);
	            Instantiate(SpawnObject, this.transform.position + new Vector3(0, y, 0), Quaternion.identity);
	        }
	        Invoke("Spawn", Random.Range(timeMin, timeMax));
	    }

	    private GameObject SpawnObject;
	    public GameObject[] SpawnObjects;

	    public float timeMin = 0.7f;
	    public float timeMax = 2f;
	}
}