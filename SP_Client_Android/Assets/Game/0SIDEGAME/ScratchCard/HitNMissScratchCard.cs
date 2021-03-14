using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class HitNMissScratchCard : MonoBehaviour {

    public GameObject objBoard;
    public GameObject objReSetBtn;

    public GameObject objStartPanel;


    private void Start()
    {
        //objReSetBtn.SetActive(false);
        objStartPanel.SetActive(true);
    }



    public void OnRandom()
    {
        objStartPanel.SetActive(false);
    }




    public void Reset()
    {
        SceneChanger.LoadScene("ScratchCard", objBoard);
    }

    public void GoHome()
    {
        SceneChanger.LoadScene("HitNMiss", objBoard);
    }

}
