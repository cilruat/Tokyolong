using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public static Score scoreScript;
    public static string maxScoreString = "MAX_SCORE";
    Text textScore;
    int score;
    int maxScore;
	// Use this for initialization
	void Start () {
        textScore = GameObject.Find("Score").GetComponent<Text>();
        scoreScript = gameObject.GetComponent<Score>();
        maxScore = PlayerPrefs.GetInt(maxScoreString);
        textScore.text = maxScore.ToString();
    }

    public void AddScore(int count) { // The method is responsible for adding points and writing the maximum value
        score += count;
        if (maxScore < score) {
            maxScore = score;
            PlayerPrefs.SetInt(maxScoreString, maxScore);
        }
        if (score < 0)
            score = 0;
        textScore.text = score.ToString();
    }

    public void SetMaxScore() { 
        maxScore = PlayerPrefs.GetInt(maxScoreString);
        textScore.text = maxScore.ToString();
    }
	


}
