using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Gatcha : MonoBehaviour {



    public Text Text;
    public List<string> GachaList = new List<string>();

    public void Gacha()
    {
        for (int i = 0; i < 1; i++)
        {
            int rand = Random.Range(0, GachaList.Count);
            print(GachaList[rand]);
            Text.text = GachaList[rand].ToString();
            GachaList.RemoveAt(rand);
        }
    }
}
