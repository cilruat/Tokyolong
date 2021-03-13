using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class HitNMissScratchCard : MonoBehaviour {

    public GameObject objBoard;
    public GameObject objReSetBtn;

    public GameObject objStartPanel;

    public List<Image> ImageList = new List<Image>();


    private void Start()
    {
        objReSetBtn.SetActive(false);
        objStartPanel.SetActive(true);
    }


    //startbutton OnClick
    public void OnRandom()
    {
        objStartPanel.SetActive(false);
        for (int i = 0; i < 1; i++)
        {
            if (ImageList.Count != 0)
            {
                int rand = Random.Range(0, ImageList.Count);
                print(ImageList[rand]);
                Image WithOneMouthImg = Instantiate(ImageList[i], gameObject.transform);
                WithOneMouthImg = ImageList[rand];
            }
            else
            {
                objReSetBtn.SetActive(true);
            }
        }
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
