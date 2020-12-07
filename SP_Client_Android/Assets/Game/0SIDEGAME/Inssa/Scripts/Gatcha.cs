using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Gatcha : MonoBehaviour {

public GameObject objBoard;
public Text Text;
public GameObject objReSetBtn;
public GameObject FingerClap;




    public List<string> GachaList = new List<string>();


    private void Start()
    {
        objReSetBtn.SetActive(false);
        FingerClap.SetActive(true);

    }


    public void Gacha()
    {
        for (int i = 0; i < 1; i++)
        {
            if(GachaList.Count != 0)
            {
            int rand = Random.Range(0, GachaList.Count);
            print(GachaList[rand]);
            Text.text = GachaList[rand].ToString();
            GachaList.RemoveAt(rand);
            }
            else
            {
                objReSetBtn.SetActive(true);
                FingerClap.SetActive(false);
            }
        }
    }

    public void Reset()
    {
        SceneChanger.LoadScene("InssaFingerClap", objBoard);
    }

    public void GoHome()
    {
        SceneChanger.LoadScene("InSsa", objBoard);
    }

}
