using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ColorGame
{
    public class GameCntrl : MonoBehaviour
    {

        float score;
        GameObject[] colorObjects = new GameObject[4];

        public GameObject ColorObjectPref;
        public Vector3[] Positions;
        public Text ScoreText;
        public Button RestartBtn;
        public Button RetrunHomeBtn;

        void Start()
        {
            score = 0;
            ScoreText.text = "색깔이 똑같은것을 선택해주세요!";

            InitColorObj();

            RestartBtn.GetComponent<Button>().onClick.AddListener(Restart);
            RetrunHomeBtn.GetComponent<Button>().onClick.AddListener(ReturnHome);

            RestartBtn.gameObject.SetActive(false);
            RetrunHomeBtn.gameObject.SetActive(false);

        }

        void InitColorObj()
        {
            for (int i = 0; i < Positions.Length; i++)
            {
                colorObjects[i] = Instantiate(ColorObjectPref, Positions[i], Quaternion.identity) as GameObject;
            }

            RandomizeColors();
        }

        void RandomizeColors()
        {
            Color randColor = Random.ColorHSV(0f, 1f,
                                               0.2f, 1f,
                                               0.2f, 1f);

            float randRange = -.005f * score + 1f;

            GameObject.Find("MainColorObject").GetComponent<ColorObject>().SetColor(randColor, randRange);

            SetRandomRight();

            for (int i = 0; i < Positions.Length; i++)
            {
                colorObjects[i].GetComponent<ColorObject>().SetColor(randColor, randRange);
            }
        }

        void SetRandomRight()
        {
            int right = Random.Range(0, Positions.Length);

            for (int i = 0; i < Positions.Length; i++)
            {
                if (i == right)
                { colorObjects[i].GetComponent<ColorObject>().isRight = true; }
                else
                { colorObjects[i].GetComponent<ColorObject>().isRight = false; }
            }
        }

        public void GameOver()
        {
            ScoreText.text = "게임오버 당신의 점수는 : " + score.ToString();

            for (int i = 0; i < Positions.Length; i++)
            {
                colorObjects[i].GetComponent<ColorObject>().isEnable = false;
            }

            RestartBtn.gameObject.SetActive(true);
            RetrunHomeBtn.gameObject.SetActive(true);
        }

        public void NextLevel()
        {
            score++;
            ScoreText.text = score.ToString();

            RandomizeColors();
        }

        void Restart()
        {
            RestartBtn.gameObject.SetActive(false);

            score = 0;
            ScoreText.text = score.ToString();

            for (int i = 0; i < Positions.Length; i++)
            {
                colorObjects[i].GetComponent<ColorObject>().isEnable = true;
            }

            RandomizeColors();
        }


        void ReturnHome()
        {
            RestartBtn.gameObject.SetActive(false);
            SceneManager.LoadScene("ArcadeGame");
        }

    }
}