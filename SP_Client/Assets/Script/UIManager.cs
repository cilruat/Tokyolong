using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum eUI
{
	eChat,
	eTableSetting,
	eBillDetail,
	eBillSending,
	eMusicRequest,
	eWaiting,
}

public class UIManager : SingletonMonobehaviour<UIManager> {

	[System.Serializable]
	public class UI
	{
		public eUI ui;
		public GameObject obj;
	}

	public GameObject objShadow;
	public List<UI> listUI;

	Dictionary<eUI, GameObject> dicObject = new Dictionary<eUI, GameObject> ();

	void Awake () 
	{
		collect();
//		Hide_All ();

        DontDestroyOnLoad(this);
	}

	void collect()
	{
		for (int i = 0; i < listUI.Count; i++) 
			dicObject.Add (listUI [i].ui, listUI [i].obj);
	}		

	public GameObject Show(eUI page)
	{
		objShadow.SetActive (true);
		dicObject [page].SetActive (true);
		return dicObject [page];
	}

	public void Hide(eUI page)
	{	
		objShadow.SetActive (false);	
		dicObject [page].SetActive (false);
	}
		
	public void Hide_All()
	{
		foreach (KeyValuePair<eUI, GameObject> pair in dicObject)
			pair.Value.SetActive (false);
	}

	public bool IsActive(eUI page)
	{
		return dicObject [page].activeSelf;
	}		
}
