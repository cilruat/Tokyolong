using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiceManager : MonoBehaviour 
{
    public List<Dice> diceList;
    public int totalValue;
    public UnityEvent EndRollEvent;

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
            if(diceList[i].isRolling == true)
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
}