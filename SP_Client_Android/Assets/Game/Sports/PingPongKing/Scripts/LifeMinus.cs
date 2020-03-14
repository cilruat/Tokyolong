using UnityEngine;
using System.Collections;
namespace Takgu
{
    /// <summary>
    /// This script controls the reduction of lives 
    /// </summary>
    public class LifeMinus : MonoBehaviour
    {
        int lives;
        public GameObject life1;
        public GameObject life2;
        public GameObject life3;
        void Start()
        {
            //on game start all 3 lives should be active
            life1.SetActive(true);
            life2.SetActive(true);
            life3.SetActive(true);
            lives = 3;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            // on missing a ball reducing a life
            lives -= 1;
            //play sound for reducing life
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
            switch (lives)
            {
                //if player has 3 lives all 3 lives gameobject would be active
                case 3:
                    life1.SetActive(true);
                    life2.SetActive(true);
                    life3.SetActive(true);
                    break;
                case 2:
                    //if player has 2 lives only 2 lives gameobject would be active
                    life1.SetActive(true);
                    life2.SetActive(true);
                    life3.SetActive(false);
                    break;
                case 1:
                    //if player has 1 life only 1 life gameobject would be active
                    life1.SetActive(true);
                    life2.SetActive(false);
                    life3.SetActive(false);
                    break;
                case 0:
                    //if player has no life no life gameobject would be active
                    life1.SetActive(false);
                    life2.SetActive(false);
                    life3.SetActive(false);
                    //run the GameOver command in the GameController
                    GameObject.FindGameObjectWithTag("GameController").SendMessage("GameOver");
                    break;
                default:
                    //at default none should be visible
                    life1.SetActive(false);
                    life2.SetActive(false);
                    life3.SetActive(false);
                    break;
            }
        }
    }
}