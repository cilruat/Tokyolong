using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ScrollManager
{
	public class NestedScrollManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler

	{



		void Start () 
		{
		
		}
	

		public void OnBeginDrag(PointerEventData eventData)
		{
			print ("드래그" + eventData.position);
		}

		public void OnDrag(PointerEventData eventData)
		{
		}

		public void OnEndDrag(PointerEventData eventData)
		{
		}







		void Update () 
		{
		
		}
	}
}