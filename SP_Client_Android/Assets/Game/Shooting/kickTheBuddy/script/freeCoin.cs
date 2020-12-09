using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;

namespace KickTheBuddy
{
    public class freeCoin : MonoBehaviour
    {
        public Text[] coinText;
        public string coinName = "coin";
        public int freeCoinCount;
        public GameObject[] freeCoinBtn;
        public Text[] freeCoinText;


        // Use this for initialization
        void Start()
        {//Setting up advertisements to get gold coins tips
            for (int i = 0; i < freeCoinBtn.Length; i++)
            {
                if (freeCoinText[i]) freeCoinText[i].text = "+" + freeCoinCount;
                //if(freeCoinBtn[i])freeCoinBtn[i].SetActive(AdManager.Instance.IsReadyReward()); //Judge whether advertising is successful or not.
            }
        }
        public void freeCoinCheck()
        {
            for (int i = 0; i < freeCoinBtn.Length; i++)
            {
                //if(freeCoinBtn[i])freeCoinBtn[i].SetActive(AdManager.Instance.IsReadyReward());  //Judge whether advertising is successful or not.
            }
        }
        // Update is called once per frame
        public void ShowRewardAD()
        {
            //Advertisements increase gold coins.
            int coinCount = PlayerPrefs.GetInt(coinName) + freeCoinCount;
            PlayerPrefs.SetInt(coinName, coinCount);

            for (int i = 0; i < coinText.Length; i++)
            {
                if (coinText[i])
                    coinText[i].text = "" + coinCount;
            }

            gameManage_charSelect gm = GameObject.FindGameObjectWithTag("gameManage").GetComponent<gameManage_charSelect>();
            if (gm)
            {
                gm.coinCount = coinCount;

            }
            else
            {
                gameManage gm2 = GameObject.FindGameObjectWithTag("gameManage").GetComponent<gameManage>();
                if (gm2)
                    gm2.coinCount = coinCount;

            }
        }

    }
}