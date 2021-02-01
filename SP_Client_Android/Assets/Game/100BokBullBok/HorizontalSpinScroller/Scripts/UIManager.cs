using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GameBench
{
    public class UIManager : MonoBehaviour
    {
        public GameObject objCanvas;
        public SpriteRenderer[] borderSpriteRend, arrowSpriteRend;
        public Image backImg;
        //public Image addMoreImg;

        public Image[] spinBtnImg;

        public SpinTurnTimer spinTimer;
        TurnType spinTurnType = TurnType.PaidOnly;
        public Text spinsText, coinsText, spinCostText, animTextCost;
        public const string COINS_COUNT = "COINS_COUNT", SPIN_COUNT = "SPIN_COUNT";
        public GameObject _animCost;
        int _coins, _spins;
        public Button spinBtnFree, spinBtnPaid;

        public GameObject BlindPanel;


        private void Awake()
        {
            /*
            Spins = PlayerPrefs.GetInt(SPIN_COUNT, 10);
            // Spin과 Coin의 합체

            Coins = PlayerPrefs.GetInt(COINS_COUNT, 3000);
            // 현재 내 포인트를 넣읍시다
            */

            Coins = Info.GamePlayCnt;

            SetTurnType();
            switch (spinTurnType)
            {
                case TurnType.PaidOnly:
                    spinBtnPaid.gameObject.SetActive(true);
                    //paidTurnCostText.text = (-Config.Instance.spinTurnCost).ToString();
                    spinTimer.gameObject.SetActive(false);
                    spinBtnFree.gameObject.SetActive(false);
                    break;
                case TurnType.FreeOnly:
                    spinBtnFree.gameObject.SetActive(true);
                    spinTimer.gameObject.SetActive(false);
                    spinBtnPaid.gameObject.SetActive(false);
                    break;
                case TurnType.Both:
                    spinBtnPaid.gameObject.SetActive(true);
                    spinTimer.gameObject.SetActive(true);
                    //paidTurnCostText.text = (-Config.Instance.spinTurnCost).ToString();
                    spinBtnFree.gameObject.SetActive(false);
                    break;
            }
            spinBtnFree.onClick.AddListener(OnClickFreeSpin);
            //spinBtnPaid.onClick.AddListener(OnClickPaidSpin);
            UpdateUI();
        }

        private void Start()
        {
            BlindPanel.SetActive(false);

        }

        void UpdateUI()
        {
            foreach (var item in borderSpriteRend)
            {
                item.sprite = Config.Instance.CurrentTheme.borderSprite;
            }
            foreach (var item in arrowSpriteRend)
            {
                item.sprite = Config.Instance.CurrentTheme.arrowSprite;
            }
            foreach (var item in spinBtnImg)
            {
                item.sprite = Config.Instance.CurrentTheme.spinButtonBg;
            }
            backImg.sprite = Config.Instance.CurrentTheme.backButonBg;
            //addMoreImg.sprite = Config.Instance.CurrentTheme.addMoreButtonBg;
        }

        // 결국 코인과 스핀은 같은것이라 값을 취하고 이미지만 딱 바꾸면 끝인건가?

        public int Coins
        {
            get { return _coins; }
            set
            {
                //_coins = value;
                _coins = Info.GamePlayCnt;
                coinsText.text = _coins.ToString();
            }
        }

        public int Spins
        {
            get { return _spins; }
            set
            {
                _spins = value;
                //_spins = Info.GamePlayCnt;
                spinCostText.text = (value == 0) ? "코인이 필요해요" : "돌리기";
                spinsText.text = string.Format("남은횟수 {0}", _spins);
            }
        }

        void SetTurnType()
        {
            if (!Config.Instance.freeTurn && !Config.Instance.paidTurn
                || (Config.Instance.freeTurn && !Config.Instance.paidTurn))
            {
                spinTurnType = TurnType.FreeOnly;
            }
            else if (Config.Instance.freeTurn && Config.Instance.paidTurn)
            {
                spinTurnType = TurnType.Both;
            }
            else if (Config.Instance.paidTurn && !Config.Instance.freeTurn)
            {
                spinTurnType = TurnType.PaidOnly;
            }
        }

        public void ActivateFreeSpin()
        {
            spinBtnFree.interactable = true;
            spinBtnFree.gameObject.SetActive(true);
            spinBtnPaid.gameObject.SetActive(false);
            Debug.Log("ActivateFreeSpin");

        }

        public void OnClickFreeSpin()
        {
            spinBtnFree.interactable = false;
            SpinScroller.Instance.StartSpin();
        }

        // 여기를 코인으로 바꾸면 되겟네
        public void OnClickPaidSpin()
        {
            /* //원본
            if (Spins > 0)
            {
                UpdateSpin(-1);
                spinBtnPaid.interactable = false;
                SpinScroller.Instance.StartSpin();
            }
            else
            {
                if (Coins >= Config.Instance.spinTurnCost)
                {
                    UpdateSpin(10);
                }
            }*/

            // Coins 가 아니라 내가 가진값을 넣기 // Config.Instance.spinTurnCost 이걸 넣어주면 되겟나...
            if (Info.GamePlayCnt >= Config.Instance.spinTurnCost)
            {
                NetworkManager.Instance.GameCountInput_REQ(Info.TableNum, -Config.Instance.spinTurnCost);
                spinBtnPaid.interactable = false;
                SpinScroller.Instance.StartSpin();
            }
            //예외처리

            else if(Info.GamePlayCnt < Config.Instance.spinTurnCost)
            {
                BlindPanel.SetActive(true);
            }

            else
            {
                Debug.Log("예외처리");
            }

        }

        /*
        bool IsPaidTurnPossible()
        {
            //1씩 까는걸 여기서 설정, 순서도 여기있네
            return Coins >= Config.Instance.spinTurnCost;
        }
        */

        public void SpinCompleted()
        {
            if (spinTurnType == TurnType.FreeOnly)
            {
                spinBtnFree.interactable = true;
            }
            else if (spinTurnType == TurnType.Both)
            {
                spinBtnPaid.gameObject.SetActive(true);
                //spinBtnPaid.interactable = IsPaidTurnPossible();
                spinTimer.gameObject.SetActive(true);
            }
            else if (spinTurnType == TurnType.PaidOnly)
            {
                spinBtnPaid.interactable = true;
            }
            SpinScroller.Instance.particleAnim.SetActive(false);
            Debug.Log("SpinCompleted");

        }

        //Update는 안써서 둘다 빼긴함
        public void UpdateSpin(int val)
        {
            animTextCost.text = string.Format("{0} {1}", val > 0 ? "+" : "", val);
            _animCost.gameObject.SetActive(true);
            Spins += val;
        }

        public void UpdateCoins(int val)
        {
            animTextCost.text = string.Format("{0} {1}", val > 0 ? "+" : "", val);
            _animCost.gameObject.SetActive(true);
            Coins += val;
            Debug.LogFormat("Got Coins {0}", val);
            Debug.Log("UpdateCoins");

        }

        public void RefreshCoins()
        {
            //_coins = Info.GamePlayCnt;
            coinsText.text = Info.GamePlayCnt.ToString();
        }


        public void OnThemeChanged(int themeID)
        {
            UpdateUI();
        }

        public void OnCloseBlindPanel()
        {
            BlindPanel.SetActive(false);
        }

        public void OnGoFirst()
        {
            SceneChanger.LoadScene("LuckGame", objCanvas);

        }

        //이게 의미가 있는건가 어디쓰는지 보아야할것
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                PlayerPrefs.SetInt(COINS_COUNT, Coins);
                PlayerPrefs.SetInt(SPIN_COUNT, Spins);
                PlayerPrefs.Save();
            }
        }

        private static UIManager _instance;

        public static UIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<UIManager>();
                }

                return _instance;
            }
        }
    }
public enum TurnType
{
    PaidOnly = 0,
    FreeOnly,
    Both
}
}