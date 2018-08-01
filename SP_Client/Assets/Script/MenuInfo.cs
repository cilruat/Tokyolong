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
        Transform child = transform.Find("Desc");
        title = child.GetComponent<Text>();

        child = transform.Find("Price");
        price = child.GetComponent<Text>();

        this.data = MenuData.Get(menu);

        title.text = "" + data.menuName + "";
        price.text = data.price.ToString();
	}
}
