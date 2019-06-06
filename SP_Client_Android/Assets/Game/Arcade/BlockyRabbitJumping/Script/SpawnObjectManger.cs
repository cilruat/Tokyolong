using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlockJumpRabbit
{
[RequireComponent(typeof(PoolObjectManager))]
public class SpawnObjectManger : MonoBehaviour,ISerializationCallbackReceiver {
    
	PoolObjectManager poolObjManager;

	#region Dictionary Serilizable
	public List<TypePlatform> listType;
	public List<int> listPrior;
	public Dictionary<TypePlatform,int> DictionaryPercentageType;
    public AIController aiController;

	private List<TypePlatform> listType_backup;
	private List<int> listPrior_backup;

	private int TotalPriorList;

	public void OnBeforeSerialize(){}

	public void OnAfterDeserialize()
	{
		DictionaryPercentageType = new Dictionary<TypePlatform, int> ();

		if(listType.Count < listPrior.Count) {
			listPrior.RemoveRange(listType.Count, listPrior.Count - listType.Count);
		}else if(listType.Count > listPrior.Count) {
			while(listType.Count != listPrior.Count) {
				listPrior.Add (1);
			}
		}

		List<int> newPriorList = ValidatePriorList (listPrior);												// Validate

		for(int i=0;i<listType.Count;i++) {
			if(!DictionaryPercentageType.ContainsKey(listType[i]))
				DictionaryPercentageType.Add (listType [i], newPriorList [i]);
		}
	}

	/// <summary>
	/// Validates the prior list to use algorithm random
	/// </summary>
	List<int> ValidatePriorList(List<int> priorList) {

		List<int> newPriorList = new List<int> ();

		newPriorList.Add (priorList [0]);
		for(int i=1;i<priorList.Count;i++) {
			newPriorList.Add (priorList [i] + newPriorList [i - 1]);
		}

		TotalPriorList = newPriorList [newPriorList.Count - 1];

		#if UNITY_EDITOR
		// Debug list
		DebugList (newPriorList);
		#endif

		return newPriorList;
	}

    /// <summary>
    /// This method use for add new element to dictionary in run time game
    /// </summary>
    public void AddElementToDictionary(KeyValuePair<TypePlatform, int> newElement)
    {
        listType.Add(newElement.Key);
        listPrior.Add(newElement.Value);

        OnAfterDeserialize();
    }
	#endregion

	private Queue<GameObject> queuePlatformSpawned;
	private float currentDistanceSpawn;
	private Platform lastPlatformAddQueue;

	private int minQuantifyObjMaintain;

	private Vector2 firstQueuePostion;

	private float realWidthScreen;

	private const float constYAxisPlatform = -0.93f;
    private bool stopSpawn;

	void Awake(){
		poolObjManager = GetComponent<PoolObjectManager> ();
		queuePlatformSpawned = new Queue<GameObject> ();
		currentDistanceSpawn = 0.0f;

        listType_backup = new List<TypePlatform>();
        listPrior_backup = new List<int>();

        stopSpawn = true;

        #region Backup
        foreach (TypePlatform typePlatform in listType)
            listType_backup.Add(typePlatform);


        foreach (int num in listPrior)
            listPrior_backup.Add(num);
        #endregion

        CaculateNumberObjectMaintain();
		
	}

    void Start()
    {
       // InitalSpawnObj();
    }

	/// <summary>
	/// Postion (DISTANCE_OBJECT,x)
	/// </summary>
	public void InitalSpawnObj(){

        for (int i=0;i<minQuantifyObjMaintain;i++) {
			SpawnPlatformRandom ();
		}

		// Get first postion element in queue
		firstQueuePostion = queuePlatformSpawned.Peek ().transform.position;

        aiController.IntialAIChasePlayer(new Vector2(((int)(-minQuantifyObjMaintain /2) + 1)* GameController.DISTANCE_OBJ * 2 , constYAxisPlatform));

        stopSpawn = false;
    }

	/// <summary>
	/// Different screen size need maintain defferent number object active.
	/// </summary>
	void CaculateNumberObjectMaintain(){
		realWidthScreen = Screen.width * Camera.main.orthographicSize * 2 / Screen.height;

		minQuantifyObjMaintain = (int) (realWidthScreen / GameController.DISTANCE_OBJ) + 4;
	}

	void Update(){

        if (!stopSpawn)
        {
            if (queuePlatformSpawned.Count < minQuantifyObjMaintain)
                SpawnPlatformRandom();

            DeactivePlatformInQueue();
        }
	}

	/// <summary>
	/// Process spawn platform
	/// </summary>
	void SpawnPlatformRandom(bool isSnake = false){
		currentDistanceSpawn += GameController.DISTANCE_OBJ;
		GameObject obj;
			
		if (isSnake)
			obj = poolObjManager.GetPoolObj (TypePlatform.Empty);
		else {
			if (lastPlatformAddQueue == null || lastPlatformAddQueue.typePlatform != TypePlatform.Empty)
				obj = poolObjManager.GetPoolObj (GetTypeRandomWithPriorList (false).Value);
			else
				obj = poolObjManager.GetPoolObj (GetTypeRandomWithPriorList (true).Value);
		}

		obj.transform.position = new Vector2 (currentDistanceSpawn, constYAxisPlatform);
		queuePlatformSpawned.Enqueue (obj);
		lastPlatformAddQueue = obj.GetComponent<Platform> ();
        aiController.EnQueueAIPath(lastPlatformAddQueue);

        if(lastPlatformAddQueue.typePlatform == TypePlatform.Snake)
        {
			SpawnPlatformRandom(true);
            currentDistanceSpawn += GameController.DISTANCE_OBJ;
        }
	}

	void DeactivePlatformInQueue(){
		if (firstQueuePostion.x < Camera.main.transform.position.x - realWidthScreen / 2 - GameController.DISTANCE_OBJ*2) {
			GameObject objRemove = queuePlatformSpawned.Dequeue ();
			objRemove.GetComponent<Platform> ().Deactive ();
			firstQueuePostion = queuePlatformSpawned.Peek ().transform.position;			// When dequeue, we need get first postion element in queue
		}	
	}

	public TypePlatform? GetTypeRandomWithPriorList(bool lastEmpty = false){
		
		int rand;

		if (lastEmpty)
			rand = UnityEngine.Random.Range (listPrior [0], TotalPriorList);
		else
			rand = UnityEngine.Random.Range (0, TotalPriorList);

		foreach(TypePlatform typePla in DictionaryPercentageType.Keys) {
			if (DictionaryPercentageType[typePla] > rand)
				return typePla;
		}

		return null;
	}

	public static TypePlatform RandomPlatformType(){
		System.Array arr = System.Enum.GetValues (typeof(TypePlatform));
		return (TypePlatform) arr.GetValue (UnityEngine.Random.Range (0, arr.Length));
	}

    #region LevelDifficult
    public void Level2ChangePriorList()
    {
        listPrior[(int)(TypePlatform.Empty)] += 4;
        listPrior[(int)(TypePlatform.Crocodile)]++;
        listPrior[(int)(TypePlatform.Turtle)]++;
        OnAfterDeserialize();
        aiController.AISpeedChase = 1.5f;
    }

    public void Level3ChangePriorList()
    {
        listPrior[(int)(TypePlatform.Empty)] += 8;
        listPrior[(int)(TypePlatform.Crocodile)]++;
        listPrior[(int)(TypePlatform.Turtle)]++;
        OnAfterDeserialize();

        aiController.AISpeedChase = 1.2f;
    }

	public void Level5ChangePriorList()
	{
		listPrior[(int)(TypePlatform.Empty)] += 18;
		listPrior[(int)(TypePlatform.Crocodile)] += 3;
		listPrior [(int)(TypePlatform.Turtle)] += 1;
		OnAfterDeserialize();

		aiController.AISpeedChase = 1.0f;
	}


    #region GameEvent

    public void StopSpawn()
    {
        stopSpawn = true;
    }
    public void ResetSpawn()
    {
        while(queuePlatformSpawned.Count > 0)
        {
            GameObject obj = queuePlatformSpawned.Dequeue();

			obj.GetComponent<Platform> ().Deactive ();
        }

        currentDistanceSpawn = 0.0f;
        queuePlatformSpawned.Clear();

        #region Restore

        listType.Clear();
        listPrior.Clear();

        foreach (TypePlatform typePlatform in listType_backup)
            listType.Add(typePlatform);


        foreach (int num in listPrior_backup)
            listPrior.Add(num);
        #endregion

        OnAfterDeserialize();

        stopSpawn = false;

        InitalSpawnObj();
        
    }
    #endregion

    #endregion
    #region Debug
#if UNITY_EDITOR

    [ContextMenu("GetRandomType")]
	void DebugGetRandom(){
		Debug.Log("DEBUG GET RANDOM TYPE:---"+GetTypeRandomWithPriorList ().ToString()+"---") ;
	}

	[ContextMenu("Add typePlatform to Dictionary")]
	void DebugAddTypePlatformToDictionary(){
		AddElementToDictionary(new KeyValuePair<TypePlatform, int>(TypePlatform.Snake,2));
	}
		
	void DebugList(List<int> list) {
		StringBuilder builder = new StringBuilder ();
		foreach(int num in list) {
			builder.Append (num).Append (",");
		}
		//Debug.Log ("DEBUG VALIDATE LIST PRIOR:---" + builder.ToString () + "---!!");
	}

	#endif
	#endregion

}
}