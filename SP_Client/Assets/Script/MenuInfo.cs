using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInfo : MonoBehaviour {

	public int menu;
	Text title;
	Text price;

    MenuData data = null;

	void Awake()
	{
		this.data = MenuData.Get(menu);
		if (this.data == null)
			return;

		if (this.data.show == false) {
			gameObject.SetActive (false);
			return;
		}

        Transform child = transform.Find("Desc");
        title = child.GetComponent<Text>();

        child = transform.Find("Price");
        price = child.GetComponent<Text>();		       

        title.text = "" + data.menuName + "";
        price.text = data.price.ToString();
	}
}
