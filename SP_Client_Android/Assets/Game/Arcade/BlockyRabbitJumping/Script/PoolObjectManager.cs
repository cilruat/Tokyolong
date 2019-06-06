using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BlockJumpRabbit
{
[Serializable]
public struct DataPool
{
    public TypePlatform typePlatform;
    public List<GameObject> objPools;

    public DataPool(TypePlatform typePlatform,List<GameObject> objPools)
    {
        this.typePlatform = typePlatform;
        this.objPools = objPools;
    }
}

public class PoolObjectManager : MonoBehaviour {

    public List<Platform> prefabsObjPlatform;
    public int countEachObj = 5;

    private List<DataPool> poolList;

    void Awake()
    {
        poolList = new List<DataPool>();

        SpawnObjectPool();
    }


	public void OnAfterDeserialize()
	{
		
	}

    /// <summary>
    /// Spawn object pool and set to poolList
    /// </summary>
    void SpawnObjectPool()
    {
        foreach(Platform pla in prefabsObjPlatform)
        {
            DataPool? addDataPool = GetDataPool(pla.typePlatform);

            if (addDataPool == null)
            {
                DataPool bufferDataPool = new DataPool(pla.typePlatform, new List<GameObject>());
                for (int i = 0; i < countEachObj; i++)
                {
                    GameObject obj = Instantiate(pla.gameObject) as GameObject;
                    obj.SetActive(false);                                                                       // Deactive all obj

                    bufferDataPool.objPools.Add(obj);
                }
                poolList.Add(bufferDataPool);
            }
            else
            {
                for (int i = 0; i < countEachObj; i++)
                {
                    GameObject obj = Instantiate(pla.gameObject) as GameObject;
                    obj.SetActive(false);                                                                       // Deactive all obj
                    addDataPool.Value.objPools.Add(obj);
                }
            }
        }
    }

    /// <summary>
    /// Check type platform, we need add to same data pool if same type plaform
    /// </summary>
    DataPool? GetDataPool(TypePlatform typePlaform)
    {
        foreach (DataPool data in poolList)
            if (data.typePlatform == typePlaform)
                return data;

        return null;
    }

	// Return object with type Platform
    public GameObject GetPoolObj(TypePlatform typePlatform)
    {
		foreach (DataPool data in poolList)
			if (typePlatform == data.typePlatform) {
				if (data.objPools.Count > countEachObj) {
					#region Random In List Object pool same type
					int rand = (int)(UnityEngine.Random.Range (0, data.objPools.Count)) / countEachObj;

					for (int i = rand * countEachObj; i < rand * countEachObj + countEachObj; i++) {
						if (!data.objPools [i].activeSelf) {
							data.objPools [i].SetActive (true);
							return data.objPools [i];
						}
					}
					#endregion
				} else {
					foreach(GameObject obj in data.objPools) {
						if(!obj.activeSelf)
						{
							obj.SetActive (true);
							return obj;
						}
					}
				}
			}

		Debug.Log ("Type: " + typePlatform + "out range!");

        return null;
    }
	}
}
