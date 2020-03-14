using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Takgu
{
    /// <summary>
    /// This script controls hitting of ball, bouncing it back & scores
    /// </summary>
    public class BounceBack : MonoBehaviour
    {
        public Text scoretext;
        int score = 0;
        void OnTriggerEnter2D(Collider2D other)
        {
            //on hitting by racket bouncing command should start of the BallMove script
            other.gameObject.SendMessage("BounceStart");
            //add score by 1
            score++;
            //set the score to ScoreText
            scoretext.text = "" + score.ToString();
            //play the audio for bouncing
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
            //run the command score of GameController
            GameObject.FindGameObjectWithTag("GameController").SendMessage("Score");
        }
    }
}