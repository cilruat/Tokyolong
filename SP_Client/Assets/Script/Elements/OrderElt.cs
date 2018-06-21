using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderElt : MonoBehaviour {

	public Text table;
	public Text order;

	int id = -1;

	public void SetInfo(int id, string packing)
	{
		this.id = id;
	}

	public void OnDetail()
	{
	}

	public int GetID() { return id; }
}
