using UnityEngine;
using UnityEngine.UI;

namespace GameBench
{
    public class UIManager : MonoBehaviour
    {
        public SpriteRenderer[] borderSpriteRend, arrowSpriteRend;
        public Image backImg, addMoreImg;
        public Image[] spinBtnImg;

        public SpinTurnTimer spinTimer;
        TurnType spinTurnType = TurnType.PaidOnly;
        public Text spinsText, coinsText, spinCostText, animTextCost;
        public const string COINS_COUNT = "COINS_COUNT", SPIN_COUNT = "SPIN_COUNT";
        public GameObject _animCost;
        int _coins, _spins;
        public Button spinBtnFree, spinBtnPaid;

        private void Awake()
        {
            Spins = PlayerPrefs.GetInt(SPIN_COUNT, 10);
            Coins = PlayerPrefs.GetInt(COINS_COUNT, 3000);
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
            spinBtnPaid.onClick.AddListener(OnClickPaidSpin);
            UpdateUI();
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
            addMoreImg.sprite = Config.Instance.CurrentTheme.addMoreButtonBg;
        }

        public int Coins
        {
            get { return _coins; }
            set
            {
                _coins = value;
                coinsText.text = _coins.ToString();
            }
        }

        public int Spins
        {
            get { return _spins; }
            set
            {
                _spins = value;
                spinCostText.text = (value == 0) ? "Buy 10 Spins" : "Spin";
                spinsText.text = string.Format("Spins {0}", _spins);
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
        }

        public void OnClickFreeSpin()
        {
            spinBtnFree.interactable = false;
            SpinScroller.Instance.StartSpin();
        }

        public void OnClickPaidSpin()
        {
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
            }
        }

        bool IsPaidTurnPossible()
        {
            return Coins >= Config.Instance.spinTurnCost;
        }

        public void SpinCompleted()
        {

            if (spinTurnType == TurnType.FreeOnly)
            {
                spinBtnFree.interactable = true;
            }
            else if (spinTurnType == TurnType.Both)
            {
                spinBtnPaid.gameObject.SetActive(true);
                spinBtnPaid.interactable = IsPaidTurnPossible();
                spinTimer.gameObject.SetActive(true);
            }
            else if (spinTurnType == TurnType.PaidOnly)
            {
                spinBtnPaid.interactable = IsPaidTurnPossible();
            }
            SpinScroller.Instance.particleAnim.SetActive(false);
        }

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
        }

        public void OnThemeChanged(int themeID)
        {
            UpdateUI();
        }

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