
// UI SCORE SCRIPT 20171215 //


using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace AirHokey
{
    public class UiScoreScript : MonoBehaviour
    {

        // These are the numeric font object meshes
        public Sprite[] numericOption;
        //public Sprite[] numericOptionP2;

        public Material scoreBaseMat; // The base score material
        public Material scoreFxMat; // The highlighted score material

        public RectTransform uiScore1;
        public RectTransform uiScore2;

        public Image uiScore1Image;
        public Image uiScore2Image;
        public Image uiIndentImage;

        public Color baseColor;
        public Color scoreColor;

        private void Start()
        {
            uiScore1Image.color = uiScore2Image.color = uiIndentImage.color = baseColor; // Set Sprite font colors
        }

        public void ResetUIScore()
        {
            uiScore1Image.sprite = uiScore2Image.sprite = numericOption[0]; // Set Sprite font numeric values
        }

        public void UpdateUIScore(int player, int score)
        {

            if (player == 1)
            {
                uiScore1Image.sprite = numericOption[score]; // Update the score display
                StartCoroutine(ScoreFx(uiScore1, 1.25f, 1, 0.2f));  // Call 'bump' fx
            }
            else if (player == 2)
            {
                uiScore2Image.sprite = numericOption[score]; // Update the score display
                StartCoroutine(ScoreFx(uiScore2, 1.25f, 1, 0.2f)); // Call 'bump' fx
            }

        }

        // This function manages the button's score effect
        IEnumerator ScoreFx(RectTransform scoreTr, float startPos, float endPos, float time)
        {

            scoreTr.GetComponent<Image>().color = scoreColor; // Hghlight the new score

            float i = 0;
            float rate = 1.0f / time;

            // Lerp the transform's local position from "startPos" to "endPos" at the given speed "time"
            while (i < 1.0f)
            {
                i += Time.deltaTime * rate;
                scoreTr.localScale = new Vector3(Mathf.Lerp(startPos, endPos, i), Mathf.Lerp(startPos, endPos, i), Mathf.Lerp(startPos, endPos, i));
                yield return null;
            }

            scoreTr.GetComponent<Image>().color = baseColor;

        }

    }
}