using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class MarbleEvent : MonoBehaviour {


    public List<GameObject> InShowPanelobj = new List<GameObject>();

    public Button btnDiceRoll;

    public GameObject objGoldeCard;
    public GameObject objBadCard;

    public List<string> GoldenCardList = new List<string>();
    public List<string> BadCardList = new List<string>();

    public Text GoldenCardText;
    public Text BadCardText;

    public Animator FlipGoldenCard;


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


    //카드가 등장하게된다
    //애니메이터 달고
    public void OnOpenGoldenCard()
    {
        //애니메이션 후 카드가 시작된다
        GoldenCardText.text = "";
        FlipGoldenCard.Play("Flip");
        StartCoroutine(GoldenCard());

    }


    IEnumerator GoldenCard()
    {
        yield return new WaitForSeconds(0.4f);

        objGoldeCard.SetActive(true);

    }



    public void OnCloseGoldenCard(int idx)
    {
        objGoldeCard.SetActive(false);
        InShowPanelobj[idx].SetActive(false);
        btnDiceRoll.interactable = true;



    }

    public void OnCloseBadCard(int idx)
    {
        objBadCard.SetActive(false);
        InShowPanelobj[idx].SetActive(false);
        btnDiceRoll.interactable = true;



    }


}



