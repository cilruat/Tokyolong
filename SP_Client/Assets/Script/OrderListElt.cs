using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderListElt : MonoBehaviour {

	public Text textDesc;
	public Image imgCheck;

	// test code
	List<string> listOrder = new List<string>();
	List<string> listDesc = new List<string>();

	public void SetInfo()
	{
		listOrder.Add ("앞접시");
		listOrder.Add ("숟가락");
		listOrder.Add ("젓가락");
		listOrder.Add ("물컵");
		listOrder.Add ("소주잔");
		listOrder.Add ("맥주잔");

		string make_order = "";
		int order_cnt = Random.Range (1, 4);
		for (int i = 0; i < order_cnt; i++)
		{
			int rand = Random.Range (0, listOrder.Count);
			make_order += listOrder [rand] + " x " + Random.Range(1, 11).ToString();

			if (i < order_cnt - 1)
				make_order += ", ";
		}

		textDesc.text = make_order;

		Debug.Log (order_cnt);
	}

	public void Check()
	{
		imgCheck.enabled = !imgCheck.enabled;
	}
}