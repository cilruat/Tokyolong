using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace KickTheBuddy
{
    public class gameManage_charSelect : MonoBehaviour
    {
        public Text coinText;
        public Text pieceText;
        public Text succesText;
        public GameObject[] partsObj;
        public Sprite[] partsIcon;
        public int[] pieceCount;
        public Text btnBuyText;
        public GameObject noMoneyUI;
        private int partsNum;
        [HideInInspector]
        public int coinCount;
        private freeCoin freeCoinScript;
        // Use this for initialization
        void Start()
        {
            if (PlayerPrefs.GetInt("coin") == 0)
                PlayerPrefs.SetInt("coin", 500);
            freeCoinScript = GameObject.FindGameObjectWithTag("gameManage").GetComponent<freeCoin>();

            coinCount = PlayerPrefs.GetInt("coin");
            coinText.text = "" + coinCount;

            check();

        }

        public void btnPrevious()
        {
            freeCoinScript.freeCoinCheck();
            partsNum -= 1;

            if (partsNum < 0)
                partsNum = partsObj.Length - 1;

            check();
        }
        public void btnNext()
        {
            freeCoinScript.freeCoinCheck();
            partsNum += 1;
            if (partsNum > partsObj.Length - 1)
                partsNum = 0;
            check();
        }
        public void btnPlay()
        {
            SceneManager.LoadScene("game");

        }
        public void btnBuy()
        { //Spare parts purchase
            if (PlayerPrefs.GetInt("parts" + partsNum) == 1)
            {
                //PlayerPrefs.SetInt ("partsNum",partsNum );

            }
            else
            {
                if (coinCount >= pieceCount[partsNum])
                {
                    coinCount -= pieceCount[partsNum];
                    PlayerPrefs.SetInt("coin", coinCount);
                    coinText.text = "" + coinCount;

                    PlayerPrefs.SetInt("parts" + partsNum, 1);
                    succesText.text = "Purchase Success";
                    succesText.gameObject.SetActive(false);
                    succesText.gameObject.SetActive(true);

                    pieceText.gameObject.SetActive(false);
                    btnBuyText.text = "Owned";

                }
                else
                {
                    noMoneyUI.SetActive(true);
                    freeCoinScript.freeCoinCheck();
                }
            }

        }
        void check()
        { //Accessories display
            for (int i = 0; i < partsObj.Length; i++)
            {
                if (PlayerPrefs.GetInt("parts" + i) == 1)
                {
                    partsObj[i].SetActive(true);
                    //} else if (PlayerPrefs.GetInt ("parts" + i) == 2) {
                    //	partsObj [i].SetActive (false);
                }
                else if (PlayerPrefs.GetInt("parts" + i) == 0)
                {
                    partsObj[i].SetActive(false);
                }
            }
            partsObj[partsNum].SetActive(true);
            pieceText.transform.parent.Find("Image").gameObject.GetComponent<Image>().sprite = partsIcon[partsNum];
            pieceText.text = "" + pieceCount[partsNum];
            int num = PlayerPrefs.GetInt("parts" + partsNum);
            if (num == 1)
            {
                pieceText.gameObject.SetActive(false);
                btnBuyText.text = "Owned";
            }
            else
            {
                pieceText.gameObject.SetActive(true);
                btnBuyText.text = "Buy";
            }
        }



    }
}