using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Emoji
{
    public class CharracterSelector2D : MonoBehaviour
    {
        public GameObject displayedCharacterTemplate;
        public Vector3 pivot;
        public float spacing;
        [Range(0f, 1f)]
        public float slideSmoothness;

        public Text coinText;
        public Text priceText;
        public Image priceImg;
        public GameObject lockedButton;
        public GameObject unlockButton;
        public GameObject selectButton;
        public GameObject prevButton;
        public GameObject nextButton;

        public SunburstAnim sunburstAnim;

        private int currentCharacterIndex;
        private int numberOfCharacter;

        private GameObject root;
        private Vector3 snapPoint;

        public void Start()
        {
            currentCharacterIndex = CharacterManager.Instance.CurrentCharacterIndex;
            numberOfCharacter = CharacterManager.Instance.characters.Length;
            InitLayout();
        }

        public void InitLayout()
        {
            CreateRoot();

            Vector3 instantiatePos;
            for (int i = 0; i < numberOfCharacter; ++i)
            {
                instantiatePos = pivot + i * spacing * Vector3.right;
                GameObject g = Instantiate(displayedCharacterTemplate.gameObject, instantiatePos, Quaternion.identity) as GameObject;
                g.transform.SetParent(root.transform);
                g.GetComponent<SpriteRenderer>().sprite = CharacterManager.Instance.characters[i].sprite;
            }
            selectButton.SetActive(true);
            lockedButton.SetActive(false);
            unlockButton.SetActive(false);
        }

        public void CreateRoot()
        {
            root = new GameObject();
            root.name = "root";
            root.transform.position = pivot;
        }

        public void Update()
        {
            coinText.text = CoinManager.Instance.Coins.ToString();
            snapPoint = pivot - currentCharacterIndex * spacing * Vector3.right;
            root.transform.position = Vector3.Lerp(root.transform.position, snapPoint, 1 - slideSmoothness);

            UpdateButtons();
        }

        public void Next()
        {
            if (currentCharacterIndex < numberOfCharacter - 1)
            {
                ++currentCharacterIndex;
            }
        }

        public void Prev()
        {
            if (currentCharacterIndex > 0)
            {
                --currentCharacterIndex;
            }
        }

        public void UnlockCurrentViewCharacter()
        {
            if (CharacterManager.Instance.characters[currentCharacterIndex].Unlock())
            {
                unlockButton.SetActive(false);
                priceText.gameObject.SetActive(false);
                priceImg.gameObject.SetActive(false);
                coinText.text = CoinManager.Instance.Coins.ToString();
                //CharacterManager.Instance.CurrentCharacterIndex = currentCharacterIndex;
                selectButton.SetActive(true);
                sunburstAnim.PlayAnim();
                SoundManager.Instance.PlaySound(SoundManager.Instance.unlock);
            }
        }

        public void Select()
        {
            CharacterManager.Instance.CurrentCharacterIndex = currentCharacterIndex;
            Back();
        }

        public void Back()
        {
            SceneManager.LoadScene("Main");
        }

        void UpdateButtons()
        {
            if (currentCharacterIndex == 0)
                prevButton.SetActive(false);
            else if (currentCharacterIndex == numberOfCharacter - 1)
                nextButton.SetActive(false);
            else
            {
                prevButton.SetActive(true);
                nextButton.SetActive(true);
            }

            Character currentViewCharacter = CharacterManager.Instance.characters[currentCharacterIndex];
            if (currentViewCharacter.IsUnlocked)
            {
                priceText.gameObject.SetActive(false);
                priceImg.gameObject.SetActive(false);
                unlockButton.SetActive(false);
                lockedButton.SetActive(false);
                selectButton.SetActive(true);
            }
            else
            {
                priceText.text = currentViewCharacter.price.ToString();
                priceText.gameObject.SetActive(true);
                priceImg.gameObject.SetActive(true);
                bool canUnlock = CoinManager.Instance.Coins >= currentViewCharacter.price;
                unlockButton.SetActive(canUnlock);
                lockedButton.SetActive(!canUnlock);
                selectButton.SetActive(false);
            }
        }
    }
}