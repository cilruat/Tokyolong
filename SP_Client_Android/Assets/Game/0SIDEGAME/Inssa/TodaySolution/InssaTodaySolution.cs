using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InssaTodaySolution : MonoBehaviour
{

    public GameObject objBoard;
    public GameObject objReSetBtn;
    public GameObject objTextTouchMe;
    public GameObject PanelGatcha;
    public Text Text;
    public GameObject objExplainBox;

    public GameObject objBookCloseBtn;

    public GameObject objOpenBookPanel;
    public GameObject objCloseBookPanel;

    public GameObject objBlindPanel;

    public List<string> GachaList = new List<string>();


    private void Start()
    {
        objReSetBtn.SetActive(false);
        objTextTouchMe.SetActive(false);
        objExplainBox.SetActive(true);
        objBookCloseBtn.SetActive(false);
        PanelGatcha.SetActive(false);
        objOpenBookPanel.SetActive(true);
        objCloseBookPanel.SetActive(false);
        objBlindPanel.SetActive(false);
    }


    public void ExplainBox()
    {
        objExplainBox.SetActive(false);
        objOpenBookPanel.SetActive(true);
        objTextTouchMe.SetActive(true);
    }


    public void Gacha()
    {
        objTextTouchMe.SetActive(false);
        Text.text = "";

        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        Debug.Log("ShowText");
        objBlindPanel.SetActive(true);

        yield return new WaitForSeconds(5.5f);
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
        PanelGatcha.SetActive(true);
        yield return new WaitForSeconds(1f);
        objBookCloseBtn.SetActive(true);
    }


    //타로에서 쓰이는 랜덤뽑기도 이곳에서 사용합니다, 시간이 다르기때문에 함수를 한개 추가합니다
    public void TaroOtherGacha()
    {
        objTextTouchMe.SetActive(false);
        Text.text = "";
        StartCoroutine(ShowTaroCard());
    }

    IEnumerator ShowTaroCard()
    {
        Debug.Log("ShowTaroCard");
        objBlindPanel.SetActive(true);

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
        PanelGatcha.SetActive(true);
        yield return new WaitForSeconds(1f);
        objBookCloseBtn.SetActive(true);
    }



    public void ActiveBookCloseBtn()
    {
        objBookCloseBtn.SetActive(false);
        objBlindPanel.SetActive(false);
        objCloseBookPanel.SetActive(true);
        objOpenBookPanel.SetActive(false);
        PanelGatcha.SetActive(false);
        StartCoroutine(RefreshOpen());
    }



    IEnumerator RefreshOpen()
    {
        yield return new WaitForSeconds(2f);
        objOpenBookPanel.SetActive(true);
        objCloseBookPanel.SetActive(false);
    }


    public void Reset()
    {
        SceneChanger.LoadScene("InssaTodaySolution", objBoard);
    }

    public void GoHome()
    {
        SceneChanger.LoadScene("InSsa", objBoard);
    }


}

