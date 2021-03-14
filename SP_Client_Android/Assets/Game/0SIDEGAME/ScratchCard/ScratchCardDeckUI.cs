using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScratchCardDeckUI : MonoBehaviour {

    public Image chr;

    public void CardUISet(ScratchCard card)
    {
        chr.sprite = card.cardImage;
    }

}
