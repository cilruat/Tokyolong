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

    public GameObject BlindPanel;
    public GameObject TurnPanel;
    public Text TurnText;
    public Animator MoveToPanel;

    public Text PanelTurnText;

    private int Turn;


    private void Start()
    {
        BlindPanel.SetActive(false);
        Turn = 1;
        Debug.Log(DiceRollMain.PeopleNum+ "넘어온사람수");
    }



    IEnumerator OffBlindPanel()
    {


        
        if (Turn < DiceRollMain.PeopleNum)
        {
            Turn++;
            Debug.Log(Turn);
        }
        else if(Turn == DiceRollMain.PeopleNum)
        {
            Turn = 1;
            Debug.Log("같을때입니다");
        }

        BlindPanel.SetActive(true);
        MoveToPanel.Play("MoveTurnPanel");
        TurnText.text = Turn.ToString();
        PanelTurnText.text = Turn.ToString();
        yield return new WaitForSeconds(1f);
        BlindPanel.SetActive(false);
    }

    public void OnClosePopUp(int idx)
    {
        InShowPanelobj[idx].SetActive(false);

        StartCoroutine(OffBlindPanel());


        //DiceManager dice = new DiceManager(); // 왜 안되는지 알때는 언제일까 2021.7.12
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
        StartCoroutine(OffBlindPanel());

    }


    public void OnCloseGoldenCard2(int idx)
    {
        InShowPanelobj[idx].SetActive(false);
        objGoldeCard2.SetActive(false);
        btnDiceRoll.interactable = true;
        StartCoroutine(OffBlindPanel());

    }


    public void OnCloseBadCard(int idx)
    {
        objBadCard.SetActive(false);
        InShowPanelobj[idx].SetActive(false);
        btnDiceRoll.interactable = true;
        StartCoroutine(OffBlindPanel());

    }

    public void OnCloseBadCard2(int idx)
    {
        objBadCard2.SetActive(false);
        InShowPanelobj[idx].SetActive(false);
        btnDiceRoll.interactable = true;
        StartCoroutine(OffBlindPanel());

    }


    public void OnClosePopUpStackSoju(int idx)
    {
        InShowPanelobj[idx].SetActive(false);
        PlayerToken StackSoju = new PlayerToken();
        StackSoju.ClearSoju();
        btnDiceRoll.interactable = true;
        StartCoroutine(OffBlindPanel());

    }



}



