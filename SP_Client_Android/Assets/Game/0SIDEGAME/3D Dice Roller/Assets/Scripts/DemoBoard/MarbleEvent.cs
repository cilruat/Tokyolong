using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class MarbleEvent : MonoBehaviour {


    public List<GameObject> InShowPanelobj = new List<GameObject>();

    public Button btnDiceRoll;

    public GameObject objGoldeCard;
    public GameObject objGoldeCard2;


    public GameObject objBadCard;
    public GameObject objBadCard2;

    public List<string> GoldenCardList = new List<string>();
    public List<string> BadCardList = new List<string>();

    public Text GoldenCardText;
    public Text GoldenCardText2;

    public Text BadCardText;
    public Text BadCardText2;

    public Animator FlipGoldenCard;
    public Animator FlipGoldenCard2;
    public Animator FlipBadCard;
    public Animator FlipBadCard2;


    public void OnClosePopUp(int idx)
    {
        InShowPanelobj[idx].SetActive(false);

        //DiceManager dice = new DiceManager(); // 왜 안되는지 알때는 언제일까 2021.7.12
        btnDiceRoll.interactable = true;
    }

    public void OnClosePopUpStackSoju(int idx)
    {
        InShowPanelobj[idx].SetActive(false);
        PlayerToken StackSoju = new PlayerToken();
        StackSoju.stackSoju = 0;
        btnDiceRoll.interactable = true;

    }


    public void OnOpenGoldenCard()
    {
        GoldenCardText.text = "";
        FlipGoldenCard.Play("Flip");
        StartCoroutine(GoldenCard());
    }

    public void OnOpenGoldenCard2()
    {
        GoldenCardText2.text = "";
        FlipGoldenCard2.Play("Flip");
        StartCoroutine(GoldenCard2());
    }

    public void OnOpenBadCard()
    {
        BadCardText.text = "";
        FlipBadCard.Play("Flip");
        StartCoroutine(BadCard());
    }


    public void OnOpenBadCard2()
    {
        BadCardText2.text = "";
        FlipBadCard2.Play("Flip");
        StartCoroutine(BadCard2());
    }



    IEnumerator GoldenCard()
    {
        yield return new WaitForSeconds(0.4f);

        objGoldeCard.SetActive(true);
        for(int i = 0; i < 1; i++)
        {
            if(GoldenCardList.Count !=0)
            {
                int rand = Random.Range(0, GoldenCardList.Count);
                print(GoldenCardList[rand]);
                GoldenCardText.text = GoldenCardList[rand].ToString();

            }
        }
    }

    IEnumerator GoldenCard2()
    {
        yield return new WaitForSeconds(0.4f);

        objGoldeCard2.SetActive(true);
        for (int i = 0; i < 1; i++)
        {
            if (GoldenCardList.Count != 0)
            {
                int rand = Random.Range(0, GoldenCardList.Count);
                print(GoldenCardList[rand]);
                GoldenCardText2.text = GoldenCardList[rand].ToString();
            }
        }
    }

    IEnumerator BadCard()
    {
        yield return new WaitForSeconds(0.4f);

        objBadCard.SetActive(true);
        for (int i = 0; i < 1; i++)
        {
            if (BadCardList.Count != 0)
            {
                int rand = Random.Range(0, BadCardList.Count);
                print(BadCardList[rand]);
                BadCardText.text = BadCardList[rand].ToString();
            }
        }
    }

    IEnumerator BadCard2()
    {
        yield return new WaitForSeconds(0.4f);

        objBadCard2.SetActive(true);
        for (int i = 0; i < 1; i++)
        {
            if (BadCardList.Count != 0)
            {
                int rand = Random.Range(0, BadCardList.Count);
                print(BadCardList[rand]);
                BadCardText2.text = BadCardList[rand].ToString();
            }
        }
    }



    public void OnCloseGoldenCard(int idx)
    {
        InShowPanelobj[idx].SetActive(false);
        objGoldeCard.SetActive(false);
        btnDiceRoll.interactable = true;
    }


    public void OnCloseGoldenCard2(int idx)
    {
        InShowPanelobj[idx].SetActive(false);
        objGoldeCard2.SetActive(false);
        btnDiceRoll.interactable = true;
    }


    public void OnCloseBadCard(int idx)
    {
        objBadCard.SetActive(false);
        InShowPanelobj[idx].SetActive(false);
        btnDiceRoll.interactable = true;
    }

    public void OnCloseBadCard2(int idx)
    {
        objBadCard2.SetActive(false);
        InShowPanelobj[idx].SetActive(false);
        btnDiceRoll.interactable = true;
    }

}



