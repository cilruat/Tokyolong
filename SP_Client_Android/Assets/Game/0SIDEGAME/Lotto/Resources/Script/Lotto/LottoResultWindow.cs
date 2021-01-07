using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
// Lotto Result Window.
/// </summary>
public class LottoResultWindow : MonoBehaviour {
	SpriteRenderer[] _m= new SpriteRenderer[3];
	SpriteRenderer[] _s= new SpriteRenderer[3];
	TextMesh _gold;

	//initialize variables!
	void Awake(){
		_m[0] = transform.Find("MyPic/ball1").gameObject.GetComponent<SpriteRenderer>();
		_m[1] = transform.Find("MyPic/ball2").gameObject.GetComponent<SpriteRenderer>();
		_m[2] = transform.Find("MyPic/ball3").gameObject.GetComponent<SpriteRenderer>();

		_s[0] = transform.Find("Selected/ball1").gameObject.GetComponent<SpriteRenderer>();
		_s[1] = transform.Find("Selected/ball2").gameObject.GetComponent<SpriteRenderer>();
		_s[2] = transform.Find("Selected/ball3").gameObject.GetComponent<SpriteRenderer>();

		_gold = transform.Find("gold/text").gameObject.GetComponent<TextMesh>();
	}

	// how many matching ball?
	int getGrade(List<int> _my, List<int> _lottery){
		int count = 0;
		foreach(int v1 in _my){
			foreach(int v2 in _lottery){
				if(v1 == v2){
					count++;
					break;
				}
			}
		}
		return count;
	}

	//result window update information. and incrase gold
	public void result(List<int> _my, List<int> _lottery){
		int i;

		i=0;
		foreach(int v in _my){
			_m[i].sprite = Resources.Load<Sprite>("Texture/Lotto_"+v); i++;
		}

		i=0;
		foreach(int v in _lottery){
			_s[i].sprite = Resources.Load<Sprite>("Texture/Lotto_"+v); i++;
		}

		int grade = getGrade(_my,_lottery);
		if(grade == 3){
			_gold.text = "+ "+50;
			PlayerMeta.incraseGold(50);
		}else if(grade == 2){
			_gold.text = "+ " + 15;
			PlayerMeta.incraseGold(15);
		}else if(grade == 1){
			_gold.text = "+ " + 3;
			PlayerMeta.incraseGold(3);
		}else{
			_gold.text = "+ 0";
		}
	}
	
	// close animation finish
    public void OnFinish(){
        Destroy(gameObject);
    }
}
