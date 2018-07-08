using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInfo : MonoBehaviour {

	public EMenuDetail eMenu;
	Text title;
	Text price;

	void Awake()
	{
        Transform child = transform.Find("Desc");
        title = child.GetComponent<Text>();

        child = transform.Find("Price");
        price = child.GetComponent<Text>();

        title.text = Info.MenuName (eMenu);
		price.text = Info.MakeMoneyString (Info.MenuPrice (eMenu));
	}
}
