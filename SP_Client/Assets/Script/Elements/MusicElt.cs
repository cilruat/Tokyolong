using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicElt : MonoBehaviour {

    public Text priority;
	public Text title;
	public Text singer;
	public Text table;

	int id = -1;

	public void SetInfo(int id, string packing)
	{
		this.id = id;

        if (priority != null)
            priority.text = string.Format("{00}", id.ToString());


	}

	public void OnDelete()
	{
		PageAdmin.Instance.RemoveElt (false, id);
	}

	public int GetID() { return id; }
}
