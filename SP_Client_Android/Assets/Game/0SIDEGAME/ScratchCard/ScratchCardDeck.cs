using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScratchCard
{
    public Sprite cardImage;
    public int weight;

    public ScratchCard(ScratchCard card)
    {
        this.cardImage = card.cardImage;
        this.weight = card.weight;
    }
}
