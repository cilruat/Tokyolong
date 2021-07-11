using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class MarbleEvent : MonoBehaviour {


    public List<GameObject> InShowPanelobj = new List<GameObject>();

    public Button btnDiceRoll;



    public void OnClosePopUp(int idx)
    {
        InShowPanelobj[idx].SetActive(false);

        //DiceManager dice = new DiceManager(); // 왜 안되는지 알때는 언제일까 2021.7.12
        btnDiceRoll.interactable = true;
    }
    
}



