using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class InssaOmikuji : MonoBehaviour {

    public GameObject objBoard;
    public GameObject objReSetBtn;
    public GameObject objExplainPanel;
    public GameObject objTouchPanel;
    public GameObject objWoodTrayPanel;

    //스타트 그거 Dottween 한번 써야겟네 혹은 애니메이션 스타트 해야겟노..

    public Text Text;

    //public GameObject objBlindPanel;

    public List<string> GachaList = new List<string>();


    private void Start()
    {
        objReSetBtn.SetActive(false);
        objTouchPanel.SetActive(false);
        objWoodTrayPanel.SetActive(false);
    }


    public void ExplainPanel()
    {
        objExplainPanel.SetActive(false);
        objTouchPanel.SetActive(true);
    }


    public void Gacha()
    {
        //objTouchPanel.SetActive(false);
        Text.text = "";

        StartCoroutine(ShowText());
    }


    IEnumerator ShowText()
    {
        Debug.Log("ShowText");
        //objBlindPanel.SetActive(true);

        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 1; i++)
        {
            if (GachaList.Count != 0)
            {
                int rand = Random.Range(0, GachaList.Count);
                print(GachaList[rand]);
                Text.text = GachaList[rand].ToString();
                GachaList.RemoveAt(rand);
            }
            else
            {
                objReSetBtn.SetActive(true);
            }
        }
        objTouchPanel.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        objWoodTrayPanel.SetActive(true);
        DOTween.PlayAll();

    }

    public void GoHome()
    {
        SceneChanger.LoadScene("InSsa", objBoard);
    }

    public void Reset()
    {
        SceneChanger.LoadScene("InssaOmikuji", objBoard);

    }

}
