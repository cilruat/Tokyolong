using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealPoint : MonoBehaviour {

    Image healPointSprite;
    public List<Sprite> allHealPointSprite;
    int healpoint = 4;
    public static bool GameOver; // The variable is responsible for the start and end of the game
    public static HealPoint healPointScript;
	// Use this for initialization
	void Start () {
        healPointScript = gameObject.GetComponent<HealPoint>();
        healPointSprite = gameObject.GetComponent<Image>();
        GameOver = true;
    }

    public void ChangeHealPoint(int count) { // The method adds and takes away health and checks its quantity, if the health is over, then the game over
        healpoint += count;
        healpoint = Mathf.Clamp(healpoint,0,4);
        healPointSprite.sprite = allHealPointSprite[healpoint];
        if (healpoint <= 0) {
            PlaceManager.placeManagerScript.StopGame();
            GameOver = true;
            StartGame.startGameScript.LoseTheGame();
        }
    }
}
