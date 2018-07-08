using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInfo : MonoBehaviour {

	public EMenuDetail eMenu;
	public Text title;
	public Text price;

	void Awake()
	{		
        title.text = Info.MenuName (eMenu);
		price.text = Info.MakeMoneyString (Info.MenuPrice (eMenu));
	}
}
