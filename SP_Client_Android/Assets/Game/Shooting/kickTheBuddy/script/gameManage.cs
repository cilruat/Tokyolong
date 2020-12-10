using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace KickTheBuddy
{
    public class gameManage : MonoBehaviour
    {
        public GameObject[] charObj;
        public GameObject[] atkObj;
        //public string[] atkObjName;
        public int[] atkObjPiece;
        public Sprite[] wqImage;
        public Transform[] autoFirePoint;
        public GameObject btn;

        public float autoFireCD;
        public Text coinText;
        [HideInInspector]
        public int coinCount;
        public float saveCD = 5;
        public GameObject noMoneyUI;
        public GameObject getAtkObjUI;
        public GameObject[] partsObj;
        private Animator coinAni;
        private float timeTemp;
        private Animator aniHead;
        // Use this for initialization
        void Start()
        {

            aniHead = GameObject.FindGameObjectWithTag("head").GetComponent<Animator>();
            for (int i = 0; i < charObj.Length; i++)
            {
                charObj[i].SetActive(false);
            }
            int charNum = PlayerPrefs.GetInt("charNum");
            charObj[charNum].SetActive(true);

            //btn.SetActive (false);
            coinCount = PlayerPrefs.GetInt("coin");
            coinText.text = "" + coinCount;
            this.GetComponent<control>().atkObj = atkObj;
            //Create a weapon button
            for (int i = 0; i < atkObj.Length; i++)
            {
                GameObject btnObj = Instantiate(btn);
                btnObj.transform.SetParent(btn.transform.parent);
                btnObj.transform.localScale = new Vector3(1, 1, 1);
                btnObj.transform.Find("btnAtkObj").gameObject.GetComponent<btncontrol>().atkObjNum = i;
                btnObj.transform.Find("btnUnlock").gameObject.GetComponent<btncontrol>().atkObjNum = i;
                btnObj.transform.Find("btnUnlock").gameObject.GetComponent<btncontrol>().unlockPiece = atkObjPiece[i];
                btnObj.transform.Find("btnAutoFire").gameObject.GetComponent<btncontrol>().atkObjNum = i;
                btnObj.transform.Find("btnAutoFire").gameObject.GetComponent<btncontrol>().autoFireCD = autoFireCD;
                //btnObj.transform.Find ("btnAtkObj/name").gameObject.GetComponent<Text > ().text = "" + atkObjName [i];
                btnObj.transform.Find("btnAtkObj/Image").gameObject.GetComponent<Image>().sprite = wqImage[i];
                btnObj.SetActive(true);
            }
            //parts
            for (int i = 0; i < partsObj.Length; i++)
            {
                if (PlayerPrefs.GetInt("parts" + i) == 1)
                {
                    partsObj[i].SetActive(true);
                }
                else if (PlayerPrefs.GetInt("parts" + i) == 0)
                {
                    partsObj[i].SetActive(false);
                }
            }
            coinAni = coinText.transform.parent.gameObject.GetComponent<Animator>();
        }

        // Update is called once per frame

        void FixedUpdate()
        {//Record data on time
            if (Time.time > timeTemp)
            {
                timeTemp = Time.time + saveCD;
                PlayerPrefs.SetInt("coin", coinCount);
            }
        }
        public void coinAdd(int coinCountAdd)
        {//Gold coin acquisition
            aniHead.SetTrigger("hit");
            coinCount += coinCountAdd;
            coinText.text = "" + coinCount;
            coinAni.SetTrigger("play");

            int coins = Mathf.FloorToInt(coinCountAdd / 5) + 1;
            for (int i = 0; i < coins; i++)
            {
                Instantiate(Resources.Load("FX/coin") as GameObject, GameObject.FindGameObjectWithTag("playerPoint").transform.position, Quaternion.identity);
            }
        }
        public void btnAtkObj(int num)
        {
            this.GetComponent<control>().objNum = num;

        }
        public void btnCharSelect()
        {

            SceneManager.LoadScene("ArcadeGame");
        }
        private bool openDO;
        public void btnAutoFire(int num)
        {//Automatic fire settings
            openDO = false;
            for (int i = 0; i < autoFirePoint.Length; i++)
            {
                if (autoFirePoint[i].gameObject.activeInHierarchy == true)
                {
                    autoFireCD aCD = Instantiate(atkObj[num], autoFirePoint[i].position, Quaternion.identity).GetComponent<autoFireCD>();

                    aCD.autoCD = autoFireCD;
                    aCD.autoFirePoint = autoFirePoint[i].gameObject;
                    aCD.begin();


                    break;
                }
            }
            for (int i = 0; i < autoFirePoint.Length; i++)
            {
                if (autoFirePoint[i].gameObject.activeInHierarchy == true)
                {
                    openDO = true;
                }
            }

            if (openDO == false)
            {
                GameObject[] btnAutoFireObj = GameObject.FindGameObjectsWithTag("btnAutoFire");
                for (int i = 0; i < btnAutoFireObj.Length; i++)
                {
                    btnAutoFireObj[i].transform.localPosition = new Vector3(-2000, 0, 0);
                    if (btnAutoFireObj[i].GetComponent<btncontrol>().autoDO == false)
                    {
                        btnAutoFireObj[i].transform.parent.Find("timeText").gameObject.SetActive(false);
                    }
                }
            }

        }
        public void getAtkObj(int num)
        {
            getAtkObjUI.SetActive(true);
            getAtkObjUI.transform.Find("Image").gameObject.GetComponent<Image>().sprite = wqImage[num];
        }
    }
}