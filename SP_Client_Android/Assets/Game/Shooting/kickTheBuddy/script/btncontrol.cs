using UnityEngine;  
using UnityEngine.Events;  
using UnityEngine.EventSystems;  
using System.Collections;  
using UnityEngine.UI;

namespace KickTheBuddy
{
    public class btncontrol : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {

        [HideInInspector]
        public int atkObjNum;
        [HideInInspector]
        public float autoFireCD;
        [HideInInspector]
        public int unlockPiece;
        [HideInInspector]
        public bool autoDO;
        private float lastTime;
        private float nowTime;
        private float textTime;
        private Text timeText;
        private gameManage gm;

        //private Animator  freeCoinTis;
        void Start()
        {  //Judgement button type
            gm = GameObject.FindGameObjectWithTag("gameManage").GetComponent<gameManage>();


            timeText = this.transform.parent.Find("timeText").GetComponent<Text>();
            if ((atkObjNum == 0 || atkObjNum == 5) && this.name == "btnAutoFire")
            { //Flying knife and tank have no automatic firing function.
                this.gameObject.SetActive(false);
                timeText.gameObject.SetActive(false);
            }
            if (atkObjNum == 0 && this.name == "btnAtkObj") //Flying cutter is the weapon selected by default.
                this.GetComponent<Image>().color = new Color(0.32f, 1, 0);

            if (atkObjNum == 0 && this.name == "btnUnlock")
            { //Flying cutter is the weapon of default release.
                this.gameObject.SetActive(false);
            }
            if (PlayerPrefs.GetInt("atkObj" + atkObjNum) == 1 && this.name == "btnUnlock")
            {       //Lock the lock button when the weapon is unlocked.
                this.gameObject.SetActive(false);
            }
            else if (PlayerPrefs.GetInt("atkObj" + atkObjNum) == 0 && this.name == "btnUnlock")
            {       //Unlocked weapons do not display automatic fire button, do not show countdown, show price.
                this.transform.parent.Find("btnAutoFire").gameObject.SetActive(false);
                this.transform.parent.Find("timeText").gameObject.SetActive(false);
                this.transform.Find("piece").gameObject.GetComponent<Text>().text = "" + unlockPiece;
            }



        }


        void FixedUpdate()
        {

            textTime = (lastTime + autoFireCD) - Time.time;
            timeText.text = "" + Mathf.Floor(textTime / 60) + ":" + Mathf.Floor(textTime - Mathf.Floor(textTime / 60) * 60); //Countdown display
            if (textTime < 0)
            {
                timeText.text = "0:0";
            }

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //keyDown.Invoke();  

            if (this.name == "btnAtkObj")
            {//Click on the weapon button.
                gm.btnAtkObj(atkObjNum);
                GameObject[] gos = GameObject.FindGameObjectsWithTag("btnAtkObj");
                for (int i = 0; i < gos.Length; i++)
                {
                    gos[i].GetComponent<Image>().color = new Color(1, 1, 1);
                }
                this.GetComponent<Image>().color = new Color(0.32f, 1, 0);
            }
            else if (this.name == "btnAutoFire")
            {//Click the automatic fire button.

                autoDO = true;
                gm.btnAutoFire(atkObjNum);

                this.transform.localPosition = new Vector3(-2000, 0, 0);
                StartCoroutine(waitAutoFire());

                lastTime = Time.time;


            }
            else if (this.name == "btnUnlock")
            { //Click the unlock button.
                if (gm.coinCount >= unlockPiece)
                {
                    gm.coinCount -= unlockPiece;
                    gm.coinText.text = "" + gm.coinCount;
                    PlayerPrefs.SetInt("coin", gm.coinCount);
                    PlayerPrefs.SetInt("atkObj" + atkObjNum, 1);
                    this.gameObject.SetActive(false);

                    if (atkObjNum != 5)
                    {
                        this.transform.parent.Find("btnAutoFire").gameObject.SetActive(true);
                        this.transform.parent.Find("timeText").gameObject.SetActive(true);
                    }
                    gm.getAtkObj(atkObjNum);


                    gm.btnAtkObj(atkObjNum);
                    GameObject[] gos = GameObject.FindGameObjectsWithTag("btnAtkObj");
                    for (int i = 0; i < gos.Length; i++)
                    {
                        gos[i].GetComponent<Image>().color = new Color(1, 1, 1);
                    }
                    this.transform.parent.Find("btnAtkObj").gameObject.GetComponent<Image>().color = new Color(0.32f, 1, 0);
                }
                else
                {
                    gm.noMoneyUI.SetActive(true);


                }
            }
        }
        IEnumerator waitAutoFire()
        { //Automatic fire duration
            yield return new WaitForSeconds(autoFireCD);
            autoDO = false;
            GameObject[] btnAutoFireObj = GameObject.FindGameObjectsWithTag("btnAutoFire");
            for (int i = 0; i < btnAutoFireObj.Length; i++)
            {

                if (btnAutoFireObj[i].GetComponent<btncontrol>().autoDO == false)
                {

                    btnAutoFireObj[i].transform.localPosition = new Vector3(95, 0, 0);
                    btnAutoFireObj[i].transform.parent.Find("timeText").gameObject.SetActive(true);
                }
            }

        }
        public void OnPointerUp(PointerEventData eventData)
        {
            //keyUp.Invoke();  

        }

        public void OnPointerExit(PointerEventData eventData)
        {

        }
    }
}