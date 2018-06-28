using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageShowOrder : SingletonMonobehaviour<PageShowOrder> {

	public Text textTableNo;
	public Text textDesc;
	public List<ShowOrderElt> listElt = new List<ShowOrderElt>();

	bool _animating = false;
	List<KeyValuePair<int, int>> _listAddOrder = new List<KeyValuePair<int, int>> (); // key : order num, value : table num
	List<int> _listRemoveOrder = new List<int>();

	void Awake()
	{
		_refresh_total_order_cnt ();
	}

	void Update()
	{
		_check_remove_order ();
	}		

	void _check_remove_order()
	{
		if (_listRemoveOrder.Count == 0)
			return;

		if (_animating)
			return;

		_remove_ani (_listRemoveOrder [0]);
	}		

	void _remove_ani(int _order_num)
	{		
		_animating = true;

		Vector3 _prev_pos = Vector3.zero;
		Vector3 _last_pos = listElt [listElt.Count - 1].transform.localPosition;

		ShowOrderElt[] objCopy = new ShowOrderElt[listElt.Count];			

		int _remove_idx = -1;
		bool _visit = false;
		UITween _move_elt = null;
		for (int i = 0; i < listElt.Count; i++)
		{
			if (listElt[i].GetOrderNum() == _order_num)
			{
				_visit = true;
				_remove_idx = i;
				_prev_pos = listElt [i].transform.localPosition;

				_move_elt = UITweenAlpha.Start (listElt [i].gameObject, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
				objCopy[objCopy.Length-1] = listElt [i];
			}
			else 
			{
				if (_visit) 
				{
					UITweenPosition.Start (listElt [i].gameObject, listElt [i].transform.localPosition, _prev_pos,
						TWParam.New (.3f).Curve (TWCurve.CurveLevel2), false);

					_prev_pos = listElt [i].transform.localPosition;
					objCopy [i - 1] = listElt [i];
				} 
				else 
					objCopy [i] = listElt [i];
			}
		}

		if (_move_elt != null)
			_move_elt.AddCallback (() => _move_elt_to_end (_remove_idx, _order_num, _last_pos, objCopy));

		for (int i = 0; i < _listAddOrder.Count; i++) 
		{
			if (_listAddOrder [i].Key == _order_num) 
			{
				_listAddOrder.RemoveAt (i);
				break;
			}
		}

		_refresh_total_order_cnt ();
	}

	void _move_elt_to_end(int _remove_idx, int _order_num, Vector3 _last_pos, ShowOrderElt[] objCopy)
	{
		if (_remove_idx == -1)
			return;

		listElt [_remove_idx].SetPosition (_last_pos);
			
		listElt.Clear ();
		for (int i = 0; i < objCopy.Length; i++) 
		{			
			listElt.Add (objCopy [i]);
			listElt [i].transform.SetSiblingIndex (objCopy.Length - i);
		}
			
		_set_next_order ();

		_listRemoveOrder.Remove (_order_num);
		_animating = false;
	}

	void _set_next_order()
	{		
		if (_listAddOrder.Count < listElt.Count)
			return;

		int _order_num = _listAddOrder [listElt.Count-1].Key;
		int _table_num = _listAddOrder [listElt.Count-1].Value;
		listElt [listElt.Count - 1].RefreshInfo (_order_num, _table_num);
	}

	void _refresh_total_order_cnt()
	{
		textDesc.text = "주문 대기   <size='50'>" + _listAddOrder.Count.ToString() + "개</size>";
	}		

	IEnumerator _call_waiter(int tableNo)
	{
		textTableNo.text = "TABLE <size='100'>" + tableNo.ToString () + "</size>";

		textTableNo.gameObject.SetActive (true);
		textDesc.gameObject.SetActive (false);

		yield return new WaitForSeconds (4.3f);

		textTableNo.gameObject.SetActive (false);
		textDesc.gameObject.SetActive (true);
	}

	public void AddOrder(int orderNo, int tableNo)
	{
		int _hide_idx = -1;
		for (int i = 0; i < listElt.Count; i++) 
		{
			if (listElt [i].CheckAlpha () == false)
			{
				_hide_idx = i;
				break;
			}
		}

		if (_hide_idx != -1) 
			listElt [_hide_idx].RefreshInfo (orderNo, tableNo);
		
		_listAddOrder.Add (new KeyValuePair<int, int> (orderNo, tableNo));
		_refresh_total_order_cnt ();
	}

	public void RemoveOrder(int orderNum)
	{
		bool find = false;
		for (int i = 0; i < _listAddOrder.Count; i++) 
		{
			if (_listAddOrder [i].Key != orderNum)
				continue;
				
			find = true;
			break;
		}			

		if (_listRemoveOrder.Contains (orderNum) || find == false)
			return;
		
		_listRemoveOrder.Add (orderNum);
	}

	public void CallWaiter(int tableNo)
	{	
		StopAllCoroutines();
		StartCoroutine (_call_waiter(tableNo));
	}		
}