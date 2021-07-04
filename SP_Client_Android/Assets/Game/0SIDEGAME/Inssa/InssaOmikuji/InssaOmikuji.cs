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
    public GameObject objResultPanel;

    //스타트 그거 Dottween 한번 써야겟네 혹은 애니메이션 스타트 해야겟노..

    public Text Text;
    public List<string> GachaList = new List<string>();

    //public GameObject objBlindPanel;

    public Text TextLuck;
    public List<string> LuckList = new List<string>();

    public List<GameObject> WoodTrayPanel;
    public List<TabButton> tabWoodTray;
    public List<GameObject> tabWoodTaryOpenImg;




    private void Start()
    {
        objReSetBtn.SetActive(false);
        objTouchPanel.SetActive(false);
        objWoodTrayPanel.SetActive(false);
        objResultPanel.SetActive(false);

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


    //주루마블 이거하면 안되냐?
    public void ClickWoodPanel(int id)
    {
        for(int i = 0; i < WoodTrayPanel.Count; i++)
        {
            if(i == id)
            {
                WoodTrayPanel[i].SetActive(true);
                tabWoodTray[i].Selected();
                tabWoodTaryOpenImg[i].SetActive(true);
            }
            else
            {
                WoodTrayPanel[i].SetActive(false);
                tabWoodTray[i].DeSelected();
                tabWoodTaryOpenImg[i].SetActive(false);
            }
        }
        objReSetBtn.SetActive(true);
        objResultPanel.SetActive(true);
    }


    // Text한계에 있기때문에 이 문제는 홀수 짝수, 24개니깐 6개의 패널로 나누어서 하자

    public void GachaLuckText()
    {
        //objTouchPanel.SetActive(false);
        TextLuck.text = "";

        StartCoroutine(ShowLuckText());
    }


    IEnumerator ShowLuckText()
    {
        Debug.Log("ShowLuckText");

        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 1; i++)
        {
            if (LuckList.Count != 0)
            {
                int rand = Random.Range(0, LuckList.Count);
                print(LuckList[rand]);
                TextLuck.text = LuckList[rand].ToString();
                LuckList.RemoveAt(rand);
            }
        }
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
