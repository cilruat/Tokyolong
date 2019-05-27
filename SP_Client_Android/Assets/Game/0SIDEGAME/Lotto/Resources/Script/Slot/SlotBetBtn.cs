using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
// Slot Betting Button
/// </summary>
public class SlotBetBtn : MonoBehaviour {
	public int _bet = 1;

	public SlotCylinder[] _cylinder;
	List<int> _slot = new List<int>();

	//information clear
	void Clear(){
		_slot.Clear();
	}

	//one line finish.
	//if called 3times, calculate result.
	void SlotFinished(int i){
		_slot.Add(i);
		if(_slot.Count == 3){
			int prev = -1;
			foreach(int type in _slot){
				if(prev == -1){
					prev = type;
					continue;
				}

				if( prev != type){
					return ;
				}

				prev = type;
			}

			int gold = (i+1) * 20 * _bet;
			PlayerMeta.incraseGold(gold);

			GameObject[] lights = GameObject.FindGameObjectsWithTag("SlotLight");
	        for (int _i=0;_i<lights.Length;_i++) {
	        	lights[_i].GetComponent<Animator>().Play("SlotLight");
	        }

		}
	}

	//Play Slot!!
	void OnMouseClicked(){
		GameObject[] lights = GameObject.FindGameObjectsWithTag("SlotLight");
        for (int i=0;i<lights.Length;i++) {
        	lights[i].GetComponent<Animator>().Play("Idle");
        }

		SlotBetBtn[] btns = FindObjectsOfType(typeof(SlotBetBtn)) as SlotBetBtn[];
        for (int i=0;i<btns.Length;i++) {
        	btns[i].Clear();
		}

		for(int i=0;i<_cylinder.Length;i++){
			_cylinder[i].Play(gameObject,i/3f);
		}

		PlayerMeta.decreaseGold(_bet);
	}

	void OnMouseRelease(){
		transform.localScale = new Vector3(1.5f,1.5f,1);
	}

	void OnMouseOver(){
		transform.localScale = new Vector3(1.6f,1.6f,1);
	}

	void OnMouseOut(){
		transform.localScale = new Vector3(1.5f,1.5f,1);
	}

}
