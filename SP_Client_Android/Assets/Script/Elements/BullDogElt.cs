using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BullDogElt : SingletonMonobehaviour<BullDogElt>
{
    public int teeth;

    public GameObject FullState;
    public GameObject ClickState;




    public void OnAct(int teeth)
    {
        FullState.SetActive(false);
        ClickState.SetActive(true);
    }



    public void OnCaught()
    {
        //애니메이션 스타트
        // 애니메이션 스타트하는거 보여주는 NOT 보내야함

    }

}
