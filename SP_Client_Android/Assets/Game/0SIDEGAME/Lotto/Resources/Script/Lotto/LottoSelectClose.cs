using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
// Close Select Window and Play Lotto.
/// </summary>
public class LottoSelectClose : MonoBehaviour {

    public GameObject objCloseBtn;

    private void Start()
    {
        int GameCoin = Info.GamePlayCnt;

        PlayerMeta.RefreshGold(GameCoin);

        if (GameCoin < 1)
        {
            objCloseBtn.SetActive(true);
        }
        else
        {
            objCloseBtn.SetActive(false);
        }
    }

    //How many pick balls.
    int getToggleNum(){
		int num = 0;
		LottoSelectBall[] toggles = FindObjectsOfType(typeof(LottoSelectBall)) as LottoSelectBall[];
		foreach(LottoSelectBall toggle in toggles){
			if(toggle._toggle){
				num++;
			}
		}
		return num;
	}

	//pick balls list
	List<int> getToggle(){
		List<int> list = new List<int>();
		LottoSelectBall[] toggles = FindObjectsOfType(typeof(LottoSelectBall)) as LottoSelectBall[];
		foreach(LottoSelectBall toggle in toggles){
			if(toggle._toggle){
				list.Add(toggle.type);
			}
		}
		return list;
	}

    //if 3 picked balls, play Lotto
    void OnMouseClicked(){

            if (getToggleNum() == 3)
            {
                GameObject window = transform.parent.gameObject;
                Animator anim = window.GetComponent<Animator>();
                anim.Play("AlertDisappear");

                LottoPicker picker = FindObjectOfType(typeof(LottoPicker)) as LottoPicker;
                picker.Play(getToggle());
            }
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

    public void OnCloseScene()
    {
        SceneManager.LoadScene("LuckGame");
    }
}
