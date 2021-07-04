﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class DiceManager : MonoBehaviour
{
    public List<Dice> diceList;
    public int totalValue;
    public UnityEvent EndRollEvent;
    public GameObject objBoard;

    //추가부분//
    public List<GameObject> WoodTrayPanel;
    //public List<TabButton> tabWoodTray;
    //public List<GameObject> tabWoodTaryOpenImg;



    TileClass Tilenum = new TileClass();

    
    
    public void RollAllDie()
    {
        totalValue = 0;
        //reset total value

        for (int i = 0; i < diceList.Count; i++)
        {
            diceList[i].AddForceToDice();
        }
    }
    public void CountAllDieValues()
    {
        for (int i = 0; i < diceList.Count; i++)
        {
            if (diceList[i].isRolling == true)
            {
                totalValue = 0;
                return;
            }
            else
            {
                totalValue += diceList[i].value;
            }
        }
        EndRollEvent.Invoke();
    }

    // 복불복 게임에서 쓸것

    public void RollAllDiceMain()
    {
        totalValue = 0;
        //reset total value

        for (int i = 0; i < diceList.Count; i++)
        {
            diceList[i].AddForceDiceMain();
        }
    }



    public void OnGoHome(string SceneName)
    {
        SceneChanger.LoadScene(SceneName, objBoard);
    }




}
